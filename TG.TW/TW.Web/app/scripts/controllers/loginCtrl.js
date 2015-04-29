define(['controllers/controllers', 'services/commonService', 'services/configService', 'services/baseService', 'services/loginService', 'services/permissionService', 'services/userService', 'services/authService'], function (controllers) {
    controllers.controller('loginCtrl'
        , ['$rootScope', '$scope'
            , 'commonService', 'configService', 'baseService', 'loginService', 'permissionService', 'userService', 'authService'
        , function ($rootScope, $scope, commonService, configService, baseService, loginService, permissionService, userService, authService) {

            $scope.homePageUrl = 'index.html';
            $scope.User = {};

            $scope.setAPIUrl = function () {
                var url = commonService.getAPIDefaultUrl();
                if (url == null || url == '') {
                    configService.getConfig()
                        .success(function (result) {
                            var x2js = new X2JS();
                            url = x2js.xml_str2json(result).configuration.apiUrl.defaultAPIUrl;
                            if (url != null && url != '' && url != undefined) {
                                commonService.setSession(commonService.SESSION_KEY_API_DEFAULT_URL, url);
                            } else {
                                alert("����ʧ����");
                            }
                        })
                        .error(function () {
                            alert("�����ļ�ȱʧ��");
                        });
                }
            }

            //��¼
            $scope.signin = function () {
                if ($scope.user == null || $scope.user.account == undefined || $scope.user.account == null)
                    return false;

                //��¼
                loginService.signin($scope.user.account, $scope.user.password)
                    .success(function (result, status, headers, config) {
                        authService.setUser(result);
                        //�ɹ�����ת����ҳ
                        //return $location.path($scope.homePageUrl);
                        window.location.href = $scope.homePageUrl;
                        ////��ȡ��ǰ�û�
                        //userService.getCurrentUser()
                        //    .success(function (result, status, headers, config) {
                        //        if (result == null) {
                        //            return $location.path("login");
                        //        }
                        //        commonService.setSession(commonService.SESSION_KEY_CURRENT_USER, result);

                        //        //��ȡ�û��˵�
                        //        permissionService.getMenu()
                        //            .success(function (result, status, headers, config) {
                        //                commonService.setSession(commonService.SESSION_KEY_CURRENT_USER_MENU, result);

                        //                //��ȡ�û�Ȩ��
                        //                permissionService.getPermission()
                        //                    .success(function (result, status, headers, config) {
                        //                        commonService.setSession(commonService.SESSION_KEY_CURRENT_USER_PERMISSION, result);


                        //                        //�ɹ�����ת����ҳ
                        //                        //return $location.path($scope.homePageUrl);
                        //                        window.location.href = $scope.homePageUrl;
                        //                    })
                        //                    .error(function () {
                        //                        alert("��ȡ�û�Ȩ��ʧ��");
                        //                    });
                        //            })
                        //            .error(function () {
                        //                alert("��ȡ��ǰ�û��˵�ʧ��");
                        //            });
                        //    })
                        //    .error(function () {
                        //        alert("��ȡ��ǰ�û�ʧ��");
                        //    });
                    })
                    .error(function () {
                        alert("��¼ʧ��");
                    });
            };

            $scope.initPage = function () {
                $scope.setAPIUrl();

                var txtAccount = document.getElementById("txtAccount");
                txtAccount.focus();
                $scope.user = { account: 'mark', password: '504' };

            }
            $scope.initPage();
        }]);
});