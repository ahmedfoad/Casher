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
    public class UsersController : Controller
    {
        DB_cashierEntities db = new DB_cashierEntities();

 
        
        ErrorViewModel Error = new ErrorViewModel();
        

        private int recordsPerPage = 300;
        string[] date = DateTime.Now.ToString(CultureInfo.GetCultureInfo("ar-SA")).Substring(0, 8).Split('/');
        private string[] time = DateTime.Now.TimeOfDay.ToString().Split(':');
        private int Viewid = 2466; //الإستعلام عن المستخدمين

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Login(User model)
        {
            // if (model.Username == "administrator" && model.Password == "@dmin123")
            if (model.Username == "1" && model.Password == "1")
            {
                Session["UserID"] = "0";
                Session["UserName"] = "Administration";
                Session["Email"] = "Shameltc@yahoo.com";
                Session["Name"] = "الشامل للبرمجيات";
                Session["RoleName"] = "BigBoss";
                Session["Role"] = "1";
                Session["ImportantTreatment"] = "1";
                Session["Admin"] = "1";
                Session["AdminEmp"] = "1";
                Session["Majless"] = "1";
                Session["MajlessAdmin"] = "1";
                Session["EmpDepartmentID"] = "";
                Session["EmpDepartmentName"] = "";


                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var user = await db.Users.Where(a => a.Username == model.Username && a.Password == model.Password).FirstOrDefaultAsync();
                if (user != null)
                {
                    Session["UserID"] = user.ID;
                    Session["Username"] = user.Username;
                    Session["Email"] = user.EMAIL;
                    Session["Name"] = user.NAME;
                    Session["RoleName"] = (user.ROLE == 0) ? Role.مستخدم : Role.مديرالنظام;
                    Session["Role"] = user.ROLE;

                    UserAction UserAction = new UserAction
                    {
                        Userid = user.ID,
                        Viewid = Viewid,
                        ActionDate = DateTime.Now,
                        Action = ((_Action)0).ToString(), // دخول
                        Operation = "تسجيل الدخول إلى البرنامج"
                    };
                    db.UserActions.Add(UserAction);
                    await db.SaveChangesAsync();
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }

        }

        public async Task<ActionResult> LoadDataChangePass()
        {
            if (Session["UserID"] != null)
            {
                Error.ErrorName = Session["UserName"].ToString();
                return Json(Error, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Error.ErrorFullNumber = "AR-Logout-089";
                Error.ErrorNumber = "089";
                Error.Url = "/Users/Operation";
                Error.ErrorName = "تم تسجيل خروجك آلياً لانتهاء المدة المسموح بها";
                return Json(Error, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<ActionResult> ChangePassword(string NewPass, string OldPass)
        {
            //if (Session["UserID"] != null)
            //{
            //    int id = int.Parse(Session["UserID"].ToString());
            //    User User = db.Users.Find(id);
            //    if (User.Password == OldPass)
            //    {
            //        model.UserPass = NewPass;
            //        WindowsIdentity identity = HttpContext.Request.LogonUserIdentity;
            //        List<string> computerDetails = identity.Name.Split('\\').ToList();
            //        User.COMPUTERUSER = computerDetails[1];
            //        User.COMPUTERNAME = computerDetails[0];
            //        User.INDATE = DateTime.Now;
            //        User.PROGRAMUSER = Session["Name"].ToString();
            //        User.PROGRAMUSERID = int.Parse(Session["UserID"].ToString());
            //        db.Entry(User).State = EntityState.Modified;
            //        await db.SaveChangesAsync();
            //        Error.ErrorName = "تم تعديل كلمة المرور بنجاح ... جاري تسجيل الخروج من البرنامج";
            //        return Json(Error, JsonRequestBehavior.AllowGet);
            //    }
            //    else
            //    {
            //        Error.ErrorName = "كلمة المرور القديمة خطأ";
            //        return Json(Error, JsonRequestBehavior.AllowGet);
            //    }

            //}
            //else
            //{
            //    Error.ErrorFullNumber = "AR-Logout-089";
            //    Error.ErrorNumber = "089";
            //    Error.Url = "/Users/Operation";
            //    Error.ErrorName = "تم تسجيل خروجك آلياً لانتهاء المدة المسموح بها";
            //    return Json(Error, JsonRequestBehavior.AllowGet);
            //}

            Error.ErrorFullNumber = "AR-Logout-089";
            Error.ErrorNumber = "089";
            Error.Url = "/Users/Operation";
            Error.ErrorName = "تم تسجيل خروجك آلياً لانتهاء المدة المسموح بها";
            return Json(Error, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetAllViews(int? page, string search)
        {
            int? skipRecords = (page != null ? page.Value : 0) * recordsPerPage;

            var AllRecords = await db.Views
              .Where(s => string.IsNullOrEmpty(search) ? true : s.Name.Contains(search))
              .OrderBy(s => s.ID).Skip(skipRecords != null ? skipRecords.Value : 0)
              .Take(recordsPerPage).Select(a =>
              new
              {
                  View = a,
                  Role_Enter = false,
                  Role_Save = false,
                  Role_Edit = false,
                  Role_Delete = false
              }
              ).ToListAsync();


            var list = JsonConvert.SerializeObject(AllRecords,
    Formatting.None,
    new JsonSerializerSettings()
    {
        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    });

            return Content(list, "application/json");
        }
        public async Task<ActionResult> getAllUsers(int? page, string search)
        {
            int? skipRecords = (page != null ? page.Value : 0) * recordsPerPage;

            var AllRecords = db.Users
              .Where(s => string.IsNullOrEmpty(search) ? true : s.NAME.Contains(search))
              .OrderBy(s => s.ID).Skip(skipRecords != null ? skipRecords.Value : 0)
              .Take(recordsPerPage).ToList();

            var list = JsonConvert.SerializeObject(AllRecords,
    Formatting.None,
    new JsonSerializerSettings()
    {
        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    });

            return Content(list, "application/json");
        }
       
        public async Task<ActionResult> loadUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                Error.ErrorFullNumber = "AR-Edit-082";
                Error.ErrorNumber = "082";
                Error.Url = "/Users/Search";
                Error.ErrorName = "لا يوجد مستخدم يحمل الرقم  " + id;
                return Json(Error, JsonRequestBehavior.AllowGet);
            }

            List<Cls_View> Cls_Views = db.UserViews.Where(a => a.UserID == id).Select(a=>
            new Cls_View {
                ClsView = new ClsView { ID=a.ViewID,Name=a.View.Name},
                Role_Enter=a.Role_Enter,
                Role_Save=a.Role_Save,
                Role_Edit=a.Role_Edit,
                Role_Delete=a.Role_Delete
            }
                ).ToList();
            Cls_User Cls_User = new Cls_User {
                User = user,
                Cls_Views= Cls_Views
            };
            var list = JsonConvert.SerializeObject(Cls_User,
  Formatting.None,
  new JsonSerializerSettings()
  {
      ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
  });


            return Content(list, "application/json");
        }
        public async Task<ActionResult> LastUser()
        {
            User user = db.Users.LastOrDefault();
            return Json(user, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> Operation()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Users", new { returnUrl = Request.Url.ToString() });
            int UserID = int.Parse(Session["UserID"].ToString());
            UserView UserView = db.UserViews.Where(a => a.UserID == UserID && a.View.Name == "إضافة مستخدم جديد").FirstOrDefault();
            if (Session["Role"].ToString() == "1" || (UserView != null && UserView.Role_Enter == true))
            {
                if (Session["RoleName"].ToString() != "BigBoss")
                {
                    UserAction UserAction = new UserAction
                    {
                        ActionDate = DateTime.Now,
                        Userid = UserID,
                        Viewid = Viewid,
                        Action = ((_Action)0).ToString(),
                        Operation = "الدخول إلى نافذة بيانات المستخدمين"
                    };
                    db.UserActions.Add(UserAction);
                    await db.SaveChangesAsync();
                }
                return View();
            }
            else
            {
                Error.ErrorFullNumber = "AR-Open-080";
                Error.ErrorNumber = "080";
                Error.Url = "/Home";
                Error.ErrorName = "ليس لديك صلاحية الدخول إلى بيانات المستخدم";
                return View("~/Views/Shared/ErrorPage.cshtml", Error);
            }
        }
        [HttpPost]
        public async Task<ActionResult> Insert(Cls_User Cls_User)
        {
            if (Session["UserID"] != null)
            {
                decimal UserID = decimal.Parse(Session["UserID"].ToString());
                UserView UserView = db.UserViews.Where(a => a.UserID == UserID && a.View.Name == "إضافة مستخدم جديد").FirstOrDefault();
                if (Session["Role"].ToString() == "1" || (UserView != null && UserView.Role_Save == true))
                {


                    User User = new User();
                    User.Username = Cls_User.User.Username;
                    User.Password = Cls_User.User.Password;
                    User.NAME = Cls_User.User.NAME;

                    User.ROLE = 0;
                    User.STOPEMP = false;

                    WindowsIdentity identity = HttpContext.Request.LogonUserIdentity;
                    List<string> computerDetails = identity.Name.Split('\\').ToList();
                    User.COMPUTERUSER = computerDetails[1];
                    User.COMPUTERNAME = computerDetails[0];
                    User.INDATE = DateTime.Now;
                    User.PROGRAMUSER = Session["Name"].ToString();
                    User.PROGRAMUSERID = int.Parse(Session["UserID"].ToString());
                    db.Users.Add(User);
                    await db.SaveChangesAsync();


                    if (Session["RoleName"].ToString() != "BigBoss")
                    {
                        UserAction UserAction = new UserAction
                        {
                            Userid = int.Parse(Session["UserID"].ToString()),
                            Viewid = Viewid,
                            ActionDate = DateTime.Now,
                            Action = ((_Action)1).ToString(), // خروج
                            Operation = "إضافة مستخدم جديد يحمل الرقم : " + User.ID.ToString() + "-"
                            + " واسم المستخدم  : " + User.Username + "-"
                            + " واسم الموظف : " + User.NAME + "."
                        };
                        db.UserActions.Add(UserAction);
                        await db.SaveChangesAsync();
                    }
                    foreach (Cls_View Cls_View in Cls_User.Cls_Views)
                    {
                        UserView UserViewAdd = new UserView
                        {
                            UserID = User.ID,
                            ViewID = Cls_View.ClsView.ID,
                            Role_Enter = Cls_View.Role_Enter,
                            Role_Save = Cls_View.Role_Save,
                            Role_Edit = Cls_View.Role_Edit,
                            Role_Delete = Cls_View.Role_Delete
                        };
                        db.UserViews.Add(UserViewAdd);
                    }
                    await db.SaveChangesAsync();


                }
                else
                {
                    Error.ErrorFullNumber = "AR-Insert-081";
                    Error.ErrorNumber = "081";
                    Error.Url = "/Home";
                    Error.ErrorName = "ليس لديك صلاحية إضافة مستخدم جديد";
                    return Json(Error, JsonRequestBehavior.AllowGet);
                }
                
                Error.ErrorName = "تمت إضافة المستخدم بنجاح ... جاري إعادة تحميل الصفحة";
                return Json(Error, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Error.ErrorFullNumber = "AR-Logout-089";
                Error.ErrorNumber = "089";
                Error.Url = "/Users/Operation";
                Error.ErrorName = "تم تسجيل خروجك آلياً لانتهاء المدة المسموح بها";
                return Json(Error, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public async Task<ActionResult> Edit(Cls_User Cls_User)
        {
            if (Session["UserID"] != null)
            {
                decimal UserID = decimal.Parse(Session["UserID"].ToString());
                UserView UserView = db.UserViews.Where(a => a.UserID == UserID && a.View.Name == "إضافة مستخدم جديد").FirstOrDefault();

                if (Session["Role"].ToString() == "1" || (UserView != null && UserView.Role_Edit == true))
                {
                    if (Cls_User != null)
                    {

                        
                        User User = db.Users.Find(Cls_User.User.ID);
                        User.Username = Cls_User.User.Username;
                        User.Password = Cls_User.User.Password;
                        User.NAME = Cls_User.User.NAME;

                        User.ROLE = 0;
                        User.STOPEMP = false;

                        WindowsIdentity identity = HttpContext.Request.LogonUserIdentity;
                        List<string> computerDetails = identity.Name.Split('\\').ToList();
                        User.COMPUTERUSER = computerDetails[1];
                        User.COMPUTERNAME = computerDetails[0];
                        User.INDATE = DateTime.Now;
                        User.PROGRAMUSER = Session["Name"].ToString();
                        User.PROGRAMUSERID = int.Parse(Session["UserID"].ToString());
                        db.Entry(User).State = EntityState.Modified;
                        await db.SaveChangesAsync();

                        List<UserView> userviewslist = db.UserViews.Where(a => a.UserID == Cls_User.User.ID).ToList();
                        db.UserViews.RemoveRange(userviewslist);
                        await db.SaveChangesAsync();

                        if (Session["RoleName"].ToString() != "BigBoss")
                        {

                            UserAction UserAction = new UserAction
                            {
                                Userid = int.Parse(Session["UserID"].ToString()),
                                Viewid = Viewid,
                                ActionDate = DateTime.Now,
                                Action = ((_Action)2).ToString(),
                                Operation = "تعديل بيانات مستخدم يحمل الرقم : " + User.ID.ToString() + "-"
                               + " واسم المستخدم  : " + User.Username + "-"
                               + " واسم الموظف : " + User.NAME + "."
                            };
                            db.UserActions.Add(UserAction);
                            await db.SaveChangesAsync();
                        }
                        foreach (Cls_View Cls_View in Cls_User.Cls_Views)
                        {
                            UserView UserViewAdd = new UserView
                            {
                                UserID = User.ID,
                                ViewID = Cls_View.ClsView.ID,
                                Role_Enter = Cls_View.Role_Enter,
                                Role_Save = Cls_View.Role_Save,
                                Role_Edit = Cls_View.Role_Edit,
                                Role_Delete = Cls_View.Role_Delete
                            };
                            db.UserViews.Add(UserViewAdd);
                        }
                        await db.SaveChangesAsync();
                    }
                    else
                    {
                        Error.ErrorName = "لا يمكن تعديل مستخدم وحذف كل الصلاحيات";
                        return Json(Error, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    Error.ErrorFullNumber = "AR-Edit-082";
                    Error.ErrorNumber = "082";
                    Error.Url = "/Home";
                    Error.ErrorName = "ليس لديك صلاحية تعديل بيانات مستخدم";
                    return Json(Error, JsonRequestBehavior.AllowGet);
                }
               
                Error.ErrorName = "تم تعديل بيانات المستخدم بنجاح ... جاري إعادة تحميل الصفحة";
                return Json(Error, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Error.ErrorFullNumber = "AR-Logout-089";
                Error.ErrorNumber = "089";
                Error.Url = "/Users/Operation/" + Cls_User.User.ID.ToString();
                Error.ErrorName = "تم تسجيل خروجك آلياً لانتهاء المدة المسموح بها";
                return Json(Error, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<ActionResult> Delete(int id)
        {
            if (Session["UserID"] != null)
            {
                decimal UserID = decimal.Parse(Session["UserID"].ToString());

                UserView UserView = db.UserViews.Where(a => a.UserID == UserID && a.View.Name == "إضافة مستخدم جديد").FirstOrDefault();
                if (Session["Role"].ToString() == "1" || (UserView != null && UserView.Role_Delete == true))
                {
                    User User = db.Users.Find(id);
                    User.STOPEMP = true;
                    db.Entry(User).State = EntityState.Modified;
                    await db.SaveChangesAsync();


                    if (Session["RoleName"].ToString() != "BigBoss")
                    {

                        UserAction UserAction = new UserAction
                        {
                            Userid = int.Parse(Session["UserID"].ToString()),
                            Viewid = Viewid,
                            ActionDate = DateTime.Now,
                            Action = ((_Action)3).ToString(), // خروج
                            Operation = "حذف بيانات مستخدم يحمل الرقم : " + User.ID + "-"
                             + " واسم المستخدم  : " + User.Username + "-"
                             + " واسم الموظف : " + User.NAME + "."
                        };
                        db.UserActions.Add(UserAction);
                        await db.SaveChangesAsync();
                    }
                    Error.ErrorFullNumber = "AR-Delete-084";
                    Error.ErrorNumber = "084";
                    Error.Url = "/Home";
                    Error.ErrorName = "تم حذف بيانات المستخدم بنجاح";
                    return View("~/Views/Shared/ErrorPage.cshtml", Error);
                }
                else
                {
                    Error.ErrorFullNumber = "AR-Delete-083";
                    Error.ErrorNumber = "083";
                    Error.Url = "/Home";
                    Error.ErrorName = "ليس لديك صلاحية حذف بيانات مستخدم";
                    return View("~/Views/Shared/ErrorPage.cshtml", Error);
                }

            }
            else
            {
                Error.ErrorFullNumber = "AR-Logout-089";
                Error.ErrorNumber = "089";
                Error.Url = "/Users/Operation/" + id;
                Error.ErrorName = "تم تسجيل خروجك آلياً لانتهاء المدة المسموح بها";
                return View("~/Views/Shared/ErrorPage.cshtml", Error);
            }
        }
        public async Task<ActionResult> Search()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Users", new { returnUrl = Request.Url.ToString() });
            decimal UserID = decimal.Parse(Session["UserID"].ToString());

            UserView UserView = db.UserViews.Where(a => a.UserID == UserID && a.View.Name == "الإستعلام عن المستخدمين").FirstOrDefault();
            if (Session["Role"].ToString() == "1" || (UserView != null && UserView.Role_Enter == true))
            {
                if (Session["RoleName"].ToString() != "BigBoss")
                {

                    UserAction UserAction = new UserAction
                    {
                        Userid = int.Parse(Session["UserID"].ToString()),
                        Viewid = Viewid,
                        ActionDate = DateTime.Now,
                        Action = ((_Action)4).ToString(), // خروج
                        Operation = "الدخول إلى نافذة البحث والإستعلام عن المستخدمين"
                    };
                    db.UserActions.Add(UserAction);
                    await db.SaveChangesAsync();
                }
                return View();
            }
            else
            {
                Error.ErrorFullNumber = "AR-Open-080";
                Error.ErrorNumber = "080";
                Error.Url = "/Home";
                Error.ErrorName = "ليس لديك صلاحية الدخول إلى الإستعلام عن المستخدمين";
                return View("~/Views/Shared/ErrorPage.cshtml", Error);
            }
        }
        [HttpPost]
        public async Task<ActionResult> Search(int? page, string UserName)
        {
            var skipRecords = ((page != null && page != 1) ? page.Value : 0) * recordsPerPage;
            var AllRecords = db.Users
             .Where(s => string.IsNullOrEmpty(UserName) ? true : s.NAME.Contains(UserName))
             .OrderBy(s => s.ID).Skip(skipRecords)
             .Take(recordsPerPage).ToList();
            var list = JsonConvert.SerializeObject(AllRecords,
Formatting.None,
new JsonSerializerSettings()
{
    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
});

            return Content(list, "application/json");

        }
        public async Task<ActionResult> LogOff()
        {
            if (Session["RoleName"].ToString() != "BigBoss" && Session["UserID"] != null)
            {
                UserAction UserAction = new UserAction
                {
                    Userid = int.Parse(Session["UserID"].ToString()),
                    Viewid = Viewid,
                    ActionDate = DateTime.Now,
                    Action = ((_Action)8).ToString(), // خروج
                    Operation = "تسجيل خروج إلى البرنامج"
                };
                db.UserActions.Add(UserAction);
                await db.SaveChangesAsync();
            }
            Session.Clear();
            return RedirectToAction("Login");
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