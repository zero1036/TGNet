'use strict'
define(['services/services', 'services/baseService'], function (services) {
    services.factory('userService', function (baseService) {
        var service = {};
        var api = 'User';

        //获取用户列表
        service.getUserList = function () {
            return baseService.doPost(api, "GetList");
        }

        //获取当前用户
        service.getCurrentUser = function () {
            return baseService.doPost(api, "GetCurrentUser");
        }

        return service;
    });
});
