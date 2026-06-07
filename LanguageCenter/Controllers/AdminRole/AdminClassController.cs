using LanguageCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LanguageCenter.Controllers.AdminRole
{
    public class AdminClassController : Controller
    {
        // GET: Class
        dbLanguageCenterDataContext db = new dbLanguageCenterDataContext();
        public ActionResult Index()
        {
            var classList = db.Classes.Distinct().ToList();
            return View("~/Views/Admin/AdminClass/Index.cshtml", classList);
        }
    }
}