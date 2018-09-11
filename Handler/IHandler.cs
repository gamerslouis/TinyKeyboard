using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyKeyboard.Handler
{
    public interface IHandler
    {
        void KeyPress();
        void KeyUp();
        void Set(string set);
    }
}
