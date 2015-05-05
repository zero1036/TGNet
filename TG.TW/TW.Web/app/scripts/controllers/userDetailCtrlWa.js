'use strict';
define(['controllers/controllers', 'services/commonService', 'services/orgService']
        , function (controllers) {
            controllers.controller('userDetailCtrlWa'
                , ['$scope', '$location', '$routeParams', 'commonService', 'orgService'
                    , function ($scope, $location, $routeParams, commonService, orgService) {
                        //监听下拉pullDown事件
                        $scope.$on('pullDownAction', function (e, myScroll) {
                        });                        //监听上拉pullup事件                        $scope.$on('pullUpAction', function (e, myScroll) {
                        });                        //监听返回事件                        $scope.$on('goBack', function (e, param) {
                            $location.path("/orgList");
                        });                        //
                        $scope.childInit = function () {
                            $scope.loadUser(null, false);
                        }
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
                                $location.path("/orgList");
                            }
                        }
                        $scope.showInfo = function () {
                            alert('请安装EG Skype支援应用!');
                        }
                    }
                ]
            )
        }
);