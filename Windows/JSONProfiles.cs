using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyKeyboard
{
    public class JSONRoot
    {
        public int profileIndex;
        public JSONProfileContainer jSONProfileContainer;
    }
    public class JSONProfileContainer
    {
        public JSONProfile[] jSONProfiles;

        public void Check()
        {
            if (jSONProfiles.Length > GlobalSetting.ProfileMaxNumber)
            {
                Array.Resize(ref jSONProfiles, GlobalSetting.ProfileMaxNumber);
            }

            for (int i = 0; i < jSONProfiles.Length; i++)
            {
                if (jSONProfiles[i] == null) jSONProfiles[i] = new JSONProfile();

                jSONProfiles[i].Check();
            }
        }
    }

    public class JSONProfile
    {
        public string Name { get; set; }
        public JSONMode[] jSONModes;

        public void Check()
        {
            if (jSONModes.Length != GlobalSetting.KeyMaxNumber)
            {
                Array.Resize(ref jSONModes, GlobalSetting.KeyMaxNumber);
            }
            for (int i = 0; i < jSONModes.Length; i++)
            {
                if (jSONModes[i].Name == "") jSONModes[i].Name = "nullmode";
            }
        }
    }

    public class JSONMode
    {
        public string Name { get; set; }
        public string Set { get; set; }
    }
}
