using Casher.Models;
using Casher.Models.Administration;
using Casher.Models.Search;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Casher.GlobalClass;
using Casher.GlobalClass.Reports;
using System.Data.Entity;
using Casher.Models.Reports;
using Casher.Reports.Product;
using DevExpress.XtraPrinting;

namespace Casher.Controllers.Report
{
    public class Report_ProductController : Controller
    {
        DB_cashierEntities db = new DB_cashierEntities();
        private int recordsPerPage = 50;
        ErrorViewModel Error = new ErrorViewModel();
        int viewid = 2501;//تقرير جرد الاصناف
        [HttpGet]
        public async Task<ActionResult> AllProducts()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Users", new { returnUrl = Request.Url.ToString() });
            decimal UserID = decimal.Parse(Session["UserID"].ToString());
            UserView UserView = db.UserViews.Where(a => a.UserID == UserID && a.View.ID == viewid).FirstOrDefault(); // 2470=الصنف

            if (Session["Role"].ToString() == "1" || (UserView != null && UserView.Role_Enter == true))
            {
                if (Session["RoleName"].ToString() != "BigBoss")
                {
                    UserAction UserAction = new UserAction
                    {
                        Userid = int.Parse(Session["UserID"].ToString()),
                        Viewid = viewid,
                        ActionDate = DateTime.Now,
                        Action = _Action.استعلام.ToString(),
                        Operation = "الدخول إلى نافذة تقرير جرد الاصناف"
                    };
                    db.UserActions.Add(UserAction);
                    await db.SaveChangesAsync();
                }
                return View();
            }
            else
            {
                Error.ErrorFullNumber = "AR-Open-000";
                Error.ErrorNumber = "000";
                Error.Url = "/Home";
                Error.ErrorName = "ليس لديك صلاحية الدخول إلى نافذة تقرير جرد الاصناف";
                return View("~/Views/Shared/ErrorPage.cshtml", Error);
            }
        }
        [HttpPost]
        public async Task<string> AllProducts(Cls_Product Cls_Product)
        {
            if (Session["UserID"] != null)
            {
                
                
                List<Cls_ProductSummary> Cls_ProductSummary =
                    await db.Products.Select(a =>
                new Cls_ProductSummary
                {
                    Product_Name = a.Name,
                    Company_Name=a.Company.Name,
                    Department_Name=a.Company.Department.Name,
                    Current_Amount=0,
                    Prev_Amount=0,
                    Sell_Amount=0
                }).ToListAsync();
               
                string _path = System.Web.HttpContext.Current.Server.MapPath(@"~/Reports/pdf");
                Random random = new Random();
                string tick = DateTime.Now.Ticks.ToString();
                string reportPath = Path.Combine(_path, "Rpt_Products.pdf");

                Rpt_Products report = new Rpt_Products();
                report.DataSource = Cls_ProductSummary;

                PdfExportOptions pdfOptions = report.ExportOptions.Pdf;
                pdfOptions.Compressed = true;
                pdfOptions.ImageQuality = PdfJpegImageQuality.Low;
                pdfOptions.NeverEmbeddedFonts = "Tahoma;Courier New";
                pdfOptions.DocumentOptions.Application = "Human Resources Application";
                pdfOptions.DocumentOptions.Author = "مؤسسة حوران الشامل لقنية الحاسب الآلي والإنترنت";
                pdfOptions.DocumentOptions.Subject = "باركود الصنف";
                pdfOptions.DocumentOptions.Title = "باركود الصنف";
                report.ExportToPdf(reportPath);

                return "/Reports/pdf/Rpt_Products.pdf";
            }
            else
            {
                Error.ErrorFullNumber = "AR-Logout-089";
                Error.ErrorNumber = "089";
                Error.Url = "URLReport";
                Error.ErrorName = "تم تسجيل خروجك آلياً لانتهاء المدة المسموح بها";
                return "";
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}