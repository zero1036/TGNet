define(['controllers/controllers', 'services/commonService'],
    function (controllers) {
        controllers.controller('waMainCtrl', ['$scope', 'commonService', function ($scope, commonService) {
            $scope.name = "home main";
            $scope.pullUpEnabled = false;
            $scope.$on('childInit', function (e, pPullUpEnabled) {
                $scope.pullUpEnabled = pPullUpEnabled;
                $scope.loaded();
            });

            var myScroll,
                pullDownEl, pullDownOffset,
                pullUpEl, pullUpOffset;
            //generatedCount = 0;
            $scope.loaded = function () {
                //console.log(commonService.page4Config);

                pullDownEl = document.getElementById('pullDown');
                pullDownOffset = pullDownEl.offsetHeight;
                if ($scope.pullUpEnabled) {
                    pullUpEl = document.getElementById('pullUp');
                    pullUpOffset = pullUpEl.offsetHeight;
                }
                else {
                    pullUpEl = document.getElementById('pullUp');
                    pullUpEl.outerText = "";
                }

                myScroll = new iScroll('wrapper', {
                    useTransition: true,
                    topOffset: pullDownOffset,
                    onRefresh: function () {
                        if (pullDownEl.className.match('loading')) {
                            pullDownEl.className = '';
                            pullDownEl.querySelector('.pullDownLabel').innerHTML = 'Pull down to refresh...';
                        } else if ($scope.pullUpEnabled && pullUpEl.className.match('loading')) {
                            pullUpEl.className = '';
                            pullUpEl.querySelector('.pullUpLabel').innerHTML = 'Pull up to load more...';
                        }
                    },
                    onScrollMove: function () {
                        if (this.y > 5 && !pullDownEl.className.match('flip')) {
                            pullDownEl.className = 'flip';
                            pullDownEl.querySelector('.pullDownLabel').innerHTML = 'Release to refresh...';
                            this.minScrollY = 0;
                        } else if (this.y < 5 && pullDownEl.className.match('flip')) {
                            pullDownEl.className = '';
                            pullDownEl.querySelector('.pullDownLabel').innerHTML = 'Pull down to refresh...';
                            this.minScrollY = -pullDownOffset;
                        } else if ($scope.pullUpEnabled && this.y < (this.maxScrollY - 5) && !pullUpEl.className.match('flip')) {
                            pullUpEl.className = 'flip';
                            pullUpEl.querySelector('.pullUpLabel').innerHTML = 'Release to refresh...';
                            this.maxScrollY = this.maxScrollY;
                        } else if ($scope.pullUpEnabled && this.y > (this.maxScrollY + 5) && pullUpEl.className.match('flip')) {
                            pullUpEl.className = '';
                            pullUpEl.querySelector('.pullUpLabel').innerHTML = 'Pull up to load more...';
                            this.maxScrollY = pullUpOffset;
                        }
                    },
                    onScrollEnd: function () {
                        if (pullDownEl.className.match('flip')) {
                            pullDownEl.className = 'loading';
                            pullDownEl.querySelector('.pullDownLabel').innerHTML = 'Loading...';
                            pullDownAction();	// Execute custom function (ajax call?)
                        } else if ($scope.pullUpEnabled && pullUpEl.className.match('flip')) {
                            pullUpEl.className = 'loading';
                            pullUpEl.querySelector('.pullUpLabel').innerHTML = 'Loading...';
                            pullUpAction();	// Execute custom function (ajax call?)
                        }
                    }
                });

                document.getElementById('wrapper').style.left = '0';
                //setTimeout(function () { document.getElementById('wrapper').style.left = '0'; }, 800);
            }
            //pullDownAction
            function pullDownAction() {
                $scope.$broadcast('pullDownAction', myScroll);
            }
            //pullUpAction
            function pullUpAction() {
                $scope.$broadcast('pullUpAction', myScroll);

            }
        }]);
    });
