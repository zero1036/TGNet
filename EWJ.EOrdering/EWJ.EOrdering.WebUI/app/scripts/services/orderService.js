'use strict'
define(['services/services', 'services/baseService'], function (services) {
    services.factory('orderService', function (baseService) {
        var service = {};
        var api = 'Order';

        //获取订购周期集合
        service.getOrderCycleList = function () {
            return baseService.doPost(api, "GetCycles");
        }

        //获取订购周期最新编号
        service.getOrderCycleNo = function () {
            return baseService.doPost(api, "GetCycleNo");
        }

        //更新订购周期集合
        service.updateOrderCycleList = function (vals) {
            return baseService.doPost(api, "UpdateCycles", vals);
        }

        return service;
    });
});
