define(['services/services'],function(services) {
    services.factory('testService', [
      function() {
        return {
          getUser: function() {
            return '苹果表';
          }
        };
    }]);
});
