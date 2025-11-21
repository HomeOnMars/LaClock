using System.Collections.Generic;
using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using Game.UI;
using Game.UI.Widgets;
using System;

namespace LaClock
{
    [FileLocation(nameof(LaClock))]
    [SettingsUIGroupOrder(kFormatGroup, kFrictionGroup)]
    [SettingsUIShowGroupName(kFormatGroup, kFrictionGroup)]
    public class ModSettings : ModSetting
    {
        public const string kSection = "Main";

        public const string kHiddenGroup = "(Hidden)";
        public const string kFormatGroup = "Formatting";
        public const string kFrictionGroup = "Friction";

        public static readonly DateTime kExampleDateTime = new DateTime(2025, 9, 8, 21, 3, 4);


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
                switch (value)
                {
                    case ClockFormatEnum.Custom:
                        break;
                    case ClockFormatEnum.St:
                        ClockFormatString = "t";
                        break;
                    case ClockFormatEnum.Sts:
                        ClockFormatString = "T";
                        break;
                    case ClockFormatEnum.Sg:
                        ClockFormatString = "g";
                        break;
                    case ClockFormatEnum.Sgs:
                        ClockFormatString = "G";
                        break;
                    case ClockFormatEnum.HoM1:
                        ClockFormatString = "**HH:mm** | ddd dd MMM";
                        break;
                    case ClockFormatEnum.HoM1s:
                        ClockFormatString = "**HH:mm:ss** | ddd dd MMM";
                        break;
                    default:
                        ClockFormatString = "Error: Unexpected `ClockFormatChoice` input. Should not have happened.";
                        break;
                }
            }
        }

        public bool ClockFormatChoiceIsCustom() => ClockFormatChoice == ClockFormatEnum.Custom;

        [SettingsUITextInput]
        [SettingsUISection(kSection, kFormatGroup)]
        [SettingsUIDisableByCondition(typeof(ModSettings), nameof(ClockFormatChoiceIsCustom), invert: true)]
        public string ClockFormatString { get; set; } = "t";

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
            HoM1,
            HoM1s,
        }

        public DropdownItem<int>[] GetBlinkPerMinDropdownItems()
        {
            var items = new List<DropdownItem<int>>
            {
                new() { value =   0, displayName = "Never" },
                new() { value = 120, displayName = "2 hours" },
                new() { value =  60, displayName = "hour" },
                new() { value =  30, displayName = "30 minutes" },
                new() { value =   1, displayName = "minute :-p" },
                new() { value =  -1, displayName = "Always??" },
            };
            return items.ToArray();
        }

    }

}
