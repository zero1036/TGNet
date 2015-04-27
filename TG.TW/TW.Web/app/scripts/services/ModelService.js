'use strict'
define(['services/services', 'services/baseService'], function (services) {
    services.factory('modelService', function (baseService) {
        var service = {};
        var api = 'SCModel';

        //获取所有發鑲珠寶列表
        service.getSCModelList = function (val) {
            return baseService.doPost(api, "GetModels",val);
        }

      

        return service;
    });
});