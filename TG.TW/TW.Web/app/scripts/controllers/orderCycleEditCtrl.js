'use strict';
define(['controllers/controllers', 'services/orderService', 'services/commonService', 'services/configService']
        , function (controllers) {
            controllers.controller('orderCycleEditCtrl'
                , ['$scope', 'orderService', '$translatePartialLoader', '$translate', 'configService', 'commonService'
                    , function ($scope, orderService, $translatePartialLoader, $translate, configService, commonService, sysService) {
                        //获取服务器当前时间，注意有网络延迟
                        var pToday = null;

                        $scope.getOCNo = function () {
                            orderService.getOrderCycleNo().success(function (result) {
                                if (result.success != undefined && result.success != null && result.success == true) {
                                    $scope.cycleConfig = result.data;
                                    pToday = new Date($scope.cycleConfig.curYear, $scope.cycleConfig.curMonth - 1, $scope.cycleConfig.curDay, $scope.cycleConfig.curHour, $scope.cycleConfig.curMinute);
                                    //输出保存结果
                                    $scope.opCycles =
                                        {
                                            cycleID: null,
                                            cycleNo: $scope.cycleConfig.curCyNo,
                                            startDate: pToday,
                                            endDate: null,
                                            remark: "",
                                            status: "",
                                            modelType: ""
                                        };
                                }
                                else {
                                    //输出异常，以后接入前端框架的模式
                                    console.log(result.message);
                                }
                            }).error(function () {
                                //console.log("");
                            });
                        }

                        $scope.ocEditM = commonService.ocEditM;
                        var pEditM = ($scope.ocEditM == undefined || $scope.ocEditM == null) ? 1 : $scope.ocEditM.mode;
                        //pEditM值为1是‘新增’状态，否则为修改；‘新增’状态下需要重新获取周期编号
                        if (pEditM == 1) {
                            $scope.getOCNo();
                        }
                        else {
                            //输出保存结果
                            $scope.opCycles =
                                {
                                    cycleID: null,
                                    cycleNo: $scope.ocEditM.cycleNo,
                                    startDate: $scope.ocEditM.sDate,
                                    endDate: $scope.ocEditM.eDate,
                                    remark: "",
                                    status: "",
                                    modelType: ""
                                };
                        }

                        var verifyCycle = function () {
                            var sDate = $scope.opCycles.startDate;
                            var eDate = $scope.opCycles.endDate;
                            if (sDate == null || eDate == null)
                                return "请设置起始日期及终止日期";
                            if (sDate >= eDate)
                                return "终止日期不能早于起始日期";
                            if (eDate <= pToday)
                                return "不允许设置已过去的时间段";
                            return "";
                        }
                        //保存
                        $scope.saveNew = function () {
                            var err = verifyCycle();
                            if (err != "") {
                                //输出异常，以后接入前端框架的模式
                                console.log(err);
                                return;
                            }
                            orderService.updateOrderCycleList($scope.opCycles).success(function (result) {
                                if (result.success != undefined && result.success != null && result.success == true) {
                                    console.log(result.message);
                                }
                                else {
                                    //输出异常，以后接入前端框架的模式
                                    console.log(result.message);
                                }
                            }).error(function () {
                                //console.log("");
                            });
                        }
                    }
                ]
            )
        }
);