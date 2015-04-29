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
        }
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
                return _user.role >= lvl;
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
});