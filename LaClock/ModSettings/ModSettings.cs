using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.SceneFlow;
using Game.Settings;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace LaClock
{
    [FileLocation(nameof(LaClock))]
    [SettingsUIGroupOrder(kFormatGroup, kFrictionGroup)]
    [SettingsUIShowGroupName(kFormatGroup, kFrictionGroup)]
    public class ModSettings : ModSetting
    {
        public const string kSection = "Main";

        //public const string kHiddenGroup = "(Hidden)";
        public const string kFormatGroup = "Formatting";
        public const string kFrictionGroup = "Friction";

        public static readonly DateTime kExampleDateTime = new DateTime(2025, 9, 8, 21, 3, 4, DateTimeKind.Local);
        public static readonly Dictionary<ClockFormatEnum, string> kClockFormatEnumToString = new()
        {   // https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings
            { ClockFormatEnum.St,  "t"},
            { ClockFormatEnum.Sts, "T"},
            { ClockFormatEnum.Sg,  "g"},
            { ClockFormatEnum.Sgs, "G"},
            { ClockFormatEnum.Sf,  "f"},
            { ClockFormatEnum.Sfs, "F"},
            { ClockFormatEnum.Su,  "u"},
            { ClockFormatEnum.ISO8601syk, "yyyy-MM-ddTHH:mm:ssK"},
            { ClockFormatEnum.HoM0, "ddd | **HH:mm**"},
            { ClockFormatEnum.HoM0s, "ddd | **HH:mm:ss**"},
            { ClockFormatEnum.HoM0y, "ddd | **HH:mm** | yyyy-MM-dd"},
            { ClockFormatEnum.HoM0sy, "ddd | **HH:mm:ss** | yyyy-MM-dd"},
            { ClockFormatEnum.HoM1, "**HH:mm** | ddd dd MMM"},
            { ClockFormatEnum.HoM1s, "**HH:mm:ss** | ddd dd MMM"},
            { ClockFormatEnum.HoM1y, "**HH:mm** | ddd dd MMM yyyy"},
            { ClockFormatEnum.HoM1sy, "**HH:mm:ss** | ddd dd MMM yyyy"},
        };


        public ModSettings(IMod mod) : base(mod)
        {

        }

        private ClockFormatEnum _clockFormatChoice = ClockFormatEnum.Custom;
        [SettingsUITextInput]
        [SettingsUISection(kSection, kFormatGroup)]
        public ClockFormatEnum ClockFormatChoice
        {
            get { return _clockFormatChoice; }
            set
            {
                _clockFormatChoice = value;
                UpdateClockFormatSettings();
            }
        }

        public bool ClockFormatChoiceIsCustom() => ClockFormatChoice == ClockFormatEnum.Custom;

        private string _clockFormatString = "t";
        [SettingsUITextInput]
        [SettingsUISection(kSection, kFormatGroup)]
        [SettingsUIHideByCondition(typeof(ModSettings), nameof(ClockFormatChoiceIsCustom), invert: true)]
        public string ClockFormatString
        {
            get { return _clockFormatString; }
            set {
                _clockFormatString = value;
                UpdateClockFormatSettings();
            }
        }

        private string _clockFormatPreview = "";
        [SettingsUISection(kSection, kFormatGroup)]
        [SettingsUIHideByCondition(typeof(ModSettings), nameof(ClockFormatChoiceIsCustom), invert: true)]
        public string ClockFormatPreview => _clockFormatPreview;

        private ClockCultureEnum _clockCultureChoice = ClockCultureEnum.FollowSystem;
        [SettingsUITextInput]
        [SettingsUISection(kSection, kFormatGroup)]
        public ClockCultureEnum ClockCultureChoice
        {
            get { return _clockCultureChoice; }
            set
            {
                _clockCultureChoice = value;
                UpdateClockFormatSettings();
                UpdateActiveLocaleEntries();
            }
        }


        private float _clockSizeMultiplier { get; set; } = 1.2f;
        [SettingsUISlider(min = 50f, max = 200f, step = 5f, scalarMultiplier = 100f, unit = "percentage")]
        [SettingsUISection(kSection, kFormatGroup)]
        public float ClockSizeMultiplier
        {
            get { return _clockSizeMultiplier; }
            set
            {
                _clockSizeMultiplier = value;
                UpdateClockFormatSettings();
            }
        }

        public string ClockWidth { get; private set; } = "240rem";

        public CultureInfo ClockCultureInfo { get; private set; }


        // must be initialized the same as _clockFormatString
        public string ClockFormatStringActual { get; private set; } = "t";
        public void UpdateClockFormatSettings()
        {
            if (_clockFormatChoice == ClockFormatEnum.Custom)
            {
                ClockFormatStringActual = _clockFormatString;
            }
            else if (kClockFormatEnumToString.TryGetValue(_clockFormatChoice, out var tmpClockFormatString))
            {
                ClockFormatStringActual = tmpClockFormatString;
            }
            else
            {
                ClockFormatStringActual = $"Error: Unexpected ClockFormatChoice.";
                log.Error($"Error: Unexpected `ClockFormatChoice` input '{_clockFormatChoice}'. Should not have happened.");
            }

            switch (_clockCultureChoice)
            {
                case ClockCultureEnum.FollowSystem:
                    ClockCultureInfo = CultureInfo.CurrentCulture;
                    break;
                case ClockCultureEnum.FollowGame:
                    try
                    {
                        ClockCultureInfo = new CultureInfo(GameManager.instance.localizationManager.activeLocaleId);
                    }
                    catch (CultureNotFoundException)
                    {
                        try
                        {
                            ClockCultureInfo = new CultureInfo(GameManager.instance.localizationManager.fallbackLocaleId);
                        }
                        catch (CultureNotFoundException)
                        {
                            ClockCultureInfo = new CultureInfo("en-US");
                        }
                    }
                    break;
                default:
                    ClockCultureInfo = CultureInfo.CurrentCulture;
                    Mod.log.Error($"{nameof(UpdateClockFormatSettings)}: Unknown ClockCultureChoice. Should not have happened.");
                    break;
            }

            _clockFormatPreview = GetTimeString(kExampleDateTime);
            // ClockSizeRem minimum 10rem
            int ClockSizeRem = Math.Max((int)((ClockFormatPreview.Length + 4f) * 8 * ClockSizeMultiplier), 10);
            ClockWidth = $"{ClockSizeRem:D}rem";

            Mod.log.Info($@"{nameof(UpdateClockFormatSettings)}():
                {nameof(ClockFormatStringActual)}: {ClockFormatStringActual}
                {nameof(ClockFormatPreview)}: {ClockFormatPreview}
                {nameof(ClockWidth)}: {ClockWidth}
                {nameof(CultureInfo)}.{nameof(CultureInfo.CurrentCulture)}: {CultureInfo.CurrentCulture.Name}
                {nameof(GameManager.instance.localizationManager.activeLocaleId)}: {GameManager.instance.localizationManager.activeLocaleId}
                {nameof(GameManager.instance.localizationManager.fallbackLocaleId)}: {GameManager.instance.localizationManager.fallbackLocaleId}
                {nameof(ClockCultureInfo)}: {ClockCultureInfo.Name}");
        }

        public void UpdateActiveLocaleEntries() => LocaleEN.UpdateLocaleDictionaryEntries(GameManager.instance.localizationManager.activeDictionary, this);


        public static string GetTimeString(DateTime time, string format, CultureInfo culture)
        {
            // See <https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings>
            // and <https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings>
            try
            {
                return time.ToString(format, culture);
            }
            catch (FormatException)
            {
                return "Invalid Formatting";
            }
        }
        public string GetTimeString(DateTime time) => GetTimeString(time, ClockFormatStringActual, ClockCultureInfo);


        [SettingsUISection(kSection, kFrictionGroup)]
        public bool EnableBlink { get; set; } = false;

        [SettingsUIDropdown(typeof(ModSettings), nameof(GetBlinkPerMinDropdownItems))]
        [SettingsUISection(kSection, kFrictionGroup)]
        [SettingsUIDisableByCondition(typeof(ModSettings), nameof(EnableBlink), invert: true)]
        public int BlinkPerMin { get; set; } = 60;


        [SettingsUISlider(min = 1, max = 60, step = 1)]
        [SettingsUISection(kSection, kFrictionGroup)]
        [SettingsUIDisableByCondition(typeof(ModSettings), nameof(EnableBlink), invert: true)]
        public int BlinkDurationSec { get; set; } = 12;



        public override void SetDefaults()
        {
            ClockFormatChoice = ClockFormatEnum.Custom;
            ClockFormatString = "t";
            ClockCultureChoice = ClockCultureEnum.FollowSystem;
            ClockSizeMultiplier = 1.2f;  // set to 120% to accommodate some languages having wide characters
            EnableBlink = false;
            BlinkPerMin = 60;
            BlinkDurationSec = 12;
            //throw new System.NotImplementedException();
        }



        public enum ClockFormatEnum
        {
            Custom,
            St,
            Sts,
            Sg,
            Sgs,
            Sf,
            Sfs,
            Su,
            ISO8601syk,
            HoM0,
            HoM0s,
            HoM0y,
            HoM0sy,
            HoM1,
            HoM1s,
            HoM1y,
            HoM1sy,
        }

        public enum ClockCultureEnum
        {
            FollowSystem,
            FollowGame,
        }

        public DropdownItem<int>[] GetBlinkPerMinDropdownItems()
        {
            var items = new List<DropdownItem<int>>
            {
                new() { value =   0, displayName = "Never" },
                new() { value = 120, displayName = "2 hours" },
                new() { value =  60, displayName = "1 hour" },
                new() { value =  30, displayName = "30 minutes" },
                new() { value =   1, displayName = "1 minute :-p" },
                new() { value =  -1, displayName = "Always??" },
            };
            return items.ToArray();
        }

    }

}
