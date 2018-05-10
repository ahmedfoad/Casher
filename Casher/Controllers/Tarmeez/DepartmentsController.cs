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
    public class DepartmentsController : Controller
    {
        DB_cashierEntities db = new DB_cashierEntities();
        private int recordsPerPage = 20;
        ErrorViewModel Error = new ErrorViewModel();
        public async System.Threading.Tasks.Task<ActionResult> getAllDepartment(int? page, string Search)
        {
            int? skipRecords = (page != null ? page.Value : 0) * recordsPerPage;

            var AllRecords = await db.Departments
              .Where(s => string.IsNullOrEmpty(Search) ? true : s.Name.Contains(Search))
              .OrderBy(s => s.ID).Skip(skipRecords != null ? skipRecords.Value : 0)
              .Take(recordsPerPage).Select(a=> new Cls_Department
              {
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
            //return Json(AllRecords, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Department Department)
        {
            Department item = await db.Departments.FindAsync(Department.ID);
            item.Name = Department.Name;
            db.Entry(item).State = EntityState.Modified;
            await db.SaveChangesAsync();
            Error.ErrorName = "تمت الحفظ بنجاح ... جاري إعادة تحميل الصفحة";
            return Json(Error, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public async Task<ActionResult> Insert(Department Department)
        {
            if (db.Departments.Where(a => a.Name == Department.Name).Any() == false)
            {
                db.Departments.Add(Department);
                await db.SaveChangesAsync();
                Error.ErrorName = "تمت الحفظ بنجاح ... جاري إعادة تحميل الصفحة";
                return Json(Error, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Error.ErrorFullNumber = "AR-Insert-081";
                Error.ErrorNumber = "081";
                Error.Url = "/Home";
                Error.ErrorName = "خطأ الاسم موجود مسبقا";
                return Json(Error, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            Department Department = await db.Departments.FindAsync(id);
            db.Departments.Remove(Department);
            await db.SaveChangesAsync();
            Error.ErrorName = "تمت الحذف بنجاح ... جاري إعادة تحميل الصفحة";
            return Json(Error, JsonRequestBehavior.AllowGet);


        }

        public async Task<ActionResult> Index()
        {
            return View();
            //if (Session["UserID"] == null)
            //    return RedirectToAction("Login", "Users", new { returnUrl = Request.Url.ToString() });
            //decimal UserID = decimal.Parse(Session["UserID"].ToString());
            //checkAccess = userRep.checkUserAccess(UserID, urlPage);
            //if (Session["Role"].ToString() == "1" || (checkAccess != null && checkAccess.Enter == 1))
            //{
            //    if (Session["RoleName"].ToString() != "BigBoss")
            //    {
            //        ActionModel.ID = ActionRep.GetID();
            //        ActionModel.UserName = Session["Name"].ToString();
            //        ActionModel.ProgramUserID = decimal.Parse(Session["UserID"].ToString());
            //        ActionModel.Tim = time[0] + "h:" + time[1] + "m:" + Math.Round(decimal.Parse(time[2])) + "s";
            //        ActionModel.Dat ="14"+ date[2] + "/" + date[1] + "/" + date[0];
            //        ActionModel.Action = "الدخول إلى نافذة بيانات الأقسام ";
            //        ActionModel.Operation = ((Operation)0).ToString();
            //        ActionModel.ViewName = viewName;
            //        ActionRep.AddNewRecord(ActionModel);
            //    }
            //    return View();
            //}
            //else
            //{
            //    Error.ErrorFullNumber = "AR-Open-130";
            //    Error.ErrorNumber = "130";
            //    Error.Url = "/Home";
            //    Error.ErrorName = "ليس لديك صلاحية الدخول إلى الإدارات والأقسام";
            //    return View("~/Views/Shared/ErrorPage.cshtml", Error);
            //}
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