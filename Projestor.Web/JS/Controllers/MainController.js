Projestor.controller("MainController", function ($scope, $location, sessionService) {
    $scope.init = function () {
        $scope.logout = sessionService.userLogout;
    };

    $scope.isUserLoggedIn = function () {
        return sessionService.getSessionJwt();
    };

    $scope.role = function () {
        return sessionService.getRole();
    };

    $scope.userName = function () {
        return sessionService.getUserName();
    };

    $scope.toUsers = function () {
        $location.path("/users");
    };

    $scope.toProjects = function () {
        $location.path("/projects");
    };

    $scope.toIssues = function () {
        $location.path("/issues");
    };

    $scope.toEditAccount = function () {
        $location.path('/account/' + $scope.userName() + '/editAccount');
    };

    $scope.toChangePassword = function () {
        $location.path('/account/changePassword')
    };

    $scope.toHome = function () {
        $location.path('/home');
    };

    $scope.main = function () {
        $scope.init();
    };

    $scope.main();
});
