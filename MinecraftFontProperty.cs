using Newtonsoft.Json;

namespace FontJsonGenerator;

[Serializable]
public class MinecraftFontProperty
{
    [JsonProperty("providers")]
    public List<MinecraftFontProviderProperty> Providers { get; set; } = new List<MinecraftFontProviderProperty>();

    public override string ToString() => JsonConvert.SerializeObject(this);
}