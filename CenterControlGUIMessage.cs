using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyKeyboard
{
    class CenterControlGUIMessage
    {
        private CenterControl cc;

        public CenterControlGUIMessage(CenterControl cc)
        {
            this.cc = cc;
        }

        public JSONProfile GetCurrentProfile()
        {
            return cc.profileContainer.jSONProfiles[cc.profileContainer.ProfileIndex];
        }

        public int GetCureentIndex()
        {
            return cc.profileContainer.ProfileIndex;
        }

        public string[] GetProfileNames()
        {
            string[] names = new String[cc.profileContainer.jSONProfiles.Length];
            for (int i = 0; i < cc.profileContainer.jSONProfiles.Length; i++)
            {
                names[i] = cc.profileContainer.jSONProfiles[i].Name;
            }
            return names;
        }


    }
}
