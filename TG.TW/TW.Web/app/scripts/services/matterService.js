'use strict'
define(['services/services', 'services/baseUnionService'], function (services) {
    services.factory('matterService', function (baseService) {
        var service = {};
        var api = 'Inform';
        //获取公司通告
        service.getInformsPub = function () {
            return baseService.doPost(api, "GetInformPub");
        }
        //获取内部通告
        service.getInformsPvt = function () {
            return baseService.doPost(api, "GetInformPvt");
        }
        return service;
    });
});
