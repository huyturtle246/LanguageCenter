using LanguageCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LanguageCenter.Controllers.AdminRole
{
    public class AdminTeacherController : Controller
    {
        // GET: Teacher
        dbLanguageCenterDataContext db = new dbLanguageCenterDataContext();
        public ActionResult Index()
        {
            var teacherList = db.Teachers.Distinct().ToList();
            return View("~/Views/Admin/AdminTeacher/Index.cshtml", teacherList);
        }
    }
}