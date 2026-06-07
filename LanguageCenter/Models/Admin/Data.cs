using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Models.Admin
{
    public class Data
    {
        public List<Program> ProgramList { get; set; }
        public List<Class> ClassList { get; set; }
        public List<Student> StudentList { get; set; }
        public List<Teacher> TeachersList { get; set; }
        public decimal? TotalRevenue { get; set; }
}}