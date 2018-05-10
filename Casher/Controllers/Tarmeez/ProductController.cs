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
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Casher.GlobalClass;
using Casher.GlobalClass.Reports;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Casher.Controllers.Main
{
    public class ProductController : Controller
    {
        DB_cashierEntities db = new DB_cashierEntities();

        ErrorViewModel Error = new ErrorViewModel();

        private int recordsPerPage = 300;
        int viewid = 2470;// الصنف
        [HttpGet]
        public async Task<ActionResult> GetMaxBarcode()
        {
            decimal barcode = 1;
            if (db.Products.Any())
            {
                barcode = (await db.Products.MaxAsync(a => a.Barcode)) + 1;
            }
            var jsonResult = Json(barcode, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpGet]
        public ActionResult GetCompanies()
        {
            return PartialView();
        }
        public async Task<ActionResult> getAllProducts(int? page, string Search)
        {
            int? skipRecords = (page != null ? page.Value : 0) * recordsPerPage;

            var AllRecords = await db.Products
              .Where(s => string.IsNullOrEmpty(Search) ? true : s.Name.Contains(Search))
              .OrderBy(s => s.ID).Skip(skipRecords != null ? skipRecords.Value : 0)
              .Take(recordsPerPage).Select(a => new ClsProduct
              {
                  ID = a.ID,
                  Name = a.Name,
                  Company_Name = a.Company.Name,
                  Department_Name = a.Company.Department.Name,
                  Price_Unit = a.Price_Unit,
                  Taxes = a.Taxes,
                  TotalPrice = (a.Price_Unit) + (a.Price_Unit * (a.Taxes / 100))
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

        [HttpPost]
        public async Task<ActionResult> Insert(Product model)
        {

            if (Session["UserID"] != null)
            {
                int UserID = int.Parse(Session["UserID"].ToString());
                UserView UserView = db.UserViews.Where(a => a.UserID == UserID && a.View.ID == viewid).FirstOrDefault(); // 2470=الصنف
                if (Session["Role"].ToString() == "1" || (UserView != null && UserView.Role_Save == true))
                {
                    if (db.Products.Where(a => a.Barcode == model.Barcode).Any() == false)
                    {
                        WindowsIdentity identity = HttpContext.Request.LogonUserIdentity;
                        List<string> computerDetails = identity.Name.Split('\\').ToList();
                        model.User_ID = UserID;
                        model.ComputerName = computerDetails[0];
                        model.ComputerUser = computerDetails[1];
                        model.InDate = DateTime.Now;
                        db.Products.Add(model);
                        await db.SaveChangesAsync();

                        UserAction UserAction = new UserAction
                        {
                            Userid = UserID,
                            Viewid = viewid,
                            ActionDate = DateTime.Now,
                            Action = ((_Action)1).ToString(),//حفظ
                            Operation = "حفظ صنف جديد اسم الصنف : " + model.Name + "-"
                                  + " ورقم الباركوج  : " + model.Barcode.ToString()
                        };
                        db.UserActions.Add(UserAction);
                        await db.SaveChangesAsync();
                        Error.ErrorName = "تم الإضافة بنجاح ... جاري إعادة تحميل الصفحة";
                        Error.ID = model.ID;
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
                else
                {
                    Error.ErrorFullNumber = "AR-Insert-081";
                    Error.ErrorNumber = "081";
                    Error.Url = "/Home";
                    Error.ErrorName = "ليس لديك صلاحية إضافة مستخدم جديد";
                    return Json(Error, JsonRequestBehavior.AllowGet);
                }
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
        public async Task<ActionResult> InsertProductList(List<ClsProduct> model)
        {

            if (Session["UserID"] != null)
            {
                int UserID = int.Parse(Session["UserID"].ToString());
                UserView UserView = db.UserViews.Where(a => a.UserID == UserID && a.View.ID == viewid).FirstOrDefault(); // 2470=الصنف
                if (Session["Role"].ToString() == "1" || (UserView != null && UserView.Role_Save == true))
                {
                    foreach (var item in model)
                    {
                        int index = model.IndexOf(item);
                        int count = model.Count;

                        System.Globalization.DateTimeFormatInfo HijriDTFI;
                        HijriDTFI = new System.Globalization.CultureInfo("ar-SA", false).DateTimeFormat;
                        HijriDTFI.Calendar = new System.Globalization.HijriCalendar();
                        HijriDTFI.ShortDatePattern = "dd/MM/yyyy";
                        DateTime Date_Expiration = DateTime.ParseExact(item.Date_Expiration, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        string Date_Expiration_Hijri = Date_Expiration.Date.ToString("dd/MM/yyyy", HijriDTFI);

                        decimal? Carton_Barcode= null;
                        if (item.Carton_Barcode != null) {
                            Carton_Barcode = decimal.Parse(item.Carton_Barcode);
                        }
                        int? Carton_ProCount = null;
                        if (item.Carton_ProCount != null)
                        {
                            Carton_ProCount = int.Parse(item.Carton_ProCount);
                        }
                        WindowsIdentity identity = HttpContext.Request.LogonUserIdentity;
                        List<string> computerDetails = identity.Name.Split('\\').ToList();
                        Product Product = new Product
                        {
                            User_ID = UserID,
                            ComputerName = computerDetails[0],
                            ComputerUser = computerDetails[1],
                            InDate = DateTime.Now,
                            Date_Expiration = Date_Expiration,
                            Date_Expiration_Hijri = Date_Expiration_Hijri,
                            Company_Id = item.Company_Id,
                            Barcode = item.Barcode,
                            Carton_Barcode = Carton_Barcode,
                            Carton_ProCount = Carton_ProCount,
                            Price_Unit = item.Price_Unit,
                            Price_Mowrid = item.Price_Mowrid,
                            Taxes = item.Taxes,
                            Name = item.Name
                        };

                        db.Products.Add(Product);


                        UserAction UserAction = new UserAction
                        {
                            Userid = UserID,
                            Viewid = viewid,
                            ActionDate = DateTime.Now,
                            Action = ((_Action)1).ToString(),//حفظ
                            Operation = "حفظ صنف جديد اسم الصنف : " + item.Name + "-"
                                  + " ورقم الباركوج  : " + item.Barcode.ToString()
                        };
                        db.UserActions.Add(UserAction);

                    }
                    await db.SaveChangesAsync();
                    Error.ErrorName = "تم الإضافة بنجاح ... جاري إعادة تحميل الصفحة";
                    return Json(Error, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    Error.ErrorFullNumber = "AR-Insert-081";
                    Error.ErrorNumber = "081";
                    Error.Url = "/Home";
                    Error.ErrorName = "ليس لديك صلاحية إضافة مستخدم جديد";
                    return Json(Error, JsonRequestBehavior.AllowGet);
                }
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

        [HttpGet]
        public async Task<ActionResult> loadProduct(int id)
        {
            Cls_Product Cls_Product = new Cls_Product();
            Product Product = await db.Products.FindAsync(id);
            if (Product != null)
            {
                Cls_Product.ClsProduct = new ClsProduct
                {
                    ID = Product.ID,
                    Barcode = Product.Barcode,
                    Company_Id = Product.Company_Id,
                    ComputerName = Product.ComputerName,
                    ComputerUser = Product.ComputerUser,
                    Company_Name = Product.Company.Name,
                    Name = Product.Name,
                    Department_Name = Product.Company.Department.Name,
                    InDate = Product.InDate,
                    Jumla_picesCount = Product.Jumla_picesCount,
                    Price_Unit = Product.Price_Unit,
                    Price_Mowrid = Product.Price_Mowrid?? default(decimal),
                    Taxes = Product.Taxes,
                    Carton_ProCount = (Product.Carton_ProCount != null) ? (Product.Carton_ProCount ?? default(decimal)).ToString() : "",
                    Carton_Barcode = (Product.Carton_Barcode != null)  ? (Product.Carton_Barcode??default(decimal)).ToString() : "",
                    Date_Expiration = (Product.Date_Expiration != null) ?Product.Date_Expiration.Value.ToString("yyyy-mm-dd") : "",
                    User_Name = (Product.User != null) ? Product.User.NAME : ""
                };

                Barcode Barcode = new Barcode();
                Image BarCodeImage = Barcode.getBarCode(Convert.ToInt64(Cls_Product.ClsProduct.Barcode).ToString("D12"), 3500, 2000);
                Cls_Product.BarCodeArr = Barcode.imageToByteArray(BarCodeImage);
                Cls_Product.ErrorName = "تم تحميل بيانات المعاملة الصادرة بنجاح";
            }
            var list = JsonConvert.SerializeObject(Cls_Product,
           Formatting.None,
           new JsonSerializerSettings()
           {
               ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
           });
            return Content(list, "application/json");
        }
        [HttpPost]
        public async Task<ActionResult> Edit(ClsProduct ClsProduct)
        {
            Product item = await db.Products.FindAsync(ClsProduct.ID);
            item.Barcode = ClsProduct.Barcode;
            item.Company_Id = ClsProduct.Company_Id;

            //item.ComputerUser = Product.ComputerUser,
            //item.Company_Name = Product.Company.Name,
            item.Name = ClsProduct.Name;
             
            item.InDate = ClsProduct.InDate;
            item.Jumla_picesCount = ClsProduct.Jumla_picesCount;
            item.Price_Unit = ClsProduct.Price_Unit;
            item.Price_Mowrid = ClsProduct.Price_Mowrid;
            item.Taxes = ClsProduct.Taxes;
            item.Carton_ProCount = (ClsProduct.Carton_ProCount != null) ?int.Parse(ClsProduct.Carton_ProCount) : "";
            item.Carton_Barcode = (ClsProduct.Carton_Barcode != null) ? (ClsProduct.Carton_Barcode ?? default(decimal)).ToString() : "";
           // item.Date_Expiration = (ClsProduct.Date_Expiration != null) ? ClsProduct.Date_Expiration.Value.ToString("yyyy-mm-dd") : "";
            db.Entry(item).State = EntityState.Modified;
            await db.SaveChangesAsync();
            Error.ErrorName = "تمت الحفظ بنجاح ... جاري إعادة تحميل الصفحة";
            return Json(Error, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Operation()
        {
            return View();
        }
        //جلب الامنتجات لفاتورة المشترى----------------------
        [HttpGet]
        public ActionResult GetProducts()
        {
            return PartialView();
        }
        //----------------------------------------
        public async Task<ActionResult> PrintBarCodeReport(decimal id)
        {
            List<Cls_Product> Cls_Product = new List<Models.Cls_Product>();
            Product Product = await db.Products.FindAsync(id);
            Barcode Barcode = new Barcode();
            Image BarCodeImage = Barcode.getBarCode(Convert.ToInt64(Product.Barcode).ToString("D12"), 1750, 1000);
            ClsProduct ClsProduct = new ClsProduct
            {
                ID = Product.ID,
                Barcode = Product.Barcode,
                Company_Id = Product.Company_Id,
                ComputerName = Product.ComputerName,
                ComputerUser = Product.ComputerUser,
                Company_Name = Product.Company.Name,
                Name = Product.Name,
                Department_Name = Product.Company.Department.Name,
                InDate = Product.InDate,
                Jumla_picesCount = Product.Jumla_picesCount,
                Price_Jumla = Product.Price_Jumla,
                Price_Unit = Product.Price_Unit,
                Taxes = Product.Taxes,
                User_Name = (Product.User != null) ? Product.User.NAME : ""
            };
            Cls_Product.Add(new Models.Cls_Product
            {
                ClsProduct = ClsProduct,
                BarCodeImg = BarCodeImage,
                BarCodeArr = Barcode.imageToByteArray(BarCodeImage)
            });
            Rpt_Product Rpt_Product = new Rpt_Product();
            string fileName = Rpt_Product.ExportBarCodeReport(Cls_Product);
            if (fileName != "")
            {
                string _path = System.Web.HttpContext.Current.Server.MapPath(@"~/Reports/pdf");
                string path = Path.Combine(_path, fileName + ".pdf");
                return File(path, "application/pdf");
            }



            //if (Session["UserID"] != null)
            //{
            //    decimal UserID = decimal.Parse(Session["UserID"].ToString());
            //    checkAccess = userRep.checkUserAccess(UserID, URLReport);
            //    if (Session["Role"].ToString() == "1" || (checkAccess != null && checkAccess.Save == 1))
            //    {
            //        string fileName = listExportRep.ExportBarCodeReport(id);
            //        if (fileName != "")
            //        {
            //            if (Session["RoleName"].ToString() != "BigBoss")
            //            {
            //                ActionModel.ID = ActionRep.GetID();
            //                ActionModel.UserName = Session["Name"].ToString();
            //                ActionModel.ProgramUserID = decimal.Parse(Session["UserID"].ToString());
            //                ActionModel.Tim = time[0] + "h:" + time[1] + "m:" + Math.Round(decimal.Parse(time[2])) + "s";
            //                ActionModel.Dat = "14" + date[2] + "/" + date[1] + "/" + date[0];
            //                ActionModel.Action = "طباعة الباركود للمعاملة الصادرة التي تحمل الرقم المسلسل : " + id;
            //                ActionModel.Operation = ((Operation)5).ToString();
            //                ActionModel.ViewName = viewName;
            //                ActionRep.AddNewRecord(ActionModel);
            //            }
            //            string path = Path.Combine(UniversalRepository.GlobalVariables.GlobalReportPath, fileName + ".pdf");
            //            return File(path, "application/pdf");
            //        }
            //        else
            //        {
            //            Error.ErrorFullNumber = "AR-Print-007";
            //            Error.ErrorNumber = "007";
            //            Error.Url = "/Home";
            //            Error.ErrorName = "حدث خطأ أثناء طباعة الملف برجاء المحاولة مرة آخري";
            //            return View("~/Views/Shared/ErrorPage.cshtml", Error);
            //        }
            //    }
            //    else
            //    {
            //        Error.ErrorFullNumber = "AR-Print-000";
            //        Error.ErrorNumber = "000";
            //        Error.Url = "/Home";
            //        Error.ErrorName = "ليس لديك صلاحية طباعة  الباركود للمعاملة الصادرة";
            //        return View("~/Views/Shared/ErrorPage.cshtml", Error);
            //    }
            //}
            else
            {
                Error.ErrorFullNumber = "AR-Logout-089";
                Error.ErrorNumber = "089";
                Error.Url = "/ListExport/Operation/" + id;
                Error.ErrorName = "تم تسجيل خروجك آلياً لانتهاء المدة المسموح بها";
                return View("~/Views/Shared/ErrorPage.cshtml", Error);
            }
        }
        [HttpGet]
        public async Task<ActionResult> GetbyBarcode(decimal BarCode)
        {

            var AllRecords = await db.Products.Where(a => a.Barcode == BarCode).Select(
                a => new ClsProduct
                {
                    ID = a.ID,
                    Name = a.Name,
                    Barcode = a.Barcode,
                    Company_Id = a.Company_Id,
                    Price_Unit = a.Price_Unit,
                    Taxes = a.Taxes,
                    Price_Jumla = a.Price_Jumla,
                }
                ).FirstOrDefaultAsync();

            var list = JsonConvert.SerializeObject(AllRecords,
    Formatting.None,
    new JsonSerializerSettings()
    {
        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    });

            return Content(list, "application/json");
        }


        public ActionResult ProductList()
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