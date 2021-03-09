Projestor.controller('HomeController', function ($scope, sessionService) {
    $scope.init = function () {
        sessionService.checkSession();
        $scope.role = sessionService.getRole();
    };

    $scope.getUserName = function () {
        return sessionService.getUserName();
    };


    $scope.main = function () {
        $scope.init();
    };

    $scope.main();

});