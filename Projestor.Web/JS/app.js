var Projestor = angular.module('Projestor', ['ngRoute']);

Projestor.config(function($routeProvider, $locationProvider) {
    $routeProvider.when('/home', {
        templateUrl: '/templates/home.html',
        controller: 'HomeController'
    }).when('/users', {
        templateUrl: '/Templates/users.html',
        controller: 'UsersController'
    }).when('/projects', {
        templateUrl: '/Templates/projects.html',
        controller: 'ProjectsController'  
    }).when('/projects/createProject', {
        templateUrl: '/Templates/addOrEditProject.html',
        controller: 'AddOrEditProjectController' 
    }).when('/projects/project/:projectId/projectIssues', {
        templateUrl: '/Templates/issues.html',
        controller: 'IssuesController'
    }).when('/issues', {
        templateUrl: '/Templates/issues.html',
        controller: 'IssuesController'
    }).when('/issues/addIssue', {
        templateUrl: '/Templates/addOrEditIssue.html',
        controller: 'AddOrEditIssueController'
    }).when('/issues/issue/:issueId', {
        templateUrl: '/Templates/issueDetails.html',
        controller: 'IssueDetailsController'
    }).when('/projects/project/:projectId/projectIssues/addIssue', {
        templateUrl: '/Templates/addOrEditIssue.html',
        controller: 'AddOrEditIssueController'
    }).when('/projects/project/:projectId/projectIssues/editIssue/:issueId', {
        templateUrl: '/Templates/addOrEditIssue.html',
        controller: 'AddOrEditIssueController'
    }).when('/projects/project/:projectId/editProject', {
        templateUrl: '/Templates/addOrEditProject.html',
        controller: 'AddOrEditProjectController'
    }).when('/projects/project/:projectId', {
        templateUrl: '/Templates/projectDetails.html',
        controller: 'ProjectDetailsController'
    }).when('/login', {
        templateUrl: '/Templates/login.html',
        controller: 'LoginController'
    }).when('/register', {
        templateUrl: '/Templates/addOrEditAccount.html',
        controller: 'AddOrEditAccountController'
    }).when('/account/:userName/editAccount', {
        templateUrl: '/Templates/addOrEditAccount.html',
        controller: 'AddOrEditAccountController'
    }).when('/account/:userName/userDetails', {
        templateUrl: '/Templates/userDetails.html',
        controller: 'UserDetailsController'
    }).when('/account/changePassword', {
        templateUrl: '/Templates/changePassword.html',
        controller: 'ChangePasswordController'
    }).otherwise({
        redirectTo: '/home'
    })
    $locationProvider.html5Mode(true);

});

Projestor.run(function ($window, $location) {
    if ($window.localStorage.getItem('jwt')) {
        $location.path('/home');
    } else {
        $location.path('/login');
    }
});