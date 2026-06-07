using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Models
{
    public class Guest_HomeView
    {
        public List<Program> ProgramList { get; set; }
        public List<Class> ClassList { get; set; }
        public List<Teacher> TeachersList { get; set;}
    }
}