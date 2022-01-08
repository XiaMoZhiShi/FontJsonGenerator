using FontJsonGenerator.Extensions;
using FontJsonGenerator.GenerateConfig;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
// ReSharper disable AccessToDisposedClosure

namespace FontJsonGenerator.Generator;

public class GlyphGenerator
{
    public void Generate(GlyphGenConfig glyphConfig, Dictionary<int, string> map)
    {
        var sideLength = glyphConfig.TileSize * 16;
        var glyphImage = new Image<Rgba32>(sideLength, sideLength);

        string fallbackPath = glyphConfig.BasePath + glyphConfig.FallbackTexturePath?.ToResourcePath() + ".png";
        bool fallbackAvaliable = File.Exists(fallbackPath);

        Point point = new Point(0, 0);

        int index = 0;
        int page = 0;

        foreach (var path in map.Values)
        {
            //获取纹理
            string filePath = glyphConfig.BasePath + path.ToResourcePath();

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
                
                Console.WriteLine($"{index} / {map.Count}: draw {path} at {point}");
                drawImage(targetImage, glyphImage, point, glyphConfig);

                if (glyphConfig.SaveSteps)
                    glyphImage.Save($"page_step_{index}.png");
            }

            //移动位置
            index++;
            point.X = (index % 16) * 16;

            //换行
            if (index % 16 == 0 && index > 0)
            {
                point.Y += 16;
                point.X = 0;
            }

            //自动换页
            if (index % 256 == 0 && index > 0 || index == map.Values.Count)
            {
                glyphImage.Save($"page_{page}.png");
                glyphImage = new Image<Rgba32>(sideLength, sideLength);
                page++;
                point.X = point.Y = 0;
            }
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
            x.Resize(glyphConfig.TileSize, glyphConfig.TileSize));

        //绘制到图片上
        target.Mutate(x =>
        {
            x.DrawImage(source, position, 1);
        });
    }
}