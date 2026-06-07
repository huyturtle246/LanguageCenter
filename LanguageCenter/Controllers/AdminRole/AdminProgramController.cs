using LanguageCenter.Models;
using LanguageCenter.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LanguageCenter.Controllers.AdminRole
{
    public class AdminProgramController : Controller
    {
        // GET: Program
        dbLanguageCenterDataContext db = new dbLanguageCenterDataContext();
        public ActionResult Index()
        {
            var programList = db.Programs.Distinct().ToList();
            return View("~/Views/Admin/AdminProgram/Index.cshtml", programList);
        }
    }
}