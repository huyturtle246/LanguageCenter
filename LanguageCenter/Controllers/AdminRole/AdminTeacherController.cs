using LanguageCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace LanguageCenter.Controllers.AdminRole
{
    public class AdminTeacherController : Controller
    {
        // GET: Teacher
        dbLanguageCenterDataContext db = new dbLanguageCenterDataContext();
        public ActionResult Index()
        {
            var teacherList = db.Teachers.Distinct().ToList();
            return View("~/Views/Admin/AdminTeacher/Index.cshtml", teacherList);
        }

        public ActionResult Edit(int? id)
        {
            var teacher = db.Teachers.SingleOrDefault(i => i.Teacher_ID == id);
            return View("~/Views/Admin/AdminTeacher/Edit.cshtml", teacher);
        }

        [HttpPost]
        public ActionResult Edit(int? id, FormCollection collection)
        {
            var teacher = db.Teachers.SingleOrDefault(i => i.Teacher_ID == id);
            if (teacher == null) return HttpNotFound();

            var t_name = collection["Teacher_Name"];
            var t_gender = collection["Gender"];
            var t_phone = collection["Phone"];
            var t_address = collection["Address"];
            var t_certificate = collection["Certificate"];
            var t_image = collection["UserAccount.Image"];

            var u_username = collection["UserAccount.User_Name"];
            var u_password = collection["UserAccount.Password"];
            var u_email = collection["UserAccount.Email"];
            var u_isActive = collection["UserAccount.isActive"];

            var user = db.UserAccounts.FirstOrDefault(x => x.User_ID == teacher.User_ID);

            // Name
            if (string.IsNullOrEmpty(t_name))
            {
                ModelState.AddModelError("Teacher_Name", "Tên giáo viên không được để trống");
            }

            // Gender
            if (string.IsNullOrEmpty(t_gender))
            {
                teacher.Gender = null;
            }
            else
            {
                if (t_gender.Trim().ToLower() == "nam" || t_gender.Trim().ToLower() == "nữ")
                {
                    teacher.Gender = t_gender;
                }
                else
                {
                    ModelState.AddModelError("Gender", "Nam hoặc nữ");
                    teacher.Gender = null;
                }
            }

            // Phone
            if (string.IsNullOrEmpty(t_phone))
            {
                teacher.Phone = null;
            }
            else
            {
                t_phone = t_phone.Trim();
                if (!Regex.IsMatch(t_phone, @"^[0-9]+$"))
                {
                    ModelState.AddModelError("Phone", "Kí tự phải là số từ 0 đến 9");
                }
                else if (t_phone.Length != 10)
                {
                    ModelState.AddModelError("Phone", "Độ dài số điện thoại phải là 10");
                }
            }

            // DOB
            if (!string.IsNullOrEmpty(collection["DOB"]))
            {
                try
                {
                    var t_dob = Convert.ToDateTime(collection["DOB"]);
                    if (t_dob >= DateTime.Now)
                    {
                        ModelState.AddModelError("DOB", "Ngày sinh không được ở tương lai");
                    }
                    teacher.DOB = t_dob;
                }
                catch (FormatException)
                {
                    ModelState.AddModelError("DOB", "Ngày tháng năm sinh không đúng định dạng");
                }
            }

            // Salary
            if (!string.IsNullOrEmpty(collection["Salary"]))
            {
                try
                {
                    var t_salary = Convert.ToDecimal(collection["Salary"]);
                    teacher.Salary = t_salary;
                }
                catch (FormatException)
                {
                    ModelState.AddModelError("Salary", "Lương phải là số");
                }
            }

            if (user != null)
            {
                if (string.IsNullOrEmpty(u_username))
                {
                    ModelState.AddModelError("UserAccount.User_Name", "Tên đăng nhập không được để trống");
                }
                else
                {
                    var isExist = db.UserAccounts.Any(x => x.User_Name == u_username.Trim() && x.User_ID != user.User_ID);
                    if (isExist)
                    {
                        ModelState.AddModelError("UserAccount.User_Name", "Tên đăng nhập này đã có người sử dụng");
                    }
                }

                if (!string.IsNullOrWhiteSpace(u_password))
                {
                    user.Password = u_password.Trim();
                }
                else
                {
                    ModelState.AddModelError("UserAccount.Password", "Mật khẩu của tài khoản không được để trống");
                }
            }

            if (ModelState.IsValid)
            {
                teacher.Teacher_Name = t_name;
                teacher.Phone = t_phone;
                teacher.Address = t_address;
                teacher.Certificate = t_certificate;

                if (user != null)
                {
                    user.User_Name = u_username.Trim();
                    user.Email = string.IsNullOrEmpty(u_email) ? null : u_email.Trim();
                    user.Image = t_image;
                    user.isActive = (u_isActive == "true" || u_isActive == "True" || u_isActive == "1");
                }

                db.SubmitChanges();
                return RedirectToAction("Index");
            }

            return View("~/Views/Admin/AdminTeacher/Edit.cshtml", teacher);
        }

        public ActionResult Create()
        {
            var teacher = new Teacher();
            return View("~/Views/Admin/AdminTeacher/Create.cshtml", teacher);
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var teacher = new Teacher();

            var t_name = collection["Teacher_Name"];
            var t_gender = collection["Gender"];
            var t_phone = collection["Phone"];
            var t_address = collection["Address"];
            var t_certificate = collection["Certificate"];
            var t_image = collection["UserAccount.Image"];

            //Name
            if (string.IsNullOrEmpty(t_name))
            {
                ModelState.AddModelError("Teacher_Name", "Tên giáo viên không được để trống");
            }

            //Gender
            if (string.IsNullOrEmpty(t_gender))
            {
                teacher.Gender = null;
            }
            else
            {
                if (t_gender.Trim().ToLower() == "nam" || t_gender.Trim().ToLower() == "nữ")
                {
                    teacher.Gender = t_gender;
                }
                else
                {
                    ModelState.AddModelError("Gender", "Nam hoặc nữ");
                    teacher.Gender = null;
                }
            }

            //Phone
            if (string.IsNullOrEmpty(t_phone))
            {
                teacher.Phone = null;
            }
            else
            {
                t_phone = t_phone.Trim();
                if (!Regex.IsMatch(t_phone, @"^[0-9]+$"))
                {
                    ModelState.AddModelError("Phone", "Kí tự phải là số từ 0 đến 9");
                }
                else if (t_phone.Length != 10)
                {
                    ModelState.AddModelError("Phone", "Độ dài số điện phải là 10");
                }
            }

            //DOB
            try
            {
                var t_dob = Convert.ToDateTime(collection["DOB"]);

                if (t_dob >= DateTime.Now)
                {
                    ModelState.AddModelError("DOB", "Ngày sinh không được ở tương lai");
                }

                teacher.DOB = t_dob;
            }
            catch (FormatException)
            {
                ModelState.AddModelError("DOB", "Ngày tháng năm sinh phải là kiểu ngày-tháng-năm");
            }

            //Salary
            try
            {
                var t_salary = Convert.ToDecimal(collection["Salary"]);
                teacher.Salary = t_salary;
            }
            catch (FormatException)
            {
                ModelState.AddModelError("Salary", "Lương phải là số thập phân");
            }

            //Image
            var user = db.UserAccounts.FirstOrDefault(x => x.User_ID == teacher.User_ID);

            if (user != null)
            {
                user.Image = t_image;
            }

            if (ModelState.IsValid)
            {
                UserAccount newUser = new UserAccount();
                newUser.User_Name = "gv_" + t_phone;
                newUser.Password = "123456";
                newUser.Role = "Teacher";
                newUser.Image = t_image;
                newUser.isActive = true;

                db.UserAccounts.InsertOnSubmit(newUser);
                db.SubmitChanges();

                teacher.User_ID = newUser.User_ID;
                teacher.Teacher_Name = t_name;
                teacher.Phone = t_phone;
                teacher.Address = t_address;
                teacher.Certificate = t_certificate;

                db.Teachers.InsertOnSubmit(teacher);
                db.SubmitChanges();
                return RedirectToAction("Index", "AdminTeacher");
            }
            return View("~/Views/Admin/AdminTeacher/Create.cshtml");

        }

        public ActionResult Detail (int? id)
        {
            var teacher = db.Teachers.FirstOrDefault(i => i.Teacher_ID == id);
            return View("~/Views/Admin/AdminTeacher/Detail.cshtml", teacher);
        }

        [HttpPost]
        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Content/images/" + file.FileName));
            return "/Content/images/" + file.FileName;
        }
    }

}