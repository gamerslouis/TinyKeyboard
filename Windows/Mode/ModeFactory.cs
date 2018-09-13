using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyKeyboard.Handler;
using TinyKeyboard.SettingForm;

namespace TinyKeyboard
{
    class ModeFactory
    {
        public static Mode[] Modes;

        public static Mode nullmode;

        public static void AddMode<THandler>(string name, string optionInfo, SettingForm.ISettingForm settingForm)
            where THandler : Handler.IHandler, new()
        {
            var mode = new Mode();
            mode.Name = name;
            mode.OptionInfo = optionInfo;
            mode.SettingForm = settingForm;
            mode.CreateHanlder = mode.CreateHanlderT<THandler>;

            Array.Resize(ref Modes, Modes.Length + 1);
            Modes[Modes.Length - 1] = mode;
        }

        static ModeFactory()
        {
            AddMode<NullMode>("nullmode", "未啟用", new NullModeSettingForm());
            AddMode<KeySimulatorMode>("keysimulatormode", "鍵盤模擬", new KeySimulatorModeForm());
        }

        public class NoModeFoundException : Exception { }

        public static Mode Get(string name)
        {
            foreach (var mode in Modes)
            {
                if (mode.Name == name) return mode;
            }
            throw new NoModeFoundException();
        }
    }
}
