using Colossal;
using Colossal.Localization;
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

        public static void UpdateLocaleDictionaryEntries(Dictionary<string, string> localeEntries, ModSettings setting)
        {
            foreach (var entry in ModSettings.kClockFormatEnumToString)
            {
                localeEntries[setting.GetEnumValueLocaleID(entry.Key)] = ModSettings.GetTimeString(
                    ModSettings.kExampleDateTime, entry.Value, setting.ClockCultureInfo);
            }
        }
        public static void UpdateLocaleDictionaryEntries(LocalizationDictionary localeEntries, ModSettings setting)
        {
            foreach (var entry in ModSettings.kClockFormatEnumToString)
            {
                localeEntries.Add(
                    setting.GetEnumValueLocaleID(entry.Key),
                    ModSettings.GetTimeString(
                        ModSettings.kExampleDateTime, entry.Value, setting.ClockCultureInfo));
            }
        }

        public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts)
        {
            var localeEntries = new Dictionary<string, string>
            {
                { m_Setting.GetSettingsLocaleID(), "La Clock" },
                { m_Setting.GetOptionTabLocaleID(ModSettings.kSection), "Main" },

                { m_Setting.GetOptionGroupLocaleID(ModSettings.kFormatGroup), "Formatting" },
                { m_Setting.GetOptionGroupLocaleID(ModSettings.kFrictionGroup), "Friction" },

                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.ClockFormatChoice)), "Clock formatting" },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.ClockFormatChoice)), "Pick a format for the clock." },

                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.ClockFormatString)), "Clock formatting (Custom...)" },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.ClockFormatString)), @$"Display format of the clock.

Examples:

- **HH:mm**     | like <23:45>
- **hh:mm tt**  | like <11:45 PM>
- **\*\*HH:mm\*\* | ddd dd MMM**  | like <**23:45** \| Sat 01 Nov>
- **yyyy-MM-dd hh:mm:ss tt**    | like <2025-09-01 11:45:02 PM>
- For more information, search online for "".NET Custom date and time format strings""

Note: You can also surround parts with double asterisks to make them **\*\*bold\*\***.
" },
                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.ClockCultureChoice)), "Datetime format localization" },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.ClockCultureChoice)), "The localization style choice for the clock's date and time display." },

                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.ClockSizeMultiplier)), "Clock Width" },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.ClockSizeMultiplier)), "Change the multipler for the clock's width (applies in game only, editor not affected)" },

                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.EnableBlink)), "Enable blink" },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.EnableBlink)), "Enable blinking when below conditions are met." },

                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.BlinkPerMin)), "Blink every..." },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.BlinkPerMin)), "How often to make the clock blink?" },

                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.BlinkDurationSec)), "Blink duration (in seconds)" },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.BlinkDurationSec)), "How many seconds to blink?" },

                { m_Setting.GetEnumValueLocaleID(ModSettings.ClockCultureEnum.FollowSystem), "Follow System" },
                { m_Setting.GetEnumValueLocaleID(ModSettings.ClockCultureEnum.FollowGame), "Follow Game" },

                { m_Setting.GetEnumValueLocaleID(ModSettings.ClockFormatEnum.Custom), "Custom..." },
            };

            UpdateLocaleDictionaryEntries(localeEntries, m_Setting);

            Mod.log.Info($"{nameof(localeEntries)} updated.");

            return localeEntries;
        }

        public void Unload()
        {

        }
    }

}
