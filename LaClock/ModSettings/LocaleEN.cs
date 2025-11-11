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

- **t** | (Default) Your local cultural time format (short ver, without seconds)
- **T** | Your local time format (long ver, with seconds)
- **f** | Your local time format (short full ver, without seconds but with dates)
- **HH:mm** | like 23:45
- **hh:mm tt**  | like 11:45 PM
- **HH:mm | ddd dd MMM**    | like 23:45 | Sat 01 Nov
- **yyyy-MM-dd hh:mm tt**   | like 2025-09-01 11:45 PM

For more information, google ""C# Custom date and time format strings"", or see <https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings>.
" },

            };
        }

        public void Unload()
        {

        }
    }

}
