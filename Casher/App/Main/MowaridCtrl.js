app.controller("MowaridCtrl", function ($scope, $http, $window, $timeout, $fancyModal, $uibModal) {
    //----------------------------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------------------------
    // switch between Inser && edit 
    //----------------------------------------------------------------------------------------------------------------------------
    $scope.Mowrid = { ID:0, Name: "", JawalNO:"",Address:""};
    $scope.hideElement = function (arr) {
       
        for (var i = 0; i < arr.length; i++) {
            if (document.getElementById(arr[i]) != null)
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
       
        var id = document.URL.split("/")[5];
        if (id == null) {
            $scope.SaveBtn = null;
            $scope.EditBtn = 1;
            $scope.DeleteBtn = 1;
            $scope.dataEntire = null;
            var hiddenElements = ["EditElements"]
            $scope.hideElement(hiddenElements);
            //$http.get("/Mowrid/InitialOperationForm")
            //.success(function (response) {
            //    $scope.Mowrid = response;
            //    $scope.MowridType = "Public";
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
                
                 
                $http.get('/Mowrid/loadMowrid/' + id)
                   .success(function (response) {
                       if (response == null) {
                           $fancyModal.open({
                               template: " <div class='portlet box red' style='min-height:150px !important;min-width: 541px !important;'> <div class='portlet-title'> <div class='caption'> <i class='fa-lg fa fa-warning'></i> تنبيه هام </div><div class='tools'> </div></div><div class='portlet-body form portlet-empty' style='min-height: 140px;'> <div class='col-md-12 page-404'> <div class='number'>" + response.ErrorNumber + "</div><div class='details'> <h3 style='font-family: sans-serif; text-align: center; font-weight: bold;'>" + response.ErrorFullNumber + "</h3> <p style='font-size: 12px;'> " + response.ErrorName + " <br/> <br/> <a class='btn btn-default btn-sm' href='/home'> الصفحة الرئيسية </a> <a class='btn btn-default btn-sm' href='" + response.Url + "' style='margin-right: 4px;'>المحاولة مرة آخري</a> </p></div></div></div></div>",
                               closeOnEscape: false,
                               closeOnOverlayClick: false
                           });
                       }
                       else   {
                           $scope.Mowrid = {ID:response.ID, Name: response.Name, JawalNO: response.JawalNO, Address: response.Address };
                       }
                   });
            }
            else {
                $fancyModal.open({
                    template: " <div class='portlet box red' style='min-height:150px !important;min-width: 541px !important;'> <div class='portlet-title'> <div class='caption'> <i class='fa-lg fa fa-warning'></i> تنبيه هام </div><div class='tools'> </div></div><div class='portlet-body form portlet-empty' style='min-height: 140px;'> <div class='col-md-12 page-404'> <div class='number'>900</div><div class='details'> <h3 style='font-family: sans-serif; text-align: center; font-weight: bold;'>AR-ContainChar-900</h3> <p style='font-size: 12px;'> رقم غير صحيح برجاء المحاولة مرة آخري <br/> <br/> <a class='btn btn-default btn-sm' href='/home'> الصفحة الرئيسية </a> </p></div></div></div></div>",
                    closeOnEscape: false,
                    closeOnOverlayClick: false
                });
            }
        }
    }
    $scope.switchCase();
    ////----------------------------------------------------------------------------------------------------------------------------

   
  
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
    $scope.FormValidation = function (Mowrid) {
       // var DatExportCheck = Mowrid.DatExport.match("^(14[0-9]{2})(/|-)([1-9]|1[0-2]|0[1-9])(/|-)([1-9]|(0|1|2)[0-9]|30)$")
        var JawalNOCheck = Mowrid.JawalNO.match("^05[0-9]{8}$");
        if (Mowrid.Name == null || Mowrid.Name == "") {
            $scope.ErrorName = "برجاء ادخال اسم المورد"
            var ListIDRed = ["Name"];
            $scope.changeBorder(ListIDRed, "red");
            return false;
        }
        //else if (Mowrid.DepartmentToID == null || Mowrid.DepartmentToID == "") {
        //    $scope.ErrorName = "برجاء اختيار الجهة المرسل لها"
        //    var ListID = ["DepartmentFromID", "DepartmentFromName"];
        //    $scope.changeBorder(ListID, "#c2cad8");
        //    var ListIDRed = ["DepartmentToID", "DepartmentToName"];
        //    $scope.changeBorder(ListIDRed, "red");
        //    return false;
        //}
        //else if (Mowrid.TreatmentTypeID == null || Mowrid.TreatmentTypeID == "") {
        //    $scope.ErrorName = "برجاء اختيار نوع المعاملة"
        //    var ListID = ["DepartmentFromID", "DepartmentFromName", "DepartmentToID", "DepartmentToName"];
        //    $scope.changeBorder(ListID, "#c2cad8");
        //    var ListIDRed = ["TreatmentTypeID", "TreatmentTypeName"];
        //    $scope.changeBorder(ListIDRed, "red");
        //    return false;
        //}
        //else if (Mowrid.DatExport == null || Mowrid.DatExport == "") {
        //    $scope.ErrorName = "برجاء اختيار تاريخ الصادر"
        //    var ListID = ["DepartmentFromID", "DepartmentFromName", "DepartmentToID", "DepartmentToName",
        //         "TreatmentTypeID", "TreatmentTypeName"];
        //    $scope.changeBorder(ListID, "#c2cad8");
        //    var ListIDRed = ["DatExport"];
        //    $scope.changeBorder(ListIDRed, "red");
        //    return false;
        //}
        //else if (DatExportCheck == null) {
        //    $scope.ErrorName = "تاريخ الصادر خطأ . برجاء اختيار تاريخ صحيح"
        //    var ListID = ["DepartmentFromID", "DepartmentFromName", "DepartmentToID", "DepartmentToName",
        //    "TreatmentTypeID", "TreatmentTypeName"];
        //    $scope.changeBorder(ListID, "#c2cad8");
        //    return false;
        //}
        $scope.AlertParameter = null;
        var ListID = ["Name", "JawalNO", "Address"];
        $scope.changeBorder(ListID, "#c2cad8");
        return true;
    }
    $scope.SaveMowrid = function (Mowrid) {
        //Mowrid.Admin = 0; Mowrid.Majless = 0; Mowrid.MajlessAdmin = 0;
        //Mowrid.AdminEmp = 0; Mowrid.OldID = 0; Mowrid.SubjectID = 0;
      
        Mowrid.Name = document.getElementById("Name").value;
        Mowrid.JawalNO = document.getElementById("JawalNO").value;
        Mowrid.Address = document.getElementById("Address").value;
       
        var CheckForm = $scope.FormValidation(Mowrid);
        if (CheckForm == true) {
            $scope.DBsave(Mowrid);
        }
        else {
            $scope.AlertParameter = 1;
            document.body.scrollTop = 0;
        }

    }
    $scope.DBsave = function (Mowrid) {
        var MowridID = document.URL.split("/")[5];
        var parameter = JSON.stringify(Mowrid);
        if (MowridID == null) {
            $http.post('/Mowrid/Insert', parameter)
                 .success(function (response) {
                     if (response.ErrorName.indexOf("صلاحية") > -1 || response.ErrorName.indexOf("المسموح") > -1) {
                         $fancyModal.open({
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
                                 $window.location.href = '/Mowrid/Operation/' + response.ID;
                             }, 3000);
                         }
                     }
                 });
        }
        else {
            Mowrid.BarCodeArr = null;
            $http.post('/Mowrid/Edit', parameter)
               .success(function (response) {
                   if (response.ErrorName.indexOf("صلاحية") > -1 || response.ErrorName.indexOf("المسموح") > -1) {
                       $fancyModal.open({
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
                               $window.location.href = '/Mowrid/Operation/' + Mowrid.ID;
                           }, 3000);
                       }
                   }
               });
        }
    }
    $scope.DeleteMowrid = function () {
        var ID = document.URL.split("/")[5];
        $fancyModal.open({
            template: " <div class='portlet light' style='max-width: 333px;'> <div class='portlet-title'><div class='caption font-red-sunglo'> <i class='fa fa-link font-red-sunglo'></i> <span class='caption-subject bold'> هل أنت متأكد من حذف المعاملة الصادرة</span> </div></div><div class='portlet-body'> <div class='row' style='text-align:center;margin-top:5px;'> <div class='form-actions'> <a class='btn btn-default red btn-sm' href='/Mowrid/Delete/" + ID + "'>حذف</a><a class='btn btn-default btn-sm fancymodal-close' style='margin-right: 4px;' href='#'> إلغاء </a> </div></div></div></div>",
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
            $scope.Mowrid.DatExport = Year + "/" + Month + "/" + Day;
            var x = document.getElementsByClassName("calendars-cmd-close")[0].click();
            
            $timeout(function () { angular.element('.calendars-cmd-close').trigger('click'); }, 0);
        }, 400);
    }   
});

 

//app.directive('datepickerstart', function () {
//    return function (scope, element, attrs) {
//        element.Zebra_DatePicker({
//            direction: false
//           , pair: $('.datepickeend')
//        });
//    }
//});

//app.directive('datepickeend', function () {
//    return function (scope, element, attrs) {
//        element.Zebra_DatePicker({
//            direction: 1
//        });
//    }
//});