define(['controllers/controllers', 'services/testService', 'services/commonService', 'services/authService'],
    function (controllers) {
        controllers.controller('mainCtrl', ['$scope', 'testService', 'commonService', 'authService', '$translatePartialLoader', '$translate',
          function ($scope, testService, commonService, authService, $translatePartialLoader, $translate) {
              // 选择语言
              /*
            $scope.changeLanguage = function (langKey){
                $translate.use(langKey);
            };
              $scope.name = testService.getUser();
              */
              $scope.Menu = commonService.getCurrentUserMenu();
              $scope.name = authService.getToken();
              $scope.signout = function () {
                  commonService.clearSessionData();
                  window.location.href = 'login.html';
              };

          }]);
    });
