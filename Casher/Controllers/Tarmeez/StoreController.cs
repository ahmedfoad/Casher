using Casher.Models;
using Casher.Models.Administration;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq; using System.Threading.Tasks;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace Casher.Controllers.Tarmeez
{
    public class StoreController : Controller
    {
        DB_cashierEntities db = new DB_cashierEntities();
        private int recordsPerPage = 20;
        ErrorViewModel Error = new ErrorViewModel();
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> getAll(int? page, string Search)
        {
            int? skipRecords = (page != null ? page.Value : 0) * recordsPerPage;

            var AllRecords = await db.Stores
              .Where(s => string.IsNullOrEmpty(Search) ? true : s.Name.Contains(Search))
              .OrderBy(s => s.ID).Skip(skipRecords != null ? skipRecords.Value : 0)
              .Take(recordsPerPage).Select(a=>new Cls_Store{
                  ID=a.ID,
                  Name=a.Name
              }).ToListAsync();
            var list = JsonConvert.SerializeObject(AllRecords,
 Formatting.None,
 new JsonSerializerSettings()
 {
     ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
     
 });
           

            return Content(list, "application/json");
        }
        [HttpGet]
        public ActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<ActionResult> Insert(Store Store)
        {
            db.Stores.Add(Store);
            await db.SaveChangesAsync();
            Error.ErrorName = "تمت الحفظ بنجاح ... جاري إعادة تحميل الصفحة";
            return Json(Error, JsonRequestBehavior.AllowGet);
           

        }
        [HttpPost]
        public async Task<ActionResult> Edit(Store Store)
        {
            Store item = await db.Stores.FindAsync(Store.ID);
            item.Name = Store.Name;
            db.Entry(item).State = EntityState.Modified;
            await db.SaveChangesAsync();
            Error.ErrorName = "تمت الحفظ بنجاح ... جاري إعادة تحميل الصفحة";
            return Json(Error, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            Store Store = await db.Stores.FindAsync(id);
            db.Stores.Remove(Store);
            await db.SaveChangesAsync();
            Error.ErrorName = "تمت الحذف بنجاح ... جاري إعادة تحميل الصفحة";
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