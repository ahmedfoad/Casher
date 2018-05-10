using Casher.Models;
using Casher.Models.Administration;

using Newtonsoft.Json;
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
    public class CompanyController : Controller
    {
        DB_cashierEntities db = new DB_cashierEntities();
        private int recordsPerPage = 20;
        ErrorViewModel Error = new ErrorViewModel();
        public async Task<ActionResult>  getAll(int? page, string Search)
        {
            int? skipRecords = (page != null ? page.Value : 0) * recordsPerPage;

            var AllRecords = await db.Companies
              .Where(s => string.IsNullOrEmpty(Search) ? true : s.Name.Contains(Search))
              .OrderBy(s => s.ID).Skip(skipRecords != null ? skipRecords.Value : 0)
              .Take(recordsPerPage).Select(a=>new Cls_Company {
                  ID=a.ID,
                  Name=a.Name,
                  Department_Name=a.Department.Name
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
        [HttpGet]
        public async Task<ActionResult> getDepartments()
        {
            var AllRecords = await db.Departments.Select(a=>new Cls_Department {
                ID=a.ID,
                Name=a.Name
            }).ToListAsync();


            var list = JsonConvert.SerializeObject(AllRecords,
    Formatting.None,
    new JsonSerializerSettings()
    {
        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    });

            return Content(list, "application/json");
        }
        
        [HttpGet]
        public async Task<ActionResult> Create()
        {
           
            return PartialView();
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Company Company)
        {
            Company item = await db.Companies.FindAsync(Company.ID);
            item.Name = Company.Name;
            item.Department_ID = Company.Department_ID;
            db.Entry(item).State = EntityState.Modified;
            await db.SaveChangesAsync();
            Error.ErrorName = "تمت الحفظ بنجاح ... جاري إعادة تحميل الصفحة";
            return Json(Error, JsonRequestBehavior.AllowGet);
        }

        

        [HttpPost]
        public async Task<ActionResult> Insert(Company Company)
        {
            db.Companies.Add(Company);
            await db.SaveChangesAsync();
            Error.ErrorName = "تمت الحفظ بنجاح ... جاري إعادة تحميل الصفحة";
            return Json(Error, JsonRequestBehavior.AllowGet);
           

        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            Company Company = db.Companies.Find(id);
            db.Companies.Remove(Company);
             await db.SaveChangesAsync();
            Error.ErrorName = "تمت الحذف بنجاح ... جاري إعادة تحميل الصفحة";
            return Json(Error, JsonRequestBehavior.AllowGet);


        }

        public async Task<ActionResult> Index()
        {
            return View();
         
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