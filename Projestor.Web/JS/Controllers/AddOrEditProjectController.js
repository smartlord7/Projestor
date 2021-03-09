Projestor.controller('AddOrEditProjectController', function ($scope, $location, $routeParams, sessionService, ajaxCaller) {
    $scope.init = function () {
        sessionService.checkSession();
        $scope.access_token = sessionService.getSessionJwt();
        $scope.project = {};
        $scope.project.id = $routeParams.projectId;
        $scope.action = $location.path().split('/').slice(-1)[0];
    };

    $scope.getProject = function () {
        if ($scope.action == 'editProject') {
            ajaxCaller.get('https://localhost:44347/api/projects/project/', $scope.access_token, $scope.project.id).then(function (response) {
                $scope.project = response.data;
            }, function (response) {
                console.log('Error retrieving project with id ' + $scope.project.id + '!');
            });
        };
    }

    $scope.saveProject = function () {
        $scope.project.managerId = -1;
        ajaxCaller.post('https://localhost:44347/api/projects/edit', $scope.access_token, $scope.project).then(function (response) {
            $scope.back();
        }, function (response) {
            console.log('Error creating project!');
        });
    };

    $scope.back = function () {
        $location.path('/projects');
    };

    $scope.main = function () {
        $scope.init();
        $scope.getProject();
    };

    $scope.main();

});