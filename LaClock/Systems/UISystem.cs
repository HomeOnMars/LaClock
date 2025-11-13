using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Colossal.UI.Binding;
using Game;
using Game.UI;
using Game.Modding;
using Game.SceneFlow;
using System;

namespace LaClock
{
    public partial class UISystem: UISystemBase
    {
        // See <https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings>
        // and <https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings>
        protected static string CurrentSystemTime()
        {
            try
            {
                return DateTime.Now.ToString(Mod.m_Setting.ClockFormatString);
            }
            catch (FormatException)
            {
                return "Invalid Formatting";
            }
        }

        protected static bool DoBlink()
        {
            if (!Mod.m_Setting.EnableBlink)
            {
                return false;
            }

            uint blinkPerMin;
            switch (Mod.m_Setting.BlinkCondition)
            {
                case ModSettings.FrictionTriggerEnum.Never:
                    return false;
                case ModSettings.FrictionTriggerEnum.Always:
                    return true;
                case ModSettings.FrictionTriggerEnum.Per60min:
                    blinkPerMin = 60;
                    break;
                case ModSettings.FrictionTriggerEnum.Per30min:
                    blinkPerMin = 30;
                    break;
                case ModSettings.FrictionTriggerEnum.Per1min:
                    blinkPerMin = 1;
                    break;

                default:
                    Mod.log.Error($"Unknown BlinkCondition: {Mod.m_Setting.BlinkCondition}. Please Contact Mod Creator.");
                    return true;    // prompt user to investigate
            }

            var now = DateTime.Now;
            // Note: Blink duration (12s) must be smaller than a minute
            if ((now.Hour * 60 + now.Minute) % blinkPerMin != 0 || now.Second >= 12)
            {
                return false; 
            }
            else
            {
                //Mod.log.Info($"currentMin: {now.Hour * 60 + now.Minute}");
                return true;
            }

        }

        protected override void OnCreate()
        {
            base.OnCreate();

            AddUpdateBinding(new GetterValueBinding<string>(Mod.ID, "CurrentSystemTime", CurrentSystemTime));
            AddUpdateBinding(new GetterValueBinding<bool>(Mod.ID, "DoBlink", DoBlink));
            Mod.log.Info($"{nameof(CurrentSystemTime)}: {CurrentSystemTime()}");
        }



    }
}
