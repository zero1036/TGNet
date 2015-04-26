require.config({
    paths: {
        /*
		 * 引入类库
		 */
        // jquery
        jquery: 'vendor/jquery',
        // Angular
        angular: 'vendor/angular/angular',
        // domReady
        domReady: 'vendor/domReady',
        cookies: 'vendor/angular/angular-cookies',
        // xml to json
        x2js: 'vendor/x2js/xml2json'
    },
    shim: {
        /*
         *对引入的类库，表明依赖关系，并暴露名称
         *需要把此处的名字，在app.js文件中define写上才能使用
         */
        angular: {
            deps: ['jquery'],
            exports: 'angular'
        },
        cookies: {
            deps: ['angular']
        },
        x2js: {
            deps: ['jquery']
        }
    }
});
require([
	'angular',
	'loginApp',
	'domReady',
	// 自定义controllers,services,directives,filters都需要在这里添加路径
    'controllers/loginCtrl'
], function (angular, loginApp, domReady) {

    loginApp.config(function ($httpProvider) {
        $httpProvider.defaults.withCredentials = true;
        $httpProvider.defaults.useXDomain = true;
        delete $httpProvider.defaults.headers.common['X-Requested-With'];
    });

    domReady(function () {
        angular.bootstrap(document, ['eOrderingApp']);

        $('html').addClass('ng-app: eOrderingApp');
    });
})