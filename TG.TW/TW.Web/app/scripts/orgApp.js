'use strict';

define(['angular', 'cookies', 'route', 'touch', 'controllers/controllers', 'services/services', 'filters/filters', 'directives/directives'], function (angular) {
    return angular.module('eOrderingApp', ['controllers', 'services', 'filters', 'directives', 'ngRoute', 'ngCookies', 'ngTouch']);
});