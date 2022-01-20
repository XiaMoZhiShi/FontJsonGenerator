using Newtonsoft.Json;

namespace FontJsonGenerator.Informations;

[Serializable]
public class PointInfo
{
    [JsonProperty("x")] public int X;
    [JsonProperty("y")] public int Y;
}