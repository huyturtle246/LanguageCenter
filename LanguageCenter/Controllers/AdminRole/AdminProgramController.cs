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
            var program = db.Programs.FirstOrDefault(i => i.Program_ID == id);
            return View("~/Views/Admin/AdminProgram/Edit.cshtml", program);
        }

        [HttpPost]
        public ActionResult Edit(int? id, FormCollection collection)
        {
            var program = db.Programs.FirstOrDefault(i => i.Program_ID == id);

            var p_Name = collection["Program_Name"];
            var p_Image = collection["Image"];
            var p_Level = collection["Level"];
            var p_Des = collection["Description"];
            var p_Output = collection["Output_Standard"];

            if (string.IsNullOrEmpty(p_Name))
            {
                ModelState.AddModelError("Program_Name", "Tên chương trình không được để trống");
            }

            try
            {
                var p_Duration = Convert.ToInt32(collection["DurationWeeks"]);
                program.DurationWeeks = p_Duration;
            }
            catch (FormatException)
            {
                ModelState.AddModelError("DurationWeeks", "Số tuần phải là số nguyên");
            }

            try
            {
                var p_Fee = Convert.ToDecimal(collection["Price"]);
                program.Price = p_Fee;
            }
            catch (FormatException)
            {
                ModelState.AddModelError("Price", "Học phí phải là số thập phân");
            }

            program.Image = p_Image;
            program.Level = p_Level;
            program.Description = p_Des;
            program.Output_Standard = p_Output;

            db.SubmitChanges();
            return this.Edit(id);
        }

        public ActionResult Detail (int? id)
        {
            var program = db.Programs.FirstOrDefault(i => i.Program_ID == id);
            return View("~/Views/Admin/AdminProgram/Detail.cshtml", program);
        }

        public ActionResult Delete(int id)
        {
            var program = db.Programs.First(m => m.Program_ID == id);
            return View("~/Views/Admin/AdminProgram/Delete.cshtml", program);
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var program = db.Programs.First(i => i.Program_ID == id);
            var relatedRegistration = db.Registrations.Where(i => i.Program_ID == id).ToList();
            db.Registrations.DeleteAllOnSubmit(relatedRegistration);
            db.Programs.DeleteOnSubmit(program);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
    }
}