'use strict';
define(['controllers/controllers', 'services/commonService', 'services/orgService']
        , function (controllers) {
            controllers.controller('usersCtrlWa'
                , ['$scope', '$location', '$routeParams', 'commonService', 'orgService'
                    , function ($scope, $location, $routeParams, commonService, orgService) {
                        //监听下拉pullDown事件
                        $scope.$on('pullDownAction', function (e, myScroll) {
                        });                        //监听上拉pullup事件                        $scope.$on('pullUpAction', function (e, myScroll) {
                        });                        //监听返回事件                        $scope.$on('goBack', function (e, param) {
                            $location.path("/orgList");
                        });                        //
                        $scope.childInit = function () {
                            $scope.loadUsers(null, false);
                            //$scope.$emit('childInit', false);
                        }
                        $scope.users = [];
                        $scope.loadUsers = function (myScroll, bRefresh) {
                            orgService.getUsers($routeParams.did)
                                .success(function (result, status, headers, config) {
                                    if (result.ok != undefined && result.ok != null && result.ok == true) {
                                        $scope.users = result.data;
                                        console.log($scope.users);

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
                        //$scope.openChildDep = function (dep) {
                        //    var pActive = dep.activeStatus ? false : true;
                        //    dep.activeStatus = pActive;
                        //    if (dep.childDid != undefined && dep.childDid != null && dep.childDid.length > 0) {
                        //        for (var i = 0, pen; pen = $scope.orgs[i++];) {
                        //            if (dep.childDid.indexOf(pen.did) >= 0) {
                        //                pen.isActive = pActive;
                        //            }
                        //        }
                        //    }
                        //}
                        //$scope.getUsers = function (dep) {

                        //}
                        //$scope.test = function () {

                        //}
                    }
                ]
            )
        }
);