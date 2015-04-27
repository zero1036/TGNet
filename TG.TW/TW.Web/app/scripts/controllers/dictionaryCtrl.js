'use strict';
define(['controllers/controllers'
        , 'services/dictionaryService'
        , 'services/commonService']
        , function (controllers) {
            controllers.controller('dictionaryCtrl'
                , ['$scope', '$translatePartialLoader', '$translate',
                    'dictionaryService', 'commonService'
                    , function ($scope, $translatePartialLoader, $translate, dictionaryService, commonService) {
                        $translatePartialLoader.addPart('dictionaryList');
                        $translate.refresh();

                        $scope.isAdding = false;
                        $scope.isUpdating = false;
                        $scope.currentDic = {};
                        $scope.nextDicCode = '';

                        //查
                        $scope.getDicByParentCode = function (code) {
                            dictionaryService.getAllListByParentCode(code)
                                .success(function (result) {
                                    var newItem = {
                                        dicCode: null
                                        , dicName: null
                                        , parentCode: $scope.currentDic.dicCode
                                        , remark: null
                                        , index: null
                                        , isDisable: null
                                        , extend1: null
                                        , extend2: null
                                        , extend3: null
                                        , extend4: null
                                    };
                                    result.push(newItem);
                                    $scope.dicList = result;
                                })
                                .error(function () {
                                });
                            dictionaryService.getNextCode($scope.currentDic.dicCode)
                                .success(function (result) {
                                    $scope.nextDicCode = result;
                                })
                                .error(function () {
                                });
                        }

                        $scope.setCurrentDic = function (dicModel) {
                            $scope.currentDic = dicModel;
                            $scope.getDicByParentCode(dicModel.dicCode);
                        }

                        $scope.getProvDicList = function () {
                            dictionaryService.getByCode($scope.currentDic.parentCode)
                                .success(function (result) {
                                    result = result == null ? {} : result;
                                    $scope.setCurrentDic(result);
                                })
                        }

                        $scope.$on('delDic', function (even, data) {
                            var code = data.dicCode;
                            dictionaryService.delete(code)
                                .success(function (result) {
                                    $scope.getDicByParentCode($scope.currentDic.dicCode);
                                })
                                .error(function () {
                                    $scope.getDicByParentCode($scope.currentDic.dicCode);
                                });
                        });

                        //保存
                        $scope.sumit = function (dicModel, isAdding, isUpdate) {
                            if (isAdding) {
                                dictionaryService.add(dicModel)
                                    .success(function (result) {
                                        $scope.getDicByParentCode($scope.currentDic.dicCode);
                                    })
                                    .error(function () {
                                        $scope.getDicByParentCode($scope.currentDic.dicCode);
                                    });

                            }
                            else if (isUpdate) {
                                dictionaryService.update(dicModel)
                                    .success(function (result) {
                                        $scope.getDicByParentCode($scope.currentDic.dicCode);
                                    })
                                    .error(function () {
                                        $scope.getDicByParentCode($scope.currentDic.DicCode);
                                    });
                            }
                        }

                        //取消
                        $scope.cancelDic = function (isAdding, isUpdating) {
                            if (isAdding) {
                                $scope.getDicByParentCode($scope.currentDic.dicCode);
                                $scope.isAdding = false;
                            } else if (isUpdating) {
                                isUpdating = false;
                            }
                        }

                        $scope.initPage = function () {
                            $scope.getDicByParentCode($scope.currentDic.dicCode);
                        };
                        $scope.initPage();
                    }]
            )
        }
);