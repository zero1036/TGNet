﻿'use strict';
define(['controllers/controllers', 'services/baseUnionService', 'services/orgService']
        , function (controllers) {
            //通讯录部门列表
            controllers.controller('orgCtrl'
                , ['$scope', '$location', '$routeParams', 'commonService', 'orgService'
                    , function ($scope, $location, $routeParams, commonService, orgService) {
                        //监听下拉pullDown事件
                        $scope.$on('pullDownAction', function (e, myScroll) {
                            $scope.loadOrgs(myScroll, true);
                        });                        //监听上拉pullup事件                        $scope.$on('pullUpAction', function (e, myScroll) {
                        });                        //
                        $scope.childInit = function () {
                            $scope.loadOrgs(null, false);
                        };
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
                                    console.log('');
                                });
                        };
                        $scope.openChildDep = function (dep) {
                            var pActive = dep.activeStatus == true ? false : true;
                            dep.activeStatus = pActive;
                            if (dep.childDid != undefined && dep.childDid != null && dep.childDid.length > 0) {
                                for (var i = 0, pen; pen = $scope.orgs[i++];) {
                                    if (dep.childDid.indexOf(pen.did) >= 0) {
                                        pen.isActive = pActive;
                                    }
                                }
                            }
                        };
                        $scope.getUsers = function (dep) {
                            if (dep.usersCount > 0) {
                                $location.path('/userListWa/' + dep.did);
                            }
                        };
                        $scope.test = function () {

                        };

                    }
                ]
            );
            //通讯录用户列表
            controllers.controller('usersCtrlWa'
                , ['$scope', '$location', '$routeParams', 'commonService', 'orgService'
                    , function ($scope, $location, $routeParams, commonService, orgService) {
                        //监听下拉pullDown事件
                        $scope.$on('pullDownAction', function (e, myScroll) {
                        });                        //监听上拉pullup事件                        $scope.$on('pullUpAction', function (e, myScroll) {
                        });                        //监听返回事件                        $scope.$on('goBack', function (e, param) {
                            $location.path('/orgList');
                        });                        //
                        $scope.childInit = function () {
                            $scope.loadUsers(null, false);
                            //$scope.$emit('childInit', false);
                        };
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
                                    console.log('');
                                });
                        };
                        $scope.openUserInfo = function (user) {
                            commonService.curUser = user;
                            $location.path('/userDetailWa');
                        };
                    }
                ]
            );
            //通讯录用户明细
            controllers.controller('userDetailCtrlWa'
                , ['$scope', '$location', '$routeParams', 'commonService', 'orgService'
                    , function ($scope, $location, $routeParams, commonService, orgService) {
                        //监听下拉pullDown事件
                        $scope.$on('pullDownAction', function (e, myScroll) {
                        });                        //监听上拉pullup事件                        $scope.$on('pullUpAction', function (e, myScroll) {
                        });                        //监听返回事件                        $scope.$on('goBack', function (e, param) {
                            $location.path('/orgList');
                        });                        //
                        $scope.childInit = function () {
                            $scope.loadUser(null, false);
                        };
                        $scope.loadUser = function (myScroll, bRefresh) {
                            if (commonService.curUser != undefined && commonService.curUser != null) {
                                $scope.user = commonService.curUser;
                                commonService.curUser = null;
                                console.log($scope.user);

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
                                $location.path('/orgList');
                            }
                        };
                        $scope.showInfo = function () {
                            alert('请安装EG Skype支援应用!');
                        };
                    }
                ]
            );
            return controllers;
        }
);