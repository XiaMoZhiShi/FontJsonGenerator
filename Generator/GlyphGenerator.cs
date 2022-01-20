using FontJsonGenerator.Extensions;
using FontJsonGenerator.GenerateConfig;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

// ReSharper disable AccessToDisposedClosure

namespace FontJsonGenerator.Generator;

public class GlyphGenerator
{
    public void Generate(GlyphGenConfig glyphConfig, List<MinecraftFontProviderProperty> providerProperties)
    {
        //确定长宽
        var sideLength = glyphConfig.TileSize * 16;
        int imageWidth = sideLength + glyphConfig.HorizonalSpacing * (16 - 1);
        int imageHeight = sideLength + glyphConfig.VerticalSpacing * (16 - 1);
        var glyphImage = new Image<Rgba32>(imageWidth, imageHeight);

        //确定后备纹理以及其是否可用
        string fallbackPath = glyphConfig.BasePath + glyphConfig.FallbackTexturePath?.ToFilePath() + ".png";
        bool fallbackAvaliable = File.Exists(fallbackPath);

        //用于提供绘制位置
        Point point = new Point(0, 0);

        int index = -1; //当前字符在map中的位置
        int page = 0; //当前页面

        foreach (var providerProperty in providerProperties)
        {
            //移动位置
            index++;

            string resourcePath = providerProperty.ResourcePath;

            //更新point
            updatePoint(ref point,
                index, 
                providerProperties.Count,
                imageWidth,
                imageHeight,
                glyphConfig,
                ref glyphImage,
                ref page);

            //获取纹理
            string filePath = resourcePath.StartsWith('/')
                ? resourcePath
                : glyphConfig.BasePath + resourcePath.ToFilePath();

            //如果纹理不存在，则尝试使用后备纹理
            if (!File.Exists(filePath))
            {
                if (fallbackAvaliable)
                {
                    Console.WriteLine($"{filePath}不存在，将使用{fallbackPath}");
                    filePath = fallbackPath;
                }
                else
                    Console.WriteLine($"{filePath}和{fallbackPath}均不存在");
            }

            if (File.Exists(filePath))
            {
                //加载纹理
                using var targetImage = Image.Load(filePath);

                Console.WriteLine($"{index} / {providerProperties.Count}: draw {resourcePath} at {point}");
                drawImage(targetImage, glyphImage, point, glyphConfig);

                if (glyphConfig.SaveSteps)
                    glyphImage.Save($"/dev/shm/work/page_step_{index}.png");
            }

            if (index == providerProperties.Count - 1)
            {
                glyphImage.Save($"page_{page}.png");
            }
        }
    }

    /// <summary>
    /// 更新point
    /// </summary>
    /// <param name="targetPoint"></param>
    /// <param name="index"></param>
    /// <param name="maxValue"></param>
    /// <param name="imageWidth"></param>
    /// <param name="imageHeight"></param>
    /// <param name="glyphConfig"></param>
    /// <param name="glyphImage"></param>
    /// <param name="pageNumber"></param>
    private void updatePoint(ref Point targetPoint,
        int index,
        int maxValue,
        int imageWidth,
        int imageHeight,
        GlyphGenConfig glyphConfig,
        ref Image<Rgba32> glyphImage,
        ref int pageNumber)
    {
        //更新point

        targetPoint.X = (index % 16) * 16 + (glyphConfig.HorizonalSpacing * (index % 16));

        //换行
        if (index % 16 == 0 && index > 0)
        {
            targetPoint.Y += 16 + glyphConfig.VerticalSpacing;
            targetPoint.X = 0;
        }

        //换页
        if (index % 256 == 0 && index > 0)
        {
            glyphImage.Save($"page_{pageNumber}.png");
            glyphImage = new Image<Rgba32>(imageWidth, imageHeight);
            pageNumber++;
            targetPoint.X = targetPoint.Y = 0;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="source">要绘制的Image</param>
    /// <param name="target">被绘制的Image</param>
    /// <param name="position">位置</param>
    /// <param name="glyphConfig">配置</param>
    private void drawImage(Image source, Image target, Point position, GlyphGenConfig glyphConfig)
    {
        //缩放
        source.Mutate(x =>
            x.Resize(new Size(glyphConfig.TileSize)));

        //绘制到图片上
        target.Mutate(x =>
        {
            x.DrawImage(source, position, 1);
        });
    }
}