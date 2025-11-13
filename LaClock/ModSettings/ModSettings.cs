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

        public override void SetDefaults()
        {
            throw new System.NotImplementedException();
        }

    }

}
