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
                    //if (result == null) {
                    result = "/api";
                    //}
                    result += '/' + api;
                    if (action != null && action != '') {
                        result += '/' + action;
                    }
                    return result;
                };

                var startWaiting = function () {
                    //开启等待
                    commonService.wait();
                };

                var EndWaiting = function (result) {
                    //结束等待
                    result.success(function (result, status, headers, config) {
                        commonService.wait();
                    }).error(function () {
                        commonService.wait();
                    });
                };

                service.doPost = function (api, action, param, url) {
                    var result = {};
                    var url = getPostUrl(api, action, url);

                    startWaiting();
                    result = $http.post(url, param);
                    EndWaiting(result);

                    return result;
                }

                service.doPostToken = function (api, action, param, url) {
                    var result = {};
                    var url = getPostUrl(api, action, url);

                    startWaiting();
                    result = $http({
                        method: 'POST',
                        url: url,
                        data: param,
                        //headers: { 'Authorization': authService.getToken() }
                    });
                    EndWaiting(result);

                    return result;
                }

                return service;
            });
        });