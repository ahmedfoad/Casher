using Casher.Models;
using Casher.Models.Administration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq; using System.Threading.Tasks;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
namespace Casher.Controllers.Administration
{
    public class ActionsController : Controller
    {
        DB_cashierEntities db = new DB_cashierEntities();
        private int recordsPerPage = 10;
        private string URLSearch = "/Actions/Search";
        public async Task<ActionResult> Search()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Users", new { returnUrl = Request.Url.ToString() });
            int UserID = int.Parse(Session["UserID"].ToString());
            
            UserView UserView = db.UserViews.Where(a => a.UserID == UserID && a.View.Name == "المراقبة").FirstOrDefault();
            if (Session["Role"].ToString() == "1" || (UserView != null && UserView.Role_Enter == true))
            {
                //ViewBag.ViewName = new SelectList(ActionsRepository.ViewName().Values);
                //var ActionType = from Operation e in Enum.GetValues(typeof(Operation))
                //                 select new { ID = (int)e, Name = e.ToString() };
                //ViewBag.ActionType = new SelectList(ActionType, "ID", "Name");
                return View();
            }
            else
            {
                ErrorViewModel error = new ErrorViewModel();
                error.ErrorFullNumber = "AR-Open-080";
                error.ErrorNumber = "080";
                error.Url = "/Home";
                error.ErrorName = "ليس لديك صلاحية الدخول إلى شاشة مراقبة المستخدمين";
                return View("~/Views/Shared/ErrorPage.cshtml", error);
            }
        }
        [HttpPost]
        public async Task<ActionResult> Search(int? page, string UserName, string DatBegin, string DatEnd, string UserID, string Operation, string ViewName, string action)
        {
            try
            {
                if (UserID == "" || UserID == null) { UserID = "-1"; }

                var skipRecords = ((page != null && page != 1) ? page.Value : 0) * recordsPerPage;
                var AllRecords = db.UserActions
                 .Where(s => string.IsNullOrEmpty(UserName) ? true : s.User.Username.Contains(UserName))
                 .OrderBy(s => s.ID).Skip(skipRecords)
                 .Take(recordsPerPage).Select(x => new cls_UserAction
                 {
                     ActionDate =x.ActionDate,
                     Action = x.Action,
                     Operation=x.Operation,
                     EmpName=x.User.NAME
                     
                 }).ToList();
                var list = JsonConvert.SerializeObject(AllRecords,
    Formatting.None,
    new JsonSerializerSettings()
    {
        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,

    });

                return Content(list, "application/json");
            }
            catch (Exception ex)
            {
                return new JsonResult
                {
                    Data = new { ErrorMessage = ex.Message, Success = false },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.DenyGet
                };
            }



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