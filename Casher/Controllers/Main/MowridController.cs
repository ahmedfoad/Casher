using Casher.Models;
using Casher.Models.Administration;
using System.Data.Entity;
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

namespace Casher.Controllers.Main
{
    public class MowridController : Controller
    {
        DB_cashierEntities db = new DB_cashierEntities();
        string[] allFormats = { "yyyy/MM/dd", "yyyy/M/d", "dd/MM/yyyy", "d/M/yyyy", "dd/M/yyyy", "d/MM/yyyy", "yyyy-MM-dd", "yyyy-M-d", "dd-MM-yyyy", "d-M-yyyy", "dd-M-yyyy", "d-MM-yyyy", "yyyy MM dd", "yyyy M d", "dd MM yyyy", "d M yyyy", "dd M yyyy", "d MM yyyy" };
        ErrorViewModel Error = new ErrorViewModel();

        private int recordsPerPage = 300;

        //جلب الموردين لفاتورة المشترى----------------------
        [HttpGet]
        public ActionResult GetMowrid()
        {
            return PartialView();
        }
        //----------------------------------------
        [HttpGet]
        public async Task<ActionResult> getAllMowrid(int? page, string Search)
        {
            int? skipRecords = (page != null ? page.Value : 0) * recordsPerPage;

            var AllRecords = await db.Mowrids
              .Where(s => string.IsNullOrEmpty(Search) ? true : s.Name.Contains(Search))
              .OrderBy(s => s.ID).Skip(skipRecords != null ? skipRecords.Value : 0)
              .Take(recordsPerPage).Select(a=>new ClsMowrid
              {
                  ID=a.ID,
                  Name = a.Name,
                  JawalNO =a.JawalNO,
                  JawalNO2=a.JawalNO2,
                  Address=a.Address
              }).OrderBy(a=>a.Name).ToListAsync();
            var list = JsonConvert.SerializeObject(AllRecords,
Formatting.None,
new JsonSerializerSettings()
{
    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
});

            return Content(list, "application/json");
            //return Json(AllRecords, JsonRequestBehavior.AllowGet);
        }
       

