'use strict'
define(['services/services', 'services/baseService'], function (services) {
    services.factory('dictionaryService', function (baseService) {
        var service = {};
        var api = 'Dictionary';

        //根据ParentCode获取字典列表
        service.getListByParentCode = function (pCode) {
            var param = { 'ParentCode': pCode };
            return baseService.doPost(api, 'GetListByParentCode', param);
        };

        //获取全部字典列表
        service.getAllListByParentCode = function (pCode) {
            var param = { 'ParentCode': pCode };
            return baseService.doPost(api, 'GetAllListByParentCode', param);
        };

        //根据code获取数据字典
        service.getByCode = function (code) {
            var param = { 'DicCode': code };
            return baseService.doPost(api, 'GetByCode', param);
        };

        //更新字典
        service.update = function (item) {
            var param = { 'Model': item };
            return baseService.doPost(api, 'Update', param);
        };

        //新增字典
        service.add = function (item) {
            var param = { 'Model': item };
            return baseService.doPost(api, 'Add', param);
        };

        //删除字典
        service.delete = function (code) {
            var param = { 'DicCode': code };
            return baseService.doPost(api, 'Delete', param);
        };

        service.getNextCode = function (pCode) {
            var param = { 'ParentCode': pCode };
            return baseService.doPost(api, 'GetNextCode', param);
        };

        return service;
    });
});
