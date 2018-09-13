using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyKeyboard
{
    public class Mode
    {
        public delegate Handler.IHandler CreateHanlderDelegate(string set);
        public Handler.IHandler CreateHanlderT<THandler>(string set) where THandler : Handler.IHandler, new()
        {
            Handler.IHandler h = new THandler();
            h.Set(set);
            return h;
        }

        public CreateHanlderDelegate CreateHanlder;
        public SettingForm.ISettingForm SettingForm;
        public string OptionInfo;
        public string Name;
    }
}
