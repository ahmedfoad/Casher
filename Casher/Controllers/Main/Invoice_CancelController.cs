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
using System.Linq; using System.Threading.Tasks;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Casher.GlobalClass;
using Casher.GlobalClass.Reports;
using Casher.Reports.Invoices;
using DevExpress.XtraPrinting;
using System.Data.Entity;

namespace Casher.Controllers.Main
{
    public class Invoice_CancelController : Controller
    {
        DB_cashierEntities db = new DB_cashierEntities();
        string[] allFormats = { "yyyy/MM/dd", "yyyy/M/d", "dd/MM/yyyy", "d/M/yyyy", "dd/M/yyyy", "d/MM/yyyy", "yyyy-MM-dd", "yyyy-M-d", "dd-MM-yyyy", "d-M-yyyy", "dd-M-yyyy", "d-MM-yyyy", "yyyy MM dd", "yyyy M d", "dd MM yyyy", "d M yyyy", "dd M yyyy", "d MM yyyy" };
        ErrorViewModel Error = new ErrorViewModel();
        private int recordsPerPage = 300;
        //جلب شاشاة دخول للمدير لتأكيد عملية الارجاع----------------------
        [HttpGet]
        public ActionResult AuthorizeManager()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<ActionResult> IsAuthorizeManager(Cls_LogIn Cls_LogIn)
        {
            User User = await db.Users.Where(a => a.Username == Cls_LogIn.UserName && a.Password == Cls_LogIn.Password).FirstOrDefaultAsync();
            if (User != null)
            {
                int viewid = 2477;//ارتجاع صنف
                UserView UserView = await db.UserViews.Where(a => a.UserID == User.ID && a.ViewID == viewid).FirstOrDefaultAsync();
                if (Session["Role"].ToString() == "1" || (UserView != null && UserView.Role_Save == true))
                {
                    
                    return Content("1", "application/json");
                }
                else
                {
                    return Content("2", "application/json");
                }
            }
            {
                return Content("3", "application/json");
            }
        }
        //جلب الموردين لفاتورة المشترى----------------------
        [HttpGet]
        public ActionResult GetMoshtary()
        {
            return PartialView();
        }
        //----------------------------------------
        [HttpGet]
        public async Task<ActionResult> getAllMoshtary(int? page, string Search)
        {
            int? skipRecords = (page != null ? page.Value : 0) * recordsPerPage;

            var AllRecords = await db.Moshtaries
              .Where(s => string.IsNullOrEmpty(Search) ? true : s.Name.Contains(Search))
              .OrderBy(s => s.ID).Skip(skipRecords != null ? skipRecords.Value : 0)
              .Take(recordsPerPage).Select(a=>new Cls_Moshtary
              {
                  ID=a.ID,
                  Name=a.Name,
                  Sejil=a.Sejil,
                  JawalNO=a.JawalNO,
                  Address=a.Address
              }).ToListAsync();
            var list = JsonConvert.SerializeObject(AllRecords,
Formatting.None,
new JsonSerializerSettings()
{
    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
});

            return Content(list, "application/json");
            //return Json(AllRecords, JsonRequestBehavior.AllowGet);
        }
       
       

      
        //************************************************************************************************************
        //*************************الفاتورة**************************************************************************
        //************************************************************************************************************
        [HttpGet]
        public ActionResult Invoice_Cancel()
        {
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> loadInvoice(int id)
        {
            Cls_Invoice_Cancel Cls_Invoice_Cancel = new Cls_Invoice_Cancel();
            Invoice_Cancel Invoice_Cancel = await db.Invoice_Cancel.FindAsync(id);
            Cls_Invoice_Cancel.ID = Invoice_Cancel.ID;
            Cls_Invoice_Cancel.Moshtary_id = Invoice_Cancel.Moshtary_id;
            Cls_Invoice_Cancel.Moshtary_Name = (Invoice_Cancel.Moshtary_id != null) ?Invoice_Cancel.Moshtary.Name:"";
            Cls_Invoice_Cancel.Date_Invoice = Invoice_Cancel.Date_Invoice.ToString("yyyy-mm-dd");
            Cls_Invoice_Cancel.Date_Invoice_Hijri = Invoice_Cancel.Date_Invoice_Hijri;
            Cls_Invoice_Cancel.Price = Invoice_Cancel.Price;
            Cls_Invoice_Cancel.Total_Sadad = Invoice_Cancel.Total_Sadad;
            Cls_Invoice_Cancel.User_ID = Invoice_Cancel.User_ID;
            Cls_Invoice_Cancel.ComputerName = Invoice_Cancel.ComputerName;
            Cls_Invoice_Cancel.ComputerUser = Invoice_Cancel.ComputerUser;
            Cls_Invoice_Cancel.InDate = Invoice_Cancel.InDate.ToString("yyyy-mm-dd");
            Cls_Invoice_Cancel.ClsInvoiceCancel_Product = new List<ClsInvoiceCancel_Product>();
            foreach (var item in Invoice_Cancel.Invoice_Cancel_Product)
            {

                Cls_Invoice_Cancel.ClsInvoiceCancel_Product.Add(
                    new ClsInvoiceCancel_Product
                    {
                        ID = item.ID,
                        Invoice_Cancel_Id = item.Invoice_Cancel_Id,
                        Product_Id = item.Product_Id,
                        Product_Name = item.Product.Name,
                        Amount = item.Amount,
                        Price = item.Price,
                        Taxes=item.Taxes,
                        TotalPrice=item.TotalPrice,
                        Date_Poduction = item.Date_Poduction.ToString("yyyy-mm-dd"),
                        Date_Poduction_Hijri = item.Date_Poduction_Hijri,
                        Date_Expiration = item.Date_Expiration.ToString("yyyy-mm-dd"),
                        Date_Expiration_Hijri = item.Date_Expiration_Hijri,
                    }
                    );
            }

            var list = JsonConvert.SerializeObject(Cls_Invoice_Cancel,
Formatting.None,
new JsonSerializerSettings()
{
    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
});

            return Content(list, "application/json");
        }
        [HttpPost]
        public async Task<ActionResult> InsertInvoice(Cls_Invoice_Cancel Cls_Invoice_Cancel)
        {
            DateTime Date_Invoice = DateTime.Now;

            System.Globalization.DateTimeFormatInfo HijriDTFI;
            HijriDTFI = new System.Globalization.CultureInfo("ar-SA", false).DateTimeFormat;
            HijriDTFI.Calendar = new System.Globalization.HijriCalendar();
            HijriDTFI.ShortDatePattern = "dd/MM/yyyy";
            string Date_Invoice_Hijri = Date_Invoice.Date.ToString("dd/MM/yyyy", HijriDTFI);

            WindowsIdentity identity = HttpContext.Request.LogonUserIdentity;
            List<string> computerDetails = identity.Name.Split('\\').ToList();

            Invoice_Cancel _Invoice_Cancel = new Invoice_Cancel
            {
                Moshtary_id = Cls_Invoice_Cancel.Moshtary_id,
                Date_Invoice = Date_Invoice,
                Date_Invoice_Hijri = Date_Invoice_Hijri,
                Price = Cls_Invoice_Cancel.Price,
                Total_Sadad = 0,
                User_ID = int.Parse(Session["UserID"].ToString()),
                ComputerName = computerDetails[0],
                ComputerUser = computerDetails[1],
                InDate = Date_Invoice
            };
            db.Invoice_Cancel.Add(_Invoice_Cancel);
            await db.SaveChangesAsync();
            foreach (var item in Cls_Invoice_Cancel.ClsInvoiceCancel_Product)
            {

                int index = Cls_Invoice_Cancel.ClsInvoiceCancel_Product.IndexOf(item);
                int count = Cls_Invoice_Cancel.ClsInvoiceCancel_Product.Count;

                if (index < (count - 1))
                {
                    DateTime Date_Poduction = DateTime.ParseExact(item.Date_Poduction, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    DateTime Date_Expiration = DateTime.ParseExact(item.Date_Expiration, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    string Date_Poduction_Hijri = Date_Poduction.Date.ToString("dd/MM/yyyy", HijriDTFI);
                    string Date_Expiration_Hijri = Date_Expiration.Date.ToString("dd/MM/yyyy", HijriDTFI);
                    Invoice_Cancel_Product Invoice_Cancel_Product = new Invoice_Cancel_Product
                    {
                        Invoice_Cancel_Id = _Invoice_Cancel.ID,
                        Product_Id = item.Product_Id,
                        Amount= item.Amount,
                        Price = item.Price,
                        Store_id = 1, //// ** محتاج تعديل
                        Date_Poduction = Date_Poduction,
                        Date_Poduction_Hijri = Date_Poduction_Hijri,
                        Date_Expiration = Date_Expiration,
                        Date_Expiration_Hijri = Date_Expiration_Hijri,
                    };
                    db.Invoice_Cancel_Product.Add(Invoice_Cancel_Product);
                }
            }
            await db.SaveChangesAsync();
            //db.Invoice_Cancel.Add(_Invoice_Cancel);

            Error.ErrorName = "تم الإضافة بنجاح ... جاري إعادة تحميل الصفحة";
            Error.ID = _Invoice_Cancel.ID;
            return Json(Error, JsonRequestBehavior.AllowGet);

        }
        public ActionResult PrintInvoice(decimal id)
        {
            string fileName = ExportReport();
            if (fileName != "")
            {
                string _path = System.Web.HttpContext.Current.Server.MapPath(@"~/Reports/pdf");
                string path = Path.Combine(_path, fileName + ".pdf");
                return File(path, "application/pdf");
            }



            //if (Session["UserID"] != null)
            //{
            //    decimal UserID = decimal.Parse(Session["UserID"].ToString());
            //    checkAccess = userRep.checkUserAccess(UserID, URLReport);
            //    if (Session["Role"].ToString() == "1" || (checkAccess != null && checkAccess.Save == 1))
            //    {
            //        string fileName = listExportRep.ExportBarCodeReport(id);
            //        if (fileName != "")
            //        {
            //            if (Session["RoleName"].ToString() != "BigBoss")
            //            {
            //                ActionModel.ID = ActionRep.GetID();
            //                ActionModel.UserName = Session["Name"].ToString();
            //                ActionModel.ProgramUserID = decimal.Parse(Session["UserID"].ToString());
            //                ActionModel.Tim = time[0] + "h:" + time[1] + "m:" + Math.Round(decimal.Parse(time[2])) + "s";
            //                ActionModel.Dat = "14" + date[2] + "/" + date[1] + "/" + date[0];
            //                ActionModel.Action = "طباعة الباركود للمعاملة الصادرة التي تحمل الرقم المسلسل : " + id;
            //                ActionModel.Operation = ((Operation)5).ToString();
            //                ActionModel.ViewName = viewName;
            //                ActionRep.AddNewRecord(ActionModel);
            //            }
            //            string path = Path.Combine(UniversalRepository.GlobalVariables.GlobalReportPath, fileName + ".pdf");
            //            return File(path, "application/pdf");
            //        }
            //        else
            //        {
            //            Error.ErrorFullNumber = "AR-Print-007";
            //            Error.ErrorNumber = "007";
            //            Error.Url = "/Home";
            //            Error.ErrorName = "حدث خطأ أثناء طباعة الملف برجاء المحاولة مرة آخري";
            //            return View("~/Views/Shared/ErrorPage.cshtml", Error);
            //        }
            //    }
            //    else
            //    {
            //        Error.ErrorFullNumber = "AR-Print-000";
            //        Error.ErrorNumber = "000";
            //        Error.Url = "/Home";
            //        Error.ErrorName = "ليس لديك صلاحية طباعة  الباركود للمعاملة الصادرة";
            //        return View("~/Views/Shared/ErrorPage.cshtml", Error);
            //    }
            //}
            else
            {
                Error.ErrorFullNumber = "AR-Logout-089";
                Error.ErrorNumber = "089";
                Error.Url = "/ListExport/Operation/" + id;
                Error.ErrorName = "تم تسجيل خروجك آلياً لانتهاء المدة المسموح بها";
                return View("~/Views/Shared/ErrorPage.cshtml", Error);
            }
        }

        internal string ExportReport()
        {
            Rpt_Invoice_Moshtary report = new Rpt_Invoice_Moshtary();
            string _path = System.Web.HttpContext.Current.Server.MapPath(@"~/Reports/pdf");
            Random random = new Random();
            string tick = DateTime.Now.Ticks.ToString();
            string reportPath = Path.Combine(_path, "Rpt_Invoice_Cancel.pdf");


            PdfExportOptions pdfOptions = report.ExportOptions.Pdf;
            pdfOptions.Compressed = true;
            pdfOptions.ImageQuality = PdfJpegImageQuality.Low;
            pdfOptions.NeverEmbeddedFonts = "Tahoma;Courier New";
            pdfOptions.DocumentOptions.Application = "Human Resources Application";
            pdfOptions.DocumentOptions.Author = "مؤسسة حوران الشامل لقنية الحاسب الآلي والإنترنت";
            pdfOptions.DocumentOptions.Subject = "باركود الصنف";
            pdfOptions.DocumentOptions.Title = "باركود الصنف";

            report.ExportToPdf(reportPath);
            return "Rpt_Invoice_Cancel";

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