Projestor.controller('AddOrEditAccountController', function ($scope, $location, $routeParams, $window, sessionService, ajaxCaller) {
    $scope.init = function () {
        $location.url($location.path());
        $scope.action = $location.path().split('/').slice(-1)[0];
        $scope.user = {};
        $scope.role = sessionService.getRole();
        if ($scope.action == 'register') $scope.user = { role: 'Programmer' };
        $scope.errors = [];
    };

    $scope.getUser = function () {
        ajaxCaller.get('https://localhost:44347/api/accounts/user/' + $routeParams.userName, $scope.access_token).then(function (response) {
            $scope.user = response.data;
            $scope.user.birthDate = new Date($scope.user.birthDate);
        }, function (response) {
            console.log("Error retrieving user!");
        });
    };

    $scope.passwordCheck = function () {
        return $scope.user.password == $scope.user.confirmPassword;
    };

    $scope.isValidForm = function () {
        return $scope.userForm.$valid && $scope.passwordCheck();
    };

    $scope.saveUser = function () {
        if ($scope.action == 'editAccount') {
            if ($scope.isValidForm()) {
                ajaxCaller.post('https://localhost:44347/api/accounts/edit/' + $routeParams.userName, $scope.access_token, $scope.user).then(function (response) {
                    $scope.back();
                }, function (response) {
                    console.log("Error saving user!");
                });
                if ($routeParams.userName == sessionService.getUserName()) {
                    var sessionData = {
                        jwt: sessionService.getSessionJwt(),
                        roles: sessionService.getRoles(),
                        userName: $scope.user.userName
                    }
                    sessionService.updateSessionData(sessionData);
                }
            }
        } else {
            if ($scope.isValidForm()) {
                ajaxCaller.post('https://localhost:44347/api/accounts/create', '', $scope.user).then(function (response) {
                    $scope.errors = [];
                    if (!$scope.user.emailConfirmed)
                    alert('A confirmation email was sent to ' + $scope.user.email + '.Please click the activation link in it to confirm your registration.');
                    $scope.back();
                }, function (response) {
                    if (response.data.modelState) {
                        for (var key in response.data.modelState)
                            for (var i = 0; i < response.data.modelState[key].length; i++)
                                $scope.errors.push(response.data.modelState[key][i]);
                    }
                    console.log($scope.errors);
                })
            }
        }
    };

    $scope.back = function () {
        $window.history.back();
    };

    $scope.main = function () {
        $scope.init();
        if ($scope.action == 'editAccount') {
            $scope.access_token = sessionService.getSessionJwt();
            sessionService.checkSession();
            $scope.getUser();
        }
    };

    $scope.main();
});