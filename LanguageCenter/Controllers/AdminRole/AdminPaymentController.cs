using LanguageCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LanguageCenter.Controllers.AdminRole
{
    public class AdminPaymentController : Controller
    {
        dbLanguageCenterDataContext db = new dbLanguageCenterDataContext();

        public ActionResult Index()
        {
            var paymentList = db.Payments.Distinct().ToList();
            return View("~/Views/Admin/AdminPayment/Index.cshtml", paymentList);
        }

        public ActionResult Edit(int? id)
        {
            var payment = db.Payments.SingleOrDefault(p => p.Payment_ID == id);
            if (payment == null) return HttpNotFound();

            return View("~/Views/Admin/AdminPayment/Edit.cshtml", payment);
        }

        [HttpPost]
        public ActionResult Edit(int? id, FormCollection collection)
        {
            var payment = db.Payments.SingleOrDefault(p => p.Payment_ID == id);
            if (payment == null) return HttpNotFound();

            var p_method = collection["PaymentMethod"];
            var p_status = collection["Status"];

            if (!string.IsNullOrEmpty(collection["Amount"]))
            {
                try
                {
                    var p_amount = Convert.ToDecimal(collection["Amount"]);
                    if (p_amount < 0)
                    {
                        ModelState.AddModelError("Amount", "Số tiền thanh toán không được âm");
                    }
                    payment.Amount = p_amount;
                }
                catch (FormatException)
                {
                    ModelState.AddModelError("Amount", "Số tiền phải là số thập phân hợp lệ");
                }
            }

            if (string.IsNullOrEmpty(p_method))
            {
                payment.PaymentMethod = null;
            }
            else
            {
                payment.PaymentMethod = p_method.Trim();
            }

            if (string.IsNullOrEmpty(p_status))
            {
                ModelState.AddModelError("Status", "Trạng thái thanh toán không được để trống");
            }
            else
            {
                p_status = p_status.Trim();
                if (p_status == "Đã thanh toán" || p_status == "Chờ thanh toán" || p_status == "Thất bại")
                {
                    payment.Status = p_status;
                    if (p_status == "Đã thanh toán" && payment.PaymentDate == null)
                    {
                        payment.PaymentDate = DateTime.Now;
                    }
                }
                else
                {
                    ModelState.AddModelError("Status", "Trạng thái không hợp lệ");
                }
            }

            if (!string.IsNullOrEmpty(collection["PaymentDate"]))
            {
                try
                {
                    payment.PaymentDate = Convert.ToDateTime(collection["PaymentDate"]);
                }
                catch (FormatException)
                {
                    ModelState.AddModelError("PaymentDate", "Ngày thanh toán không đúng định dạng");
                }
            }

            if (ModelState.IsValid)
            {
                db.SubmitChanges();
                return RedirectToAction("Index");
            }

            return View("~/Views/Admin/AdminPayment/Edit.cshtml", payment);
        }

        [HttpPost]
        public ActionResult ConfirmQuick(int id)
        {
            var payment = db.Payments.SingleOrDefault(p => p.Payment_ID == id);
            if (payment != null)
            {
                payment.Status = "Đã thanh toán";
                payment.PaymentDate = DateTime.Now;
                db.SubmitChanges();
            }
            return RedirectToAction("Index");
        }
    }
}