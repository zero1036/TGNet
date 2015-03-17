'use strict';
//Main configuration file. Sets up AngularJS module and routes and any other config objects

var appRoot = angular.module('WXApp', ['ngRoute', 'WXService', 'WXDirective', 'WXBaseController', 'WXOrgController', 'WXResController']);     //Define the main module

appRoot.config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {
    //Setup routes to load partial templates from server. TemplateUrl is the location for the server view (Razor .cshtml view)
    //$locationProvider.html5Mode(true);
    //$locationProvider.hashPrefix('!');
    $routeProvider
        .when('/HomeX', { templateUrl: '/Home/Introduce', controller: 'BaseCtrl' })
        .when('/Home/Logout', { templateUrl: '/Home/Logout', controller: 'BaseCtrl' })
        .when('/User/List', { templateUrl: '/User/List', controller: 'BaseCtrl' })
        .when('/Group/List', { templateUrl: '/Group/List', controller: 'BaseCtrl' })
        .when('/ActiveUser/List', { templateUrl: '/ActiveUser/List', controller: 'BaseCtrl' })
        .when('/TemplateMessage/Message', { templateUrl: '/Scripts/Views/Page/Message.html', controller: 'OrgCtrl' })
        .when('/TemplateMessage/GroupSend', { templateUrl: '/Scripts/Views/Page/GroupSend.html', controller: 'GroupSendCtrl' })
        .when('/TemplateMessage/GroupSendReview', { templateUrl: '/Scripts/Views/Page/GroupSendReview.html', controller: 'GroupSendReviewCtrl' })
        .when('/responseChainConfig/treeIndex', { templateUrl: '/responseChainConfig/treeIndex', controller: 'BaseCtrl' })
        .when('/responseChainConfig', { templateUrl: '/responseChainConfig', controller: 'BaseCtrl' })
        .when('/WXMenu/Index', { templateUrl: '/WXMenu/Index', controller: 'BaseCtrl' })
        .when('/WXResource/WXPictureConfig', { templateUrl: '/Scripts/Views/Page/WXPictureConfig.html', controller: 'PictureListCtrl' })
        .when('/WXResource/WXArticlesConfig', { templateUrl: '/Scripts/Views/Page/WXArticlesConfig.html', controller: 'ArticleListCtrl' })        
        .when('/WXResource/WXVideosConfig', { templateUrl: '/Scripts/Views/Page/WXVideosConfig.html', controller: 'VideoListCtrl' })
        .when('/WXResource/WXVoicesConfig', { templateUrl: '/Scripts/Views/Page/WXVoicesConfig.html', controller: 'VoiceListCtrl' })
        .when('/TemplateMessage/MessageConfig', { templateUrl: '/TemplateMessage/MessageConfig', controller: 'BaseCtrl' })
        .when('/SimulateTool/Index', { templateUrl: '/SimulateTool/Index', controller: 'BaseCtrl' })
        .when('/WebConfig/Index', { templateUrl: '/WebConfig/Index', controller: 'BaseCtrl' })
        .when('/WXResource/WXArticleConfig/:IsCreated/', { templateUrl: '/Scripts/Views/Page/WXArticleEdit.html', controller: 'ArticleEditCtrl' })
        .otherwise({ redirectTo: '/' });
}]);