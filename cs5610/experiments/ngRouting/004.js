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
          , controller: 'ProfileCtrl'
      }).
      when('/details/:idIMDB', {
          templateUrl: 'movie/details.html'
          , controller: 'DetailsCtrl'
      }).
      otherwise({
          redirectTo: '/home'
      });
}]);

app.controller("HomeCtrl", function ($scope, MovieService) {
    $scope.searchMovies = function () {
        MovieService.search($scope.searchTitle, function (response) {
            $scope.movies = response;
        });
    }

    $scope.addToFavorites = function(movie)
    {
        MovieService.addToFavorites(movie);
    }
});

app.controller("ProfileCtrl", function ($scope, MovieService) {
    var fav = MovieService.getFavorites();
    console.log(fav);
    $scope.favoriteMovies = fav;
});
