require.config({
    paths: {
        /*
		 * 引入类库
		 */
        // jquery
        jquery: 'vendor/jquery',
        // Angular
        angular: 'vendor/angular/angular',
        // domReady
        domReady: 'vendor/domReady',
        cookies: 'vendor/angular/angular-cookies',
        // xml to json
        x2js: 'vendor/x2js/xml2json'
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
        x2js: {
            deps: ['jquery']
        }
    }
});
require([
	'angular',
	'loginApp',
	'domReady',
	// 自定义controllers,services,directives,filters都需要在这里添加路径
    'controllers/loginCtrl'
], function (angular, loginApp, domReady) {
    'use strict';
    loginApp.constant('ACCESS_LEVELS', {
        pub: 1,
        user: 2
    });


    loginApp.config(function ($httpProvider) {
        $httpProvider.defaults.withCredentials = true;
        $httpProvider.defaults.useXDomain = true;
        delete $httpProvider.defaults.headers.common['X-Requested-With'];

        $httpProvider.interceptors.push(function ($q, $location, $rootScope, Auth) {
            return {
                'response': function (resp) {
                    //if (resp.config.url == '/Login/Login') {
                        // 假设API服务器返回的数据格式如下:
                        // { token: "AUTH_TOKEN" }
                    //Auth.setToken(resp.data.token);
                    //}
                    return resp;
                }
            };
        });
    });

    loginApp.factory('Auth', function ($cookieStore, ACCESS_LEVELS) {
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


    //loginApp.config(function ($httpProvider) {
    //    // 在这里构造拦截器
    //    var interceptor = function ($q, $rootScope, Auth) {
    //        return {
    //            'response': function (resp) {
    //                if (resp.config.url == '/Login/Login') {
    //                    // 假设API服务器返回的数据格式如下:
    //                    // { token: "AUTH_TOKEN" }
    //                    Auth.setToken(resp.data.token);
    //                }
    //                return resp;
    //            },
    //            'responseError': function (rejection) {
    //                // 错误处理
    //                switch (rejection.status) {
    //                    case 401:
    //                        if (rejection.config.url !== 'api/login')
    //                            // 如果当前不是在登录页面
    //                            $rootScope.$broadcast('auth:loginRequired');
    //                        break;
    //                    case 403:
    //                        $rootScope.$broadcast('auth:forbidden');
    //                        break;
    //                    case 404:
    //                        $rootScope.$broadcast('page:notFound');
    //                        break;
    //                    case 500:
    //                        $rootScope.$broadcast('server:error');
    //                        break;
    //                }
    //                return $q.reject(rejection);
    //            }
    //        };
    //    };
    //    // 将拦截器和$http的request/response链整合在一起
    //    $httpProvider.interceptors.push(interceptor);

    //});

    loginApp.run(function ($rootScope, $location, Auth) {
        // 给$routeChangeStart设置监听
        $rootScope.$on('$routeChangeStart', function (evt, next, curr) {
            if (!Auth.isAuthorized(next.$$route.access_level)) {
                if (Auth.isLoggedIn()) {
                    // 用户登录了，但没有访问当前视图的权限
                    $location.path('/');
                } else {
                    $location.path('/login');
                }
            }
        });
    });

    domReady(function () {
        angular.bootstrap(document, ['eOrderingApp']);

        $('html').addClass('ng-app: eOrderingApp');
    });
})