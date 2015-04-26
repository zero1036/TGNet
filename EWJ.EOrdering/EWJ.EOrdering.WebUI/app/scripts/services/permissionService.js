'use strict'
define(['services/services'
        , 'services/baseService']
        , function (services) {
            services.factory('permissionService', function (baseService) {
                var service = {};
                var api = 'Permission';

                //获取当前用户菜单
                service.getMenu = function () {
                    return baseService.doPost(api, 'GetMenu');
                };

                service.getPermission = function () {
                    return baseService.doPost(api, 'GetPermission');
                };

                return service;
            });
        });