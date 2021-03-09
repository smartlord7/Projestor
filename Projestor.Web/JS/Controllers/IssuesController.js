Projestor.controller("IssuesController", function ($scope, $location, $routeParams, sessionService, ajaxCaller) {
    $scope.init = function () {
        sessionService.checkSession();
        $scope.access_token = sessionService.getSessionJwt();
        $scope.role = sessionService.getRole();
        $location.url($location.path());
        $scope.action = $location.path().split('/').slice(-1)[0];
        $scope.project = {};
        $scope.project.id = $routeParams.projectId;
        $scope.selectedIssues = {};
        $scope.newCurrentState = '4';
        $scope.issues = [];
        $scope.ascendingSortOrder = true;
        $scope.sortOrders = [['name', true], ['projectName', true], ['programmerName', true], ['limitDate', true], ['createdDateTime', true], ['state', true], ['prio', true]];
        $scope.nameStart = '';
        $scope.sortOrder = '';
        $scope.selectedFilter = 'state';
        $scope.issueNotes = '';
        $scope.selIssueToEditNotes = {};
        $scope.allSelected = false;
        $scope.filters = {
            'state': [
                [0, 'NA'], [1, 'NOT STARTED'], [2, 'FROZEN'], [3, 'IN PROGRESS'], [4, 'FINISHED']
            ],
            'prio': [
                [0, 'NA'], [1, 'LOW'], [2, 'INTERMEDIATE'], [3, 'HIGH']
            ]
        };
    };

    $scope.toggleIssue = function (index, issue) {
        if (!$scope.selectedIssues[index]) {
            $scope.selectedIssues[index] = issue;
        } else {
            $scope.allSelected = false;
            delete $scope.selectedIssues[index];
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
            $scope.selectedIssues = [];
        }
    };

    $scope.noSelectedIssues = function () {
        var counter = 0;
        for (var key in $scope.selectedIssues) counter++;
        return counter == 0;
    };

    $scope.includeIssue = function (index, issue) {
        if ($scope.allSelected) $scope.selectedIssues[index] = issue;
    };

    $scope.getUserIssues = function () {
        ajaxCaller.get('https://localhost:44347/api/issues/list', $scope.access_token).then(function (response) {
            $scope.issues = response.data;
            var projectNames = [];
            for (var i = 0; i < $scope.issues.length; i++) {
                var currentProjectName = $scope.issues[i].projectName;
                if (!projectNames.includes(currentProjectName)) projectNames.push(currentProjectName);
            }
            $scope.sortOrder = 'prio';
            $scope.filters['projectName'] = projectNames;
        }, function (response) {
            console.log("Error retrieving user issues!");
        });
    };

    $scope.getProjectIssues = function () {
        ajaxCaller.get('https://localhost:44347/api/issues/projectIssues/', $scope.access_token, $scope.project.id).then(function (response) {
            $scope.issues = response.data;
            var programmerNames = [];
            for (var i = 0; i < $scope.issues.length; i++) {
                var currentProgrammerName = $scope.issues[i].userName;
                if (!programmerNames.includes(currentProgrammerName)) programmerNames.push(currentProgrammerName);
            }
            $scope.sortOrder = 'prio';
            $scope.filters['userName'] = programmerNames;
        }, function (response) {
            console.log('Error retrieving project ' + $scope.project.name + ' issues');
        });
    };

    $scope.loadGrid = function () {
        if ($scope.action == 'issues') {
            $scope.getUserIssues();
        } else {
            $scope.getProjectIssues();
        }
    };

    $scope.unselectRow = function (index) {
        $scope.selectedIssues[index] = {};
    };

    $scope.markSelectedIssues = function () {
        var selIssues = [];
        for (var key in $scope.selectedIssues) {
            selIssues.push({ id: $scope.selectedIssues[key].id, state: $scope.newCurrentState});
        }
        ajaxCaller.post('https://localhost:44347/api/issues/issuesState', $scope.access_token, selIssues).then(function (response) {
            $scope.loadGrid();
            $scope.selectedIssues = {};
        }, function (response) {
            console.log('Error marking selected issues! ' + response.data);
        });
    };

    $scope.deleteSelectedIssues = function () {
            var ids = [];
            for (var key in $scope.selectedIssues) {
                ids.push($scope.selectedIssues[key].id);
            }
            ajaxCaller.delete('https://localhost:44347/api/issues/delete', $scope.access_token, ids).then(function (response) {
                $scope.loadGrid();
                $scope.selectedIssues = {};
            }, function (response) {
                console.log('Error deleting selected issues! ' + response.data);
            });
    };

    $scope.getIssueNotes = function (issueId) {
        ajaxCaller.get('https://localhost:44347/api/issues/issueNotes/', $scope.access_token, issueId).then(function (response) {
            $scope.issueNotes = response.data;
        }, function (response) {
            console.log('Error retrieving issue ' + issueId + ' notes!');
        });
    };

    $scope.saveIssueNotes = function (issueId, notes) {
        ajaxCaller.post('https://localhost:44347/api/issues/issueNotes', $scope.access_token, { issueId: issueId, notes: notes }).then(function (response) {
        }, function (response) {
            console.log('Error saving issue ' + issueId + ' notes!');
        });
    };

    $scope.toIssueDetails = function (issueId) {
        $location.path('/issues/issue/' + issueId);
    };

    $scope.toCreateIssue = function () {
        if ($scope.action == 'projectIssues') {
            $location.path('/projects/project/' + $scope.project.id + '/projectIssues/addIssue');
        } else {
            $location.path('/issues/addIssue');
        }
    };

    $scope.toEditIssue = function (issueId) {
        $location.path('/projects/project/' + $scope.project.id + '/projectIssues/editIssue/' + issueId);
    };

    $scope.toEditNotes = function (issueId, issueName) {
        $scope.selIssueToEditNotes.id = issueId;
        $scope.selIssueToEditNotes.name = issueName;
    };

    $scope.toProjectDetails = function (projectId) {
        $location.path('/projects/project/' + projectId);
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
