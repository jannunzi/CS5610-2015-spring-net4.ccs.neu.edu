var app = angular.module("MovieApp", ['ngRoute']);

app.config(['$routeProvider',
function ($routeProvider) {
    $routeProvider.
      when('/home', {
          templateUrl: 'movie/home.html'
          //            ,controller: 'PhoneListCtrl'
      }).
      when('/profile', {
          templateUrl: 'movie/profile.html'
      }).
      when('/details', {
          templateUrl: 'movie/details.html'
      }).
      otherwise({
          redirectTo: '/home'
      });
}]);