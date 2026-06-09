using LanguageCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LanguageCenter.Controllers.AdminRole
{
    public class AdminStudentController : Controller
    {
        dbLanguageCenterDataContext db = new dbLanguageCenterDataContext();
        public ActionResult Index()
        {
            var studentList = db.Students.Distinct().ToList();
            return View("~/Views/Admin/AdminStudent/Index.cshtml", studentList);
        }

        public ActionResult Edit(int? id)
        {
            var student = db.Students.FirstOrDefault(i => i.Student_ID == id);
            return View("~/Views/Admin/AdminStudent/Edit.cshtml", student);
        }

        [HttpPost]
        public ActionResult Edit(int? id, FormCollection collection)
        {
            var student = db.Students.FirstOrDefault(i => i.Student_ID == id);
          
            var s_Image = collection["Image"];
            var s_Name = collection["Student_Name"];
            var s_Gender = collection["Gender"];
            var s_Phone = collection["Phone"];
            var s_Address = collection["Address"];

            if (string.IsNullOrEmpty(s_Name))
            {
                ModelState.AddModelError("Student_Name", "Tên học sinh không được để trống");
            }

            try
            {
                var s_DOB = Convert.ToDateTime(collection["DOB"]);

                if (s_DOB >= DateTime.Now)
                {
                    ModelState.AddModelError("DOB", "Ngày sinh không được ở tương lai");
                }

                student.DOB = s_DOB;
            }
            catch (FormatException)
            {
                ModelState.AddModelError("DOB", "Ngày tháng năm sinh phải là kiểu ngày-tháng-năm");
            }

            student.UserAccount.Image = s_Image;
            student.Student_Name = s_Name;
            student.Gender = s_Gender;
            student.Phone = s_Phone;
            student.Address = s_Address;

            db.SubmitChanges();
            return this.Edit(id);
        }

        public ActionResult Detail (int? id)
        {
            var student = db.Students.FirstOrDefault(i => i.Student_ID == id);
            return View("~/Views/Admin/AdminStudent/Detail.cshtml", student);
        }
    }
}