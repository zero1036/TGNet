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
                                alert("配置失敗！");
                            }
                        })
                        .error(function () {
                            alert("配置文件缺失！");
                        });
                }
            }

            //登录
            $scope.signin = function () {
                if ($scope.user == null || $scope.user.account == undefined || $scope.user.account == null)
                    return false;

                //登录
                loginService.signin($scope.user.account, $scope.user.password)
                    .success(function (result, status, headers, config) {
                        authService.setUser(result);
                        //成功后跳转到首页
                        //return $location.path($scope.homePageUrl);
                        window.location.href = $scope.homePageUrl;
                        ////获取当前用户
                        //userService.getCurrentUser()
                        //    .success(function (result, status, headers, config) {
                        //        if (result == null) {
                        //            return $location.path("login");
                        //        }
                        //        commonService.setSession(commonService.SESSION_KEY_CURRENT_USER, result);

                        //        //获取用户菜单
                        //        permissionService.getMenu()
                        //            .success(function (result, status, headers, config) {
                        //                commonService.setSession(commonService.SESSION_KEY_CURRENT_USER_MENU, result);

                        //                //获取用户权限
                        //                permissionService.getPermission()
                        //                    .success(function (result, status, headers, config) {
                        //                        commonService.setSession(commonService.SESSION_KEY_CURRENT_USER_PERMISSION, result);


                        //                        //成功后跳转到首页
                        //                        //return $location.path($scope.homePageUrl);
                        //                        window.location.href = $scope.homePageUrl;
                        //                    })
                        //                    .error(function () {
                        //                        alert("获取用户权限失败");
                        //                    });
                        //            })
                        //            .error(function () {
                        //                alert("获取当前用户菜单失败");
                        //            });
                        //    })
                        //    .error(function () {
                        //        alert("获取当前用户失败");
                        //    });
                    })
                    .error(function () {
                        alert("登录失败");
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