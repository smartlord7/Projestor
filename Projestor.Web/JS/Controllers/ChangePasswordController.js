Projestor.controller('ChangePasswordController', function ($scope, $location, $window, sessionService, ajaxCaller) {
    $scope.init = function () {
        sessionService.checkSession();
        $scope.access_token = sessionService.getSessionJwt();
        $location.url($location.path());
        $scope.changePasswordModel = {};
        $scope.errors = [];
    };

    $scope.back = function () {
        $window.history.back();
    };

    $scope.passwordCheck = function () {
        return $scope.changePasswordModel.newPassword == $scope.changePasswordModel.confirmPassword;
    };

    $scope.isValidForm = function () {
        return $scope.changePasswordForm.$valid && $scope.passwordCheck();
    };

    $scope.changePassword = function () {
        if ($scope.isValidForm()) {
            ajaxCaller.post('https://localhost:44347/api/accounts/changePassword', $scope.access_token, $scope.changePasswordModel).then(function (response) {
                $scope.errors = [];
                $scope.back();
            }, function (response) {
                $scope.errors = [];
                if (response.data.modelState) {
                    for (var key in response.data.modelState) {
                        for (var i = 0; i < response.data.modelState[key].length; i++)
                            $scope.errors.push(response.data.modelState[key][i]);
                    }
                }
                console.log($scope.errors);
            });
        };
    };

    $scope.main = function () {
        $scope.init();
    };

    $scope.main();
});