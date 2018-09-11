using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TinyKeyboard.Handler
{
    class ScriptMode : IHandler
    {
        public string ScriptPath;

        public ScriptMode()
        {
            ScriptPath = "";
        }

        void IHandler.KeyPress()
        {
            if (ScriptPath != "")
            {
                try
                {
                    Process.Start(ScriptPath);
                }
                catch(Exception e)
                {
                    throw e;
                }
            }
        }

        void IHandler.KeyUp()
        {
            return;
        }

        void IHandler.Set(string set)
        {
            ScriptPath = set;
        }

        class ScriptModeStartFailException : Exception { }
    }
}
