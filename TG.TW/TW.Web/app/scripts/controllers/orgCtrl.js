'use strict';
define(['controllers/controllers', 'services/commonService', 'services/orgService']
        , function (controllers) {
            controllers.controller('orgCtrl'
                , ['$scope', 'commonService', 'orgService'
                    , function ($scope, commonService, orgService) {
                        //监听下拉pullDown事件
                        $scope.$on('pullDownAction', function (e, myScroll) {

                        });                        //监听上拉pullup事件                        $scope.$on('pullUpAction', function (e, myScroll) {

                        });                        //
                        $scope.childInit = function () {
                            $scope.loadOrgs();
                        }
                        $scope.orgs = [];
                        $scope.loadOrgs = function () {
                            orgService.getOrgs()
                                .success(function (result, status, headers, config) {
                                    console.log(result);
                                    $scope.orgs = result.data;
                                    console.log($scope.orgs);
                                    if (result.ok != undefined && result.ok != null && result.ok == true) {
                                        $scope.orgs = result.data;
                                        $scope.$emit('childInit', false);
                                    }
                                    else {
                                        //输出异常，以后接入前端框架的模式
                                        console.log(result.message);
                                    }
                                }).error(function () {
                                    console.log("");
                                });
                        }

                    }
                ]
            )
        }
);