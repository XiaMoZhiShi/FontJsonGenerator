using Newtonsoft.Json;

namespace FontJsonGenerator;

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

    public override string ToString() => JsonConvert.SerializeObject(this);
}