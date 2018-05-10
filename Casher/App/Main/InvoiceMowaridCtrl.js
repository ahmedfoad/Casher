app.controller("InvoiceMowaridCtrl", function ($scope, $http, $window, $timeout, $uibModal) {
    $scope.Intaize_product = function () {
        $scope.Cls_Invoice_mowrid = {
            ID: "", Mowrid_id: "", Mowrid_Name: "", Date_Invoice: "", Date_Invoice_Hijri: "", Price: 0, Total_Sadad: "",
            ClsInvoiceMowrid_Product: null
        };
        var product = { Product_Id: "", Product_Name: "", Date_Poduction: "", Date_Expiration: "", Amount_ByJumla: "", Amount_ByUnit: "",  Price: 0, Carton_Count: "" };
        $scope.Cls_Invoice_mowrid.ClsInvoiceMowrid_Product = new Array();
        $scope.Cls_Invoice_mowrid.ClsInvoiceMowrid_Product.push(product);
        document.getElementById('InvoicePrice').innerHTML = $scope.Cls_Invoice_mowrid.Price;

    };
    $scope.hideElement = function (arr) {

        for (var i = 0; i < arr.length; i++) {
            if (document.getElementById(arr[i]) != null)
                document.getElementById(arr[i]).style.display = 'none';
        }
    }
    //----------------------------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------------------------
    // فاتورة مورد
    //----------------------------------------------------------------------------------------------------------------------------
    var DealingTypeView;
    //-------------------------------------------------
    $scope.getView = function () {
        $http.get("/Product/GetProducts")
         .success(function (response) {
             DealingTypeView = response;
         });
    };
    $scope.getView();
    $scope.loadproduct = function () {
        $uibModal.open({
            scope: $scope,
            template: DealingTypeView,
            resolve: {
                Invoice_Mowrid: function () {
                    return $scope.Cls_Invoice_mowrid;
                }
            }
        });
    };
    var mowridlist;

    $scope.getMowrid = function () {
        $http.get("/Mowrid/GetMowrid")
         .success(function (response) {
             mowridlist = response;
         });
    };
    $scope.getMowrid();
    $scope.loadMowrid = function () {


        $uibModal.open({
            template: mowridlist,
            scope: $scope,
            resolve: {
                Invoice_Mowrid: function () {
                    return $scope.Cls_Invoice_mowrid;
                }
            }
        });
    };
    
    //------------------تحميل الصنفات بعد فتح النافذة
    $scope.getAllMowrid = function (Page, Search) {
        var parameter = { 'Search': Search }
        var sentData = "page=" + Page;
        $http.get("/Mowrid/getAllMowrid?" + sentData, { params: parameter })
       .success(function (response) {
           if (response != null) {

               if ($scope.MowridList == null) {
                   $scope.MowridList = response;
               }
               else {
                   $scope.MowridList = $scope.MowridList.concat(response);
               }
           }
           //if (response == null || response == "") { otherData = 1; hideLoadMore = null; }
           //else {  otherData = null; hideLoadMore = 1; }
       });
    }
    $scope.getAllMowrid();
    //-------------------------------------------------
    //===================
    //=======عند الضفط على صف
  
    $scope.removeRow = function (index) {
        
        if ($scope.Cls_Invoice_mowrid.ClsInvoiceMowrid_Product.length > 1) {
            $scope.Cls_Invoice_mowrid.ClsInvoiceMowrid_Product.splice(index, 1);
            $scope.Calculate_price();
        }
    }
    $scope.closeUiModal = function () {
        this.$close();
    }
    //***************العمليات الحسابية -----------------------------------------------------------------------------
    $scope.Calculate_price = function () {
        $scope.Cls_Invoice_mowrid.Price = 0;
        for (var i = 0; i < $scope.Cls_Invoice_mowrid.ClsInvoiceMowrid_Product.length; i++) {
            if (isNaN(parseFloat($scope.Cls_Invoice_mowrid.ClsInvoiceMowrid_Product[i].Price)) == false) {
                $scope.Cls_Invoice_mowrid.Price += parseFloat($scope.Cls_Invoice_mowrid.ClsInvoiceMowrid_Product[i].Price);
            }
            document.getElementById('InvoicePrice').innerHTML = $scope.Cls_Invoice_mowrid.Price;
        }
    }
    //-----
    $scope.Calculate_items = function (item) {
        if (isNaN(parseFloat(item.Amount_ByJumla)) == false
            && isNaN(parseFloat(item.Carton_Count)) == false) {
            item.Amount_ByUnit = parseFloat(item.Amount_ByJumla) * parseFloat(item.Carton_Count);
        }
    }
    //----------------------------------------------------------------------------------------------------------------------------
    //------------------تحميل الصنفات بعد فتح النافذة
    $scope.getAllRecords = function (Page, Search) {
        var parameter = { 'Search': Search }
        var sentData = "page=" + Page;
        $http.get("/Product/getAllProducts?" + sentData, { params: parameter })
       .success(function (response) {
           if (response != null) {

               if ($scope.ProductList == null) {
                   $scope.ProductList = response;
               }
               else {
                   $scope.ProductList = $scope.ProductList.concat(response);
               }
           }
           //if (response == null || response == "") { otherData = 1; hideLoadMore = null; }
           //else {  otherData = null; hideLoadMore = 1; }
       });
    }

    $scope.getAllRecords();
    $scope.clicktr = function (row) {

        var index = $scope.Cls_Invoice_mowrid.ClsInvoiceMowrid_Product.length - 1;
        $scope.Cls_Invoice_mowrid.ClsInvoiceMowrid_Product[index].Product_Id = row.ID;
        $scope.Cls_Invoice_mowrid.ClsInvoiceMowrid_Product[index].Product_Name = row.Name;

        var product = { Product_Id: "", Product_Name: "", Date_Poduction: "", Date_Expiration: "", Amount_ByJumla: "", Amount_ByUnit: "",  Price: 0, Carton_Count: "" };
        $scope.Cls_Invoice_mowrid.ClsInvoiceMowrid_Product.push(product);
        this.$close();


    }
    //=======عند الضفط على صف
    $scope.clicktrMowarid = function (row) {
       $scope.Cls_Invoice_mowrid.Mowrid_id = row.ID;
       $scope.Cls_Invoice_mowrid.Mowrid_Name = row.Name;
        this.$close();
    }
    //****************************************************************************
    //-- عملية حفظ / تعديل / حذف الفاتورة
    $scope.switchCase = function () {

        var id = document.URL.split("/")[5];
        if (id == null) {
            $scope.SaveBtn = null;
            $scope.EditBtn = 1;
            $scope.DeleteBtn = 1;
            $scope.dataEntire = null;
            var hiddenElements = ["Numbers", "Barcode", "EditElements", "EditElements2"]
            $scope.hideElement(hiddenElements);
        }
        else {
            var numericExpression = /^[0-9]+$/;
            if (id.match(numericExpression)) {
                $scope.SaveBtn = 1;
                $scope.dataEntire = 1;
                $scope.EditBtn = null;
                $scope.DeleteBtn = null;
                
                
                $http.get('/Mowrid/loadInvoice/' + id)
                   .success(function (response) {
                       if (response == null) {
                           $uibModal.open({
                               template: " <div class='portlet box red' style='min-height:150px !important;min-width: 541px !important;'> <div class='portlet-title'> <div class='caption'> <i class='fa-lg fa fa-warning'></i> تنبيه هام </div><div class='tools'> </div></div><div class='portlet-body form portlet-empty' style='min-height: 140px;'> <div class='col-md-12 page-404'> <div class='number'>" + response.ErrorNumber + "</div><div class='details'> <h3 style='font-family: sans-serif; text-align: center; font-weight: bold;'>" + response.ErrorFullNumber + "</h3> <p style='font-size: 12px;'> " + response.ErrorName + " <br/> <br/> <a class='btn btn-default btn-sm' href='/home'> الصفحة الرئيسية </a> <a class='btn btn-default btn-sm' href='" + response.Url + "' style='margin-right: 4px;'>المحاولة مرة آخري</a> </p></div></div></div></div>",
                               closeOnEscape: false,
                               closeOnOverlayClick: false
                           });
                       }
                       else {
                           console.log(response);
                           $scope.Cls_Invoice_mowrid = response;
                           document.getElementById('InvoicePrice').innerHTML = $scope.Cls_Invoice_mowrid.Price;
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
    $scope.changeBorder = function (arr, color) {
        for (var i = 0; i < arr.length; i++) {
            document.getElementById(arr[i]).style.borderColor = color;
        }
    }

    $scope.changeBorderJQ = function (input, color) {
        $('input[name="' + input + '"]').css("borderColor", color);
    }
    $scope.Save = function () {

        var CheckForm = $scope.FormValidation();
        if (CheckForm == true) {
            $scope.DBsave($scope.Cls_Invoice_mowrid);
        }
        else {
            $scope.AlertParameter = 1;
            document.body.scrollTop = 0;
        }

    }
    $scope.ReomveAlert = function () {
        $scope.AlertParameter = null;
    }
    $scope.FormValidation = function () {
        $scope.Cls_Invoice_mowrid.Date_Invoice = document.getElementById("Date_Invoice").value;
       // var JawalNOCheck = Mowrid.JawalNO.match("^05[0-9]{8}$");
        if ($scope.Cls_Invoice_mowrid.Mowrid_Name == null || $scope.Cls_Invoice_mowrid.Mowrid_Name == "") {
            $scope.ErrorName = "برجاء ادخال اسم المورد";
            var ListID = ["Mowrid_Name"];
            $scope.changeBorder(ListID, "red");
            return false;
        } else {
            var ListID = ["Mowrid_Name"];
            $scope.changeBorder(ListID, "#c2cad8");
        }

        if ($scope.Cls_Invoice_mowrid.Date_Invoice == null || $scope.Cls_Invoice_mowrid.Date_Invoice == "") {
            $scope.ErrorName = "برجاء ادخال تاريخ الشراء";
            var ListID = ["Date_Invoice"];
            $scope.changeBorder(ListID, "red");
            return false;
        } else {
            var ListID = ["Date_Invoice"];
            $scope.changeBorder(ListID, "#c2cad8");
        }
        var len = $scope.Cls_Invoice_mowrid.ClsInvoiceMowrid_Product.length;
        for (var i = 0; i < len; i++) {
            $scope.Cls_Invoice_mowrid.ClsInvoiceMowrid_Product[i].Date_Poduction = $("input[name='Date_Poduction[" + i +"]']").val();
            $scope.Cls_Invoice_mowrid.ClsInvoiceMowrid_Product[i].Date_Expiration = $("input[name='Date_Expiration[" + i +"]']").val();

        }
        for (var i = 0; i < len; i++) {
            if (len = 1
                ||
               ((len > 1) && i < (len - 1))) {

                if ($scope.Cls_Invoice_mowrid.ClsInvoiceMowrid_Product[i].Product_Name == null || $scope.Cls_Invoice_mowrid.ClsInvoiceMowrid_Product[i].Product_Name == "") {
                    $scope.ErrorName = "برجاء ادخال اسم الصنف";
                    var ListID = ["Product_Name[" + i +"]"];
                    $scope.changeBorderJQ(ListID, "red");
                    return false;
                }
                else {
                    var ListID = ["Product_Name[" + i +"]"];
                    $scope.changeBorderJQ(ListID, "#c2cad8");
                }

                if ($scope.Cls_Invoice_mowrid.ClsInvoiceMowrid_Product[i].Date_Poduction == null
                    || $scope.Cls_Invoice_mowrid.ClsInvoiceMowrid_Product[i].Date_Poduction == "") {
                    $scope.ErrorName = "برجاء ادخال تاريخ الانتاج";
                    var ListID = ["Date_Poduction[" + i +"]"];
                    $scope.changeBorderJQ(ListID, "red");
                    return false;
                }
                else {
                    var ListID = ["Date_Poduction[" + i +"]"];
                    $scope.changeBorderJQ(ListID, "#c2cad8");
                }

                if ($scope.Cls_Invoice_mowrid.ClsInvoiceMowrid_Product[i].Date_Expiration == null
                    || $scope.Cls_Invoice_mowrid.ClsInvoiceMowrid_Product[i].Date_Expiration == "") {
                    $scope.ErrorName = "برجاء ادخال تاريخ الانتهاء";
                    var ListID = ["Date_Expiration[" + i +"]"];
                    $scope.changeBorderJQ(ListID, "red");
                    return false;
                }
                else {
                    var ListID = ["Date_Expiration[" + i +"]"];
                    $scope.changeBorderJQ(ListID, "#c2cad8");
                }

                if ($scope.Cls_Invoice_mowrid.ClsInvoiceMowrid_Product[i].Amount_ByJumla == null
                   || $scope.Cls_Invoice_mowrid.ClsInvoiceMowrid_Product[i].Amount_ByJumla == "") {
                    $scope.ErrorName = "برجاء ادخال العدد بالجملة";
                    var ListID = ["Amount_ByJumla[" + i +"]"];
                    $scope.changeBorderJQ(ListID, "red");
                    return false;
                }
                else {
                    var ListID = ["Amount_ByJumla[" + i +"]"];
                    $scope.changeBorderJQ(ListID, "#c2cad8");
                }

                if ($scope.Cls_Invoice_mowrid.ClsInvoiceMowrid_Product[i].Carton_Count == null
                  || $scope.Cls_Invoice_mowrid.ClsInvoiceMowrid_Product[i].Carton_Count == "") {
                    $scope.ErrorName = "برجاء ادخال عدد الكراتين";
                    var ListID = ["Carton_Count[" + i +"]"];
                    $scope.changeBorderJQ(ListID, "red");
                    return false;
                }
                else {
                    var ListID = ["Carton_Count[" + i +"]"];
                    $scope.changeBorderJQ(ListID, "#c2cad8");
                }

                if ($scope.Cls_Invoice_mowrid.ClsInvoiceMowrid_Product[i].Amount_ByUnit == null
                  || $scope.Cls_Invoice_mowrid.ClsInvoiceMowrid_Product[i].Amount_ByUnit == "") {
                    $scope.ErrorName = "برجاء ادخال العدد بالحبة";
                    var ListID = ["Amount_ByUnit[" + i +"]"];
                    $scope.changeBorderJQ(ListID, "red");
                    return false;
                }
                else {
                    var ListID = ["Amount_ByUnit[" + i +"]"];
                    $scope.changeBorderJQ(ListID, "#c2cad8");
                }

                if ($scope.Cls_Invoice_mowrid.ClsInvoiceMowrid_Product[i].Price == null || $scope.Cls_Invoice_mowrid.ClsInvoiceMowrid_Product[i].Price == "") {
                    $scope.ErrorName = "برجاء ادخال السعر";
                    var ListID = ["Price[" + i +"]"];
                    $scope.changeBorderJQ(ListID, "red");
                    return false;
                }
                else {
                    var ListID = ["Price[" + i +"]"];
                    $scope.changeBorderJQ(ListID, "#c2cad8");
                }
            }
           
        }
       
        
        return true;
    }
    $scope.DBsave = function (Mowrid) {
        var ID = document.URL.split("/")[5];
        var parameter = JSON.stringify($scope.Cls_Invoice_mowrid);
        if (ID == null) {
            $http.post('/Mowrid/InsertInvoice', parameter)
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
                                 $window.location.href = '/Mowrid/Invoice_Mowrid/' + response.ID;
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
                               $window.location.href = '/Mowrid/Operation/' + Mowrid.ID;
                           }, 3000);
                       }
                   }
               });
        }
    }

});