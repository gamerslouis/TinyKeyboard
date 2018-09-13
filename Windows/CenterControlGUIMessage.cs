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

        public JSONProfile GetProfile(int index)
        {
            return cc.profileContainer.jSONProfiles[index];
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

        public bool ChangeProfileName(int index, string name)
        {
            cc.profileContainer.jSONProfiles[index].Name = name;
            return cc.profileContainer.Save();
        }

        public bool AddNewProfile(string name)
        {
            var profile = new JSONProfile();
            profile.Check();
            profile.Name = name;
            Array.Resize(ref cc.profileContainer.jSONProfiles, cc.profileContainer.jSONProfiles.Length + 1);
            cc.profileContainer.jSONProfiles[cc.profileContainer.jSONProfiles.Length - 1] = profile;
            if (!cc.profileContainer.Save())
            {
                Array.Resize(ref cc.profileContainer.jSONProfiles, cc.profileContainer.jSONProfiles.Length - 1);
                return false;
            }
            return true;
        }

        public bool RemoveProfile(int index)
        {
            var ErrorProtectProfile = cc.profileContainer.jSONProfiles[index];
            for (int i = index; i < cc.profileContainer.jSONProfiles.Length - 1; i++)
            {
                cc.profileContainer.jSONProfiles[i] = cc.profileContainer.jSONProfiles[i + 1];
            }
            Array.Resize(ref cc.profileContainer.jSONProfiles, cc.profileContainer.jSONProfiles.Length - 1);

            if (!cc.profileContainer.Save())
            {
                Array.Resize(ref cc.profileContainer.jSONProfiles, cc.profileContainer.jSONProfiles.Length + 1);
                for (int i = index; i < cc.profileContainer.jSONProfiles.Length - 1; i++)
                {
                    cc.profileContainer.jSONProfiles[i + 1] = cc.profileContainer.jSONProfiles[i];
                }
                cc.profileContainer.jSONProfiles[index] = ErrorProtectProfile;
            }

            return true;
        }

        public void ForceDetectPorts()
        {
            if (cc.serialPortMessage != null)
            {
                cc.serialPortMessage.Dispose();
                cc.serialPortMessage = null;
            }
            cc.ScanPorts();
        }
    }
}
