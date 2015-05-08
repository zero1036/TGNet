/// <reference path="expCtrlWa.js" />
'use strict';
define(['controllers/controllers', 'services/baseUnionService', 'services/matterService'], function (controllers) {
    controllers.controller('newsModelCtrlWa', ['$scope', '$location', '$routeParams', 'commonService', 'matterService', function ($scope, $location, $routeParams, commonService, matterService) {
        //监听下拉pullDown事件
        $scope.$on('pullDownAction', function (e, myScroll) {
            myScroll.refresh();
        });        //监听上拉pullup事件        $scope.$on('pullUpAction', function (e, myScroll) {
        });        //监听返回事件        $scope.$on('goBack', function (e, param) {
            if (commonService.returnPath != undefined && commonService.returnPath != null && commonService.returnPath != "")
                $location.path(commonService.returnPath);
        });        //
        $scope.childInit = function () {
            //$scope.$emit('childInit', false);
            $scope.loadCurNewsModel(null, false);
        };
        $scope.loadCurNewsModel = function (myScroll, bRefresh) {
            if (commonService.curNewsModel != undefined && commonService.curNewsModel != null) {
                $scope.curNewsModel = commonService.curNewsModel;
                angular.element("#nmw-div-content").html($scope.curNewsModel.content);
                commonService.curNewsModel = null;

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
                if (commonService.returnPath != undefined && commonService.returnPath != null && commonService.returnPath != "")
                    $location.path(commonService.returnPath);
            }

        };

    }
    ]
    )
}
);