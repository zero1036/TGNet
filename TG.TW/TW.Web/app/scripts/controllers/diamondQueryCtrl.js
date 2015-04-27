define(['controllers/controllers', 'services/diamondService']
    , function (controllers) {
        controllers.controller('diamondQueryCtrl', ['$scope', '$translatePartialLoader', '$translate', 'diamondService'
            , function ($scope, $translatePartialLoader, $translate, diamondService) {
                //$translatePartialLoader.addPart('userlist');
                //$translate.refresh();


                //$scope.diamondQuery = {};
                //$scope.diamondQueryItem = {};
                //$scope.shapeCodeList = [];
                //$scope.colorCodeList = [];
                //$scope.clarityCodeList = [];
                //$scope.storeIDList = [];
                //$scope.caratRangeList = [];
                //$scope.scendingList = [];

                //查询钻石
                $scope.diamondsQueryFunc = function () {
                    $scope.diamondQuery = {};
                    $scope.diamondQueryItem = {};
                    $scope.shapeCodeList = [];
                    $scope.colorCodeList = [];
                    $scope.clarityCodeList = [];
                    $scope.storeIDList = [];
                    $scope.caratRangeList = [];
                    $scope.scendingList = [];

                    if (angular.isDefined($scope.articleNo) && $scope.articleNo!='')
                    {
                        $scope.diamondQueryItem.articleNo=$scope.articleNo;
                    }
                    if (angular.isDefined($scope.diamondNo) && $scope.diamondNo!='')
                    {
                        $scope.diamondQueryItem.diamondNo=$scope.diamondNo;
                    }
                    if (angular.isDefined($scope.shape) && $scope.shape!='') {
                        $scope.shapeCodeList.push({ attrVal: '009001' }, { attrVal: '009002' }, { attrVal: '009003' }, { attrVal: '009004' });
                        $scope.diamondQueryItem.shapeCodeList = $scope.shapeCodeList;
                        $scope.shapeCodeList=[];
                    }
                    if (angular.isDefined($scope.color) && $scope.color != '')
                    {
                        $scope.colorCodeList.push({ attrVal: '007001' }, { attrVal: '007002' }, { attrVal: '007003' }, { attrVal: '007004' });
                        $scope.diamondQueryItem.colorCodeList = $scope.colorCodeList;
                        $scope.colorCodeList = [];
                    }
                    if (angular.isDefined($scope.clarity) && $scope.clarity != '')
                    {
                        $scope.clarityCodeList.push({ attrVal: '008003' }, { attrVal: '008004' }, { attrVal: '008005' }, { attrVal: '008006' });
                        $scope.diamondQueryItem.clarityCodeList = $scope.clarityCodeList;
                        $scope.clarityCodeList = [];
                    }
                    if (angular.isDefined($scope.store) && $scope.store != '')
                    {
                        $scope.storeIDList.push({ attrVal: 'FC52E6CB-0645-C37E-4DDE-49A05B568A1a' });
                        $scope.diamondQueryItem.storeIDList = $scope.storeIDList;
                        $scope.storeIDList = [];
                    }
                    if (angular.isDefined($scope.caratMin) && $scope.caratMin != '')
                    {
                        $scope.caratRangeList.push({caratMin:0,caratMax:0.5});
                        $scope.diamondQueryItem.caratRangeList = $scope.caratRangeList;
                        $scope.caratRangeList = [];
                    }
                   
                    if (angular.isDefined($scope.uploadDate) && $scope.uploadDate != '')
                    {
                        $scope.diamondQueryItem.uploadDate = $scope.uploadDate;
                    }
                    if(angular.isDefined($scope.statusCode))
                    {
                        $scope.diamondQueryItem.statusCode=$scope.statusCode;
                    }
                    $scope.diamondQuery.diamondQueryItem = $scope.diamondQueryItem;

                    $scope.scendingList.push({scendingType:'asc',columnName:'uploadDate'});
                    $scope.diamondQuery.scendingList = $scope.scendingList;
                    $scope.scendingList = [];

                    debugger;
                    diamondService.diamondsQuery($scope.diamondQuery).success(function (response) {
                        $scope.diamondList = response.diamondList;
                    }).error(function () {

                    });
                };

                //重置查询条件
                $scope.resetQueryItem = function () {
                    $scope.articleNo = undefined;
                    $scope.diamondNo = undefined;
                    $scope.uploadDate = undefined;
                    $scope.statusCode = undefined;
                };

                //初始加载函数
                $scope.diamondsQueryFunc();
            }]);
    });
