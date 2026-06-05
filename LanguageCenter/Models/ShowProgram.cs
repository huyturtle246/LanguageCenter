using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace LanguageCenter.Models
{
    public class ShowProgram
    {
        public int programId { get; set; }
        public string programName { get; set; }
        public string programImg {  get; set; }
        public string programDes { get; set; }
        public int? programDur { get; set; }
        public decimal? programPrice { get; set; }

    }
}