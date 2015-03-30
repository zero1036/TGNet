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

//获取部门下成员
WXService.factory('memberService', ['$http', function ($http) {
    var service = {};
    var param = {};
    var result = {};
    var url;

    //獲取成員列表
    service.getMemberList = function (depPKID) {
        param.ID = depPKID;
        url = '/QYMember/GetMemberByDepPKID';
        return $http.post(url, param);
    };
    return service;
}]);