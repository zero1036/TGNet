'use strict'
define(['services/services'
        , 'services/commonService']
        , function (services) {
            services.factory('baseService', function ($http, commonService) {
                var service = {};

                function getPostUrl(api, action, url) {
                    var result = '';
                    if (url == null || url == '') {
                        result = commonService.getAPIDefaultUrl();
                    } else {
                        result = url;
                    }
                    result += '/' + api;
                    if (action != null && action != '') {
                        result += '/' + action;
                    }
                    return result;
                };

                service.doPost = function (api, action, param, url) {
                    var result = {};
                    result = $http.post(getPostUrl(api, action, url), param);
                    return result;
                }

                return service;
            });
        });