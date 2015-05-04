'use strict'
define(['services/services', 'services/baseService'], function (services) {
    services.factory('orgService', function (baseService) {
        var service = {};
        var api = 'Org';
        //获取部门
        service.getOrgs = function () {
            return baseService.doPost(api, "GetDeps");
        }
        //获取用户
        service.getUsers = function (depId) {
            //var param = { id: depId };
            return baseService.doPost(api, "GetUsers?did=" + depId);
        }
        return service;
    });
});
