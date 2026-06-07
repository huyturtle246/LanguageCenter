using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Models.Guest
{
    public class Guest_ProgramDetailView
    {
        public Program program { get; set; }
        public List<Class> ClassList { get; set; }
    }
}