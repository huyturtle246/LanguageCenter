using LanguageCenter.Models;
using LanguageCenter.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LanguageCenter.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin

        dbLanguageCenterDataContext db = new dbLanguageCenterDataContext();
        public ActionResult Index()
        {
            var viewModel = new Data();

            viewModel.ProgramList = db.Programs.Distinct().ToList();
            viewModel.ClassList = db.Classes.Distinct().ToList();
            viewModel.StudentList = db.Students.Distinct().ToList();
            viewModel.TeachersList = db.Teachers.Distinct().ToList();
            
            return View(viewModel);
        }

    }
}