using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Game;
using Game.Modding;
using Game.SceneFlow;
using System;

namespace LaClock
{
    public class Mod : IMod
    {
        internal const string ID = nameof(LaClock);

        public static ILog log = LogManager.GetLogger($"{nameof(LaClock)}").SetShowsErrorsInUI(false);
        internal static ModSettings m_Setting;

        public void OnLoad(UpdateSystem updateSystem)
        {
            log.Info(nameof(OnLoad));

            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
                log.Info($"Current mod asset at {asset.path}");

            m_Setting = new ModSettings(this);
            m_Setting.RegisterInOptionsUI();
            GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(m_Setting));


            AssetDatabase.global.LoadSettings(nameof(LaClock), m_Setting, new ModSettings(this));

            updateSystem.UpdateAt<UISystem>(SystemUpdatePhase.UIUpdate);
        }

        public void OnDispose()
        {
            log.Info(nameof(OnDispose));
            if (m_Setting != null)
            {
                m_Setting.UnregisterInOptionsUI();
                m_Setting = null;
            }
        }


        // Check out the following references
        // https://learn.microsoft.com/en-us/dotnet/api/system.datetime.now?view=net-9.0#system-datetime-now

    }
}
