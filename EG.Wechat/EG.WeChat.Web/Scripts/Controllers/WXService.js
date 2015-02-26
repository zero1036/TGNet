//服务
var WXService = angular.module('WXService', []);
//全部单例服务
WXService.factory('instance', function () {
    return {};
});
//WXService.factory('Geek', function ($resource) {
//    return $resource("/WXOrganization/QueryUserTable", {}, {
//        query: {
//            method: "POST",
//            params: { geekId: "list" },
//            isArray: true
//        }
//    });
//});
WXService.factory('ResourceService', ['$http', function ($http) {
    var factory = {};
    factory.Delete = function (lcid, ptype) {
        var url = "/WXResource/DeleteResource?lcid=" + lcid + "&ptype=" + ptype;
        $http.post(url).success(function (response) {
            if (response !== undefined && response !== null && !response.IsSuccess)
                alert(response.Message);
        });
    }
    return factory;
}]);