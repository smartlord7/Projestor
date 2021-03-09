Projestor.controller("ProjectsController", function ($scope, $location, sessionService, ajaxCaller) {
    $scope.init = function () {
        sessionService.checkSession();
        $scope.access_token = sessionService.getSessionJwt();
        $scope.role = sessionService.getRole();
        $location.url($location.path());
        $scope.projects = [];
        $scope.selectedProjects = {};
        $scope.sortOrder = 'name';
        $scope.ascendingSortOrder = true;
        $scope.sortOrders = [['name', true], ['managerName', true], ['budget', true], ['createdDateTime', true]];
        $scope.nameStart = '';
        $scope.allSelected = false;
    };

    $scope.loadGrid = function () {
        ajaxCaller.get('https://localhost:44347/api/projects/list', $scope.access_token).then(function (response) {
             $scope.projects = response.data;
        }, function () {
             console.log("Error retrieving projects: " + response.data.error_description);
        });
    };

    $scope.unselectRow = function (index) {
        $scope.selectedProjects[index] = {};
    };

    $scope.toggleProject = function (index, project) {
        if ($scope.role == 'Admin' || $scope.role == 'Manager') {
            if (!$scope.selectedProjects[index]) {
                $scope.selectedProjects[index] = project;
            } else {
                delete $scope.selectedProjects[index];
            }
        }
    };

    $scope.toggleSortOrder = function (index) {
        $scope.sortOrder = $scope.sortOrders[index][0];
        $scope.ascendingSortOrder = $scope.sortOrders[index][1];
        $scope.sortOrders[index][1] = !$scope.sortOrders[index][1];
    };

    $scope.toggleAll = function (event) {
        if (event.target.checked) $scope.allSelected = true;
        else {
            $scope.allSelected = false;
            $scope.selectedProjects = [];
        }
    };

    $scope.noSelectedProjects = function () {
        var counter = 0;
        for (var key in $scope.selectedProjects) counter++;
        return counter == 0;
    };

    $scope.addProject = function (index, project) {
        if ($scope.allSelected) $scope.selectedProjects[index] = project;
    };

    $scope.deleteSelectedProjects = function () {
        var ids = [];
        for (var key in $scope.selectedProjects) {
            ids.push($scope.selectedProjects[key].id);
        }

        ajaxCaller.delete('https://localhost:44347/api/projects/delete', $scope.access_token, ids).then(function (response) {
            $scope.loadGrid();
            $scope.selectedProjects = {};
        }, function (response) {
            console.log('Error deleting selected projects! ' + response.data);
        });
    };

    $scope.toProjectDetails = function (projectId) {
        $location.path('/projects/project/' + projectId);
    };

    $scope.toAddProject = function () {
        $location.path('/projects/createProject');
    };

    $scope.toEditProject = function (project) {
        $location.path('/projects/project/' + project.id + '/editProject')
    };

    $scope.toProjectIssues = function (projectId, projectName) {
        $location.path('/projects/project/' + projectId + '/projectIssues');
    };

    $scope.toUserDetails = function (userName) {
        $location.path('/account/' + userName + '/userDetails');
    };

    $scope.main = function () {
        $scope.init();
        $scope.loadGrid();
    };

    $scope.main();
});