using LanguageCenter.Models;
using LanguageCenter.Models.Admin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ActionResult Edit(int? id)
        {
            var programList = db.Programs.FirstOrDefault(i => i.Program_ID == id);
            return View("~/Views/Admin/AdminProgram/Edit.cshtml", programList);
        }

        [HttpPost]
        public ActionResult Edit(int? id, FormCollection collection)
        {
            var programList = db.Programs.FirstOrDefault(i => i.Program_ID == id);
            var p_Name = collection["Program_Name"];
            var p_Image = collection["Image"];
            var p_Level = collection["Level"];
            var p_Des = collection["Description"];
            var p_Output = collection["Output_Standard"];
            var p_Duration = collection["DurationWeeks"];
            var p_Fee = Convert.ToDecimal(collection["Price"]);

            programList.Program_Name = p_Name;
            programList.Image = p_Image;
            programList.Level = p_Level;
            programList.Description = p_Des;
            programList.Output_Standard = p_Output;
            programList.Price = p_Fee;
            UpdateModel(programList);
            db.SubmitChanges();
            return this.Edit(id);
        }
    }
}