Projestor.controller('UsersController', function ($scope, $location, sessionService, ajaxCaller) {
    $scope.init = function () {
        sessionService.checkSession();
        $location.url($location.path());
        $scope.role = sessionService.getRole();
        $scope.access_token = sessionService.getSessionJwt();
        $scope.users = [];
        $scope.selectedUsers = {};
        $scope.sortOrder = '';
        $scope.nameStart = '';
        $scope.selectedFilter = 'userName';
        $scope.sortOrders = [['id', true], ['userName', true], ['email', true], ['phoneNumber', true], ['accessFailedCount', true]];
        $scope.ascendingSortOrder = true;
        $scope.allSelected = false;
    };

    $scope.toggleUser = function (index, user) {
        if (!$scope.selectedUsers[index]) {
            $scope.selectedUsers[index] = user;
        } else {
            $scope.allSelected = false;
            delete $scope.selectedUsers[index];
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
            $scope.selectedUsers = [];
        }
    };

    $scope.noSelectedUsers = function () {
        var counter = 0;
        for (var key in $scope.selectedUsers) counter++;
        return counter == 0;
    };

    $scope.includeUser = function (index, user) {
        if ($scope.allSelected) $scope.selectedUsers[index] = user;
    };

    $scope.unselectRow = function (index) {
        $scope.selectedUsers[index] = {};
    };

    $scope.loadGrid = function () {
        ajaxCaller.get('https://localhost:44347/api/accounts/users', $scope.access_token).then(function (response) {
            $scope.users = response.data;
            $scope.sortOrder = 'userName';
        }, function (response) {
            console.log('Error retrieving users!');
        });
    };

    $scope.deleteSelectedUsers = function () {
        var ids = [];
        for (var key in $scope.selectedUsers) {
            ids.push($scope.selectedUsers[key].id);
        }
        ajaxCaller.delete('https://localhost:44347/api/accounts/deleteUsers', $scope.access_token, ids).then(function (response) {
            $scope.loadGrid();
            $scope.selectedUsers = {};
        }, function (response) {
            console.log('Error deleting selected issues! ' + response.data);
        });
    };

    $scope.toRegister = function () {
        $location.path('/register');
    };

    $scope.toEditUser = function (userName) {
        $location.path('/account/' + userName + '/editAccount');
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