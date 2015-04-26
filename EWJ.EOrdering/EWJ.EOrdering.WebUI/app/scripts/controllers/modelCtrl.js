'use strict';
define(['controllers/controllers', 'services/modelService', 'services/commonService', 'services/configService']
        , function (controllers) {
            controllers.controller('modelCtrl'
                , ['$scope', 'modelService', '$translatePartialLoader', '$translate', 'configService', 'commonService'
                    , function ($scope, modelService, $translatePartialLoader, $translate, configService, commonService) {
                        //commonService.setSession("tg", "1");
                        //$scope.ddd = commonService.getSession("tg");

                        $scope.ModelSearchVM =
                            {
                                EWJNo:null,
                                ModelTypeCode:null,
                                CaiZhiCode:null,
                                ColorCode:null,
                                KelaWeightCode:null,
                                Localization:null,
                                OrderType:null,
                                BeginNo:0,
                                PageNo:100
                            };
                        $scope.getlist = function () {
                            
                            modelService.getSCModelList($scope.ModelSearchVM).success(function (result) {
                              //  alert(JSON.stringify(result.data));
                                
                            //    alert(JSON.stringify(result.data.scModels));
                                $scope.list = result.data;

                            });
                        }
                       
                        $scope.getlist();
                    }
                ]
            )
        }
);