var Search;
var page = 0;
app.controller("Sadad_TypeCtrl", function ($scope, $http, $window, $timeout, $fancyModal) {
    $scope.Sadad_Type = { ID: 0, Name: "" };
 
    //----------------------------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------------------------
    // function get all records 
    //----------------------------------------------------------------------------------------------------------------------------
    $scope.getAllRecords = function (Page, Search) {
        var parameter = { 'Search': Search }
        var sentData = "page=" + Page;
        $http.get("/Sadad_Type/getAll?" + sentData, { params: parameter })
        .success(function (response) {
            if (response != null) {
                if ($scope.data == null) {
                    $scope.data = response;
                }
                else {
                    $scope.data = $scope.data.concat(response);
                }
            }
        });
    }
    //----------------------------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------------------------

    //----------------------------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------------------------
    // load all popuop data
    //----------------------------------------------------------------------------------------------------------------------------
    var  DealingTypeView ;
    $scope.Create = function () {
        $fancyModal.open({
            template: DealingTypeView,
            closeOnEscape: false,
            closeOnOverlayClick: false
        });
    };
    
    $scope.getView = function () {


        $http.get("/Sadad_Type/Create")
         .success(function (response) {
             DealingTypeView = response;
         });
        
       
    };
    $scope.getView();
    //----------------------------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------------------------
    // instanse Search for Sadad_Type
    //----------------------------------------------------------------------------------------------------------------------------
    var lastentry = "";
    $scope.keyupevt = function () {
        if ($scope.Search != lastentry) {
            $scope.data = null;
            page = 0;
            $scope.getAllRecords(null, $scope.Search);
        }
        lastentry = $scope.Search;
    };
    //----------------------------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------------------------
    // get data of Sadad_Type with scrolling 
    //----------------------------------------------------------------------------------------------------------------------------
    angular.element($window).bind("scroll", function () {
        var windowHeight = "innerHeight" in window ? window.innerHeight : document.documentElement.offsetHeight;
        var body = document.body, html = document.documentElement;
        var docHeight = Math.max(body.scrollHeight, body.offsetHeight, html.clientHeight, html.scrollHeight, html.offsetHeight);
        windowBottom = windowHeight + window.pageYOffset;
        if (windowBottom >= docHeight) {
            page++;
            $scope.getAllRecords(page, $scope.Search);
        }
    });
    //----------------------------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------------------------
    // add new Sadad_Type
    //----------------------------------------------------------------------------------------------------------------------------
    $scope.SaveData = function (Sadad_Type) {
        Sadad_Type.Name = document.getElementById("Name").value;
        var CheckForm = $scope.Form_Validation(Sadad_Type);
        if (CheckForm == true) {
            $scope.DBsave(Sadad_Type);
        }
        else {
            $scope.AlertParameter = 1;
            document.body.scrollTop = 0;
        }

    }

    $scope.DBsave = function (Sadad_Type) {
       
        var parameter = JSON.stringify(Sadad_Type);
        if ($scope.Sadad_Type.ID == 0) {
            $http.post('/Sadad_Type/Insert', parameter)
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
                                 $window.location.href = '/Sadad_Type/Index/';
                             }, 1000);
                         }
                     }
                 });
        }
        else {
             
            $http.post('/Sadad_Type/Edit', parameter)
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
                               $window.location.href = '/Sadad_Type/Index';
                           }, 1000);
                       }
                   }
               });
        }
    }
    //----------------------------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------------------------
    // Form Validation
    //----------------------------------------------------------------------------------------------
    $scope.Form_Validation = function (Sadad_Type) {
        if (Sadad_Type.Name == null || Sadad_Type.Name == "") {
            $scope.ErrorName = "برجاء ادخال اسم السداد"
            var ListIDRed = ["Name"];
            $scope.changeBorder(ListIDRed, "red");
            return false;
        }
        $scope.AlertParameter = null;
        var ListID = ["Name"];
        $scope.changeBorder(ListID, "#c2cad8");
        return true;
    }
    $scope.ReomveAlert = function () {
        
        $scope.AlertParameter = null;
    }
    $scope.changeBorder = function (arr, color) {
        for (var i = 0; i < arr.length; i++) {
            document.getElementById(arr[i]).style.borderColor = color;
        }
    }
    //----------------------------------------------------------------------------------
    $scope.InsertDiv = function () {
        $scope.Insert = 1;
    };
    $scope.RemoveDiv = function () {
        $scope.Insert = null;
        $scope.Sadad_TypeName = null;
    };
    $scope.ReomveAlert = function () {
        $scope.ReomveAlertParameter = 1;
    }
    //----------------------------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------------------------
    // Edit Sadad_Type
    //----------------------------------------------------------------------------------------------------------------------------
    $scope.Edit = function (Sadad_Type) {
        //DataService.Sadad_Type(Sadad_Type);
        //// console.log(DataService.Get());
        
        
        $fancyModal.open({
            template:
            "<div class='row' ng-controller='Sadad_TypeCtrl' style='height:200px;'>                                                                                                                             "
+ "    <div class='col-md-12'>                                                                                                                                                                       "
+ "                                                                                                                                                                                                  "
+ "        <div class='portlet light'>                                                                                                                                                               "
+ "                                                                                                                                                                                                  "
+ "            <div class='portlet-title'>                                                                                                                                                           "
+ "                <div class='caption'>                                                                                                                                                             "
+ "                    <i class='icon-paper-plane font-yellow-casablanca' style='color: #5c9acf !important;'></i>                                                                                    "
+ "                    تعديل السداد                                                                                                                                                                          "
+ "                </div>                                                                                                                                                                            "
+ "                <div class='actions'>                                                                                                                                                             "
+ "                    <a class='btn red btn-sm fancymodal-close' id='closeDealingType' ng-click='closeDealingType()' style='padding-left:15px;' href='#'> إغلاق </a>                                 "
+ "                </div>                                                                                                                                                                            "
+ "            </div>                                                                                                                                                                                "
+ "            <div class='portlet-body'>                                                                                                                                                            "
+ "                <div class='col-md-12' ng-show='AlertParameter'>                                                                                                                                  "
+ "                    <div class='m-heading-1 border-green m-bordered'>                                                                                                                             "
+ "                        <span style='color:red'> {{ErrorName}} </span>                                                                                                                            "
+ "                        <button type='button' style='margin-top: 6px;' class='close' ng-click='ReomveAlert()'></button>                                                                           "
+ "                    </div>                                                                                                                                                                        "
+ "                </div>                                                                                                                                                                            "
+ "                <div class='row' style='margin: auto;'>                                                                                                                                           "
+ "                    <label for='Name'>اسم السداد</label>                                                                                                                                   "
+ "                    <input id='Name' value='" + Sadad_Type.Name+ "'  class='form-control' placeholder='اسم السداد' type='text' />                                                          "
+ "                    <input id='ID' type='hidden' value='" + Sadad_Type.ID + "'/>                                                                                              "
+ "                                                                                                                                                                                                  "
+ "                </div>                                                                                                                                                                            "
+ "                                                                                                                                                                                                  "
+ "            </div>                                                                                                                                                                                "
+ "            <div class='row form-actions' style='text-align:center'>                                                                                                                              "

+ "                <button type='submit' class='btn green' ng-hide='EditBtn' ng-click='EditData()'>تعديل</button>                                                                            "

+ "            </div>                                                                                                                                                                                "
+ "        </div>                                                                                                                                                                                    "
+ "                                                                                                                                                                                                  "
+ "                                                                                                                                                                                                  "
+ "                                                                                                                                                                                                  "
+ "    </div>                                                                                                                                                                                        "
+ "                                                                                                                                                                                                  "
+ "                                                                                                                                                                                                  "
+ "</div>                                                                                                                                                                                            "
                ,
            closeOnEscape: false,
            closeOnOverlayClick: false
        });
    };
  
    $scope.EditData = function () {
        $scope.Sadad_Type = { ID:  document.getElementById("ID").value, Name:document.getElementById("Name").value };
        var CheckForm = $scope.Form_Validation($scope.Sadad_Type);
        if (CheckForm == true) {
            $scope.DBsave($scope.Sadad_Type);
        }
        else {
            $scope.AlertParameter = 1;
            document.body.scrollTop = 0;
        }

    }
    //----------------------------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------------------------
    // Delete Sadad_Type
    //----------------------------------------------------------------------------------------------------------------------------
    $scope.Delete = function (Sadad_Type) {
        $fancyModal.open({
            template:
            "<div class='row' ng-controller='Sadad_TypeCtrl' style='height:200px;'>                                                                                                                             "
+ "    <div class='col-md-12'>                                                                                                                                                                       "
+ "                                                                                                                                                                                                  "
+ "        <div class='portlet light'>                                                                                                                                                               "
+ "                                                                                                                                                                                                  "
+ "            <div class='portlet-title'>                                                                                                                                                           "
+ "                <div class='caption'>                                                                                                                                                             "
+ "                    <i class='icon-paper-plane font-yellow-casablanca' style='color: #5c9acf !important;'></i>                                                                                    "
+ "                    حذف السداد                                                                                                                                                                             "
+ "                </div>                                                                                                                                                                            "
+ "                <div class='actions'>                                                                                                                                                             "
+ "                    <a class='btn red btn-sm fancymodal-close' id='closeDealingType' ng-click='closeDealingType()' style='padding-left:15px;' href='#'> إغلاق </a>                                 "
+ "                </div>                                                                                                                                                                            "
+ "            </div>                                                                                                                                                                                "
+ "            <div class='portlet-body'>                                                                                                                                                            "
+ "                <div class='col-md-12' ng-show='AlertParameter'>                                                                                                                                  "
+ "                    <div class='m-heading-1 border-green m-bordered'>                                                                                                                             "
+ "                        <span style='color:red'> {{ErrorName}} </span>                                                                                                                            "
+ "                        <button type='button' style='margin-top: 6px;' class='close' ng-click='ReomveAlert()'></button>                                                                           "
+ "                    </div>                                                                                                                                                                        "
+ "                </div>                                                                                                                                                                            "
+ "                <div class='row' style='margin: auto;'>                                                                                                                                           "
+ "                    <label for='Name'>اسم السداد</label>                                                                                                                                   "
+ "                    <input id='Name' readonly='readonly' value='" + Sadad_Type.Name + "'  class='form-control' placeholder='اسم السداد' type='text' />                                                          "
+ "                    <input id='ID' type='hidden' value='" + Sadad_Type.ID + "'/>                                                                                              "
+ "                                                                                                                                                                                                  "
+ "                </div>                                                                                                                                                                            "
+ "                                                                                                                                                                                                  "
+ "            </div>                                                                                                                                                                                "
+ "            <div class='row form-actions' style='text-align:center'>                                                                                                                              "
+ "                <button type='submit' class='btn red'  ng-click='DeleteData()'>حذف</button>                                                                            "
+ "            </div>                                                                                                                                                                                "
+ "        </div>                                                                                                                                                                                    "
+ "                                                                                                                                                                                                  "
+ "                                                                                                                                                                                                  "
+ "                                                                                                                                                                                                  "
+ "    </div>                                                                                                                                                                                        "
+ "                                                                                                                                                                                                  "
+ "                                                                                                                                                                                                  "
+ "</div>                                                                                                                                                                                            "
                ,
            closeOnEscape: false,
            closeOnOverlayClick: false
        });
    };

    $scope.DeleteData = function () {
        var _id = document.getElementById("ID").value;
        $http.post('/Sadad_Type/Delete/'+ _id)
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
                                    $window.location.href = '/Sadad_Type/Index/';
                                }, 1000);
                            }
                        }
                    });

    }
    //----------------------------------------------------------------------------------------------------------------------------

    //----------------------------------------------------------------------------------------------------------------------------
    // function done after page load
    //----------------------------------------------------------------------------------------------------------------------------
    $scope.ReomveAlertParameter = 1;
    $scope.getAllRecords(null, null); 
});