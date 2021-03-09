Projestor.controller('LoginController', function ($scope, $window, $location, sessionService, ajaxCaller) {
    $scope.init = function () {
        $location.url($location.path());
        $scope.error = '';
        $scope.loginData = {};
        if (sessionService.getSessionJwt()) $location.path('/home');
    };

    $scope.login = function () {
        var access_token = '';
        var loginData = 'userName=' + $scope.loginData.username + '&password=' + $scope.loginData.password + '&grant_type=password';
        return ajaxCaller.post('https://localhost:44347/oauth/token', access_token, loginData).then(function (response) {
            access_token = response.data.access_token;
            expires_in = response.data.expires_in;
            var loginDate = new Date();
            ajaxCaller.get('https://localhost:44347/api/accounts/sessionData', access_token).then(function (response) {
                var sessionData = {
                    expirationTime: expires_in,
                    loginDate: loginDate,
                    jwt: access_token,
                    userName: response.data.userName,
                    role: response.data.role
                }
                $window.localStorage.setItem('sessionData', JSON.stringify(sessionData));
                $location.path('/');
            }, function (response) {
                console.log('Error retrieving current user session data!!');
            });
        }, function (response) {
                $scope.error = response.data.error_description;
        });
    }

    $scope.toRegister = function () {
        $location.path('/register');
    }

    $scope.main = function () {
        $scope.init();
    }

    $scope.main();
})