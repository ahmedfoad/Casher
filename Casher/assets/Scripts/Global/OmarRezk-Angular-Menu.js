﻿app.controller("menuController", ['$scope', function ($scope) {
    $scope.items = [
        {
            name: "الصفحة الرئيسية",
            link: "/Home",
            i: "icon-home",
            LiClass: "start active",
            submenu: []
        },
        {
            name: "التجهيز والإعداد",
            link: "#",
            i: "icon-wrench",
            submenu: [
                {
                    name: "الإستعلام عن المستخدمين",
                    link: "#"
                },
                {
                    name: "إضافة المستخدمين",
                    link: "#sub2"
                },
                {
                    name: "المراقبة",
                    link: "#sub2"
                },
                {
                    name: "عن البرنامج",
                    link: "#sub2"
                }
            ]
        },
        {
            name: "البيانات الأساسية",
            link: "#",
            i: "icon-settings",
            submenu: [
                {
                    name: "الموردين ",
                    link: "#"
                },
                {
                    name: "أنواع المعاملات",
                    link: "#"
                },
                {
                    name: "أنواع المواضيع",
                    link: "#"
                },
                {
                    name: "الإدارات والأقسام",
                    link: "#"
                }
            ]
        }
        ,
        {
            name: "تسجيل المعاملات",
            link: "#",
            i: "icon-pencil",
            submenu: [
                {
                    name: "تسجيل المبيعات",
                    link: "#"
                },
                {
                    name: "تسجيل المعاملات الواردة",
                    link: "#"
                },
                {
                    name: "صور الإحاطة",
                    link: "#"
                },
                {
                    name: "إحالة المعاملات الواردة",
                    link: "#"
                },
                {
                    name: "حفظ ملفات الصادر في الأرشيف",
                    link: "#"
                },
                {
                    name: "حفظ ملفات الوارد في الأرشيف",
                    link: "#"
                },
                {
                    name: "إرسال رسالة اس ام اس",
                    link: "#"
                }
            ]
        },
        {
            name: "متابعة المعاملات",
            link: "#",
            i: "icon-wallet",
            submenu: [
                {
                    name: "تسديد معاملات واردة بمعاملات صادرة ",
                    link: "#"
                },
                {
                    name: "تسديد معاملات صادرة بمعاملات واردة",
                    link: "#"
                },
                {
                    name: " متابعة المعاملات المفتوحة",
                    link: "#"
                },
                {
                    name: " إنجاز موظفي الإتصالات والإدارات",
                    link: "#"
                }
            ]
        },
        {
            name: "البحث والإستعلام",
            link: "#",
            i: "icon-magnifier",
            submenu: [
                {
                    name: "الإستعلام عن معاملة صادرة",
                    link: "#"
                },
                {
                    name: "الإستعلام عن معاملة واردة",
                    link: "#"
                },
                {
                    name: " الإستعلام عن التقارير",
                    link: "#"
                },
                {
                    name: "الإستعلام عن صور الإحاطة",
                    link: "#"
                },
                {
                    name: "الإستعلام عن إنجاز المعاملات الواردة",
                    link: "#"
                }
            ]
        },
        {
            name: "التقارير والطباعة",
            link: "#",
            i: "icon-docs",
            submenu: [
                {
                    name: " الإستعلام عن مشترى",
                    link: "#"
                },
                {
                    name: "تقارير المعاملات الواردة",
                    link: "#"
                },
                {
                    name: "تقارير  إحالة المعاملات الواردة",
                    link: "#"
                },
                {
                    name: "تقارير صور الإحاطة",
                    link: "#"
                },
                {
                    name: "تقارير متابعة المعاملات المفتوحة",
                    link: "#"
                },
                {
                    name: "تقارير إنجاز موظفي الإتصالات والإدارات",
                    link: "#"
                }
            ]
        }
    ];
}]);