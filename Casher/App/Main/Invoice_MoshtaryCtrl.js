app.controller("Invoice_MoshtaryCtrl", function ($scope, $http, $window, $timeout, $uibModal) {

    $scope.words = [];
    $scope.triggerChar = 9;
    $scope.separatorChar = 13;
    $scope.triggerCallback = function () {
        $scope.lastTrigger = 'Last trigger callback: ' + new Date().toISOString();
        $scope.$apply();
    };
    $scope.scanCallback = function (word) {
        // $scope.words.push(word);
      //  $scope.Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product[0].Product_Name = word;
        $http.get("/Product/GetbyBarcode?BarCode=" + word)
      .success(function (response) {
          if (response != null) {
              //البحث هل الصنف ده موجود قبل كدة 
              var index = $scope.Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product.findIndex(function (item, i) {
                  return item.Product_Id === response.ID;
              });

              if (index == -1) {
                  //لو الصنف مش موجود ضيف سطر جديد بعدد الاصناف = 1
                  index = $scope.Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product.length - 1;
                  $scope.Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product[index].Product_Id = response.ID;
                  $scope.Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product[index].Product_Name = response.Name;
                  $scope.Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product[index].Amount = 1;
                  $scope.Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product[index].Price = parseFloat(response.Price_Unit);
                  $scope.Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product[index].Taxes = parseFloat(response.Taxes);

                  $scope.Calculate_itemPrice($scope.Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product[index]);

                  var product = { Product_Id: "", Product_Name: "", Amount: "", Price: 0, Taxes: 0, TotalPrice: 0 };
                  $scope.Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product.push(product);
              } else {
                  // لو الصنف موجود نزود العدد +1
                  $scope.Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product[index].Amount = parseFloat($scope.Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product[index].Amount) + 1;
                  $scope.Calculate_itemPrice($scope.Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product[index]);

              }
          } else {
              alert("عفوا الصنف غير مسجل");
          }
          //if (response == null || response == "") { otherData = 1; hideLoadMore = null; }
          //else {  otherData = null; hideLoadMore = 1; }
      });
        $scope.$apply();
    };


    $scope.Intaize_product = function () {
        $scope.Cls_Invoice_Moshtary = {

            ID: "", Moshtary_id: "", Moshtary_Name: "", Date_Invoice: "", Date_Invoice_Hijri: "", Price: 0, Total_Sadad: "",
            ClsInvoiceMoshtary_Product: null
        };
        var product = { Product_Id: "", Product_Name: "", Amount: "", Price: 0 , Taxes: 0 , TotalPrice: 0 };
        $scope.Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product = new Array();
        $scope.Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product.push(product);
        document.getElementById('InvoicePrice').innerHTML = $scope.Cls_Invoice_Moshtary.Price;

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
                Cls_Invoice_Moshtary: function () {
                    return $scope.Cls_Invoice_Moshtary;
                }
            }
        });
    };
    var Moshtarylist;

    $scope.getMoshtary = function () {
        $http.get("/Invoice_Moshtary/GetMoshtary")
         .success(function (response) {
             Moshtarylist = response;
         });
    };
    $scope.getMoshtary();
    $scope.loadMoshtary = function () {


        $uibModal.open({
            template: Moshtarylist,
            scope: $scope,
            resolve: {
                Invoice_Moshtary: function () {
                    return $scope.Cls_Invoice_Moshtary;
                }
            }
        });
    };
    
    //------------------تحميل الصنفات بعد فتح النافذة
    $scope.getAllMoshtary = function (Page, Search) {
        var parameter = { 'Search': Search }
        var sentData = "page=" + Page;
        $http.get("/Invoice_Moshtary/getAllMoshtary?" + sentData, { params: parameter })
       .success(function (response) {
           if (response != null) {

               if ($scope.MoshtaryList == null) {
                   $scope.MoshtaryList = response;
               }
               else {
                   $scope.MoshtaryList = $scope.MoshtaryList.concat(response);
               }
           }
           //if (response == null || response == "") { otherData = 1; hideLoadMore = null; }
           //else {  otherData = null; hideLoadMore = 1; }
       });
    }
    $scope.getAllMoshtary();
    //-------------------------------------------------
    //===================
    //=======عند الضفط على صف
  
    $scope.removeRow = function (index) {
        
        if ($scope.Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product.length > 1) {
            $scope.Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product.splice(index, 1);
            $scope.Calculate_price();
        }
    }
    $scope.closeUiModal = function () {
        this.$close();
    }
    //***************العمليات الحسابية -----------------------------------------------------------------------------
    $scope.Calculate_price = function () {
        $scope.Cls_Invoice_Moshtary.Price = 0;
        for (var i = 0; i < $scope.Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product.length; i++) {
            if (isNaN(parseFloat($scope.Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product[i].TotalPrice)) == false) {
                $scope.Cls_Invoice_Moshtary.Price += parseFloat($scope.Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product[i].TotalPrice);
            }
            document.getElementById('InvoicePrice').innerHTML = $scope.Cls_Invoice_Moshtary.Price;
        }
    }
    //-----
    $scope.Calculate_itemPrice = function (item) {
        if (isNaN(parseFloat(item.Price)) == false && isNaN(parseFloat(item.Taxes)) == false && isNaN(parseFloat(item.Amount)) == false) {
            item.TotalPrice = (parseFloat(item.Price) + ((parseFloat(item.Price)) * (parseFloat(item.Taxes) / 100))) * parseFloat(item.Amount);
            $scope.Calculate_price();
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

        var index = $scope.Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product.length - 1;
        $scope.Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product[index].Product_Id = row.ID;
        $scope.Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product[index].Product_Name = row.Name;
        $scope.Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product[index].Price = parseFloat(row.Price_Unit);
        $scope.Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product[index].Taxes = parseFloat(row.Taxes);
        var product = { Product_Id: "", Product_Name: "", Amount: "", Price: 0 , Taxes: 0 , TotalPrice: 0 };
        $scope.Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product.push(product);
        this.$close();


    }
    //=======عند الضفط على صف
    $scope.clicktrMowarid = function (row) {
       $scope.Cls_Invoice_Moshtary.Moshtary_id = row.ID;
       $scope.Cls_Invoice_Moshtary.Moshtary_Name = row.Name;
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
                
                document.getElementById('PrintInvoice').href = "/Invoice_Moshtary/PrintInvoice/" + id;
                $http.get('/Invoice_Moshtary/loadInvoice/' + id)
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
                           $scope.Cls_Invoice_Moshtary = response;
                           document.getElementById('InvoicePrice').innerHTML = $scope.Cls_Invoice_Moshtary.Price;
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
            $scope.DBsave($scope.Cls_Invoice_Moshtary);
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
        var len = $scope.Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product.length;
        for (var i = 0; i < len; i++) {
            if (len = 1
                ||
               ((len > 1) && i < (len - 1))) {

                if ($scope.Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product[i].Product_Name == null || $scope.Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product[i].Product_Name == "") {
                    $scope.ErrorName = "برجاء ادخال اسم الصنف";
                    var ListID = ["Product_Name[" + i +"]"];
                    $scope.changeBorderJQ(ListID, "red");
                    return false;
                }
                else {
                    var ListID = ["Product_Name[" + i +"]"];
                    $scope.changeBorderJQ(ListID, "#c2cad8");
                }

                
                if ($scope.Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product[i].Amount == null
                   || $scope.Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product[i].Amount == "") {
                    $scope.ErrorName = "برجاء ادخال العدد";
                    var ListID = ["Amount[" + i +"]"];
                    $scope.changeBorderJQ(ListID, "red");
                    return false;
                }
                else {
                    var ListID = ["Amount[" + i +"]"];
                    $scope.changeBorderJQ(ListID, "#c2cad8");
                }

              

                if ($scope.Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product[i].Price == null || $scope.Cls_Invoice_Moshtary.ClsInvoiceMoshtary_Product[i].Price == "") {
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
    $scope.DBsave = function (Moshtary) {
        var ID = document.URL.split("/")[5];
        var parameter = JSON.stringify($scope.Cls_Invoice_Moshtary);
        if (ID == null) {
            $http.post('/Invoice_Moshtary/InsertInvoice', parameter)
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
                                 $window.location.href = '/Invoice_Moshtary/Invoice_Moshtary/' + response.ID;
                             }, 3000);
                         }
                     }
                 });
        }
        else {
            Moshtary.BarCodeArr = null;
            $http.post('/Invoice_Moshtary/Edit', parameter)
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
                               $window.location.href = '/Invoice_Moshtary/Operation/' + Moshtary.ID;
                           }, 3000);
                       }
                   }
               });
        }
    }

});