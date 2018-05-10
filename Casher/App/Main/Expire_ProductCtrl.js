var page = 0;
app.controller("Expire_ProductCtrl", function ($scope, $http, $window, $timeout, $fancyModal) {
    //$scope.ListExport = { "NO": 0, "DepartmentFromID": 0, "DepartmentFromName": null, "MainSubject": null, "TreatmentTypeID": 0, "TreatmentTypeName": null, "DepartmentToID": 0, "DepartmentToName": null, "SubjectID": 0, "SubjectName": null, "EthbatNo": null, "DatBegin": null, "DatEnd": null, "Admin": false, "Majless": false, "MajlessAdmin": false, "AdminEmp": false, "Important": false, "Archived": false, "page": -1, "SessionAdmin": "0", "SessionAdmin": "0", "SessionMajlessAdmin": "0", "SessionAdminEmp": "0", "SessionImportantTreatment": "0" }
    $scope.Srch_Mowarid = { Invoice_From: "", Invoice_To: "", Mowrid_id: "", DateFrom: "", DateTo: "", Price_From: "", Price_To: "" };
    //----------------------------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------------------------
    // load all popuop data
    //----------------------------------------------------------------------------------------------------------------------------

    //----------------------------------------------------------------------------------------------------------------
    // Clear all Fields
    //----------------------------------------------------------------------------------------------------------------
    $scope.emptyInputs = function () {
        //var inputsNumbers = ["DepartmentFromID", "DepartmentToID", "TreatmentTypeID"]
        //for (var i = 0; i < inputsNumbers.length; i++) {
        //    document.getElementById(inputsNumbers[i]).value = 0;
        //    document.getElementById(inputsNumbers[i]).readOnly = false;
        //}
        //var inputsString = ["DepartmentFromName", "DepartmentToName", "TreatmentTypeName", "DatBegin", "DatEnd"]
        //for (var i = 0; i < inputsString.length; i++) {
        //    document.getElementById(inputsString[i]).value = null;
        //    document.getElementById(inputsString[i]).readOnly = false;
        //}
        $scope.Srch_Mowarid.Product_Name = "";
        $scope.Srch_Mowarid.Department_id = "";
        $scope.Srch_Mowarid.Company_id = "";
        $scope.Srch_Mowarid.Price_From = "";
        $scope.Srch_Mowarid.Price_To = "";
        $scope.Srch_Mowarid.Barcode_From = "";
        $scope.Srch_Mowarid.Barcode_To = "";
        $scope.Srch_Mowarid.Taxes = "";
    };
    //----------------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------------
    // Get AllRecords Functions
    //----------------------------------------------------------------------------------------------------------------
    $scope.getAllRecords = function (Page) {
        $scope.EndSearch = null;
        $scope.TableLoading = 1;
        // $("input[name='Date_Poduction[" + i +"]']").val()
        // ListExport.DatEnd = document.getElementById("DatEnd").value;

       
        var parameter = JSON.stringify({ 'Srch_Mowarid': $scope.Srch_Mowarid, 'page': page });
        $http.post("/Search/Invoice_Mowarid", parameter)
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
    //----------------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------------
    // search button Functions
    //----------------------------------------------------------------------------------------------------------------
    $scope.SearchButton = function () {
        page = 0;
        $scope.getAllRecords(page);
    };
    $scope.SearchButton();
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
    //----------------------------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------------------------
    // Redirect To Edit Page
    //----------------------------------------------------------------------------------------------------------------------------
    $scope.CallEditPage = function (response) {
        $window.location.href = '/Mowrid/Invoice_Mowrid/' + response.ID;
    }
});