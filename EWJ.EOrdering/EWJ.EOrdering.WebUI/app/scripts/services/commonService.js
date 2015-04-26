'use strict'
define(['services/services'], function (services) {
    services.factory('commonService', function ($window, $cookieStore, $timeout) {
        var service = {};

        /*--------------------- Session Key ----------------------------*/

        //api base url
        service.SESSION_KEY_API_DEFAULT_URL = 'API_DEFAULT_URL';
        //當前用戶
        service.SESSION_KEY_CURRENT_USER = 'CURRENT_USER';
        //用戶菜單
        service.SESSION_KEY_CURRENT_USER_MENU = 'CURRENT_USER_MENU';
        //用戶權限
        service.SESSION_KEY_CURRENT_USER_PERMISSION = 'CURRENT_USER_PERMISSION';

        /*--------------------- Session Key ----------------------------*/

        service.getAPIDefaultUrl = function () {
            var val = null;
            if (service.getSession(service.SESSION_KEY_API_DEFAULT_URL) != null && service.getSession(service.SESSION_KEY_API_DEFAULT_URL) != undefined) {
                val = service.getSession(service.SESSION_KEY_API_DEFAULT_URL);
            }
            return val;
        };

        //当前用户菜单
        service.getCurrentUserMenu = function () {
            var val = null;
            if (service.getSession(service.SESSION_KEY_CURRENT_USER_MENU) != null && service.getSession(service.SESSION_KEY_CURRENT_USER_MENU) != undefined) {
                val = service.getSession(service.SESSION_KEY_CURRENT_USER_MENU);
            }
            return val;
        };

        //清除session
        service.clearSessionData = function () {
            //service.delSession(service.SESSION_KEY_API_BASE_URL);
            service.delSession(service.SESSION_KEY_CURRENT_USER);
            service.delSession(service.SESSION_KEY_CURRENT_USER_MENU);
            service.delSession(service.SESSION_KEY_CURRENT_USER_PERMISSION);
        };


        /*--------------------- Session ----------------------------*/

        //Session
        service.setSession = function (key, val) {
            $cookieStore.put(key, val);
        };

        service.getSession = function (key) {
            return $cookieStore.get(key);
        };

        service.delSession = function (key) {
            $cookieStore.remove(key);
        };

        /*--------------------- Session ----------------------------*/

        return service;
    });
});