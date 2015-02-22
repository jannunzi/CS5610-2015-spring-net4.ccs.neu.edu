var app = angular.module("MovieApp", ['ngRoute']);

app.config(['$routeProvider',
function ($routeProvider) {
    $routeProvider.
      when('/home', {
          templateUrl: 'movie/home.html'
          ,controller: 'HomeCtrl'
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

app.controller("HomeCtrl", function ($scope, $http, MovieService) {
    $scope.searchMovies = function () {
        MovieService.search($scope.searchTitle, function (response) {
            $scope.movies = response;
        });
    }
});
