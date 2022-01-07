﻿using Newtonsoft.Json;

namespace FontJsonGenerator;

public static class Program
{
    public static readonly JsonSerializerSettings? SerializerSettings = new JsonSerializerSettings
    {
        NullValueHandling = NullValueHandling.Ignore,
        MissingMemberHandling = MissingMemberHandling.Ignore
    };

    private static string inputFilePath;

    private static string outputFilePath;

    public static void Main(string[] args)
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

                case "--output":
                    if (index + 1 >= args.Length)
                        throw new ArgumentException("参数不足");

                    outputFilePath = args[index + 1];
                    break;
            }

            index++;
        }

        if (string.IsNullOrEmpty(inputFilePath))
            throw new ArgumentNullException(inputFilePath, "文件地址不能为空");

        if (!File.Exists(inputFilePath))
            throw new FileNotFoundException($"没有找到文件({inputFilePath})，请检查地址");

        string fileContent = File.ReadAllText(inputFilePath).Replace("\\", "\\\\");

        Convert(JsonConvert.DeserializeObject<FontGenConfig>(fileContent, SerializerSettings));
    }

    private static void Convert(FontGenConfig? fontConfig)
    {
        if (fontConfig == null)
            throw new NullReferenceException("字体生成配置不能是null");

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

                case "normal":
                    foreach (var value in rangeinfo.Values)
                    {
                        var providerProperty = new MinecraftFontProviderProperty
                        {
                            Ascent = 7,
                            Type = ProviderType.Bitmap,
                            Charaters = new[] { $@"\u{currentUnicodeId:X}" },
                            ResourcePath = rangeinfo.NameSpace
                                           + ":"
                                           + rangeinfo.Prefix
                                           + value
                                           + rangeinfo.Suffix
                                           + ".png"
                        };

                        fontProperty.Providers.Add(providerProperty);

                        currentUnicodeId += 1 + rangeinfo.Skips;
                    }
                    break;
                
                default:
                    throw new InvalidOperationException($"未知的类型: {rangeinfo.RangeType}");
            }
        }

        //替换"\\"为"\"
        string result = JsonConvert.SerializeObject(fontProperty, SerializerSettings).Replace("\\\\", "\\");

        if (!string.IsNullOrEmpty(outputFilePath))
            File.WriteAllText(outputFilePath, result);
        else
            Console.Write(result);
    }
}