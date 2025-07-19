using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shinrai_Client_Launcher.ProfileSystem
{
    internal class LSettings
    {
        public enum Lang
        {
            Turkish,
            English,
            German,
            Japanese
        }
        public int Ram { get; set; }
        public string Resolution { get; set; }
        public bool FullScreen { get; set; }
        public bool AutoIP { get; set; }
        public string AutoIPAdresS { get; set; }
        public Lang Langu { get; set; }
        
    }
}
