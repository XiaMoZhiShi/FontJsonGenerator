using FontJsonGenerator.Extensions;
using FontJsonGenerator.GenerateConfig;
using FontJsonGenerator.Generator;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace FontJsonGenerator;

public static class Program
{
    public static readonly JsonSerializerSettings? SerializerSettings = new JsonSerializerSettings
    {
        NullValueHandling = NullValueHandling.Ignore,
        MissingMemberHandling = MissingMemberHandling.Ignore
    };

    private static string? inputFilePath;

    private static string outputFilePath;

    private static string tempFilePath;

    private static bool generateGlyph;

    private static FontGenConfig? fontGenConfig;

    private static GlyphGenConfig glyphGenConfig => fontGenConfig.GlyphGenConfig;

    public static void Main(string?[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("没有参数");
            Environment.Exit(1);
        }

        int index = 0;
        foreach (var arg in args)
        {
            switch (arg)
            {
                case "--input":
                    if (index + 1 >= args.Length)
                        throw new ArgumentException("参数不足");

                    inputFilePath = args[index + 1];
                    break;
            }

            index++;
        }

        if (string.IsNullOrEmpty(inputFilePath))
            throw new ArgumentNullException(inputFilePath, "文件地址不能为空");

        if (!File.Exists(inputFilePath))
            throw new FileNotFoundException($"没有找到文件({inputFilePath})，请检查地址");

        string fileContent = File.ReadAllText(inputFilePath).Replace("\\", "\\\\");
        fontGenConfig = JsonConvert.DeserializeObject<FontGenConfig>(fileContent, SerializerSettings)!;

        //获取设置
        generateGlyph = fontGenConfig.GlyphGenConfig?.Enabled ?? false;
        tempFilePath = fontGenConfig.TempFileDirectory;
        outputFilePath = fontGenConfig.OptoutTarget;

        GenerateJson(fontGenConfig);

        if (generateGlyph) glyphGenerator.Generate(glyphGenConfig, charaterList);
    }

    private static readonly List<MinecraftFontProviderProperty> charaterList = new List<MinecraftFontProviderProperty>();
    private static readonly GlyphGenerator glyphGenerator = new GlyphGenerator();

    private static void cutImage()
    {
        
    }

    private static void GenerateJson(FontGenConfig fontConfig)
    {
        var fontProperty = new MinecraftFontProperty();
        int currentUnicodeId = fontConfig.StartsAt;

        foreach (var rangeinfo in fontConfig.Ranges)
        {
            switch (rangeinfo.RangeType)
            {
                //todo: 添加对诸如minecraft:font/ascii.png这类资源的Json生成支持
                //摆烂了（
                case "raw":
                    fontProperty.Providers.Add(rangeinfo.ProviderProperty);
                    currentUnicodeId += rangeinfo.Skips;
                    break;

                case "tilemap":
                {
                    var tileConfig = rangeinfo.TileMapConfig;

                    // ReSharper disable AccessToDisposedClosure
                    using var targetImage = Image.Load<Rgba32>(tileConfig.TexturePath
                                                               + tileConfig.ResourceLocation.ToFilePath());
                    var currentPoint = new Point();

                    List<string> charaters = new List<string>();

                    //图像有多少列
                    int maxRow = ( targetImage.Width / tileConfig.Size ) - 1;

                    //图像有多少行
                    int maxCol = ( targetImage.Height / tileConfig.Size ) - 1;

                    //第几行
                    for (int y = 0; y <= maxCol; y++)
                    {
                        string currentLine = string.Empty;
                        int x = 0;

                        //如果y处于范围内
                        if (y >= tileConfig.StartsAt.Y && y <= tileConfig.EndsAt.Y)
                        {
                            //第几列
                            for (; x <= maxRow; x++)
                            {
                                //如果x处于范围内
                                if (x >= ( y == tileConfig.StartsAt.Y ? tileConfig.StartsAt.X : 0 )
                                    && x <= (y == tileConfig.EndsAt.Y ? tileConfig.EndsAt.X : maxRow))
                                {
                                    if (!tileConfig.IgnoreForGlyphgenerate)
                                    {
                                        
                                        currentPoint.X = x * tileConfig.Size;
                                        currentPoint.Y = y * tileConfig.Size;

                                        using var tempImage = targetImage.Clone<Rgba32>();

                                        tempImage.Mutate(p =>
                                        {
                                            Console.WriteLine($"Crop: {currentPoint}(x:{x} y:{y}) -> {tileConfig.Size}");
                                            p.Crop(new Rectangle(currentPoint,
                                                new Size(tileConfig.Size)));
                                        });

                                        string tempPath = tempFilePath + $"x{x}" + $"y{y}";
                                        tempImage.SaveAsPng(tempPath);

                                        charaterList.Add(new MinecraftFontProviderProperty
                                        {
                                            Ascent = rangeinfo.Ascent,
                                            Height = rangeinfo.Height,
                                            Type = ProviderType.Bitmap,
                                            Charaters = new[] { $@"\u{currentUnicodeId:X}" },
                                            ResourcePath = tempPath
                                        });
                                    }

                                    currentLine += $@"\u{currentUnicodeId:X}";

                                    currentUnicodeId++;
                                }
                                else
                                    currentLine += "\u0000";
                            }
                        }

                        //向后补全
                        if (x < maxRow)
                        {
                            for (; x <= maxRow; x++)
                                currentLine += "\u0000";
                        }

                        charaters.Add(currentLine);
                    }
                    // ReSharper restore AccessToDisposedClosure

                    fontProperty.Providers.Add(new MinecraftFontProviderProperty
                    {
                        Type = ProviderType.Bitmap,
                        Ascent = 7,
                        Charaters = charaters.ToArray(),
                        ResourcePath = tileConfig.TexturePath
                    });

                    break;
                }

                case "normal":
                    foreach (var value in rangeinfo.Values)
                    {
                        string resourcePath = rangeinfo.NameSpace
                                              + ":"
                                              + rangeinfo.Prefix
                                              + value
                                              + rangeinfo.Suffix
                                              + ".png";

                        var providerProperty = new MinecraftFontProviderProperty
                        {
                            Ascent = 7,
                            Type = ProviderType.Bitmap,
                            Charaters = new[] { $@"\u{currentUnicodeId:X}" },
                            ResourcePath = resourcePath
                        };

                        fontProperty.Providers.Add(providerProperty);

                        if (generateGlyph
                            && currentUnicodeId >= glyphGenConfig.StartAt
                            && currentUnicodeId <= glyphGenConfig.EndsAt)
                        {
                            charaterList.Add(providerProperty);
                        }

                        currentUnicodeId += 1 + rangeinfo.Skips;
                    }
                    break;

                default:
                    throw new InvalidOperationException($"未知的类型: {rangeinfo.RangeType}");
            }
        }

        //替换"\\"为"\"
        string result = JsonConvert.SerializeObject(fontProperty, SerializerSettings).Replace("\\\\", "\\");

        switch (outputFilePath)
        {
            case "stdout":
                Console.Write(result);
                break;
            
            case "null":
                break;
            
            default:
                File.WriteAllText(outputFilePath, result);
                break;
        }
    }
}