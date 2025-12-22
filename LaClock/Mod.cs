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

        private bool IsInitialized = false;

        public void OnLoad(UpdateSystem updateSystem)
        {
            log.Info(nameof(OnLoad));

            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
                log.Info($"Current mod asset at {asset.path}");

            IsInitialized = false;

            m_Setting = new ModSettings(this);
            m_Setting.RegisterInOptionsUI();
            GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(m_Setting));
            GameManager.instance.localizationManager.onActiveDictionaryChanged += UpdateClockLocaleIfInitialized;
            // update again after load because the CultureInfo.CurrentCulture seems to be empty when initially loading settings?
            GameManager.instance.onGameLoadingComplete += InitializeClockLocale;

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

        private void UpdateClockLocaleIfInitialized()
        {
            if (!IsInitialized) { return; }
            log.Info($@"{nameof(UpdateClockLocaleIfInitialized)}();");
            UpdateClockLocale();
        }

        private void InitializeClockLocale(Purpose purpose, GameMode mode)
        {
            // Note: the first function call at Purpose.Cleanup
            //          does not seem to have the correct CultureInfo.CurrentCulture initialized,
            //          need to run initialization at the second time onGameLoadingComplete was triggered
            if (IsInitialized || purpose == Purpose.Cleanup) { return; }
            log.Info($@"{nameof(InitializeClockLocale)}({nameof(purpose)} {purpose}, {nameof(mode)} {mode});");
            UpdateClockLocale();
            IsInitialized = true;
        }

        private void UpdateClockLocale()
        {
            m_Setting.UpdateClockFormatSettings();
            m_Setting.UpdateActiveLocaleEntries();
        }
    }
}
