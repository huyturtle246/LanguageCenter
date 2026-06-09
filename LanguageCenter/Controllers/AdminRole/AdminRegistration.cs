using LanguageCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LanguageCenter.Controllers.AdminRole
{
    public class AdminRegistrationController : Controller
    {
        dbLanguageCenterDataContext db = new dbLanguageCenterDataContext();

        public ActionResult Index()
        {
            var registrationList = db.Registrations.Distinct().ToList();
            return View("~/Views/Admin/AdminRegistration/Index.cshtml", registrationList);
        }


        public ActionResult Edit(int? id)
        {
            var reg = db.Registrations.SingleOrDefault(i => i.Registration_ID == id);
            if (reg == null) return HttpNotFound();

            return View("~/Views/Admin/AdminRegistration/Edit.cshtml", reg);
        }

        [HttpPost]
        public ActionResult Edit(int? id, FormCollection collection)
        {
            var reg = db.Registrations.SingleOrDefault(i => i.Registration_ID == id);
            if (reg == null) return HttpNotFound();

            var r_status = collection["Status"];

            if (string.IsNullOrEmpty(r_status))
            {
                ModelState.AddModelError("Status", "Trạng thái đăng ký không được để trống");
            }
            else
            {
                r_status = r_status.Trim();
                if (r_status == "Đã xác nhận" || r_status == "Chờ xác nhận" || r_status == "Đã hủy")
                {
                    reg.Status = r_status;
                }
                else
                {
                    ModelState.AddModelError("Status", "Trạng thái phải là: 'Chờ xác nhận', 'Đã xác nhận' hoặc 'Đã hủy'");
                }
            }

            if (!string.IsNullOrEmpty(collection["Registration_Date"]))
            {
                try
                {
                    var r_date = Convert.ToDateTime(collection["Registration_Date"]);
                    reg.Registration_Date = r_date;
                }
                catch (FormatException)
                {
                    ModelState.AddModelError("Registration_Date", "Ngày đăng ký không đúng định dạng ngày-tháng-năm");
                }
            }

            if (ModelState.IsValid)
            {
                db.SubmitChanges();
                return RedirectToAction("Index");
            }

            return View("~/Views/Admin/AdminRegistration/Edit.cshtml", reg);
        }

        public ActionResult Cancel(int? id)
        {
            var reg = db.Registrations.SingleOrDefault(i => i.Registration_ID == id);
            if (reg == null) return HttpNotFound();

            return View("~/Views/Admin/AdminRegistration/Cancel.cshtml", reg);
        }

        [HttpPost]
        [ActionName("Cancel")]
        public ActionResult CancelConfirmed(int id)
        {
            var reg = db.Registrations.SingleOrDefault(i => i.Registration_ID == id);
            if (reg == null) return HttpNotFound();

            reg.Status = "Đã hủy";

            db.SubmitChanges();
            return RedirectToAction("Index");
        }
    }
}