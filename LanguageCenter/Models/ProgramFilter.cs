using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Models
{
    public class ProgramFilter
    {
        public List<Program> ProgramList { get; set; }
        public List<string> LevelList { get; set; } 

        public string currentLevel { get; set; }
        public string currentFee { get; set; }
    }
}