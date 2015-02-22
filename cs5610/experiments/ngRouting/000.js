var app = angular.module("RoutingApp", ['ngRoute']);

app.controller("RoutingController", function ($scope) {
    $scope.hello = "Hello from RoutingController";
});

app.config(['$routeProvider',
  function($routeProvider) {
      $routeProvider.
        when('/home', {
            templateUrl: 'partials/home.html'
//            ,controller: 'PhoneListCtrl'
        }).
        when('/about', {
            templateUrl: 'partials/about.html'
        }).
        when('/contact', {
            templateUrl: 'partials/contact.html'
        }).
        otherwise({
            redirectTo: '/home'
        });
  }]);