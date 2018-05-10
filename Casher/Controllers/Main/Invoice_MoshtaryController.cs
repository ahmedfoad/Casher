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
    public class Invoice_MoshtaryController : Controller
    {
        DB_cashierEntities db = new DB_cashierEntities();
        string[] allFormats = { "yyyy/MM/dd", "yyyy/M/d", "dd/MM/yyyy", "d/M/yyyy", "dd/M/yyyy", "d/MM/yyyy", "yyyy-MM-dd", "yyyy-M-d", "dd-MM-yyyy", "d-M-yyyy", "dd-M-yyyy", "d-MM-yyyy", "yyyy MM dd", "yyyy M d", "dd MM yyyy", "d M yyyy", "dd M yyyy", "d MM yyyy" };
        ErrorViewModel Error = new ErrorViewModel();
        private int recordsPerPage = 300;

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
        public ActionResult Invoice_Moshtary()
        {
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> loadInvoice(int id)
        {
            Cls_Invoice_Moshtary Cls_Invoice_Moshtary = new Cls_Invoice_Moshtary();
            Invoice_Moshtary Invoice_Moshtary = await db.Invoice_Moshtary.FindAsync(id);
            Cls_Invoice_Moshtary.ID = Invoice_Moshtary.ID;
            Cls_Invoice_Moshtary.Moshtary_id = Invoice_Moshtary.Moshtary_id;
            Cls_Invoice_Moshtary.Moshtary_Name = (Invoice_Moshtary.Moshtary_id != null) ?Invoice_Moshtary.Moshtary.Name:"";
            Cls_Invoice_Moshtary.Date_Invoice = Invoice_Moshtary.Date_Invoice.ToString("yyyy-mm-dd");
            Cls_Invoice_Moshtary.Date_Invoice_Hijri = Invoice_Moshtary.Date_Invoice_Hijri;
            Cls_Invoice_Moshtary.Price = Invoice_Moshtary.Price;
            Cls_Invoice_Moshtary.Total_Sadad = Invoice_Moshtary.Total_Sadad;
            Cls_Invoice_Moshtary.User_ID = Invoice_Moshtary.User_ID;
            Cls_Invoice_Moshtary.ComputerName = Invoice_Moshtary.ComputerName;
            Cls_Invoice_Moshtary.ComputerUser = Invoice_Moshtary.ComputerUser;
            Cls_Invoice_Moshtary.InDate = Invoice_Moshtary.InDate.ToString("yyyy-mm-dd");
            Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product = new List<ClsInvoiceMoshtary_Product>();
            foreach (var item in Invoice_Moshtary.Invoice_Moshtary_Product)
            {

                Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product.Add(
                    new ClsInvoiceMoshtary_Product
                    {
                        ID = item.ID,
                        Invoice_Moshtary_Id = item.Invoice_Moshtary_Id,
                        Product_Id = item.Product_Id,
                        Product_Name = item.Product.Name,
                        Amount = item.Amount,
                        Price = item.Price,
                        Taxes=item.Taxes,
                        TotalPrice=item.TotalPrice
                    }
                    );
            }

            var list = JsonConvert.SerializeObject(Cls_Invoice_Moshtary,
Formatting.None,
new JsonSerializerSettings()
{
    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
});

            return Content(list, "application/json");
        }
        [HttpPost]
        public async Task<ActionResult> InsertInvoice(Cls_Invoice_Moshtary Cls_Invoice_Moshtary)
        {
            DateTime Date_Invoice = DateTime.Now;

            System.Globalization.DateTimeFormatInfo HijriDTFI;
            HijriDTFI = new System.Globalization.CultureInfo("ar-SA", false).DateTimeFormat;
            HijriDTFI.Calendar = new System.Globalization.HijriCalendar();
            HijriDTFI.ShortDatePattern = "dd/MM/yyyy";
            string Date_Invoice_Hijri = Date_Invoice.Date.ToString("dd/MM/yyyy", HijriDTFI);

            WindowsIdentity identity = HttpContext.Request.LogonUserIdentity;
            List<string> computerDetails = identity.Name.Split('\\').ToList();

            Invoice_Moshtary _Invoice_Moshtary = new Invoice_Moshtary
            {
                Moshtary_id = Cls_Invoice_Moshtary.Moshtary_id,
                Date_Invoice = Date_Invoice,
                Date_Invoice_Hijri = Date_Invoice_Hijri,
                Price = Cls_Invoice_Moshtary.Price,
                Total_Sadad = 0,
                User_ID = int.Parse(Session["UserID"].ToString()),
                ComputerName = computerDetails[0],
                ComputerUser = computerDetails[1],
                InDate = Date_Invoice
            };
            db.Invoice_Moshtary.Add(_Invoice_Moshtary);
            await db.SaveChangesAsync();
            foreach (var item in Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product)
            {

                int index = Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product.IndexOf(item);
                int count = Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product.Count;

                if (index < (count - 1))
                {
                    Invoice_Moshtary_Product Invoice_Moshtary_Product = new Invoice_Moshtary_Product
                    {
                        Invoice_Moshtary_Id = _Invoice_Moshtary.ID,
                        Product_Id = item.Product_Id,
                        Amount= item.Amount,
                        Price = item.Price
                    };
                    db.Invoice_Moshtary_Product.Add(Invoice_Moshtary_Product);
                }
            }
            await db.SaveChangesAsync();
            //db.Invoice_Moshtary.Add(_Invoice_Moshtary);

            Error.ErrorName = "تم الإضافة بنجاح ... جاري إعادة تحميل الصفحة";
            Error.ID = _Invoice_Moshtary.ID;
            return Json(Error, JsonRequestBehavior.AllowGet);

        }
        public async Task<ActionResult> PrintInvoice(int id)
        {
            Cls_Invoice_Moshtary Cls_Invoice_Moshtary = new Cls_Invoice_Moshtary();
            Invoice_Moshtary Invoice_Moshtary = await db.Invoice_Moshtary.FindAsync(id);
            Cls_Invoice_Moshtary.ID = Invoice_Moshtary.ID;
            Cls_Invoice_Moshtary.Moshtary_id = Invoice_Moshtary.Moshtary_id;
            Cls_Invoice_Moshtary.Moshtary_Name = (Invoice_Moshtary.Moshtary_id != null) ? Invoice_Moshtary.Moshtary.Name : "";
            Cls_Invoice_Moshtary.Date_Invoice = Invoice_Moshtary.Date_Invoice.ToString("yyyy-mm-dd");
            Cls_Invoice_Moshtary.Date_Invoice_Hijri = Invoice_Moshtary.Date_Invoice_Hijri;
            Cls_Invoice_Moshtary.Price = Invoice_Moshtary.Price;
            Cls_Invoice_Moshtary.Total_Sadad = Invoice_Moshtary.Total_Sadad;
            Cls_Invoice_Moshtary.User_ID = Invoice_Moshtary.User_ID;
            Cls_Invoice_Moshtary.ComputerName = Invoice_Moshtary.ComputerName;
            Cls_Invoice_Moshtary.ComputerUser = Invoice_Moshtary.ComputerUser;
            Cls_Invoice_Moshtary.InDate = Invoice_Moshtary.InDate.ToString("yyyy-mm-dd");
            Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product = new List<ClsInvoiceMoshtary_Product>();
            foreach (var item in Invoice_Moshtary.Invoice_Moshtary_Product)
            {

                Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product.Add(
                    new ClsInvoiceMoshtary_Product
                    {
                        ID = item.ID,
                        Invoice_Moshtary_Id = item.Invoice_Moshtary_Id,
                        Product_Id = item.Product_Id,
                        Product_Name = item.Product.Name,
                        Amount = item.Amount,
                        Price = item.Price,
                        Taxes = item.Taxes,
                        TotalPrice = item.TotalPrice
                    }
                    );
            }

            string fileName = ExportReport(Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product);
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

        internal string ExportReport(List<ClsInvoiceMoshtary_Product> ClsInvoiceMoshtary_Product)
        {
            Rpt_Invoice_Moshtary report = new Rpt_Invoice_Moshtary();
            report.DataSource = ClsInvoiceMoshtary_Product;
            string _path = System.Web.HttpContext.Current.Server.MapPath(@"~/Reports/pdf");
            Random random = new Random();
            string tick = DateTime.Now.Ticks.ToString();
            string reportPath = Path.Combine(_path, "Rpt_Invoice_Moshtary.pdf");


            PdfExportOptions pdfOptions = report.ExportOptions.Pdf;
            pdfOptions.Compressed = true;
            pdfOptions.ImageQuality = PdfJpegImageQuality.Low;
            pdfOptions.NeverEmbeddedFonts = "Tahoma;Courier New";
            pdfOptions.DocumentOptions.Application = "Human Resources Application";
            pdfOptions.DocumentOptions.Author = "مؤسسة حوران الشامل لقنية الحاسب الآلي والإنترنت";
            pdfOptions.DocumentOptions.Subject = "باركود الصنف";
            pdfOptions.DocumentOptions.Title = "باركود الصنف";

            report.ExportToPdf(reportPath);
            return "Rpt_Invoice_Moshtary";

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