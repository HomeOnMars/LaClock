using System.Collections.Generic;
using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using Game.UI;
using Game.UI.Widgets;

namespace LaClock
{
    [FileLocation(nameof(LaClock))]
    [SettingsUIGroupOrder(kFormatGroup, kFrictionGroup)]
    [SettingsUIShowGroupName(kFormatGroup, kFrictionGroup)]
    public class ModSettings : ModSetting
    {
        public const string kSection = "Main";

        public const string kFormatGroup = "Formatting";
        public const string kFrictionGroup = "Friction";

        public ModSettings(IMod mod) : base(mod)
        {

        }

        //[SettingsUISection(kSection, kFormatGroup)]
        //public bool ComingSoon { get; set; } = true;

        [SettingsUITextInput]
        [SettingsUISection(kSection, kFormatGroup)]
        public string ClockFormatString { get; set; } = "t";

        [SettingsUISection(kSection, kFrictionGroup)]
        public bool EnableBlink { get; set; } = false;

        [SettingsUISection(kSection, kFrictionGroup)]
        [SettingsUIDisableByCondition(typeof(ModSettings), nameof(EnableBlink), invert: true)]
        public FrictionTriggerEnum BlinkCondition { get; set; } = FrictionTriggerEnum.Per60min;




        public override void SetDefaults()
        {
            throw new System.NotImplementedException();
        }


        public enum FrictionTriggerEnum
        {
            Never,
            Per60min,
            Per30min,
            Per1min,
            Always,
        }
    }

}
