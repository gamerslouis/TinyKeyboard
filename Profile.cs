using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyKeyboard
{
    class ProfileContainer
    {
        public int ProfileIndex;
        public JSONProfile[] jSONProfiles;

        public void Load()
        {
            var sr = new System.IO.StreamReader(GlobalSetting.ProfileLocation);
            var root = Newtonsoft.Json.JsonConvert.DeserializeObject<JSONRoot>(sr.ReadToEnd());
            root.jSONProfileContainer.Check();
            jSONProfiles = root.jSONProfileContainer.jSONProfiles;

            if (ProfileIndex < jSONProfiles.Length && ProfileIndex >= 0)
            {
                ProfileIndex = root.profileIndex;
            }
            else ProfileIndex = 0;

            if (jSONProfiles.Length == 0)
            {
                Array.Resize(ref jSONProfiles, jSONProfiles.Length + 1);
                jSONProfiles[0].Check();
            }
        }

        public void Save()
        {
            JSONRoot root = new JSONRoot();
            root.profileIndex = ProfileIndex;
            root.jSONProfileContainer.jSONProfiles = jSONProfiles;
            string t = Newtonsoft.Json.JsonConvert.SerializeObject(root);
        }
    }

}
