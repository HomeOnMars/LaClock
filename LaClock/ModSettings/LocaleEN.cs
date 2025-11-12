using Colossal;
using Game.Settings;
using Game.UI.Widgets;
using System.Collections.Generic;

namespace LaClock
{
    public class LocaleEN : IDictionarySource
    {
        private readonly ModSettings m_Setting;
        public LocaleEN(ModSettings setting)
        {
            m_Setting = setting;
        }
        public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                { m_Setting.GetSettingsLocaleID(), "La Clock" },
                { m_Setting.GetOptionTabLocaleID(ModSettings.kSection), "Main" },

                { m_Setting.GetOptionGroupLocaleID(ModSettings.kFormatGroup), "Formatting" },

                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.ClockFormatString)), "Clock Formatting" },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.ClockFormatString)), @$"Display Format of the Clock.

Examples:

- **t** | (Default) Your local time format (short ver, without seconds)
- **T** | Your local time format (long ver, with seconds)
- **g** | Your local time format (short general ver, without seconds but with dates)
- **HH:mm**     | like <23:45>
- **hh:mm tt**  | like <11:45 PM>
- **\*\*HH:mm\*\* | ddd dd MMM**  | like <**23:45** \| Sat 01 Nov>
- **yyyy-MM-dd hh:mm:ss tt**    | like <2025-09-01 11:45:02 PM>
- For more information, search online for ""C# date and time format strings""

(Advanced) You can also:

- Surround parts with double asterisks to make them **\*\*bold\*\***
- \#\#\#\# Add 1 to 4 hashtags at the very beginning to enlarge the text
" },

            };
        }

        public void Unload()
        {

        }
    }

}
