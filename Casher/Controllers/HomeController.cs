using Casher.Models;
using Casher.Models.Administration;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq; using System.Threading.Tasks;

using System.Web;
using System.Web.Mvc;

namespace NewTheme.Controllers
{
    public class HomeController : Controller
    {
        DB_cashierEntities db = new DB_cashierEntities();
        int viewid = 1002;
        public async Task<ActionResult> Index()
        {

            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Users", new { returnUrl = Request.Url.ToString() });
            return View();
        }

        
        public async Task<ActionResult> About()
        {
            return View();
        }
        public JsonResult Statistics(int id)
        {
            //List<StatisticsModel> Statistics = new List<StatisticsModel>();
            //int Month = int.Parse(DateTime.Now.ToString(CultureInfo.GetCultureInfo("ar-SA")).Substring(0, 8).Split('/')[1]);
            //int Year = int.Parse(year);
            //string m;
            //for (int i = 0; i < id; i++)
            //{
            //    if (Month.ToString().Length == 1) m = "0" + Month;
            //    else m = Month.ToString();
            //    StatisticsModel model = new StatisticsModel();
            //    model.الشهر = DateOperation.getHijriMonth(Month) + ":" + Year;
            //    model.الصادر = UniRep.ExMonth(Year + "/" + m);
            //    model.الوارد = UniRep.ImMonth(Year + "/" + m);
            //    Statistics.Add(model);
            //    Month--;
            //    if (Month == 0) { Month = 12; Year--; }
            //}
            //return Json(Statistics.AsEnumerable().Reverse(), JsonRequestBehavior.AllowGet);
            return Json(null, JsonRequestBehavior.AllowGet);
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