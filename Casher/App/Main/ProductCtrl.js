app.controller("ProductCtrl", function ($scope, $http, $window, $timeout, $uibModal) {
    //----------------------------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------------------------
    // switch between Inser && edit 
    //----------------------------------------------------------------------------------------------------------------------------
    // $scope.Product = { ID: 0, Name: "", Barcode: 0, Company_Id: "", Price_Unit: "", Taxes: "", image:""};
    $scope.Product = { ID: 0, Name: "", Barcode: "", Company_Id: "", Company_Name: "", Price_Unit: "", Taxes: "", Carton_Barcode: "", Carton_ProCount: "", Date_Expiration: "", Price_Mowrid: "", image: "" };
    
    $http.get("/Product/GetMaxBarcode")
          .success(function (response) {
              $scope.Product.Barcode = response;
          });
    $scope.GetBarcode = function () {
        $http.get("/Product/GetMaxBarcode")
          .success(function (response) {
              $scope.Product.Barcode = response;
          });
    }
    $scope.hideElement = function (arr) {
        for (var i = 0; i < arr.length; i++) {
            document.getElementById(arr[i]).style.display = 'none';
        }
    }
    $scope.ReadOnlyInputs = function (arr) {
        for (var i = 0; i < arr.length; i++) {
            document.getElementById(arr[i]).readOnly = true;
        }
    }
    $scope.arrayBufferToBase64 = function (buffer) {
        var binary = '';
        var bytes = new Uint8Array(buffer);
        var len = bytes.byteLength;
        for (var i = 0; i < len; i++) {
            binary += String.fromCharCode(bytes[i]);
        }
        return window.btoa(binary);
    }
    $scope.switchCase = function () {
        debugger;
        var id = document.URL.split("/")[5];
        if (id == null) {
            $scope.SaveBtn = null;
            $scope.EditBtn = 1;
            $scope.DeleteBtn = 1;
            $scope.dataEntire = null;
            var hiddenElements = ["Numbers", "Barcode" ,"EditElements2"];
            $scope.hideElement(hiddenElements);
            //$http.get("/Product/InitialOperationForm")
            //.success(function (response) {
            //    $scope.Product = response;
            //    $scope.ProductType = "Public";
            //    if (response.SessionAdmin == "1") { $scope.public = 1; $scope.Admin = 1; }
            //    if (response.SessionMajless == "1") { $scope.public = 1; $scope.Majless = 1; }
            //    if (response.SessionMajlessAdmin == "1") { $scope.public = 1; $scope.MajlessAdmin = 1; }
            //    if (response.SessionAdminEmp == "1") { $scope.public = 1; $scope.AdminEmp = 1; }
            //    $scope.today();
            //});
        }
        else {
           
            var numericExpression = /^[0-9]+$/;
            if (id.match(numericExpression)) {
                $scope.SaveBtn = 1;
                $scope.dataEntire = 1;
                $scope.EditBtn = null;
                $scope.DeleteBtn = null;
                  
                document.getElementById('PrintBarCode').href = "/Product/PrintBarCodeReport/" + id;
                //document.getElementById('PrintTicket').href = "/Product/PrintTicketReport/" + id;
                //document.getElementById('ProductRefer').href = "/Product/Refer/" + id;
                $http.get('/Product/loadProduct/' + id)
                   .success(function (response) {
                       if (response == null) {
                           $uibModal.open({
                               template: " <div class='portlet box red' style='min-height:150px !important;min-width: 541px !important;'> <div class='portlet-title'> <div class='caption'> <i class='fa-lg fa fa-warning'></i> تنبيه هام </div><div class='tools'> </div></div><div class='portlet-body form portlet-empty' style='min-height: 140px;'> <div class='col-md-12 page-404'> <div class='number'>" + response.ErrorNumber + "</div><div class='details'> <h3 style='font-family: sans-serif; text-align: center; font-weight: bold;'>" + response.ErrorFullNumber + "</h3> <p style='font-size: 12px;'> " + response.ErrorName + " <br/> <br/> <a class='btn btn-default btn-sm' href='/home'> الصفحة الرئيسية </a> <a class='btn btn-default btn-sm' href='" + response.Url + "' style='margin-right: 4px;'>المحاولة مرة آخري</a> </p></div></div></div></div>",
                               closeOnEscape: false,
                               closeOnOverlayClick: false
                           });
                       }
                       else {
                          
                           $scope.Product = { ID: response.ClsProduct.ID, Name: response.ClsProduct.Name, Barcode: response.ClsProduct.Barcode, Company_Id: response.ClsProduct.Company_Id, Price_Unit: response.ClsProduct.Price_Unit, Taxes: response.ClsProduct.Taxes };

                           $('#Company_Id').val(response.ClsProduct.Company_Id);
                           $('#Company_Name').val(response.ClsProduct.Company_Name);
                           document.getElementById('BarcodNumber').innerHTML = response.ClsProduct.Barcode;
                           document.getElementById("BarcodeImg").src = "data:image/png;base64," + response.BarCodeArr;
                           
                       }
                   });
            }
            else {
                $uibModal.open({
                    template: " <div class='portlet box red' style='min-height:150px !important;min-width: 541px !important;'> <div class='portlet-title'> <div class='caption'> <i class='fa-lg fa fa-warning'></i> تنبيه هام </div><div class='tools'> </div></div><div class='portlet-body form portlet-empty' style='min-height: 140px;'> <div class='col-md-12 page-404'> <div class='number'>900</div><div class='details'> <h3 style='font-family: sans-serif; text-align: center; font-weight: bold;'>AR-ContainChar-900</h3> <p style='font-size: 12px;'> رقم غير صحيح برجاء المحاولة مرة آخري <br/> <br/> <a class='btn btn-default btn-sm' href='/home'> الصفحة الرئيسية </a> </p></div></div></div></div>",
                    closeOnEscape: false,
                    closeOnOverlayClick: false
                });
            }
        }
    }
    $scope.switchCase();
    //----------------------------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------------------------
    // load all popuop data
    //----------------------------------------------------------------------------------------------------------------------------
    var DealingTypeView;
    $scope.getView = function () {
        $http.get("/Product/GetCompanies")
         .success(function (response) {
             DealingTypeView = response;
         });
    };
    $scope.getView();
    $scope.loadCompanies = function () {
        debugger;
        $uibModal.open({
            scope: $scope,
        template: DealingTypeView,
        resolve: {
            Product: function () {
                return $scope.Product;
            }
        }
        });
    };
   


    //------------------تحميل الشركات بعد فتح النافذة
    $scope.getAllRecords = function (Page, Search) {
        var parameter = { 'Search': Search }
        var sentData = "page=" + Page;
        $http.get("/Company/getAll?" + sentData, { params: parameter })
       .success(function (response) {
           if (response != null) {

               if ($scope.data == null) {
                   $scope.data = response;
               }
               else {
                   $scope.data = $scope.data.concat(response);
               }
           }
           //if (response == null || response == "") { otherData = 1; hideLoadMore = null; }
           //else { debugger; otherData = null; hideLoadMore = 1; }
       });
    }
    $scope.getAllRecords();
    //===================
    //=======عند الضفط على صف
    $scope.clicktr = function (row) {
        $scope.Product.Company_Id = row.ID;
        $('#Company_Id').val(row.ID);
        $('#Company_Name').val(row.Name);
        $timeout(function () { angular.element('#closebox').trigger('click'); }, 0);

        this.$close();
    }
    $scope.closeUiModal = function () {
        this.$close();
    }
    //----------------------------------------------------------------------------------------------------------------------------

    //----------------------------------------------------------------------------------------------------------------------------
    // Save Export List
    //----------------------------------------------------------------------------------------------------------------------------
    $scope.ReomveAlert = function () {
        $scope.AlertParameter = null;
    }
    $scope.changeBorder = function (arr, color) {
        for (var i = 0; i < arr.length; i++) {
            document.getElementById(arr[i]).style.borderColor = color;
        }
    }
    $scope.FormValidation = function (Product) {
      
        var Price_UnitCheck = Product.Price_Unit.match(/^[0-9]+(\.[0-9]{1,2})?$/);

        if (Product.Name == null || Product.Name == "") {
            $scope.ErrorName = "برجاء ادخال اسم الصنف"
            var ListID = ["Name"];
            $scope.changeBorder(ListID, "red");
            return false;
        } else {
            var ListID = ["Name"];
            $scope.changeBorder(ListID, "#c2cad8");
        }

        if (Product.Company_Id == null || Product.Company_Id == "") {
            $scope.ErrorName = "برجاء ادخال اسم الشركة"
            var ListID = ["Company_Name"];
            $scope.changeBorder(ListID, "red");
            return false;
        } else {
            var ListID = ["Company_Name"];
            $scope.changeBorder(ListID, "#c2cad8");
        }
        if (Price_UnitCheck == null) {
            $scope.ErrorName = "السعر غير صحيح";
            var ListID = ["Price_Unit"];
            $scope.changeBorder(ListID, "red");
            return false;
        } else if (Product.Price_Unit == null || Product.Price_Unit == "" || Product.Price_Unit == 0) {
            $scope.ErrorName = "السعر غير صحيح";
            var ListID = ["Price_Unit"];
            $scope.changeBorder(ListID, "red");
            return false;
        } else {
            var ListID = ["Price_Unit"];
            $scope.changeBorder(ListID, "#c2cad8");
        }
      

        if (Product.Barcode == null || Product.Barcode == "" || Product.Barcode == 0) {
            $scope.ErrorName = "برجاء ادخال الباركود"
            var ListID = ["Barcode"];
            $scope.changeBorder(ListID, "red");
            return false;
        } else {
            var ListID = ["Barcode"];
            $scope.changeBorder(ListID, "#c2cad8");
        }
        if (Product.Company_Id == null || Product.Company_Id == "") {
            $scope.ErrorName = "برجاء اختيار اسم الشركة"
            var ListID = ["Company_Name"];
            $scope.changeBorder(ListID, "red");
            return false;
        } else {
            var ListID = ["Company_Name"];
            $scope.changeBorder(ListID, "#c2cad8");
        }
        

        if (Product.Taxes == null || Product.Taxes == "") {
            $scope.ErrorName = "برجاء اختيار القيمة المضافة"
            var ListID = ["Taxes"];
            $scope.changeBorder(ListID, "red");

            return false;
        } else {
            var ListID = ["Taxes"];
            $scope.changeBorder(ListID, "#c2cad8");
        }
        //else if (DatExportCheck == null) {
        //    $scope.ErrorName = "تاريخ الصادر خطأ . برجاء اختيار تاريخ صحيح"
        //    var ListID = ["DepartmentFromID", "DepartmentFromName", "DepartmentToID", "DepartmentToName",
        //    "TreatmentTypeID", "TreatmentTypeName"];
        //    $scope.changeBorder(ListID, "#c2cad8");
        //    return false;
        //}
        $scope.AlertParameter = null;
        return true;
    }
    $scope.SaveProduct = function (Product) {
        //Product.Admin = 0; Product.Majless = 0; Product.MajlessAdmin = 0;
        //Product.AdminEmp = 0; Product.OldID = 0; Product.SubjectID = 0;
     

        Product.Company_Id = document.getElementById("Company_Id").value;
        Product.Price_Unit = document.getElementById("Price_Unit").value;
        Product.Taxes = document.getElementById("Taxes").value;
        var CheckForm = $scope.FormValidation(Product);
        if (CheckForm == true) {
            $scope.DBsave(Product);
        }
        else {
            $scope.AlertParameter = 1;
            document.body.scrollTop = 0;
        }

    }
    $scope.DBsave = function (Product) {
        var ProductID = document.URL.split("/")[5];
        var parameter = JSON.stringify(Product);
        if (ProductID == null) {
            $http.post('/Product/Insert', parameter)
                 .success(function (response) {
                     if (response.ErrorName.indexOf("صلاحية") > -1 || response.ErrorName.indexOf("المسموح") > -1) {
                         $uibModal.open({
                             template: " <div class='portlet box red' style='min-height:150px !important ;min-width: 541px !important;'> <div class='portlet-title'> <div class='caption'> <i class='fa-lg fa fa-warning'></i> تنبيه هام </div><div class='tools'> </div></div><div class='portlet-body form portlet-empty' style='min-height: 140px;'> <div class='col-md-12 page-404'> <div class='number'>" + response.ErrorNumber + "</div><div class='details'> <h3 style='font-family: sans-serif; text-align: center; font-weight: bold;'>" + response.ErrorFullNumber + "</h3> <p> " + response.ErrorName + " <br/> <br/> <a class='btn btn-default btn-sm' href='/home'> الصفحة الرئيسية </a> </p></div></div></div></div>",
                             closeOnEscape: false,
                             closeOnOverlayClick: false
                         });
                     }
                     else {
                         $scope.ErrorName = response.ErrorName;
                         $scope.ID = response.ID;
                         $scope.AlertParameter = 1;
                         document.body.scrollTop = 0;
                         if (response.ErrorName.indexOf("بنجاح") > -1) {
                             $timeout(function () {
                                 $window.location.href = '/Product/Operation/' + response.ID;
                             }, 3000);
                         }
                     }
                 });
        }
        else {
            Product.BarCodeArr = null;
            $http.post('/Product/Edit', parameter)
               .success(function (response) {
                   if (response.ErrorName.indexOf("صلاحية") > -1 || response.ErrorName.indexOf("المسموح") > -1) {
                       $uibModal.open({
                           template: " <div class='portlet box red' style='min-height:150px !important;min-width: 541px !important;'> <div class='portlet-title'> <div class='caption'> <i class='fa-lg fa fa-warning'></i> تنبيه هام </div><div class='tools'> </div></div><div class='portlet-body form portlet-empty' style='min-height: 140px;'> <div class='col-md-12 page-404'> <div class='number'>" + response.ErrorNumber + "</div><div class='details'> <h3 style='font-family: sans-serif; text-align: center; font-weight: bold;'>" + response.ErrorFullNumber + "</h3> <p> " + response.ErrorName + " <br/> <br/> <a class='btn btn-default btn-sm' href='/home'> الصفحة الرئيسية </a> </p></div></div></div></div>",
                           closeOnEscape: false,
                           closeOnOverlayClick: false
                       });
                   }
                   else {
                       $scope.ErrorName = response.ErrorName;
                       $scope.AlertParameter = 1;
                       document.body.scrollTop = 0;
                       if (response.ErrorName.indexOf("بنجاح") > -1) {
                           $timeout(function () {
                               $window.location.href = '/Product/Operation/' + Product.ID;
                           }, 3000);
                       }
                   }
               });
        }
    }
    $scope.DeleteProduct = function () {
        var ID = document.URL.split("/")[5];
        $uibModal.open({
            template: " <div class='portlet light' style='max-width: 333px;'> <div class='portlet-title'><div class='caption font-red-sunglo'> <i class='fa fa-link font-red-sunglo'></i> <span class='caption-subject bold'> هل أنت متأكد من حذف المعاملة الصادرة</span> </div></div><div class='portlet-body'> <div class='row' style='text-align:center;margin-top:5px;'> <div class='form-actions'> <a class='btn btn-default red btn-sm' href='/Product/Delete/" + ID + "'>حذف</a><a class='btn btn-default btn-sm fancymodal-close' style='margin-right: 4px;' href='#'> إلغاء </a> </div></div></div></div>",
            closeOnEscape: false,
            closeOnOverlayClick: false
        });
    }
    //----------------------------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------------------------
    // Today
    //----------------------------------------------------------------------------------------------------------------------------
    $scope.today = function () {
        $timeout(function () { angular.element('#DatExport').trigger('focus'); }, 0);
        $timeout(function () {
            var Day = document.getElementsByClassName("calendars-today")[0].textContent;
            if (Day.length == 1) { Day = "0" + Day }
            var YM = document.getElementsByClassName("calendars-month-year")[0];
            var Month = YM.options[YM.selectedIndex].value.split("/")[0];
            if (Month.length == 1) { Month = "0" + Month }
            var Year = YM.options[YM.selectedIndex].value.split("/")[1];
            $scope.Product.DatExport = Year + "/" + Month + "/" + Day;
            var x = document.getElementsByClassName("calendars-cmd-close")[0].click();

            $timeout(function () { angular.element('.calendars-cmd-close').trigger('click'); }, 0);
        }, 400);
    }
});

