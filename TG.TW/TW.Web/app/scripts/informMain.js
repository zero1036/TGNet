require.config({
    paths: {
        /*
         * 引入类库
         */
        // jQuery
        jquery: 'http://cdn.bootcss.com/jquery/1.9.0/jquery.min',
        //// bootstrap
        //bootstrap: 'vendor/bootstrap/js/bootstrap',
        // Angular
        angular: 'http://cdn.bootcss.com/angular.js/1.3.15/angular.min',
        cookies: 'vendor/angular/angular-cookies.min',
        route: 'vendor/angular/angular-route.min',
        touch: 'vendor/angular/angular-touch.min',
        // domReady
        domReady: 'vendor/domReady',
        //// xml to json
        //x2js: 'vendor/x2js/xml2json',
        //// angular-file-uploader
        //uploader: 'vendor/angular-file-uploader/angular-file-upload',
        // iscroll
        iscroll: 'vendor/ace/iscroll.min'
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
        touch: {
            deps: ['angular']
        },
        //bootstrap: {
        //    deps: ['jquery']
        //},
        //x2js: {
        //    deps: ['jquery']
        //},
        //uploader: {
        //    deps: ['angular']
        //}
    }
});
require([
	'angular',
	'orgApp',
	'domReady',
    'iscroll',
    // 自定义controllers,services,directives,filters都需要在这里添加路径
    //父级控制
    'controllers/waMainCtrl',
    //素材
    'controllers/newsModelCtrlWa',
    //通告
    'controllers/informCtrlWa',
],
function (angular, orgApp, domReady, iscroll) {
    'use strict';
    orgApp.constant('ACCESS_LEVELS', {
        pub: 1,
        user: 2
    });

    orgApp.config(['$routeProvider', '$httpProvider', 'ACCESS_LEVELS',
        function ($routeProvider, $httpProvider, ACCESS_LEVELS) {
            $routeProvider
            .when('/home', {
                templateUrl: 'views/home.html',
                controller: 'waMainCtrl'
            })
            //通告--公司动态
            .when('/informsPubWa', {
                templateUrl: 'views/inform/informsPubWa.html',
                controller: 'informCtrlWa',
                access_level: ACCESS_LEVELS.pub
            })
            //通告--内部通告
            .when('/informsPvtWa', {
                templateUrl: 'views/inform/informsPvtWa.html',
                controller: 'informPvtCtrlWa',
                access_level: ACCESS_LEVELS.pub
            })
            //通告--通告管理
            .when('/informsManageWa', {
                templateUrl: 'views/inform/informsManageWa.html',
                controller: 'informManageCtrlWa',
                access_level: ACCESS_LEVELS.pub
            })
            //素材--段落详细内容
            .when('/newsModelWa', {
                templateUrl: 'views/matter/newsModelWa.html',
                controller: 'newsModelCtrlWa',
                access_level: ACCESS_LEVELS.pub
            });// end


            $httpProvider.defaults.withCredentials = true;
            $httpProvider.defaults.useXDomain = true;
            delete $httpProvider.defaults.headers.common['X-Requested-With'];

            $httpProvider.interceptors.push(function ($q, $location, $rootScope) {
                return {
                    'response': function (resp) {
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

    domReady(function () {
        angular.bootstrap(document, ['eOrderingApp']);

        $('html').addClass('ng-app: eOrderingApp');

    })
});

