define(['angular'], function (angular) {
    'use strict';
    var pFilters = angular.module('filters', []);
    //�������������˼�����������Ŀ��ֵ
    pFilters.filter('filterT1', function () {
        return function (items, inx) {
            var newItems = new Array();
            var i = 0;
            angular.forEach(items, function (item) {
                if (i == inx) {
                    newItems.push(item);
                }
                i += 1;
            });
            return newItems;
        }
    });
    //�������������˼�������������Ŀ��ֵ
    pFilters.filter('filterT2', function () {
        return function (items, inx) {
            var newItems = new Array();
            var i = 0;
            angular.forEach(items, function (item) {
                if (i != inx) {
                    newItems.push(item);
                }
                i += 1;
            });
            return newItems;
        }
    });
    return pFilters;
});
