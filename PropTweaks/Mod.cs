using GDWeave;
using PropTweaks.Mods;
using Serilog;

namespace PropTweaks;

public class Mod : IMod {
	public static Mod instance;
	public ILogger logger;
	
	public Config Config;

	public Mod(IModInterface modInterface)
	{
		instance = this;
		logger = modInterface.Logger;
		this.Config = modInterface.ReadConfig<Config>();
		modInterface.Logger.Information("PropTweaks Initialized");
		modInterface.RegisterScriptMod(new PlayerPatcher());
	}

	public void Dispose() {
		// Cleanup anything you do here
	}
}