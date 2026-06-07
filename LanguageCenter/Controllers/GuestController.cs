using LanguageCenter.Models;
using LanguageCenter.Models.Guest;
using Microsoft.Ajax.Utilities;
using PagedList;
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

        //Home
        public ActionResult Index()
        {
            var viewModel = new Guest_HomeView();

            viewModel.ProgramList = db.Programs.ToList();
            viewModel.ClassList = db.Classes.ToList();
            viewModel.TeachersList = db.Teachers.ToList();
            viewModel.UserAccountsList = db.UserAccounts.ToList();
              
            return View(viewModel);
        }


        //Program List
        public ActionResult List(string levelFilter, string feeFilter, int? page, int? size, string searchString)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "3", Value = "3" });
            items.Add(new SelectListItem { Text = "6", Value = "6" });
            items.Add(new SelectListItem { Text = "12", Value = "12" });
            items.Add(new SelectListItem { Text = "24", Value = "24" });
            items.Add(new SelectListItem { Text = "48", Value = "48" });
            foreach (var item in items)
            {
                if (item.Value == size.ToString())
                {
                    item.Selected = true;
                }
            }

            if (page == null) { page = 1; }

            ViewBag.size = items;
            ViewBag.currentSize = size;
            ViewBag.Keyword = searchString;
            var viewModel = new Guest_ProgramListView();
            var query = from p in db.Programs select p;
            int pageSize = (size ?? 3);
            int pageNum = page ?? 1;

            if (!String.IsNullOrEmpty(searchString))
            {
                query = query.Where(p => p.Program_Name.Contains(searchString));
            }

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

            viewModel.ProgramList = query.ToPagedList(pageNum, pageSize);
            viewModel.LevelList = db.Programs.Select(p => p.Level).ToList();
            viewModel.currentFee = feeFilter;
            viewModel.currentLevel = levelFilter;

            return View(viewModel);

        }

        //Program Detail
        public ActionResult Detail (int? id)
        {
            var viewModel = new Guest_ProgramDetailView();

            viewModel.program = db.Programs.FirstOrDefault(i => i.Program_ID == id);
            viewModel.ClassList = db.Classes.Where(i => i.Program_ID == id).ToList();
            return View(viewModel);
        }
    }
}