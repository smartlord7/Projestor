Projestor.controller('ProjectDetailsController', function ($scope, $window, $location, $routeParams, sessionService, ajaxCaller) {
    $scope.init = function () {
        sessionService.checkSession();
        $scope.access_token = sessionService.getSessionJwt();
        $location.url($location.path());
        $scope.project = {};
        $scope.project.id = $routeParams.projectId;
    };

    $scope.getProject = function () {
        ajaxCaller.get('https://localhost:44347/api/projects/project/', $scope.access_token, $scope.project.id).then(function (response) {
            $scope.project = response.data;
        }, function (response) {
            console.log('Error retrieving project with id ' + $scope.project.id + '!');
        });
    };

    $scope.back = function () {
        $window.history.back();
    };

    $scope.main = function () {
        $scope.init();
        $scope.getProject();
    };

    $scope.main();
});