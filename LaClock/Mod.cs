using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Colossal.Serialization.Entities;
using Game;
using Game.Modding;
using Game.SceneFlow;

namespace LaClock
{
    public class Mod : IMod
    {
        internal const string ID = nameof(LaClock);

        public static ILog log = LogManager.GetLogger($"{nameof(LaClock)}").SetShowsErrorsInUI(true);
        internal static ModSettings m_Setting;

        public void OnLoad(UpdateSystem updateSystem)
        {
            log.Info(nameof(OnLoad));

            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
                log.Info($"Current mod asset at {asset.path}");

            m_Setting = new ModSettings(this);
            m_Setting.RegisterInOptionsUI();
            GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(m_Setting));
            GameManager.instance.localizationManager.onActiveDictionaryChanged += UpdateClockLocale;
            // update again after load because the CultureInfo.CurrentCulture seems to be empty when initially loading settings?
            GameManager.instance.onGameLoadingComplete += UpdateClockLocale;

            AssetDatabase.global.LoadSettings(nameof(LaClock), m_Setting, new ModSettings(this));

            updateSystem.UpdateAt<UISystem>(SystemUpdatePhase.UIUpdate);

            UpdateClockLocale();
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

        private void UpdateClockLocale()
        {
            m_Setting.UpdateClockFormatSettings();
            m_Setting.UpdateActiveLocaleEntries();
        }

        private void UpdateClockLocale(Purpose purpose, GameMode mode)
        {
            Mod.log.Info($@"{nameof(UpdateClockLocale)}(): {nameof(purpose)} {purpose.ToString()}, {nameof(mode)} {mode.ToString()}.");
            UpdateClockLocale();
        }

    }
}
