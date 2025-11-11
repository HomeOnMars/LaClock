using System.Collections.Generic;
using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using Game.UI;
using Game.UI.Widgets;

namespace LaClock
{
    [FileLocation(nameof(LaClock))]
    [SettingsUIGroupOrder(kFormatGroup)]
    [SettingsUIShowGroupName(kFormatGroup)]
    public class ModSettings : ModSetting
    {
        public const string kSection = "Main";

        public const string kFormatGroup = "Datetime Formatting";

        public ModSettings(IMod mod) : base(mod)
        {

        }

        [SettingsUITextInput]
        [SettingsUISection(kSection, kFormatGroup)]
        public string ClockFormatString { get; set; } = "t";

        public override void SetDefaults()
        {
            throw new System.NotImplementedException();
        }

    }

}
