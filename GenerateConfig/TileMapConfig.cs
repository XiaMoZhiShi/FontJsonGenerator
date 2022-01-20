using FontJsonGenerator.Informations;
using Newtonsoft.Json;

namespace FontJsonGenerator.GenerateConfig;

[Serializable]
public class TileMapConfig
{
    /// <summary>
    /// 每一片有多大
    /// </summary>
    [JsonProperty("size")]
    public int Size = 16;

    /// <summary>
    /// 起始位置
    /// </summary>
    [JsonProperty("startsAt")]
    public PointInfo StartsAt;

    /// <summary>
    /// 结束位置
    /// </summary>
    [JsonProperty("endsAt")]
    public PointInfo EndsAt;

    /// <summary>
    /// 资源地址（命名空间）
    /// </summary>
    [JsonProperty("resource_location")]
    public string ResourceLocation { get; set; }
    
    /// <summary>
    /// 纹理位置
    /// </summary>
    [JsonProperty("texture_path")]
    public string TexturePath { get; set; }
    
    [JsonProperty("glyph_ignore")]
    public bool IgnoreForGlyphgenerate { get; set; }
}