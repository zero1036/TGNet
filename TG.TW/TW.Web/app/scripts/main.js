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
            deps: ['angular']
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
    app.constant('ACCESS_LEVELS', {
        pub: 1,
        user: 2
    });

    app.config(['$routeProvider', '$httpProvider', 'ACCESS_LEVELS',
        function ($routeProvider, $httpProvider, ACCESS_LEVELS) {
            $routeProvider
            .when('/home', {
                templateUrl: 'views/home.html',
                controller: 'mainCtrl',
                access_level: ACCESS_LEVELS.pub
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
                controller: 'orderCycleCtrl',
                access_level: ACCESS_LEVELS.pub
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
            })
            //WA页面测试
            .when('/testAuth', {
                templateUrl: 'views/Test/testAuth.html',
                controller: 'orderCycleCtrl',
                access_level: ACCESS_LEVELS.pub
            });// end


            $httpProvider.defaults.withCredentials = true;
            $httpProvider.defaults.useXDomain = true;
            delete $httpProvider.defaults.headers.common['X-Requested-With'];

            $httpProvider.interceptors.push(function ($q, $location, $rootScope) {
                return {
                    'response': function (resp) {
                        //if (resp.config.url == 'api/Login/Login') {
                        //    // 假设API服务器返回的数据格式如下:
                        //    // { token: "AUTH_TOKEN" }
                        //    authService.setToken(resp.data.token);
                        //}
                        return resp;
                    },
                    'responseError': function (rejection) {
                        // 错误处理
                        switch (rejection.status) {
                            case 401:
                                if (rejection.config.url !== 'api/login')
                                    // 如果当前不是在登录页面
                                    $rootScope.$broadcast('auth:loginRequired');
                                break;
                            case 403:
                                $rootScope.$broadcast('auth:forbidden');
                                break;
                            case 404:
                                $rootScope.$broadcast('page:notFound');
                                break;
                            case 500:
                                $rootScope.$broadcast('server:error');
                                break;
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

    //app.config('authCfg'
    //    , ['$httpProvider'
    //    , function ($httpProvider) {
    //        // 在这里构造拦截器
    //        var interceptor = function ($q, $rootScope, Auth) {
    //            return {
    //                'response': function (resp) {
    //                    if (resp.config.url == '/Login/Login') {
    //                        // 假设API服务器返回的数据格式如下:
    //                        // { token: "AUTH_TOKEN" }
    //                        Auth.setToken(resp.data.token);
    //                    }
    //                    return resp;
    //                },
    //                'responseError': function (rejection) {
    //                    // 错误处理
    //                    switch (rejection.status) {
    //                        case 401:
    //                            if (rejection.config.url !== 'api/login')
    //                                // 如果当前不是在登录页面
    //                                $rootScope.$broadcast('auth:loginRequired');
    //                            break;
    //                        case 403:
    //                            $rootScope.$broadcast('auth:forbidden');
    //                            break;
    //                        case 404:
    //                            $rootScope.$broadcast('page:notFound');
    //                            break;
    //                        case 500:
    //                            $rootScope.$broadcast('server:error');
    //                            break;
    //                    }
    //                    return $q.reject(rejection);
    //                }
    //            };
    //        };
    //        // 将拦截器和$http的request/response链整合在一起
    //        $httpProvider.interceptors.push(interceptor);

    //    }]);


    app.run(function ($rootScope, $location, authService) {
        // 给$routeChangeStart设置监听
        $rootScope.$on('$routeChangeStart', function (evt, next, curr) {
            if (!authService.getUserIsUsed()) {
                alert("登录超时，请重新登陆");
                window.location.href = "login.html";
            } else {
                if (!authService.isAuthorized(next.$$route.access_level)) {
                    //if (authService.isLoggedIn()) {
                    //    // 用户登录了，但没有访问当前视图的权限
                    //    $location.path('/');
                    //} else {
                    //    $location.path('/login');
                    //}
                    if (!authService.isMenuAuthorized(next.$$route.originalPath)) {
                        $location.path('/home');
                    }
                }
                else {
                    $location.path('/home');
                }
            }
        });
    });



    domReady(function () {
        angular.bootstrap(document, ['eOrderingApp']);

        $('html').addClass('ng-app: eOrderingApp');
    })
});