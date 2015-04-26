define(['services/services'],function (services){
	services.factory('configService', ['$http',function ($http){
		var servicesInstance = {};

		servicesInstance.getConfig = function(){
			var result = $http({
				method: 'GET',
				url: '../config.xml'
			})
			return result;
		}
		return servicesInstance;
	}]);
});