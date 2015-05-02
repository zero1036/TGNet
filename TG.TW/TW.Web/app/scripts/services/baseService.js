'use strict'
define(['services/services', 'services/commonService', 'services/authService']
        , function (services) {
            services.factory('baseService', function ($http, commonService, authService) {
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

                service.doPostToken = function (api, action, param, url) {
                    var result = {};
                    var purl = getPostUrl(api, action, url);

                    result = $http({
                        method: 'POST',
                        url: purl,
                        data: param,
                        //headers: { 'Authorization': authService.getToken() }
                    })
                    return result;
                }

                return service;
            });
        });