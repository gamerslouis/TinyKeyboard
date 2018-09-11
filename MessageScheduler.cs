using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyKeyboard
{
    class MessageScheduler
    {
        public Handler.IHandler[] handlers;
        bool ProfileChangeFlag = false;

        public event EventHandler<int> ProfileChanged;

        public void ScheduleFuction(object sender,byte key)
        {
            if (key == 9) { ProfileChangeFlag = true; return; }

            if(ProfileChangeFlag)
            {
                if (key < 9 && key > 0)
                {
                    ProfileChangeFlag = false;
                    ProfileChanged?.Invoke(this, key);
                }
            }
            else
            {
                if (key < 9 && key > 0)
                {
                    handlers[key].KeyPress();
                }
                else if (key > 9 && key <= 17) 
                {
                    handlers[key - 9].KeyUp();
                }
            }
        }
    }
}
