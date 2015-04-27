define(['controllers/controllers', 'services/testService', 'services/commonService'],
    function (controllers) {
        controllers.controller('mainCtrl', ['$scope', 'testService', 'commonService', '$translatePartialLoader', '$translate',
          function ($scope, testService, commonService, $translatePartialLoader, $translate) {
              // 选择语言
              /*
            $scope.changeLanguage = function (langKey){
                $translate.use(langKey);
            };
              $scope.name = testService.getUser();
              */
              $scope.Menu = commonService.getCurrentUserMenu();

              $scope.signout = function () {
                  commonService.clearSessionData();
                  window.location.href = 'login.html';
              };

          }]);
    });
