using LanguageCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LanguageCenter.Controllers
{
    public class GuestController : Controller
    {
        dbLanguageCenterDataContext db = new dbLanguageCenterDataContext();
        public ActionResult Index()
        {
            var viewModel = new Guest_HomeView();

            viewModel.ProgramList = db.Programs.ToList();
            viewModel.ClassList = db.Classes.ToList();
            viewModel.TeachersList = db.Teachers.ToList();
              
            return View(viewModel);
        }
    }
}