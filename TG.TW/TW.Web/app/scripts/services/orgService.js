'use strict'
define(['services/services', 'services/baseService'], function (services) {
    services.factory('orgService', function (baseService) {
        var service = {};
        var api = 'Org';
        //
        service.getOrgs = function () {
            return baseService.doPost(api, "GetUsersGroup");
        }
        return service;
    });
});
