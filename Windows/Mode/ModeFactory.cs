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
        public static Mode[] Modes = new Mode[0];

        public static void AddMode<THandler>(string name, string optionInfo, SettingForm.ISettingForm settingForm)
            where THandler : Handler.IHandler, new()
        {
            var mode = new Mode
            {
                Name = name,
                OptionInfo = optionInfo,
                SettingForm = settingForm
            };
            mode.CreateHanlder = Mode.CreateHanlderT<THandler>;
            
            Array.Resize(ref Modes, Modes.Length + 1);
            Modes[Modes.Length - 1] = mode;
        }

        static ModeFactory()
        {
            AddMode<NullModeHandler>("nullmode", "未啟用", new NullModeSettingForm());
            AddMode<KeySimulatorModeHandler>("keysimulatormode", "鍵盤模擬", new KeySimulatorModeForm());
        }

        public class NoModeFoundException : Exception { }

        static public Mode Get(string name)
        {
            foreach (var mode in Modes)
            {
                if (mode.Name == name) return mode;
            }
            throw new NoModeFoundException();
        }

        static public Mode GetByOptionInfo(string OptionInfo)
        {
            foreach (var mode in Modes)
            {
                if (mode.OptionInfo == OptionInfo) return mode;
            }
            throw new NoModeFoundException();
        }
    }
}
