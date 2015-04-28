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
    'controllers/orderCycleCtrl',
    'controllers/orderCycleEditCtrl',
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
                // controller: 'mainCtrl'
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
                        if (resp.config.url == '/Login/Login') {
                            // 假设API服务器返回的数据格式如下:
                            // { token: "AUTH_TOKEN" }
                            Auth.setToken(resp.data.token);
                        }
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

                    //'responseError': function (rejection) {
                    //    if (rejection.status == 401) {      //会话过期
                    //        //$rootScope.isReLogin = true;
                    //        //return $location.path("login");
                    //        window.location.href = 'login.html';
                    //    } else if (rejection.status == 500) {       //后台出错
                    //        //return $location.path("login");
                    //    }
                    //    return $q.reject(rejection);
                    //}
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


    app.factory('Auth', function ($cookieStore, ACCESS_LEVELS) {
        var _user = $cookieStore.get('user');
        var setUser = function (user) {
            if (!user.role || user.role < 0) {
                user.role = ACCESS_LEVELS.pub;
            }
            _user = user;
            $cookieStore.put('user', _user);
        };
        return {
            isAuthorized: function (lvl) {
                return _user.role >= lvl;
            },
            setUser: setUser,
            isLoggedIn: function () {
                return _user ? true : false;
            },
            getUser: function () {
                return _user;
            },
            getId: function () {
                return _user ? _user._id : null;
            },
            getToken: function () {
                return _user ? _user.token : '';
            },
            logout: function () {
                $cookieStore.remove('user');
                _user = null;
            }
        }
    });

    app.run(function ($rootScope, $location, Auth) {
        //// 给$routeChangeStart设置监听
        //$rootScope.$on('$routeChangeStart', function (evt, next, curr) {
        //    if (!Auth.isAuthorized(next.$$route.access_level)) {
        //        if (Auth.isLoggedIn()) {
        //            // 用户登录了，但没有访问当前视图的权限
        //            $location.path('/');
        //        } else {
        //            $location.path('/login');
        //        }
        //    }
        //});
    });



    domReady(function () {
        angular.bootstrap(document, ['eOrderingApp']);

        $('html').addClass('ng-app: eOrderingApp');
    })
});