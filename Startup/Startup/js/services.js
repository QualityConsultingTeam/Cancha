
// Demonstrate how to register services
// In this case it is a simple value service.
angular.module('app.services', ['ngResource','$http'])
 //  .factory('Data', ['$http', data])
//    .value('version', '0.1');

function data($http) { // This service connects to our REST API

    var serviceBase = 'api/v1/';

    var obj = {};
    obj.toast = function (data) {
      //  toaster.pop(data.status, "", data.message, 10000, 'trustedHtml');
    }
    obj.get = function (q) {
        return $http.get(serviceBase + q).then(function (results) {
            return results.data;
        });
    };
    obj.post = function (q, object) {
        return $http.post(serviceBase + q, object).then(function (results) {
            return results.data;
        });
    };
    obj.put = function (q, object) {
        return $http.put(serviceBase + q, object).then(function (results) {
            return results.data;
        });
    };
    obj.delete = function (q) {
        return $http.delete(serviceBase + q).then(function (results) {
            return results.data;
        });
    };

    return obj;
};

 