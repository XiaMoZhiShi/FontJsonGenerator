using FontJsonGenerator.Informations;
using Newtonsoft.Json;

namespace FontJsonGenerator.GenerateConfig;

[Serializable]
public class FontGenConfig
{
    /// <summary>
    /// 字体名称
    /// 暂时未使用
    /// </summary>
    [JsonProperty("name")]
    public string FontName { get; set; } = "???";

    /// <summary>
    /// 输出目标，可以是 stdout(到终端), file(到文件), null(无)
    /// </summary>
    [JsonProperty("target")]
    public string OptoutTarget { get; set; } = "null";

    [JsonProperty("temp")]
    public string TempFileDirectory { get; set; } = "/dev/shm";

    /// <summary>
    /// 起始Unicode位置（string）
    /// </summary>
    [JsonProperty("starts")]
    public string StartsProperty { get; set; } = "-1";

    /// <summary>
    /// 起始Unicode位置（int）
    /// </summary>
    [JsonIgnore]
    public int StartsAt
    {
        get => Convert.ToInt32(StartsProperty, 16);
        set => StartsProperty = value.ToString();
    }

    /// <summary>
    /// 要生成的范围
    /// </summary>
    [JsonProperty("ranges")]
    public RangeInfo[] Ranges { get; set; } = Array.Empty<RangeInfo>();
    
    [JsonProperty("glyph")]
    public GlyphGenConfig? GlyphGenConfig { get; set; }

    public override string ToString() => JsonConvert.SerializeObject(this);
}