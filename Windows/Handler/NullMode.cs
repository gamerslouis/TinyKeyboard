using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyKeyboard.Handler
{
    class NullMode : IHandler
    {
        void IHandler.KeyPress()
        {
            return;
        }

        void IHandler.KeyUp()
        {
            return;
        }

        void IHandler.Set(string set)
        {
            return;
        }
    }
}
