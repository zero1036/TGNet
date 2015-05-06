'use strict'
define(['services/services', 'services/baseService'], function (services) {
    services.factory('matterService', function (baseService) {
        var service = {};
        var api = 'Inform';
        //获取公司通告
        service.getInformsPub = function () {
            return baseService.doPost(api, "GetInformPub");
        }
        return service;
    });
});
