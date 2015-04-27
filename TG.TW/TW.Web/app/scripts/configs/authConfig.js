define(['configs/configs'], function (configs) {
    configs.config('authCfg'
        , ['$httpProvider'
        , function ($httpProvider) {
            // 在这里构造拦截器
            var interceptor = function ($q, $rootScope, Auth) {
                return {
                    'response': function (resp) {
                        if (resp.config.url == '/Login/Login') {
                            // 假设API服务器返回的数据格式如下:
                            // { token: "AUTH_TOKEN" }
                            Auth.setToken(resp.data.token);
                        }
                        return resp;
                    },
                    'responseError': function (rejection) {
                        // 错误处理
                        switch (rejection.status) {
                            case 401:
                                if (rejection.config.url !== 'api/login')
                                    // 如果当前不是在登录页面
                                    $rootScope.$broadcast('auth:loginRequired');
                                break;
                            case 403:
                                $rootScope.$broadcast('auth:forbidden');
                                break;
                            case 404:
                                $rootScope.$broadcast('page:notFound');
                                break;
                            case 500:
                                $rootScope.$broadcast('server:error');
                                break;
                        }
                        return $q.reject(rejection);
                    }
                };
            };
            // 将拦截器和$http的request/response链整合在一起
            $httpProvider.interceptors.push(interceptor);

        }]);
});