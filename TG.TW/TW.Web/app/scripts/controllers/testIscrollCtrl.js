define(['controllers/controllers', 'services/commonService'],
    function (controllers) {
        controllers.controller('testIscrollCtrl', ['$scope', 'commonService', function ($scope, commonService) {
            //监听下拉pullDown事件
            $scope.$on('pullDownAction', function (e, myScroll) {
                setTimeout(function () {	// <-- Simulate network congestion, remove setTimeout from production!
                    var el, li, i;
                    el = document.getElementById('thelist');

                    for (i = 0; i < 3; i++) {
                        li = document.createElement('li');
                        li.innerText = 'Generated row ' + 9999;
                        el.insertBefore(li, el.childNodes[0]);
                    }

                    myScroll.refresh();		// Remember to refresh when contents are loaded (ie: on ajax completion)
                }, 1000);	// <-- Simulate network congestion, remove setTimeout from production!
            });            //监听上拉pullup事件            $scope.$on('pullUpAction', function (e, myScroll) {
                setTimeout(function () {	// <-- Simulate network congestion, remove setTimeout from production!
                    var el, li, i;
                    el = document.getElementById('thelist');

                    for (i = 0; i < 3; i++) {
                        li = document.createElement('li');
                        li.innerText = 'Generated row ' + (++generatedCount);
                        el.appendChild(li, el.childNodes[0]);
                    }

                    myScroll.refresh();		// Remember to refresh when contents are loaded (ie: on ajax completion)
                }, 1000);	// <-- Simulate network congestion, remove setTimeout from production!
            });
            $scope.childInit = function () {
                $scope.$emit('childInit', false);
            }
        }]);
    });
