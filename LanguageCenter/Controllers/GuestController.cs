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
            var showProgram = (from p in db.Programs select p).ToList();
              
            return View(showProgram);
        }

        
    }
}