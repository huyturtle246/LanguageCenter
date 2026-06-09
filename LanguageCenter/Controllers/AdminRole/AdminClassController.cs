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

        public ActionResult Edit(int? id)
        {
            var _class = db.Classes.FirstOrDefault(i => i.Class_ID == id);
            if (_class == null)
            {
                return HttpNotFound();
            }

            ViewBag.ProgramList = new SelectList(db.Programs.ToList(), "Program_ID", "Program_Name", _class.Program_ID);
            ViewBag.TeacherList = new SelectList(db.Teachers.ToList(), "Teacher_ID", "Teacher_Name", _class.Teacher_ID);
            ViewBag.StatusList = new SelectList(db.ClassStatus.ToList(), "Status_ID", "Status_Name", _class.Status_ID);

            return View("~/Views/Admin/AdminClass/Edit.cshtml", _class);
        }

        [HttpPost]
        public ActionResult Edit(int? id, FormCollection collection)
        {
            var _class = db.Classes.FirstOrDefault(i => i.Class_ID == id);

            var c_Name = collection["Class_Name"];
            var p_ID = collection["Program_ID"];
            var t_ID = collection["Teacher_ID"];
            var s_ID = collection["Status_ID"];

            //Name
            if (string.IsNullOrEmpty(c_Name))
            {
                ModelState.AddModelError("Class_Name", "Tên lớp học không được để trống");
            }

            //Dropdown list
            if (string.IsNullOrEmpty(p_ID))
                ModelState.AddModelError("Program_ID", "Vui lòng chọn chương trình học");

            if (string.IsNullOrEmpty(t_ID))
                ModelState.AddModelError("Teacher_ID", "Vui lòng chọn giáo viên phụ trách");

            if (string.IsNullOrEmpty(s_ID))
                ModelState.AddModelError("Status_ID", "Vui lòng chọn trạng thái lớp học");

            //Student quantity
            int maxStd = 0;
            try
            {
                maxStd = Convert.ToInt32(collection["MaxStudents"]);
                if (maxStd <= 0)
                {
                    ModelState.AddModelError("MaxStudents", "Số lượng học viên tối đa phải lớn hơn 0");
                }

                if (_class != null && maxStd < _class.CurrentStudents)
                {
                    ModelState.AddModelError("MaxStudents", "Sức chứa tối đa không thể nhỏ hơn số học viên hiện tại của lớp (" + _class.CurrentStudents + ")");
                }
            }
            catch (FormatException)
            {
                ModelState.AddModelError("MaxStudents", "Số lượng học viên tối đa phải là số nguyên");
            }

            //Start Date
            DateTime? startDate = null;
            try
            {
                if (!string.IsNullOrEmpty(collection["StartDate"]))
                {
                    startDate = Convert.ToDateTime(collection["StartDate"]);
                }
            }
            catch (FormatException)
            {
                ModelState.AddModelError("StartDate", "Ngày bắt đầu không đúng định dạng");
            }

            //End Date
            DateTime? endDate = null;
            try
            {
                if (!string.IsNullOrEmpty(collection["EndDate"]))
                {
                    endDate = Convert.ToDateTime(collection["EndDate"]);
                    if (startDate != null && endDate < startDate)
                    {
                        ModelState.AddModelError("EndDate", "Ngày kết thúc không được nhỏ hơn ngày bắt đầu");
                    }
                }
            }
            catch (FormatException)
            {
                ModelState.AddModelError("EndDate", "Ngày kết thúc không đúng định dạng");
            }

            if (ModelState.IsValid && _class != null)
            {
                _class.Class_Name = c_Name;
                _class.Program_ID = Convert.ToInt32(p_ID);
                _class.Teacher_ID = Convert.ToInt32(t_ID);
                _class.Status_ID = Convert.ToInt32(s_ID);
                _class.MaxStudents = maxStd;
                _class.StartDate = startDate;
                _class.EndDate = endDate;

                db.SubmitChanges();

                return RedirectToAction("Index", "AdminClass");
            }

            ViewBag.ProgramList = new SelectList(db.Programs.ToList(), "Program_ID", "Program_Name", p_ID);
            ViewBag.TeacherList = new SelectList(db.Teachers.ToList(), "Teacher_ID", "Teacher_Name", t_ID);
            ViewBag.StatusList = new SelectList(db.ClassStatus.ToList(), "Status_ID", "Status_Name", s_ID);


            return View("~/Views/Admin/AdminClass/Edit.cshtml", _class);
        }


        public ActionResult Create()
        {
            ViewBag.ProgramList = new SelectList(db.Programs.ToList(), "Program_ID", "Program_Name");
            ViewBag.TeacherList = new SelectList(db.Teachers.ToList(), "Teacher_ID", "Teacher_Name");
            ViewBag.StatusList = new SelectList(db.ClassStatus.ToList(), "Status_ID", "Status_Name");

            return View("~/Views/Admin/AdminClass/Edit.cshtml");
        }

        [HttpPost]
        public ActionResult Create (FormCollection collection)
        {
            var _class = new Class();

            var c_Name = collection["Class_Name"];
            var p_ID = collection["Program_ID"];
            var t_ID = collection["Teacher_ID"];
            var s_ID = collection["Status_ID"];

            //Name
            if (string.IsNullOrEmpty(c_Name))
            {
                ModelState.AddModelError("Class_Name", "Tên lớp học không được để trống");
            }

            //Dropdown list
            if (string.IsNullOrEmpty(p_ID))
                ModelState.AddModelError("Program_ID", "Vui lòng chọn chương trình học");

            if (string.IsNullOrEmpty(t_ID))
                ModelState.AddModelError("Teacher_ID", "Vui lòng chọn giáo viên phụ trách");

            if (string.IsNullOrEmpty(s_ID))
                ModelState.AddModelError("Status_ID", "Vui lòng chọn trạng thái lớp học");

            //Student quantity
            int maxStd = 0;
            try
            {
                maxStd = Convert.ToInt32(collection["MaxStudents"]);
                if (maxStd <= 0)
                {
                    ModelState.AddModelError("MaxStudents", "Số lượng học viên tối đa phải lớn hơn 0");
                }

                if (_class != null && maxStd < _class.CurrentStudents)
                {
                    ModelState.AddModelError("MaxStudents", "Sức chứa tối đa không thể nhỏ hơn số học viên hiện tại của lớp (" + _class.CurrentStudents + ")");
                }
            }
            catch (FormatException)
            {
                ModelState.AddModelError("MaxStudents", "Số lượng học viên tối đa phải là số nguyên");
            }

            //Start Date
            DateTime? startDate = null;
            try
            {
                if (!string.IsNullOrEmpty(collection["StartDate"]))
                {
                    startDate = Convert.ToDateTime(collection["StartDate"]);
                }
            }
            catch (FormatException)
            {
                ModelState.AddModelError("StartDate", "Ngày bắt đầu không đúng định dạng");
            }

            //End Date
            DateTime? endDate = null;
            try
            {
                if (!string.IsNullOrEmpty(collection["EndDate"]))
                {
                    endDate = Convert.ToDateTime(collection["EndDate"]);
                    if (startDate != null && endDate < startDate)
                    {
                        ModelState.AddModelError("EndDate", "Ngày kết thúc không được nhỏ hơn ngày bắt đầu");
                    }
                }
            }
            catch (FormatException)
            {
                ModelState.AddModelError("EndDate", "Ngày kết thúc không đúng định dạng");
            }

            if (ModelState.IsValid && _class != null)
            {
                _class.Class_Name = c_Name;
                _class.Program_ID = Convert.ToInt32(p_ID);
                _class.Teacher_ID = Convert.ToInt32(t_ID);
                _class.Status_ID = Convert.ToInt32(s_ID);
                _class.MaxStudents = maxStd;
                _class.StartDate = startDate;
                _class.EndDate = endDate;

                db.Classes.InsertOnSubmit(_class);
                db.SubmitChanges();

                return RedirectToAction("Index", "AdminClass");
            }

            ViewBag.ProgramList = new SelectList(db.Programs.ToList(), "Program_ID", "Program_Name", p_ID);
            ViewBag.TeacherList = new SelectList(db.Teachers.ToList(), "Teacher_ID", "Teacher_Name", t_ID);
            ViewBag.StatusList = new SelectList(db.ClassStatus.ToList(), "Status_ID", "Status_Name", s_ID);


            return View("~/Views/Admin/AdminClass/Edit.cshtml", _class);
        }

        public ActionResult Detail (int? id)
        {
            var _class = db.Classes.FirstOrDefault(i => i.Class_ID == id);
            return View("~/Views/Admin/AdminClass/Detail.cshtml", _class);
        }

    }
}