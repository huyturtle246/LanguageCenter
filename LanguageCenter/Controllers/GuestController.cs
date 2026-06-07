using LanguageCenter.Models;
using Microsoft.Ajax.Utilities;
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
            viewModel.UserAccountsList = db.UserAccounts.ToList();
              
            return View(viewModel);
        }

        public ActionResult List(string levelFilter, string feeFilter)
        {
            var viewModel = new ProgramFilter();
            var query = from p in db.Programs select p;
            
            if (!string.IsNullOrEmpty(levelFilter))
            {
                query = query.Where(i => i.Level == levelFilter);
            }

            if (!string.IsNullOrEmpty(feeFilter))
            {
                switch (feeFilter)
                {
                    case "low":
                        query = query.Where(i => i.Price < 3000000); break;
                    case "mid":
                        query = query.Where(i => i.Price >= 3000000 && i.Price <= 6000000); break;
                    case "high":
                        query = query.Where(i => i.Price > 6000000); break;
                }
            }

            viewModel.ProgramList = query.ToList();
            viewModel.LevelList = db.Programs.Select(p => p.Level).ToList();
            viewModel.currentFee = feeFilter;
            viewModel.currentLevel = levelFilter;

            return View(viewModel);

        }
    }
}