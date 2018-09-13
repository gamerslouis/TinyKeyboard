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

        public bool Load()
        {
            try
            {
                var sr = new System.IO.StreamReader(GlobalSetting.ProfileLocation);
                var root = Newtonsoft.Json.JsonConvert.DeserializeObject<JSONRoot>(sr.ReadToEnd());
                sr.Close();
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
            catch
            {
                return false;
            }
            return true;
        }

        public bool Save()
        {
            try
            {
                JSONRoot root = new JSONRoot();
                root.profileIndex = ProfileIndex;
                root.jSONProfileContainer.jSONProfiles = jSONProfiles;
                string t = Newtonsoft.Json.JsonConvert.SerializeObject(root);
                var sw = new System.IO.StreamWriter(GlobalSetting.ProfileLocation + ".tmp");
                sw.Write(t);
                sw.Close();
            }
            catch
            {
                return false;
            }

            try
            {
                System.IO.File.Delete(GlobalSetting.ProfileLocation);
            }
            catch
            {
                System.IO.File.Delete(GlobalSetting.ProfileLocation + ".tmp");
                return false;
            }

            try
            {
                System.IO.File.Move(GlobalSetting.ProfileLocation + ".tmp", GlobalSetting.ProfileLocation);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }

}
