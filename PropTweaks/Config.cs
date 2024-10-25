using System.Text.Json.Serialization;

namespace PropTweaks;

public class Config {
	[JsonInclude] public bool SomeSetting = true;
}