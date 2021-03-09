Projestor.controller('AddOrEditIssueController', function ($scope, $location, $window, $routeParams, ajaxCaller, sessionService) {
    $scope.init = function () {
        sessionService.checkSession();
        $scope.access_token = sessionService.getSessionJwt();
        $scope.role = sessionService.getRole();
        $location.url($location.path());
        $scope.issue = {};
        $scope.programmers = [];
        $scope.projects = [];
        $scope.issue.projectId = $routeParams.projectId;
        $scope.issue.id = $routeParams.issueId;
        $scope.selectedPrio = $scope.issue.prio;
        $scope.selectedProjectId = $scope.issue.projectId;
        $scope.action = $location.path().split('/').slice(-1)[0];
    };

    $scope.getIssue = function () {
        if ($scope.action != 'addIssue') {
            ajaxCaller.get('https://localhost:44347/api/issues/issue/', $scope.access_token, $scope.issue.id).then(function (response) {
                $scope.issue = response.data;
                $scope.selectedProgrammerId = $scope.issue.userId;
                $scope.issue.limitDate = new Date($scope.issue.limitDate);
            }, function (response) {
                console.log('Issue with id ' + $scope.issue.id + ' could not be retrieved!');
            });
        }
    };

    $scope.getProgrammers = function () {
        ajaxCaller.get('https://localhost:44347/api/accounts/programmers', $scope.access_token).then(function (response) {
            $scope.programmers = response.data;
        }, function (response) {
            console.log('Error retrieving programmers!');
        });
    };

    $scope.getProjects = function () {
        ajaxCaller.get('https://localhost:44347/api/projects/list', $scope.access_token).then(function (response) {
            $scope.projects = response.data;    
        }, function (response) {
            console.log('Erro retrieving projects!')
        });
    }

    $scope.saveIssue = function () {
        ajaxCaller.post('https://localhost:44347/api/issues/edit', $scope.access_token, $scope.issue).then(function (response) {
            $scope.back();
        }, function (response) {
            console.log('Error saving issue ' + $scope.issue.name + '!');
        });
    };

    $scope.back = function () {
        $window.history.back();
    };

    $scope.main = function () {
        $scope.init();
        $scope.getIssue();
        $scope.getProgrammers();
        $scope.getProjects();
    };

    $scope.main();
});