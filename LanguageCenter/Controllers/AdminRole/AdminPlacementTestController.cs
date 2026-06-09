using LanguageCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LanguageCenter.Controllers.AdminRole
{
    public class AdminPlacementTestController : Controller
    {
        dbLanguageCenterDataContext db = new dbLanguageCenterDataContext();
        public ActionResult Index()
        {
            var testList = db.PlacementTests.Distinct().ToList();
            return View("~/Views/Admin/AdminPlacementTest/Index.cshtml", testList);
        }

        public ActionResult Create()
        {
            ViewBag.Students = db.Students.ToList();
            return View("~/Views/Admin/AdminPlacementTest/Create.cshtml");
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var test = new PlacementTest();

            var studentIdStr = collection["Student_ID"];
            var testDateStr = collection["TestDate"];
            var notes = collection["Notes"];

            if (string.IsNullOrEmpty(studentIdStr))
            {
                ModelState.AddModelError("Student_ID", "Vui lòng chọn học viên tham gia thi");
            }
            else
            {
                int studentId = Convert.ToInt32(studentIdStr);
                bool isExisted = db.PlacementTests.Any(t => t.Student_ID == studentId);

                if (isExisted)
                {
                    ModelState.AddModelError("Student_ID", "Học viên này đã được xếp lịch thi trước đó rồi. Không thể tạo thêm!");
                }
            }

            if (string.IsNullOrEmpty(testDateStr))
            {
                ModelState.AddModelError("TestDate", "Vui lòng chọn ngày thi");
            }

            if (ModelState.IsValid)
            {
                test.Student_ID = Convert.ToInt32(studentIdStr);
                test.TestDate = Convert.ToDateTime(testDateStr).Date;
                test.Notes = string.IsNullOrEmpty(notes) ? null : notes.Trim();
                test.Score = null;
                test.ResultLevel = null;

                db.PlacementTests.InsertOnSubmit(test);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Students = db.Students.ToList();
            return View("~/Views/Admin/AdminPlacementTest/Create.cshtml", test);
        }

        public ActionResult Edit(int? id)
        {
            var test = db.PlacementTests.SingleOrDefault(t => t.Test_ID == id);
            if (test == null) return HttpNotFound();

            return View("~/Views/Admin/AdminPlacementTest/Edit.cshtml", test);
        }

        [HttpPost]
        public ActionResult Edit(int? id, FormCollection collection)
        {
            var test = db.PlacementTests.SingleOrDefault(t => t.Test_ID == id);
            if (test == null) return HttpNotFound();

            var scoreStr = collection["Score"];
            var resultLevel = collection["ResultLevel"];
            var notes = collection["Notes"];

            if (!string.IsNullOrEmpty(scoreStr))
            {
                try
                {
                    var score = Convert.ToDecimal(scoreStr);
                    if (score < 0 || score > 10)
                    {
                        ModelState.AddModelError("Score", "Điểm số phải nằm trong hệ điểm từ 0.00 đến 10.00");
                    }
                    test.Score = score;
                }
                catch (FormatException)
                {
                    ModelState.AddModelError("Score", "Điểm số không đúng định dạng số thập phân");
                }
            }
            else
            {
                test.Score = null;
            }

            if (ModelState.IsValid)
            {
                test.ResultLevel = string.IsNullOrEmpty(resultLevel) ? null : resultLevel.Trim();
                test.Notes = string.IsNullOrEmpty(notes) ? null : notes.Trim();

                if (!string.IsNullOrEmpty(collection["TestDate"]))
                {
                    test.TestDate = Convert.ToDateTime(collection["TestDate"]).Date;
                }

                db.SubmitChanges();
                return RedirectToAction("Index");
            }

            return View("~/Views/Admin/AdminPlacementTest/Edit.cshtml", test);
        }
    }
}