'use strict'
define(['services/services'], function (services) {
    services.factory('authService', function ($window, $cookieStore, $timeout, ACCESS_LEVELS) {
        var _user = $cookieStore.get('user');
        //var _menu = $cookieStore.get('menu');
        var getTime = function () {
            var d = new Date();
            var pMon = (d.getMonth() + 1) * 1000000;
            var pDate = (d.getDate()) * 10000;
            var pHour = (d.getHours()) * 100;
            var mins = d.getMinutes();
            return pMon + pDate + pHour + mins;
        };
        var setUser = function (user) {
            if (!user.role || user.role < 0) {
                user.role = ACCESS_LEVELS.pub;
            }
            _user = user;
            _user.time = getTime();
            $cookieStore.put('user', _user);
            //_menu = menu;
            //$cookieStore.put('menu', _menu);
        };
        return {
            isAuthorized: function (lvl) {
                return _user.role > lvl;
            },
            isMenuAuthorized: function () {
                return true;
            },
            setUser: setUser,
            isLoggedIn: function () {
                return _user ? true : false;
            },
            //验证登录是否超时120分钟
            getUserIsUsed: function () {
                if (_user == undefined || _user == null) {
                    return false;
                }
                var ptimeNow = getTime();
                var ptimeUser = _user.time;
                if (ptimeUser + 120 < ptimeNow) {
                    _user = null;
                    return false;
                }
                return true;
            },
            getUser: function () {
                return _user;
            },
            getId: function () {
                return _user ? _user.userId : null;
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

    services.factory('commonService', function ($window, $cookieStore, $timeout) {
        var service = {};

        /*--------------------- Session Key ----------------------------*/

        //api base url
        service.SESSION_KEY_API_DEFAULT_URL = 'API_DEFAULT_URL';
        //當前用戶
        service.SESSION_KEY_CURRENT_USER = 'CURRENT_USER';
        //用戶菜單
        service.SESSION_KEY_CURRENT_USER_MENU = 'CURRENT_USER_MENU';
        //用戶權限
        service.SESSION_KEY_CURRENT_USER_PERMISSION = 'CURRENT_USER_PERMISSION';

        /*--------------------- Session Key ----------------------------*/

        service.getAPIDefaultUrl = function () {
            var val = null;
            if (service.getSession(service.SESSION_KEY_API_DEFAULT_URL) != null && service.getSession(service.SESSION_KEY_API_DEFAULT_URL) != undefined) {
                val = service.getSession(service.SESSION_KEY_API_DEFAULT_URL);
            }
            return val;
        };

        //当前用户菜单
        service.getCurrentUserMenu = function () {
            var val = null;
            if (service.getSession(service.SESSION_KEY_CURRENT_USER_MENU) != null && service.getSession(service.SESSION_KEY_CURRENT_USER_MENU) != undefined) {
                val = service.getSession(service.SESSION_KEY_CURRENT_USER_MENU);
            }
            return val;
        };

        //清除session
        service.clearSessionData = function () {
            //service.delSession(service.SESSION_KEY_API_BASE_URL);
            service.delSession(service.SESSION_KEY_CURRENT_USER);
            service.delSession(service.SESSION_KEY_CURRENT_USER_MENU);
            service.delSession(service.SESSION_KEY_CURRENT_USER_PERMISSION);
        };

        service.wait = function () {
            angular.element("#wi-div-waiting").toggle();
        };
        /*--------------------- Session ----------------------------*/

        //Session
        service.setSession = function (key, val) {
            $cookieStore.put(key, val);
        };

        service.getSession = function (key) {
            return $cookieStore.get(key);
        };

        service.delSession = function (key) {
            $cookieStore.remove(key);
        };

        /*--------------------- Session ----------------------------*/

        return service;
    });

    services.factory('baseService', function ($http, commonService, authService) {
        var service = {};

        function getPostUrl(api, action, url) {
            var result = '';
            if (url == null || url == '') {
                result = commonService.getAPIDefaultUrl();
            } else {
                result = url;
            }
            //if (result == null) {
            result = "/api";
            //}
            result += '/' + api;
            if (action != null && action != '') {
                result += '/' + action;
            }
            return result;
        };

        var startWaiting = function () {
            //开启等待
            commonService.wait();
        };

        var EndWaiting = function (result) {
            //结束等待
            result.success(function (result, status, headers, config) {
                commonService.wait();
            }).error(function () {
                commonService.wait();
            });
        };

        service.doPost = function (api, action, param, url) {
            var result = {};
            var url = getPostUrl(api, action, url);

            startWaiting();
            result = $http.post(url, param);
            EndWaiting(result);

            return result;
        };

        service.doPostToken = function (api, action, param, url) {
            var result = {};
            var url = getPostUrl(api, action, url);

            startWaiting();
            result = $http({
                method: 'POST',
                url: url,
                data: param,
                //headers: { 'Authorization': authService.getToken() }
            });
            EndWaiting(result);

            return result;
        };

        return service;
    });

    return services;
});