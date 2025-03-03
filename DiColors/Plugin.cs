using IPA;
using HarmonyLib;
using IPA.Loader;
using SiraUtil.Zenject;
using System.Reflection;
using IPA.Config.Stores;
using DiColors.Installers;
using Conf = IPA.Config.Config;
using IPALogger = IPA.Logging.Logger;

namespace DiColors
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        internal static IPALogger Log { get; private set; }
        private readonly Harmony _harmony;

        [Init]
        public Plugin(Conf conf, IPALogger logger, Zenjector zenjector, PluginMetadata metadata)
        {
            Log = logger;
            Config config = conf.Generated<Config>();
            _harmony = new Harmony("dev.auros.dicolors");
			zenjector.Install<DiCInstaller>(Location.App, config, metadata.HVersion);
			zenjector.Install<DiCMenuInstaller>(Location.Menu);
            //zenjector.OnGame<DiCGameInstaller>(false);
        }

        [OnEnable]
        public void OnEnable()
        {
            _harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        [OnDisable]
        public void OnDisable()
        {
            _harmony.UnpatchSelf();
        }
    }
}