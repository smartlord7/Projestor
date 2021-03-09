Projestor.factory('ajaxCaller', function ($http) {
    return {
        get: function (endpointUrl, accessToken, id = '',) {
            return $http({
                method: 'GET',
                datatype: 'json',
                headers: {
                    'Content-Type': 'application/json, application/x-www-form-urlencoded',
                    'Authorization': 'Bearer ' + accessToken
                },
                url: endpointUrl + id
            });
        },

        post: function (endpointUrl, accessToken, data) {
            return $http({
                method: 'POST',
                datatype: 'json',
                data: data,
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + accessToken
                },
                url: endpointUrl
            });
        },

        delete: function (endpointUrl, accessToken, data) {
            return $http({
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + accessToken
                },
                data: data,
                url: endpointUrl
            });
        }
    }
});