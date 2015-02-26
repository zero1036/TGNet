var WXBaseController = angular.module('WXBaseController', ['WXService', 'WXDirective']);

WXBaseController.controller('BaseCtrl', ['$scope', '$http', 'instance', function ($scope, $http, instance) {
    //$scope.BingPicUrl = function (mediaId) {
    //    var rPath = '';
    //    if (instance.pictures !== undefined && instance.pictures !== null) {
    //        angular.forEach(instance.pictures, function (item) {
    //            if (item.media_id == mediaId) {
    //                rPath = item.RPath;
    //                //return item.RPath;
    //            }
    //        });
    //    }
    //    //return "/Images/WXResources/53-13061G01443.jpg";
    //    return rPath;
    //}
}]);