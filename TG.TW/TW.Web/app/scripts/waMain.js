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
        animate: 'vendor/angular/angular-animate',
        touch: 'vendor/angular/angular-touch',
        // domReady
        domReady: 'vendor/domReady',
        // xml to json
        x2js: 'vendor/x2js/xml2json',
        // angular-file-uploader
        uploader: 'vendor/angular-file-uploader/angular-file-upload',
        // iscroll
        iscroll: 'vendor/ace/iscroll'
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
        animate: {
            deps: ['angular']
        },
        touch: {
            deps: ['angular']
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
	'waApp',
	'domReady',
    'iscroll',
    // 自定义controllers,services,directives,filters都需要在这里添加路径
    //'controllers/mainCtrl',
    //'controllers/orderCycleCtrl',
    //'controllers/orderCycleEditCtrl',
    'controllers/waMainCtrl',
    'controllers/testWACtrl',
    'controllers/testIscrollCtrl',
],
function (angular, waApp, domReady, iscroll) {
    'use strict';
    waApp.constant('ACCESS_LEVELS', {
        pub: 1,
        user: 2
    });

    waApp.config(['$routeProvider', '$httpProvider', 'ACCESS_LEVELS',
        function ($routeProvider, $httpProvider, ACCESS_LEVELS) {
            $routeProvider
            .when('/home', {
                templateUrl: 'views/home.html',
                controller: 'waMainCtrl'
            })
            //WA页面测试
            .when('/testAuth', {
                templateUrl: 'views/Test/testAuth.html',
                controller: 'testWACtrl',
                access_level: ACCESS_LEVELS.pub
            })
            //Iscroll页面测试
            .when('/testIscroll', {
                templateUrl: 'views/Test/testIscroll.html',
                controller: 'testIscrollCtrl',
                access_level: ACCESS_LEVELS.pub
            })
            .when('/testSlider', {
                templateUrl: 'views/Test/testSlider.html',
                controller: 'testIscrollCtrl',
                access_level: ACCESS_LEVELS.pub
            });// end


            $httpProvider.defaults.withCredentials = true;
            $httpProvider.defaults.useXDomain = true;
            delete $httpProvider.defaults.headers.common['X-Requested-With'];

            $httpProvider.interceptors.push(function ($q, $location, $rootScope) {
                return {
                    'response': function (resp) {
                        //if (resp.config.url == '/Login/Login') {
                        //    // 假设API服务器返回的数据格式如下:
                        //    // { token: "AUTH_TOKEN" }
                        //    Auth.setToken(resp.data.token);
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


    domReady(function () {
        angular.bootstrap(document, ['eOrderingApp']);

        $('html').addClass('ng-app: eOrderingApp');

    })
});