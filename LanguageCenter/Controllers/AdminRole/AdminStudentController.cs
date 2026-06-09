using LanguageCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            var student = db.Students.SingleOrDefault(i => i.Student_ID == id);
            if (student == null) return HttpNotFound();
            return View("~/Views/Admin/AdminStudent/Edit.cshtml", student);
        }

        [HttpPost]
        public ActionResult Edit(int? id, FormCollection collection)
        {
            var student = db.Students.SingleOrDefault(i => i.Student_ID == id);
            if (student == null) return HttpNotFound();

            var s_name = collection["Student_Name"];
            var s_gender = collection["Gender"];
            var s_phone = collection["Phone"];
            var s_address = collection["Address"];
            var s_image = collection["UserAccount.Image"];

            var u_username = collection["UserAccount.User_Name"];
            var u_password = collection["UserAccount.Password"];
            var u_email = collection["UserAccount.Email"];
            var u_isActive = collection["UserAccount.isActive"];

            var user = db.UserAccounts.FirstOrDefault(x => x.User_ID == student.User_ID);

            if (string.IsNullOrEmpty(s_name)) ModelState.AddModelError("Student_Name", "Tên học sinh không được để trống");

            // Gender
            if (string.IsNullOrEmpty(s_gender)) student.Gender = null;
            else if (s_gender.Trim().ToLower() == "nam" || s_gender.Trim().ToLower() == "nữ") student.Gender = s_gender;
            else { ModelState.AddModelError("Gender", "Giới tính phải là Nam hoặc Nữ"); student.Gender = null; }

            // Phone
            if (string.IsNullOrEmpty(s_phone)) student.Phone = null;
            else
            {
                s_phone = s_phone.Trim();
                if (!Regex.IsMatch(s_phone, @"^[0-9]+$")) ModelState.AddModelError("Phone", "Số điện thoại phải là số");
                else if (s_phone.Length != 10) ModelState.AddModelError("Phone", "Số điện thoại phải là 10 ký tự");
            }

            // DOB
            if (!string.IsNullOrEmpty(collection["DOB"]))
            {
                try
                {
                    var s_dob = Convert.ToDateTime(collection["DOB"]);
                    if (s_dob >= DateTime.Now) ModelState.AddModelError("DOB", "Ngày sinh không hợp lệ");
                    student.DOB = s_dob;
                }
                catch { ModelState.AddModelError("DOB", "Định dạng ngày sinh không đúng"); }
            }

            if (user != null)
            {
                if (string.IsNullOrEmpty(u_username)) ModelState.AddModelError("UserAccount.User_Name", "Tên đăng nhập không được để trống");
                else
                {
                    var isExist = db.UserAccounts.Any(x => x.User_Name == u_username.Trim() && x.User_ID != user.User_ID);
                    if (isExist) ModelState.AddModelError("UserAccount.User_Name", "Tên đăng nhập đã tồn tại");
                }

                if (string.IsNullOrWhiteSpace(u_password)) ModelState.AddModelError("UserAccount.Password", "Mật khẩu không được để trống");
            }

            if (ModelState.IsValid)
            {
                student.Student_Name = s_name;
                student.Phone = s_phone;
                student.Address = s_address;

                if (user != null)
                {
                    user.User_Name = u_username.Trim();
                    user.Password = u_password.Trim();
                    user.Email = string.IsNullOrEmpty(u_email) ? null : u_email.Trim();
                    user.Image = s_image;
                    user.isActive = (u_isActive == "true" || u_isActive == "True" || u_isActive == "1");
                }

                db.SubmitChanges();
                return RedirectToAction("Index", "AdminStudent");
            }

            return View("~/Views/Admin/AdminStudent/Edit.cshtml", student);
        }

        public ActionResult Detail(int? id)
        {
            var student = db.Students.SingleOrDefault(i => i.Student_ID == id);
            return View("~/Views/Admin/AdminStudent/Detail.cshtml", student);
        }

        public ActionResult Create()
        {
            var student = new Student();
            return View("~/Views/Admin/AdminStudent/Create.cshtml", student);
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var student = new Student();

            var s_name = collection["Student_Name"];
            var s_gender = collection["Gender"];
            var s_phone = collection["Phone"];
            var s_address = collection["Address"];
            var s_image = collection["UserAccount.Image"];

            //Name
            if (string.IsNullOrEmpty(s_name))
            {
                ModelState.AddModelError("Student_Name", "Tên học sinh không được để trống");
            }

            //Gender
            if (!string.IsNullOrEmpty(s_gender))
            {
                if (s_gender.Trim().ToLower() == "nam" || s_gender.Trim().ToLower() == "nữ")
                {
                    student.Gender = s_gender;
                }
                else
                {
                    ModelState.AddModelError("Gender", "Giới tính phải là Nam hoặc Nữ");
                }
            }

            //Phone
            if (!string.IsNullOrEmpty(s_phone))
            {
                s_phone = s_phone.Trim();
                if (!Regex.IsMatch(s_phone, @"^[0-9]+$"))
                {
                    ModelState.AddModelError("Phone", "Số điện thoại chỉ được chứa ký tự số");
                }
                else if (s_phone.Length != 10)
                {
                    ModelState.AddModelError("Phone", "Số điện thoại phải là 10 ký tự");
                }
            }

            //DOB
            try
            {
                if (!string.IsNullOrEmpty(collection["DOB"]))
                {
                    var s_dob = Convert.ToDateTime(collection["DOB"]);
                    if (s_dob >= DateTime.Now)
                    {
                        ModelState.AddModelError("DOB", "Ngày sinh không được ở tương lai");
                    }
                    student.DOB = s_dob;
                }
            }
            catch (FormatException)
            {
                ModelState.AddModelError("DOB", "Định dạng ngày sinh không đúng");
            }

            if (ModelState.IsValid)
            {
                UserAccount newUser = new UserAccount();
                newUser.User_Name = "st_" + (string.IsNullOrEmpty(s_phone) ? DateTime.Now.Ticks.ToString().Substring(10) : s_phone);
                newUser.Password = "123456";
                newUser.Role = "Student";
                newUser.Image = s_image;
                newUser.isActive = true;

                db.UserAccounts.InsertOnSubmit(newUser);
                db.SubmitChanges();

                student.User_ID = newUser.User_ID;
                student.Student_Name = s_name;
                student.Phone = s_phone;
                student.Address = s_address;

                db.Students.InsertOnSubmit(student);
                db.SubmitChanges();

                return RedirectToAction("Index", "AdminStudent");
            }

            return View("~/Views/Admin/AdminStudent/Create.cshtml");
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