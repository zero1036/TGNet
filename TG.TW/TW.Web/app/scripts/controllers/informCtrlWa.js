'use strict';
define(['controllers/controllers', 'services/baseUnionService', 'services/matterService'], function (controllers) {
    controllers.controller('informCtrlWa', ['$scope', '$location', '$routeParams', 'commonService', 'matterService', function ($scope, $location, $routeParams, commonService, matterService) {
        //监听下拉pullDown事件
        $scope.$on('pullDownAction', function (e, myScroll) {
            $scope.loadInformsPub(myScroll, true);
        });        //监听上拉pullup事件        $scope.$on('pullUpAction', function (e, myScroll) {
        });        //监听返回事件        $scope.$on('goBack', function (e, param) {
        });        //
        $scope.childInit = function () {
            //$scope.$emit('childInit', false);
            $scope.loadInformsPub(null, false);
        };
        $scope.selectNewsModel = function (newsModel) {
            commonService.curNewsModel = newsModel;
            commonService.returnPath = "/informsPubWa";
            $location.path("/newsModelWa");
        };
        $scope.loadInformsPub = function (myScroll, bRefresh) {
            matterService.getInformsPub()
               .success(function (result, status, headers, config) {
                   if (result.ok != undefined && result.ok != null && result.ok == true) {
                       $scope.articles = result.data;
                       console.log($scope.articles);

                       if (bRefresh) {
                           if (myScroll != undefined && myScroll != null) {
                               myScroll.refresh();
                           }
                       }
                       else {
                           $scope.$emit('childInit', false);
                       }
                   }
                   else {
                       //输出异常，以后接入前端框架的模式
                       console.log(result.message);
                   }
               }).error(function () {
                   console.log("");
               });
        };
    }]);

    controllers.controller('informPvtCtrlWa', ['$scope', '$location', '$routeParams', 'commonService', 'matterService', function ($scope, $location, $routeParams, commonService, matterService) {
        //监听下拉pullDown事件
        $scope.$on('pullDownAction', function (e, myScroll) {
            $scope.loadInformsPub(myScroll, true);
        });        //监听上拉pullup事件        $scope.$on('pullUpAction', function (e, myScroll) {
        });        //监听返回事件        $scope.$on('goBack', function (e, param) {
        });        //
        $scope.childInit = function () {
            //$scope.$emit('childInit', false);
            $scope.loadInformsPub(null, false);
        };
        $scope.selectNewsModel = function (newsModel) {
            commonService.curNewsModel = newsModel;
            commonService.returnPath = "/informsPubWa";
            $location.path("/newsModelWa");
        };
        $scope.loadInformsPub = function (myScroll, bRefresh) {
            matterService.getInformsPub()
               .success(function (result, status, headers, config) {
                   if (result.ok != undefined && result.ok != null && result.ok == true) {
                       $scope.articles = result.data;
                       console.log($scope.articles);

                       if (bRefresh) {
                           if (myScroll != undefined && myScroll != null) {
                               myScroll.refresh();
                           }
                       }
                       else {
                           $scope.$emit('childInit', false);
                       }
                   }
                   else {
                       //输出异常，以后接入前端框架的模式
                       console.log(result.message);
                   }
               }).error(function () {
                   console.log("");
               });
        };
    }]);

    return controllers;
}
);





