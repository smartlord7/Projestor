Projestor.controller('IssueDetailsController', function ($scope, $window, $routeParams, $location, ajaxCaller, sessionService) {
    $scope.init = function () {
        sessionService.checkSession();
        $scope.access_token = sessionService.getSessionJwt();
        $location.url($location.path());
        $scope.issue = {};
        $scope.issue.id = $routeParams.issueId;
    };

    $scope.getIssue = function () {
        ajaxCaller.get('https://localhost:44347/api/issues/issue/', $scope.access_token, $scope.issue.id).then(function (response) {
            $scope.issue = response.data;
        }, function (response) {
            console.log('Issue with id ' + $scope.issue.id + ' could not be retrieved!');
        });
    };

    $scope.back = function () {
        $window.history.back();
    };

    $scope.main = function () {
        $scope.init();
        $scope.getIssue();
    };

    $scope.main();
});