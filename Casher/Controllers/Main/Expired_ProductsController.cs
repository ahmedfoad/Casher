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

namespace Casher.Controllers.Main
{
    public class Expired_ProductsController : Controller
    {
        DB_cashierEntities db = new DB_cashierEntities();
        private int recordsPerPage = 50;
        ErrorViewModel Error = new ErrorViewModel();
        // GET: Expired_Products
        #region Search Invoice_Mowarid
        int viewid  = 2500;// حسم الاصناف منتهية الصلاحية
        // GET: Search
        public async Task<ActionResult> Invoice_Mowarid()
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
                        Operation = "الدخول إلى نافذة حسم الاصناف منتهية الصلاحية"
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
                 Mowrid_Name = a.Mowrid.Name,
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
    }
}