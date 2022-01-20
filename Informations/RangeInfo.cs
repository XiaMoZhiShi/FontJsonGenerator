using System.Diagnostics.CodeAnalysis;
using FontJsonGenerator.GenerateConfig;
using Newtonsoft.Json;

namespace FontJsonGenerator.Informations;

public class RangeInfo
{
    #region 标准

    /// <summary>
    /// 前缀，完整文件名为“[命名空间]:[前缀][值][后缀].png”
    /// </summary>
    [JsonProperty("prefix")]
    public string Prefix { get; set; } = string.Empty;

    /// <summary>
    /// 后缀，完整文件名为“[命名空间]:[前缀][值][后缀].png”
    /// </summary>
    [JsonProperty("suffix")]
    public string Suffix { get; set; } = string.Empty;

    /// <summary>
    /// 命名空间
    /// </summary>
    [JsonProperty("namespace")]
    public string NameSpace { get; set; } = "minecraft";

    /// <summary>
    /// 属性类型
    /// </summary>
    [JsonProperty("type")]
    public string RangeType { get; set; } = "normal";

    /// <summary>
    /// 值
    /// </summary>
    [JsonProperty("values")]
    public string[] Values { get; set; } = Array.Empty<string>();

    [JsonProperty("ascent")]
    public int Ascent;

    [JsonProperty("height")]
    public int Height;

    #endregion

    /// <summary>
    /// 指定要跳过多少字符，可用来保留空间或与其他功能搭配使用
    /// </summary>
    [JsonProperty("skips")]
    public int Skips;

    #region 原始数据

    /// <summary>
    /// Provider原始数据，程序不会处理这些数据
    /// </summary>
    [AllowNull]
    [JsonProperty("provider")]
    public MinecraftFontProviderProperty ProviderProperty { get; set; }

    #endregion

    #region tilemap

    [JsonProperty("tile")]
    public TileMapConfig TileMapConfig { get; set; }

    #endregion
}