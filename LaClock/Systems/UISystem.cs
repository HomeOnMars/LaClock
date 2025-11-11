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

        protected override void OnCreate()
        {
            base.OnCreate();

            AddUpdateBinding(new GetterValueBinding<string>(Mod.ID, "CurrentSystemTime", CurrentSystemTime));
            Mod.log.Info(CurrentSystemTime());
        }



    }
}
