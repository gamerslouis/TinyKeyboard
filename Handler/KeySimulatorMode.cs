using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TinyKeyboard.Handler
{
    class KeySimulatorMode : IHandler
    {
        private Keys? KeyCode;

        public KeySimulatorMode()
        {
            KeyCode = null;
        }

        void IHandler.KeyPress()
        {
            if (KeyCode != null)
            {
                keybd_event((Keys)KeyCode, 0, 0, 0);
            }
        }

        void IHandler.KeyUp()
        {
            if (KeyCode != null)
            {
                keybd_event((Keys)KeyCode, 0, KEYEVENTF_KEYUP, 0);
            }
        }

        void IHandler.Set(string set)
        {
            try
            {
                KeyCode = (Keys)(int.Parse(set));
            }
            catch (Exception e)
            {
                throw new InvalidValueException();
            }
        }

        [DllImport("user32.dll", EntryPoint = "keybd_event", SetLastError = true)]
        private static extern void keybd_event(Keys bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        private const int KEYEVENTF_KEYUP = 2;
    }

    class InvalidValueException : Exception { }
}
