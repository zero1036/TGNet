define(['controllers/controllers', 'services/commonService'],
    function (controllers) {
        controllers.controller('waMainCtrl', ['$scope', 'commonService', function ($scope, commonService) {
            $scope.name = "home main";
            $scope.pullUpEnabled = false;
            $scope.$on('childInit', function (e, pPullUpEnabled) {
                $scope.pullUpEnabled = pPullUpEnabled;

                //document.addEventListener('touchmove', function (e) { e.preventDefault(); }, false);


                //非常关键：如果没有子页面childInit的时候都重新创建myScroll的话，会导致myScroll注册事件重复调用
                if (myScroll == undefined || myScroll == null) {
                    //$scope.loaded();
                    setTimeout($scope.loaded, 200);
                }


                //document.addEventListener('DOMContentLoaded', function () {
                //    setTimeout(lbload, 200);
                //}, false);

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
                    if (pullUpEl != undefined && pullUpEl != null)
                        pullUpEl.outerText = "";
                }

                myScroll = new iScroll('wrapper', {
                    handleClick: true,
                    useTransform: true, //是否使用CSS形变
                    useTransition: false,//是否使用CSS形变--重要，需要禁止，否则会拖动卡顿
                    topOffset: pullDownOffset,
                    //拖动惯性
                    momentum: true,
                    onBeforeScrollStart: function (e) { e.preventDefault(); },
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
                //setTimeout(function () { document.getElementById('wrapper').style.left = '0'; }, 100);
            }
            //pullDownAction
            function pullDownAction() {
                $scope.$broadcast('pullDownAction', myScroll);
            }
            //pullUpAction
            function pullUpAction() {
                $scope.$broadcast('pullUpAction', myScroll);
            }
            //返回按钮
            $scope.goBack = function () {
                $scope.$broadcast('goBack', 123);
            }
            //确认按钮
            $scope.enter = function () {
                $scope.$broadcast('enter', 123);
            }
            //
            $scope.goSearch = function () {
                var pHeader = angular.element("#header");
                var pWrapper = angular.element("#wrapper");
                pHeader.fadeToggle(300);
                pWrapper.toggleClass("wrap-hidden");
            }
            $scope.goSearch();
            //置顶按钮
            $scope.goUp = function () {
                myScroll.scrollTo(0, 2000, 200);
                myScroll.refresh();
            }
        }]);
    });
