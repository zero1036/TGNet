'use strict';

define(['angular', 'cookies', 'route', 'animate', 'touch', 'x2js', 'controllers/controllers', 'services/services', 'filters/filters', 'directives/directives'], function (angular) {
    return angular.module('eOrderingApp', ['controllers', 'services', 'filters', 'directives', 'ngRoute', 'ngCookies', 'ngAnimate', 'ngTouch']);
});