        [HttpPost]
        public async Task<ActionResult> Insert(Mowrid model)
        {
            db.Mowrids.Add(model);
            await db.SaveChangesAsync();
            Error.ErrorName = "تم الإضافة بنجاح ... جاري إعادة تحميل الصفحة";
            Error.ID = model.ID;
            return Json(Error, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public async Task<ActionResult> loadMowrid(int id)
        {
            Mowrid Mowrid = await db.Mowrids.FindAsync(id);

            ClsMowrid ClsMowrid = new ClsMowrid {
                ID=Mowrid.ID,
                Name=Mowrid.Name,
                JawalNO=Mowrid.JawalNO,
                JawalNO2=Mowrid.JawalNO2,
                Address=Mowrid.Address
            };
            var list = JsonConvert.SerializeObject(ClsMowrid,
Formatting.None,
new JsonSerializerSettings()
{
    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
});

            return Content(list, "application/json");
            
        }

        [HttpGet]
        public ActionResult Operation()
        {

            return View();
        }
        //************************************************************************************************************
        //*************************الفاتورة**************************************************************************
        //************************************************************************************************************
        [HttpGet]
        public ActionResult Invoice_Mowrid()
        {
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> loadInvoice(int id)
        {
            Cls_Invoice_mowrid Cls_Invoice_mowrid = new Cls_Invoice_mowrid();
            Invoice_Mowrid Invoice_Mowrid = await db.Invoice_Mowrid.FindAsync(id);
            if (Invoice_Mowrid != null)
            {
                Cls_Invoice_mowrid.User = Invoice_Mowrid.User;

                Cls_Invoice_mowrid.ID = Invoice_Mowrid.ID;
                Cls_Invoice_mowrid.Mowrid_id = Invoice_Mowrid.Mowrid_id;
                Cls_Invoice_mowrid.Mowrid_Name = Invoice_Mowrid.Mowrid.Name;
                Cls_Invoice_mowrid.Date_Invoice = Invoice_Mowrid.Date_Invoice.ToString("yyyy-mm-dd");
                Cls_Invoice_mowrid.Date_Invoice_Hijri = Invoice_Mowrid.Date_Invoice_Hijri;
                Cls_Invoice_mowrid.Price = Invoice_Mowrid.Price;
                Cls_Invoice_mowrid.Total_Sadad = Invoice_Mowrid.Total_Sadad;
                Cls_Invoice_mowrid.User_ID = Invoice_Mowrid.User_ID;
                Cls_Invoice_mowrid.ComputerName = Invoice_Mowrid.ComputerName;
                Cls_Invoice_mowrid.ComputerUser = Invoice_Mowrid.ComputerUser;
                Cls_Invoice_mowrid.InDate = Invoice_Mowrid.InDate.ToString("yyyy-mm-dd");
                Cls_Invoice_mowrid.ClsInvoiceMowrid_Product = new List<ClsInvoiceMowrid_Product>();
                foreach (var item in Invoice_Mowrid.Invoice_Mowrid_Product)
                {

                    Cls_Invoice_mowrid.ClsInvoiceMowrid_Product.Add(
                        new ClsInvoiceMowrid_Product
                        {
                            ID = item.ID,
                            Invoice_Mowrid_Id = item.Invoice_Mowrid_Id,
                            Product_Id = item.Product_Id,
                            Product_Name = item.Product.Name,
                            Date_Poduction = item.Date_Poduction.ToString("yyyy-mm-dd"),
                            Date_Poduction_Hijri = item.Date_Poduction_Hijri,
                            Date_Expiration = item.Date_Expiration.ToString("yyyy-mm-dd"),
                            Date_Expiration_Hijri = item.Date_Expiration_Hijri,
                            Amount_ByJumla = item.Amount_ByJumla,
                            Carton_Count = item.Carton_Count,
                            Amount_ByUnit = item.Amount_ByUnit,
                            Price = item.Price,
                            Store_id = 1 //*****************************
                    }
                        );
                }
            }
           
            var list = JsonConvert.SerializeObject(Cls_Invoice_mowrid,
Formatting.None,
new JsonSerializerSettings()
{
   ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
});

            return Content(list, "application/json");
        }
        [HttpPost]
        public async Task<ActionResult> InsertInvoice(Cls_Invoice_mowrid Cls_Invoice_mowrid)
        {
            DateTime Date_Invoice = DateTime.ParseExact(Cls_Invoice_mowrid.Date_Invoice, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            System.Globalization.DateTimeFormatInfo HijriDTFI;
            HijriDTFI = new System.Globalization.CultureInfo("ar-SA", false).DateTimeFormat;
            HijriDTFI.Calendar = new System.Globalization.HijriCalendar();
            HijriDTFI.ShortDatePattern = "dd/MM/yyyy";
            string Date_Invoice_Hijri = Date_Invoice.Date.ToString("dd/MM/yyyy", HijriDTFI);

            WindowsIdentity identity = HttpContext.Request.LogonUserIdentity;
            List<string> computerDetails = identity.Name.Split('\\').ToList();

            Invoice_Mowrid _Invoice_Mowrid = new Invoice_Mowrid
            {
                Mowrid_id = Cls_Invoice_mowrid.Mowrid_id,
                Date_Invoice = Date_Invoice,
                Date_Invoice_Hijri = Date_Invoice_Hijri,
                Price = Cls_Invoice_mowrid.Price,
                Total_Sadad = 0,
                User_ID = int.Parse(Session["UserID"].ToString()),
                ComputerName = computerDetails[0],
                ComputerUser = computerDetails[1],
                InDate = DateTime.Now
            };
            db.Invoice_Mowrid.Add(_Invoice_Mowrid);
            await db.SaveChangesAsync();
            foreach (var item in Cls_Invoice_mowrid.ClsInvoiceMowrid_Product)
            {

                int index = Cls_Invoice_mowrid.ClsInvoiceMowrid_Product.IndexOf(item);
                int count = Cls_Invoice_mowrid.ClsInvoiceMowrid_Product.Count;

                if (index < (count - 1))
                {
                    DateTime Date_Poduction = DateTime.ParseExact(item.Date_Poduction, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    DateTime Date_Expiration = DateTime.ParseExact(item.Date_Expiration, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    string Date_Poduction_Hijri = Date_Poduction.Date.ToString("dd/MM/yyyy", HijriDTFI);
                    string Date_Expiration_Hijri = Date_Expiration.Date.ToString("dd/MM/yyyy", HijriDTFI);
                    Invoice_Mowrid_Product Invoice_Mowrid_Product = new Invoice_Mowrid_Product
                    {
                        Invoice_Mowrid_Id = _Invoice_Mowrid.ID,
                        Product_Id = item.Product_Id,
                        Store_id = 1, //// ** محتاج تعديل
                        Date_Poduction = Date_Poduction,
                        Date_Poduction_Hijri = Date_Poduction_Hijri,
                        Date_Expiration = Date_Expiration,
                        Date_Expiration_Hijri = Date_Expiration_Hijri,
                        Amount_ByJumla = item.Amount_ByJumla,
                        Carton_Count = item.Carton_Count,
                        Amount_ByUnit = item.Amount_ByUnit,
                        Price = item.Price
                    };
                    db.Invoice_Mowrid_Product.Add(Invoice_Mowrid_Product);
                }
            }
            await db.SaveChangesAsync();
            //db.Invoice_Mowrid.Add(_Invoice_Mowrid);

            Error.ErrorName = "تم الإضافة بنجاح ... جاري إعادة تحميل الصفحة";
            Error.ID = _Invoice_Mowrid.ID;
            return Json(Error, JsonRequestBehavior.AllowGet);

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