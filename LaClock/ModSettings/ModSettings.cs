using System.Collections.Generic;
using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using Game.UI.Widgets;
using System;
using Game.Prefabs;

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
            { ClockFormatEnum.HoM0, "**HH:mm** | ddd"},
            { ClockFormatEnum.HoM0s, "**HH:mm:ss** | ddd"},
            { ClockFormatEnum.HoM0y, "**HH:mm** | ddd | yyyy-MM-dd"},
            { ClockFormatEnum.HoM0sy, "**HH:mm:ss** | ddd | yyyy-MM-dd"},
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
                UpdateClockFormatStringActual();
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
                UpdateClockFormatStringActual();
            }
        }


        private float _clockSizeMultiplier { get; set; } = 1f;
        [SettingsUISlider(min = 50f, max = 200f, step = 5f, scalarMultiplier = 100f, unit = "percentage")]
        [SettingsUISection(kSection, kFormatGroup)]
        public float ClockSizeMultiplier
        {
            get { return _clockSizeMultiplier; }
            set
            {
                _clockSizeMultiplier = value;
                UpdateClockFormatStringActual();
            }
        }

        public string ClockSize { get; private set; } = "240rem";


        // must be initialized the same as _clockFormatString
        public string ClockFormatStringActual { get; private set; } = "t";
        protected void UpdateClockFormatStringActual()
        {
            switch (_clockFormatChoice)
            {
                case ClockFormatEnum.Custom:
                    ClockFormatStringActual = _clockFormatString;
                    break;
                default:
                    if (kClockFormatEnumToString.TryGetValue(_clockFormatChoice, out var tmpClockFormatString))
                    {
                        ClockFormatStringActual = tmpClockFormatString;
                    }
                    else
                    {
                        ClockFormatStringActual = $"Error: Unexpected ClockFormatChoice.";
                        log.Error($"Error: Unexpected `ClockFormatChoice` input '{_clockFormatChoice}'. Should not have happened.");
                    }
                    break;
            }
            var ExampleDateTimeString = UISystem.GetTimeString(kExampleDateTime);
            // ClockSizeRem minimum 10rem
            int ClockSizeRem = Math.Max((int)((ExampleDateTimeString.Length+4f) * 8 * ClockSizeMultiplier), 10);
            ClockSize = $"{ClockSizeRem:D}rem";

            Mod.log.Info($@"{nameof(UpdateClockFormatStringActual)}:
                {nameof(ClockFormatStringActual)}: {ClockFormatStringActual}
                {nameof(ExampleDateTimeString)}: {ExampleDateTimeString}
                {nameof(ClockSize)}: {ClockSize}");

        }



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
            ClockSizeMultiplier = 1f;
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
