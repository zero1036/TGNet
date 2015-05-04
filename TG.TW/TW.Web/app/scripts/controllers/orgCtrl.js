'use strict';
define(['controllers/controllers', 'services/commonService', 'services/orgService']
        , function (controllers) {
            controllers.controller('orgCtrl'
                , ['$scope', 'commonService', 'orgService'
                    , function ($scope, commonService, orgService) {
                        //监听下拉pullDown事件
                        $scope.$on('pullDownAction', function (e, myScroll) {
                            $scope.loadOrgs(myScroll, true);
                        });                        //监听上拉pullup事件                        $scope.$on('pullUpAction', function (e, myScroll) {

                        });                        //
                        $scope.childInit = function () {
                            $scope.loadOrgs(null, false);
                        }
                        $scope.orgs = [];
                        $scope.loadOrgs = function (myScroll, bRefresh) {
                            orgService.getOrgs()
                                .success(function (result, status, headers, config) {
                                    if (result.ok != undefined && result.ok != null && result.ok == true) {
                                        $scope.orgs = result.data;
                                        console.log($scope.orgs);

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
                        }
                        $scope.openChildDep = function (dep) {
                            var pActive = dep.activeStatus ? false : true;
                            dep.activeStatus = pActive;
                            if (dep.childDid != undefined && dep.childDid != null && dep.childDid.length > 0) {
                                for (var i = 0, pen; pen = $scope.orgs[i++];) {
                                    if (dep.childDid.indexOf(pen.did) >= 0) {
                                        pen.isActive = pActive;
                                    }
                                }
                            }
                        }
                        $scope.getUsers = function (dep) {

                        }
                        $scope.test = function () {

                        }
                    }
                ]
            )
        }
);