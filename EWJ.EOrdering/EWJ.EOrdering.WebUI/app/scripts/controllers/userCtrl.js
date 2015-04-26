'use strict';
define(['controllers/controllers', 'services/userService', 'services/dictionaryService']
        , function (controllers) {
            controllers.controller('userCtrl'
                , ['$scope', 'userService', '$translatePartialLoader', '$translate', 'dictionaryService'
                    , function ($scope, userService, $translatePartialLoader, $translate, dictionaryService) {
                        $translatePartialLoader.addPart('userlist');
                        $translate.refresh();
                        $scope.Title = 'User List';
                        $scope.userList = [];

                        //獲取用戶列表
                        $scope.getUserList = function () {
                            userService.getUserList()
                                .success(function (result) {
                                    $scope.userList = result.userList;
                                })
                                .error(function () {
                                });

                            dictionaryService.getListByParentCode('');
                        };

                        //加載頁面
                        $scope.initPage = function () {
                            $scope.getUserList();
                        };
                        $scope.initPage();
                    }
                ]
            )
        }
);