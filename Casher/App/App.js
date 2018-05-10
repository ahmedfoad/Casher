var app = angular.module("App", ['ui.bootstrap', 'barcodeScanner' , 'vesparny.fancyModal']);

app.directive('datepicker', function () {
    return function (scope, element, attrs) {
        element.Zebra_DatePicker({
        });
    }
});

app.directive('datepickerstart', function () {
    return function (scope, element, attrs) {
        element.Zebra_DatePicker({
            direction: false,
           pair: $('.datepickeend')
        });
    }
});

app.directive('datepickeend', function () {
    return function (scope, element, attrs) {
        element.Zebra_DatePicker({
            direction: 1
        });
    }
});

app.directive('numbersOnly', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attr, ngModelCtrl) {
            function fromUser(text) {
                if (text) {
                    var transformedInput = text.replace(/[^0-9]/g, '');

                    if (transformedInput != text) {
                        ngModelCtrl.$setViewValue(transformedInput);
                        ngModelCtrl.$render();
                    }
                    return transformedInput;
                }
                return undefined;
            }
            ngModelCtrl.$parsers.push(fromUser);
        }
    };
});

