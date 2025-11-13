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
                { m_Setting.GetOptionGroupLocaleID(ModSettings.kFrictionGroup), "Friction" },

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

Note: You can also surround parts with double asterisks to make them **\*\*bold\*\***.
" },

                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.EnableBlink)), "Enable blink" },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.EnableBlink)), "Enable blinking when below conditions are met." },

                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.BlinkCondition)), "Blink when:" },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.BlinkCondition)), "Make the clock blink when the given conditions are met." },

                { m_Setting.GetEnumValueLocaleID(ModSettings.FrictionTriggerEnum.Never), "Never" },
                { m_Setting.GetEnumValueLocaleID(ModSettings.FrictionTriggerEnum.Per60min), "Every hour" },
                { m_Setting.GetEnumValueLocaleID(ModSettings.FrictionTriggerEnum.Per30min), "Every 30 minutes" },
                { m_Setting.GetEnumValueLocaleID(ModSettings.FrictionTriggerEnum.Per1min), "Every minute :-p" },
                { m_Setting.GetEnumValueLocaleID(ModSettings.FrictionTriggerEnum.Always), "Always??" },

            };
        }

        public void Unload()
        {

        }
    }

}
