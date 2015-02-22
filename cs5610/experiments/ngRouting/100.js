var app = angular.module("CourseApp", ["ngRoute"]);

app.config(['$routeProvider',
  function ($routeProvider) {
      $routeProvider.
        when('/home', {
            templateUrl: 'courseApp/home.html'
        }).
        when('/about', {
            templateUrl: 'courseApp/about.html'
        }).
        when('/contact', {
            templateUrl: 'courseApp/contact.html'
        }).
        when('/profile', {
            templateUrl: 'courseApp/profile.html'
        }).
        otherwise({
            redirectTo: '/home'
        });
  }]);