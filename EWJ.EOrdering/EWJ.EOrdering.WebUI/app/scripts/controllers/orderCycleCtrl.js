﻿'use strict';
define(['controllers/controllers', 'services/orderService', 'services/commonService', 'services/configService']
        , function (controllers) {
            controllers.controller('orderCycleCtrl'
                , ['$scope', '$location', 'orderService', '$translatePartialLoader', '$translate', 'configService', 'commonService'
                    , function ($scope, $location, orderService, $translatePartialLoader, $translate, configService, commonService) {
                        $scope.orderCycles = [];
                        $scope.getOrdreCycles = function () {
                            orderService.getOrderCycleList().success(function (result) {
                                if (result.success != undefined && result.success != null && result.success == true) {
                                    $scope.orderCycles = result.data;
                                }
                                else {
                                    //输出异常，以后接入前端框架的模式
                                    console.log(result.message);
                                }
                            }).error(function () {
                                console.log("");
                            });
                        }
                        $scope.getOrdreCycles();
                        $scope.addCycle = function () {
                            commonService.ocEditM = {
                                mode: 1,//1 新增；2 修改
                            };
                            $location.path("/orderCycleEdit");
                        }
                        $scope.modifyCycle = function (oc) {
                            commonService.ocEditM = {
                                mode: 2,//1 新增；2 修改
                                cycleNo: oc.cycleNo,
                                sDate: null,
                                eDate: null
                            };
                            $location.path("/orderCycleEdit");
                        }
                    }
                ]
            )
        }
);
