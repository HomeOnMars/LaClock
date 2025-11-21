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
            if (!Mod.m_Setting.EnableBlink || Mod.m_Setting.BlinkPerMin == 0) { return false; }
            if (Mod.m_Setting.BlinkPerMin < 0) { return true; }

            var now = DateTime.Now;
            // Note: Blink duration (15s) must be smaller than a minute
            if ((now.Hour * 60 + now.Minute) % Mod.m_Setting.BlinkPerMin != 0 || now.Second >= Mod.m_Setting.BlinkDurationSec)
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
