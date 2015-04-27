require.config({
    paths: {
        /*
         * 引入类库
         */
        // jQuery
        jquery: 'vendor/jquery',
        // bootstrap
        bootstrap: 'vendor/bootstrap/js/bootstrap',
        // Angular
        angular: 'vendor/angular/angular',
        cookies: 'vendor/angular/angular-cookies',
        route: 'vendor/angular/angular-route',
        angularTranslate: 'vendor/angular/angular-translate/angular-translate',
        // Angular-translate-loader-partial
        angularTranslateLoaderPartial: 'vendor/angular/angular-translate-loader-partial/angular-translate-loader-partial',
        // Angular-translate-storage-cookie
        angularTranslateStorageCookie: 'vendor/angular/angular-translate-storage-cookie/angular-translate-storage-cookie',
        // domReady
        domReady: 'vendor/domReady',
        // xml to json
        x2js: 'vendor/x2js/xml2json',
        // angular-file-uploader
        uploader: 'vendor/angular-file-uploader/angular-file-upload'
    },
    shim: {
        /*
         *对引入的类库，表明依赖关系，并暴露名称
         *需要把此处的名字，在app.js文件中define写上才能使用
         */
        angular: {
            deps: ['jquery'],
            exports: 'angular'
        },
        cookies: {
            deps: ['angular']
        },
        route: {
            deps: ['angular']
        },
        angularTranslate: {
            deps: ['angular']
        },
        angularTranslateLoaderPartial: {
            deps: ['angular', 'angularTranslate']
        },
        angularTranslateStorageCookie: {
            deps: ['angular', 'angularTranslate']
        },
        bootstrap: {
            deps: ['jquery']
        },
        x2js: {
            deps: ['jquery']
        },
        uploader: {
            deps:['angular']
        }
    }
});
require([
	'angular',
	'app',
	'domReady',
    // 自定义controllers,services,directives,filters都需要在这里添加路径
    'controllers/mainCtrl',
    'controllers/userCtrl',
    'controllers/diamondQueryCtrl',
    'controllers/diamondAssignCtrl',
    'controllers/diamondUploadCtrl',
    'controllers/orderCycleCtrl',
    'controllers/orderCycleEditCtrl',
    'controllers/modelCtrl',
    'controllers/dictionaryCtrl'
],
function (angular, app, domReady) {
    'use strict';
    app.config(['$routeProvider', '$httpProvider',
        function ($routeProvider, $httpProvider) {
            $routeProvider
            .when('/home', {
                templateUrl: 'views/home.html',
                // controller: 'mainCtrl'
            })
            .when('/userList', {
                templateUrl: 'views/Sys/userList.html',
                controller: 'userCtrl'
            }).when('/diamondList', {
                templateUrl: 'views/diamond/diamondQuery.html',
                controller: 'diamondQueryCtrl'
            }).when('/diamondAssign', {
                templateUrl: 'views/diamond/diamondAssign.html',
                controller: 'diamondAssignCtrl'
            }).when('/diamondUpload', {
                templateUrl: 'views/diamond/diamondUpload.html',
                controller: 'diamondUploadCtrl'
            })
            .when('/dictionaryList', {
                templateUrl: 'views/Sys/dictionaryList.html',
                controller: 'dictionaryCtrl'
            })
            // 订购周期维护
            .when('/orderCycleList', {
                templateUrl: 'views/Order/orderCycleList.html',
                controller: 'orderCycleCtrl'
            })
            .when('/orderCycleEdit', {
                templateUrl: 'views/Order/orderCycleEdit.html',
                controller: 'orderCycleEditCtrl'
            })
            //發鑲珠寶模型列表
            .when('/SCModelList', {
                templateUrl: 'views/Model/scModel.html',
                controller: 'modelCtrl'
            })
            // 静态的配石查询，配石调配，上传配石资料页面菜单路径
            .when('/html/diamondQuery', {
                templateUrl: 'views/html/diamondQuery.html'
            })
            .when('/html/diamondAssign', {
                templateUrl: 'views/html/diamondAssign.html'
            })
            .when('/html/diamondUpload', {
                templateUrl: 'views/html/diamondUpload.html'
            });// end

            $httpProvider.defaults.withCredentials = true;
            $httpProvider.defaults.useXDomain = true;
            delete $httpProvider.defaults.headers.common['X-Requested-With'];

            $httpProvider.interceptors.push(function ($q, $location, $rootScope) {
                return {
                    'responseError': function (rejection) {
                        if (rejection.status == 401) {      //会话过期
                            //$rootScope.isReLogin = true;
                            //return $location.path("login");
                            window.location.href = 'login.html';
                        } else if (rejection.status == 500) {       //后台出错
                            //return $location.path("login");
                        }
                        return $q.reject(rejection);
                    }
                };
            });
        }
    ]);

    app.config(function ($translateProvider, $translatePartialLoaderProvider) {
        $translatePartialLoaderProvider.addPart('menu');
        $translateProvider.useLoader('$translatePartialLoader', {
            urlTemplate: 'i18n/{lang}/{part}.xml'
        });
        $translateProvider.preferredLanguage('cn');
        $translateProvider.fallbackLanguage('cn');
        // $translateProvider.useCookieStorage();

    });

    domReady(function () {
        angular.bootstrap(document, ['eOrderingApp']);

        $('html').addClass('ng-app: eOrderingApp');
    })
});