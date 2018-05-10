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
using System.Data.Entity;

namespace Casher.Controllers.Main
{
    public class SearchController : Controller
    {
        DB_cashierEntities db = new DB_cashierEntities();
        private int recordsPerPage = 50;
        ErrorViewModel Error = new ErrorViewModel();
        #region Search Product
        int viewid_Product = 2482;// الإستعلام عن صنف
        // GET: Search
        public async Task<ActionResult> Product()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Users", new { returnUrl = Request.Url.ToString() });
            decimal UserID = decimal.Parse(Session["UserID"].ToString());
            UserView UserView = db.UserViews.Where(a => a.UserID == UserID && a.View.ID == viewid_Product).FirstOrDefault(); // 2470=الصنف

            if (Session["Role"].ToString() == "1" || (UserView != null && UserView.Role_Enter == true))
            {
                if (Session["RoleName"].ToString() != "BigBoss")
                {
                    UserAction UserAction = new UserAction
                    {
                        Userid = int.Parse(Session["UserID"].ToString()),
                        Viewid = viewid_Product,
                        ActionDate = DateTime.Now,
                        Action = _Action.استعلام.ToString(),
                        Operation = "الدخول إلى نافذة البحث والإستعلام عن الاصناف"
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
                Error.ErrorName = "ليس لديك صلاحية الدخول إلى الإستعلام عن المعاملات الصادرة";
                return View("~/Views/Shared/ErrorPage.cshtml", Error);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Product(int? page, Srch_Product Srch_Product)
        {
            var skipRecords = ((page != null && page >= 1) ? page.Value : 0) * recordsPerPage;
            var AllRecords = await db.Products
             .Where(s => (string.IsNullOrEmpty(Srch_Product.Product_Name) ? true : s.Name.Contains(Srch_Product.Product_Name))
                    //&& s.Carton_Barcode == 7610100055164)
                    )
             .OrderBy(s => s.ID).Skip(skipRecords)
             .Take(recordsPerPage).Select(a=>new ClsProduct {
                 ID=a.ID,
                 Name=a.Name,
                 Company_Name=a.Company.Name,
                 Department_Name=a.Company.Department.Name,
                 Price_Unit=a.Price_Unit,
                 Price_Mowrid=a.Price_Mowrid??default(decimal),
                 Taxes= a.Taxes,
                 Taxes_Price = Math.Round(((a.Price_Unit * (a.Taxes / 100))), 2),
                 TotalPrice = Math.Round(((a.Price_Unit) + (a.Price_Unit * (a.Taxes / 100))), 2),
                 Carton_Barcode=(a.Carton_Barcode != null)? a.Carton_Barcode.Value.ToString():"",
                 Carton_ProCount = (a.Carton_ProCount != null) ? a.Carton_ProCount.Value.ToString() : ""

             }).OrderBy(a=>a.Name).ToListAsync();
            var list = JsonConvert.SerializeObject(AllRecords,
Formatting.None,
new JsonSerializerSettings()
{
    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
});

            return Content(list, "application/json");

        }
        #endregion

        #region Search Invoice_Moshtary
        int viewid_Invoice_Moshtary = 2483;// الإستعلام عن حركة بيع
        // GET: Search
        public async Task<ActionResult> Invoice_Moshtary()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Users", new { returnUrl = Request.Url.ToString() });
            decimal UserID = decimal.Parse(Session["UserID"].ToString());
            UserView UserView = db.UserViews.Where(a => a.UserID == UserID && a.View.ID == viewid_Invoice_Moshtary).FirstOrDefault(); // 2470=الصنف

            if (Session["Role"].ToString() == "1" || (UserView != null && UserView.Role_Enter == true))
            {
                if (Session["RoleName"].ToString() != "BigBoss")
                {
                    UserAction UserAction = new UserAction
                    {
                        Userid = int.Parse(Session["UserID"].ToString()),
                        Viewid = viewid_Invoice_Moshtary,
                        ActionDate = DateTime.Now,
                        Action = _Action.استعلام.ToString(),
                        Operation = "الدخول إلى نافذة البحث والإستعلام عن حركة بيع"
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
                Error.ErrorName = "ليس لديك صلاحية الدخول إلى الإستعلام عن المعاملات الصادرة";
                return View("~/Views/Shared/ErrorPage.cshtml", Error);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Invoice_Moshtary(int? page, Srch_Invoice_Moshtary Srch_Invoice_Moshtary)
        {
             var skipRecords = ((page != null && page >= 1) ? page.Value : 0) * recordsPerPage;
            var AllRecords = await db.Invoice_Moshtary
             //.Where(s => string.IsNullOrEmpty(UserName) ? true : s.Name.Contains(UserName))
             .OrderBy(s => s.ID).Skip(skipRecords)
             .Take(recordsPerPage).Select(a => new Cls_Invoice_Moshtary
             {
                 ID = a.ID,
                 Moshtary_id = a.Moshtary_id,
                 _Date_Invoice = a.Date_Invoice,
                 Price = a.Price,
                 Taxes = a.Taxes,
                 TotalPrice = Math.Round(((a.Price) + (a.Price * (a.Taxes / 100))), 2),
                 Total_Sadad=a.Total_Sadad
             }).OrderByDescending(a=>a.ID).ToListAsync();
            var list = JsonConvert.SerializeObject(AllRecords,
Formatting.None,
new JsonSerializerSettings()
{
    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
});

            return Content(list, "application/json");

        }
        #endregion

        #region Search Invoice_Mowarid
        int viewid_Invoice_Mowarid = 2499;// الإستعلام عن عملية شراء
        // GET: Search
        public async Task<ActionResult> Invoice_Mowarid()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Users", new { returnUrl = Request.Url.ToString() });
            decimal UserID = decimal.Parse(Session["UserID"].ToString());
            UserView UserView = db.UserViews.Where(a => a.UserID == UserID && a.View.ID == viewid_Invoice_Mowarid).FirstOrDefault(); // 2470=الصنف

            if (Session["Role"].ToString() == "1" || (UserView != null && UserView.Role_Enter == true))
            {
                if (Session["RoleName"].ToString() != "BigBoss")
                {
                    UserAction UserAction = new UserAction
                    {
                        Userid = int.Parse(Session["UserID"].ToString()),
                        Viewid = viewid_Invoice_Mowarid,
                        ActionDate = DateTime.Now,
                        Action = _Action.استعلام.ToString(),
                        Operation = "الدخول إلى نافذة البحث والإستعلام عن حركة بيع"
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
                Error.ErrorName = "ليس لديك صلاحية الدخول إلى الإستعلام عن المعاملات الصادرة";
                return View("~/Views/Shared/ErrorPage.cshtml", Error);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Invoice_Mowarid(int? page, Srch_Invoice_Mowarid Srch_Invoice_Mowarid)
        {
            var skipRecords = ((page != null && page >= 1) ? page.Value : 0) * recordsPerPage;
            var AllRecords = await db.Invoice_Mowrid
             //.Where(s => string.IsNullOrEmpty(UserName) ? true : s.Name.Contains(UserName))
             .OrderBy(s => s.ID).Skip(skipRecords)
             .Take(recordsPerPage).Select(a => new Cls_Invoice_mowrid
             {
                 ID = a.ID,
                 Mowrid_id = a.Mowrid_id,
                 Mowrid_Name=a.Mowrid.Name,
                 _Date_Invoice = a.Date_Invoice,
                 Price = a.Price,
                 Total_Sadad = a.Total_Sadad
             }).OrderByDescending(a => a.ID).ToListAsync();
            var list = JsonConvert.SerializeObject(AllRecords,
Formatting.None,
new JsonSerializerSettings()
{
    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
});

            return Content(list, "application/json");

        }
        #endregion

        #region Search  Moshtary
        int viewid_Moshtary = 2484;// الإستعلام عن مشترى
        // GET: Search
        public async Task<ActionResult> Moshtary()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Users", new { returnUrl = Request.Url.ToString() });
            decimal UserID = decimal.Parse(Session["UserID"].ToString());
            UserView UserView = db.UserViews.Where(a => a.UserID == UserID && a.View.ID == viewid_Moshtary).FirstOrDefault(); // 2470=الصنف

            if (Session["Role"].ToString() == "1" || (UserView != null && UserView.Role_Enter == true))
            {
                if (Session["RoleName"].ToString() != "BigBoss")
                {
                    UserAction UserAction = new UserAction
                    {
                        Userid = int.Parse(Session["UserID"].ToString()),
                        Viewid = viewid_Moshtary,
                        ActionDate = DateTime.Now,
                        Action = _Action.استعلام.ToString(),
                        Operation = "الدخول إلى نافذة البحث والإستعلام عن مشترى"
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
                Error.ErrorName = "ليس لديك صلاحية الدخول إلى الإستعلام عن المعاملات الصادرة";
                return View("~/Views/Shared/ErrorPage.cshtml", Error);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Moshtary(int? page, Srch_Moshtary Srch_Moshtary)
        {
             var skipRecords = ((page != null && page >= 1) ? page.Value : 0) * recordsPerPage;
            var AllRecords = await db.Moshtaries
             //.Where(s => string.IsNullOrEmpty(UserName) ? true : s.Name.Contains(UserName))
             .OrderBy(s => s.ID).Skip(skipRecords)
             .Take(recordsPerPage).Select(a => new Srch_Moshtary
             {
                 ID = a.ID,
                 Name=a.Name,
                 Sejil=a.Sejil?? default(int),
                 JawalNO=a.JawalNO,
                 Address=a.Address
             }).OrderBy(a=>a.Name).ToListAsync();
            var list = JsonConvert.SerializeObject(AllRecords,
Formatting.None,
new JsonSerializerSettings()
{
    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
});

            return Content(list, "application/json");

        }
        #endregion


        #region Search  mowrid
        int viewid_Mowrid = 2485;// الإستعلام عن مورد
        // GET: Search
        public async Task<ActionResult> Mowrid()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Users", new { returnUrl = Request.Url.ToString() });
            decimal UserID = decimal.Parse(Session["UserID"].ToString());
            UserView UserView = db.UserViews.Where(a => a.UserID == UserID && a.View.ID == viewid_Mowrid).FirstOrDefault(); // 2470=الصنف

            if (Session["Role"].ToString() == "1" || (UserView != null && UserView.Role_Enter == true))
            {
                if (Session["RoleName"].ToString() != "BigBoss")
                {
                    UserAction UserAction = new UserAction
                    {
                        Userid = int.Parse(Session["UserID"].ToString()),
                        Viewid = viewid_Mowrid,
                        ActionDate = DateTime.Now,
                        Action = _Action.استعلام.ToString(),
                        Operation = "الدخول إلى نافذة البحث والإستعلام عن مورد"
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
                Error.ErrorName = "ليس لديك صلاحية الدخول إلى الإستعلام عن المعاملات الصادرة";
                return View("~/Views/Shared/ErrorPage.cshtml", Error);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Mowrid(int? page, Srch_Moshtary Srch_Moshtary)
        {
             var skipRecords = ((page != null && page >= 1) ? page.Value : 0) * recordsPerPage;
            var AllRecords = await db.Mowrids
             //.Where(s => string.IsNullOrEmpty(UserName) ? true : s.Name.Contains(UserName))
             .OrderBy(s => s.ID).Skip(skipRecords)
             .Take(recordsPerPage).Select(a => new Srch_Moshtary
             {
                 ID = a.ID,
                 Name = a.Name,
                 JawalNO = a.JawalNO,
                 Address = a.Address
             }).ToListAsync();
            var list = JsonConvert.SerializeObject(AllRecords,
Formatting.None,
new JsonSerializerSettings()
{
    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
});

            return Content(list, "application/json");

        }
        #endregion

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
