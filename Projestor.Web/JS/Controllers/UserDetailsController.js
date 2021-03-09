Projestor.controller('UserDetailsController', function ($scope, $location, $routeParams, $window, ajaxCaller, sessionService) {
    $scope.init = function () {
        sessionService.checkSession();
        $scope.role = sessionService.getRole();
        $scope.access_token = sessionService.getSessionJwt();
        $location.url($location.path());
        $scope.user = {};
        $scope.user.userName = $routeParams.userName;
    };

    $scope.getUser = function () {
        ajaxCaller.get('https://localhost:44347/api/accounts/user/', $scope.access_token, $scope.user.userName).then(function (response) {
            $scope.user = response.data;
        }, function (response) {
            console.log('Error retrieving user!');
        });
    }; 

    $scope.back = function () {
        $window.history.back();
    }

    $scope.main = function () {
        $scope.init();
        $scope.getUser();
    };

    $scope.main();
});