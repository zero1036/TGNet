'use strict'
define(['services/services', 'services/baseService'], function (services) {
    services.factory('loginService', function (baseService) {
        var service = {};
        var api = 'Login';

        //登入
        service.signin = function (account, pwd) {
            var param = { Account: account, Password: pwd };
            return baseService.doPost(api, "Login", param);
        }

        //登出
        service.signout = function () {
            return baseService.doPost(api, "Logout");
        }

        return service;
    });
});
