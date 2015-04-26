'use strict';

define(['angular', 'cookies', 'route', 'angularTranslateLoaderPartial', 'bootstrap', 'x2js', 'controllers/controllers', 'services/services', 'filters/filters', 'directives/directives', 'uploader'], function (angular) {
    return angular.module('eOrderingApp', ['controllers', 'services', 'filters', 'directives', 'ngRoute', 'ngCookies', 'pascalprecht.translate', 'angularFileUpload']);
});
