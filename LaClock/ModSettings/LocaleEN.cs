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

                { m_Setting.GetOptionGroupLocaleID(ModSettings.kButtonGroup), "Buttons" },
                { m_Setting.GetOptionGroupLocaleID(ModSettings.kToggleGroup), "Toggle" },
                { m_Setting.GetOptionGroupLocaleID(ModSettings.kSliderGroup), "Sliders" },
                { m_Setting.GetOptionGroupLocaleID(ModSettings.kDropdownGroup), "Dropdowns" },

                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.Button)), "Button" },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.Button)), $"Simple single button. It should be bool property with only setter or use [{nameof(SettingsUIButtonAttribute)}] to make button from bool property with setter and getter" },

                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.ButtonWithConfirmation)), "Button with confirmation" },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.ButtonWithConfirmation)), $"Button can show confirmation message. Use [{nameof(SettingsUIConfirmationAttribute)}]" },
                { m_Setting.GetOptionWarningLocaleID(nameof(ModSettings.ButtonWithConfirmation)), "is it confirmation text which you want to show here?" },

                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.Toggle)), "Toggle" },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.Toggle)), $"Use bool property with setter and getter to get toggable option" },

                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.IntSlider)), "Int slider" },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.IntSlider)), $"Use int property with getter and setter and [{nameof(SettingsUISliderAttribute)}] to get int slider" },

                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.IntDropdown)), "Int dropdown" },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.IntDropdown)), $"Use int property with getter and setter and [{nameof(SettingsUIDropdownAttribute)}(typeof(SomeType), nameof(SomeMethod))] to get int dropdown: Method must be static or instance of your setting class with 0 parameters and returns {typeof(DropdownItem<int>).Name}" },

                { m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.EnumDropdown)), "Simple enum dropdown" },
                { m_Setting.GetOptionDescLocaleID(nameof(ModSettings.EnumDropdown)), $"Use any enum property with getter and setter to get enum dropdown" },

                { m_Setting.GetEnumValueLocaleID(ModSettings.SomeEnum.Value1), "Value 1" },
                { m_Setting.GetEnumValueLocaleID(ModSettings.SomeEnum.Value2), "Value 2" },
                { m_Setting.GetEnumValueLocaleID(ModSettings.SomeEnum.Value3), "Value 3" },

            };
        }

        public void Unload()
        {

        }
    }

}
