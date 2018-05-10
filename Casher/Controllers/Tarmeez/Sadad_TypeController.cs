using Casher.Models;
using Casher.Models.Administration;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq; using System.Threading.Tasks;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Casher.Controllers.Tarmeez
{
    public class Sadad_TypeController : Controller
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

            var AllRecords = db.Sadad_Type
              .Where(s => string.IsNullOrEmpty(Search) ? true : s.Name.Contains(Search))
              .OrderBy(s => s.ID).Skip(skipRecords != null ? skipRecords.Value : 0)
              .Take(recordsPerPage).ToList();
            return Json(AllRecords, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<ActionResult> Insert(Sadad_Type Sadad_Type)
        {
            db.Sadad_Type.Add(Sadad_Type);
            await db.SaveChangesAsync();
            Error.ErrorName = "تمت الحفظ بنجاح ... جاري إعادة تحميل الصفحة";
            return Json(Error, JsonRequestBehavior.AllowGet);
           

        }
        [HttpPost]
        public async Task<ActionResult> Edit(Sadad_Type Sadad_Type)
        {
            Sadad_Type item = db.Sadad_Type.Find(Sadad_Type.ID);
            item.Name = Sadad_Type.Name;
            db.Entry(item).State = EntityState.Modified;
            await db.SaveChangesAsync();
            Error.ErrorName = "تمت الحفظ بنجاح ... جاري إعادة تحميل الصفحة";
            return Json(Error, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            Sadad_Type Sadad_Type = db.Sadad_Type.Find(id);
            db.Sadad_Type.Remove(Sadad_Type);
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