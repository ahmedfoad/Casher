var page = 0;
app.controller("AllProductsCtrl", function ($scope, $http, $window, $timeout, $fancyModal) {
    $scope.ListExport = { "MainSubject": null, "DatBegin": null, "DatEnd": null, "FromNO": 0, "ToNO": 0, "TreatmentTypeID": 0, "TreatmentTypeName": null, "Year": 0, "DepartmentID": 0, "DepartmentName": null, "DepartmentFromID": 0, "DepartmentFromName": null, "OtherDepartments": null, "DepartmentToID": 0, "DepartmentToName": null, "SubjectID": 0, "SubjectName": null, "Replay": null, "Important": null, "SortList": null, "Payment": false, "Majless": false, "MajlessAdmin": false, "page": 0, "SessionImportantTreatment": null, "SessionMajless": null, "SessionMajlessAdmin": null }

    
    //----------------------------------------------------------------------------------------------------------------------------
    // load all popuop data
    //----------------------------------------------------------------------------------------------------------------------------
    var Departmentview, DealingTypeView, SubjectView;
    $scope.LoadDepartment = function (inputType) {
        $fancyModal.open({
            template: Departmentview + "<p id='inputType' style='display: none;'>" + inputType + "</p>",
            closeOnEscape: false,
            closeOnOverlayClick: false
        });
    };
    $scope.LoadDealingType = function () {
        $fancyModal.open({
            template: DealingTypeView,
            closeOnEscape: false,
            closeOnOverlayClick: false
        });
    };
    $scope.LoadSubject = function () {
        $fancyModal.open({
            template: SubjectView,
            closeOnEscape: false,
            closeOnOverlayClick: false
        });
    };
  
    //----------------------------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------------------------
    // DropDown lists
    //----------------------------------------------------------------------------------------------------------------------------
    $scope.ReplaytDrop = ["الكل", "غير مطلوب", "مطلوب الرد"];
    $scope.SortListDrop = ["رقم المعاملة", "الجهة المرسل لها"];
    //-----------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------
    // Clear all Fields
    //-----------------------------------------------------------------------------------------------------------------------------
    $scope.emptyInputs = function () {
        var inputsNumbers = ["DepartmentFromID", "DepartmentToID",  "TreatmentTypeID"]
        for (var i = 0; i < inputsNumbers.length; i++) {
            document.getElementById(inputsNumbers[i]).value = 0;
            document.getElementById(inputsNumbers[i]).readOnly = false;
        }
        var inputsString = ["DepartmentFromName", "DepartmentToName",  "TreatmentTypeName", "DatBegin", "DatEnd"]
        for (var i = 0; i < inputsString.length; i++) {
            document.getElementById(inputsString[i]).value = null;
            document.getElementById(inputsString[i]).readOnly = false;
        }
        $scope.data = null;
        $scope.ListExport.FromNO = 0;
        $scope.ListExport.ToNO = 0;
        $scope.ListExport.Year = 0;
        $scope.ListExport.OtherDepartments = null;
        $scope.ListExport.MainSubject = null;
        $scope.ListExport.page = -1;
        $scope.ListExport.Payment = false;
        $scope.ListExport.Majless = false;
        $scope.ListExport.MajlessAdmin = false;
        $scope.ListExport.SortList = null;
        $scope.ListExport.Replay = "الكل";
        $scope.ListExport.Important = "الكل";
    }
    //-----------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------
    // Get AllRecords Functions
    //-----------------------------------------------------------------------------------------------------------------------------
    $scope.getAllRecords = function (Page) {
        $scope.EndSearch = null;
        $scope.TableLoading = 1;

        // $("input[name='Date_Poduction[" + i +"]']").val()
        // ListExport.DatEnd = document.getElementById("DatEnd").value;
        var parameter = JSON.stringify({ 'Srch_Product': $scope.Srch_Product, 'page': page });
        $http.post("/Search/Product", parameter)
    .success(function (response) {
        $scope.TableLoading = null;
        if (response.length >= 1) {

            if (page == 0 || $scope.data == null) {

                $scope.data = response;

            }
            else {

                $scope.data = $scope.data.concat(response);
            }
            page++;
        }
        else if (response == '' || response == null) {
            $scope.EndSearch = 1;
        }
        else if (response.ErrorName.indexOf("المسموح") > -1) {
            $fancyModal.open({
                template: " <div class='portlet box red' style='min-height:150px !important;min-width: 541px !important;'> <div class='portlet-title'> <div class='caption'> <i class='fa-lg fa fa-warning'></i> تنبيه هام </div><div class='tools'> </div></div><div class='portlet-body form portlet-empty' style='min-height: 140px;'> <div class='col-md-12 page-404'> <div class='number'>" + response.ErrorNumber + "</div><div class='details'> <h3 style='font-family: sans-serif; text-align: center; font-weight: bold;'>" + response.ErrorFullNumber + "</h3> <p style='font-size: 12px;'> " + response.ErrorName + " <br/> <br/> <a class='btn btn-default btn-sm' href='/home'> الصفحة الرئيسية </a> <a class='btn btn-default btn-sm' href='" + response.Url + "' style='margin-right: 4px;'>المحاولة مرة آخري</a> </p></div></div></div></div>",
                closeOnEscape: false,
                closeOnOverlayClick: false
            });
        }
    });
    };
    //-----------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------
    // search button Functions
    //-----------------------------------------------------------------------------------------------------------------------------
    $scope.SearchButton = function (ListExport) {
        $scope.data = null;
        ListExport.page = 0;
        $scope.getAllRecords(ListExport);
    };
    //----------------------------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------------------------
    // get data of users with scrolling 
    //----------------------------------------------------------------------------------------------------------------------------
    angular.element($window).bind("scroll", function () {
        var windowHeight = "innerHeight" in window ? window.innerHeight : document.documentElement.offsetHeight;
        var body = document.body, html = document.documentElement;
        var docHeight = Math.max(body.scrollHeight, body.offsetHeight, html.clientHeight, html.scrollHeight, html.offsetHeight);
        windowBottom = windowHeight + window.pageYOffset;
        if (windowBottom >= docHeight) {

            $scope.getAllRecords(page);
        }
    });
    //-----------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------
    // print button Functions
    //-----------------------------------------------------------------------------------------------------------------------------   
    $scope.PrintButton = function (ListExport) {
       
        //$fancyModal.open({
        //    template: "<html><style>#myProgress{position: relative; width: 100%; height: 30px;margin-top: -25px; background-color: #ddd;}#myBar{position: absolute; width: 1%; height: 100%; background-color: #337ab7;}#label{text-align: center; line-height: 30px; color: white;}</style><body><div class='portlet box blue' style='min-height:150px !important'> <div class='portlet-title'> <div class='caption'> <i class='fa-lg fa fa-warning'></i> تنبيه هام </div><div class='actions'> <a class='btn default btn-sm fancymodal-close' id='Reportloading' style='padding-left:15px;display:none;' href='#'> إغلاق </a> </div></div><div class='portlet-body form portlet-empty' style='min-height: 140px;'> <div class='col-md-12 page-404'> <div class='number'>...</div><div class='details'> <h3 style='font-family: sans-serif; text-align: center; font-weight: bold;'>تحميل التقرير</h3> <p style='font-size: 12px;'> برجاء الإنتظار جاري تحميل التقرير</p><br/> <div id='myProgress'> <div id='myBar'> <div id='label'>1%</div></div></div></div></div></div><button id='BarClick' style='display:none;' onclick='move()'>Click Me</button><script>document.getElementById('BarClick').click();function move(){var elem=document.getElementById('myBar'); var width=1; var id=setInterval(frame, 1000); function frame(){if (width >=100){clearInterval(id);}else{width++; elem.style.width=width + '%'; document.getElementById('label').innerHTML=width * 1 + '%';}}}</script></body></html>",
        //    closeOnEscape: false,
        //    closeOnOverlayClick: false
        //});
        var parameter = JSON.stringify($scope.ListExport);
        $http.post("/Report_Product/AllProducts", parameter)
        .success(function (response) {
            //document.getElementById('Reportloading').click();
            if (response !="") {
                var myPdfUrl = response;
                $window.open(myPdfUrl);
            }
            else {
                if (response.indexOf("المسموح") > -1 || response.indexOf("تقارير") > -1 || response.indexOf("الملف") > -1) {
                    $fancyModal.open({
                        template: " <div class='portlet box red' style='min-height:150px !important;min-width: 541px !important;'> <div class='portlet-title'> <div class='caption'> <i class='fa-lg fa fa-warning'></i> تنبيه هام </div><div class='tools'> </div></div><div class='portlet-body form portlet-empty' style='min-height: 140px;'> <div class='col-md-12 page-404'> <div class='number'>" + response.ErrorNumber + "</div><div class='details'> <h3 style='font-family: sans-serif; text-align: center; font-weight: bold;'>" + response.ErrorFullNumber + "</h3> <p style='font-size: 12px;'> " + response.ErrorName + " <br/> <br/> <a class='btn btn-default btn-sm' href='/home'> الصفحة الرئيسية </a> <a class='btn btn-default btn-sm' href='" + response.Url + "' style='margin-right: 4px;'>المحاولة مرة آخري</a> </p></div></div></div></div>",
                        closeOnEscape: false,
                        closeOnOverlayClick: false
                    });
                }
            }
        });
    }
});