define(['controllers/controllers', ],
    function (controllers) {
        controllers.controller('testWACtrl', ['$scope', function ($scope) {
            $scope.name = "tg";
            $scope.ok = true;
            $scope.direction = 'left';
            $scope.currentIndex = 0;
            $scope.setCurrentSlideIndex = function (index) {
                $scope.direction = (index > $scope.currentIndex) ? 'left' : 'right';
                $scope.currentIndex = index;
            };
            $scope.isCurrentSlideIndex = function (index) {
                return $scope.currentIndex === index;
            };
            $scope.prevSlide = function () {
                $scope.direction = 'left';
                $scope.currentIndex = ($scope.currentIndex < $scope.slides.length - 1) ? ++$scope.currentIndex : 0;
            };
            $scope.nextSlide = function () {
                $scope.direction = 'right';
                $scope.currentIndex = ($scope.currentIndex > 0) ? --$scope.currentIndex : $scope.slides.length - 1;
            };
            $scope.tagx = function () {
                $scope.name = "Mark";
                if ($scope.ok) $scope.ok = false;
                else $scope.ok = true;
            }
        }]);
    });
