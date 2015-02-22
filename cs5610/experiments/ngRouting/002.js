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

app.controller("HomeCtrl", function ($scope, $http) {
    $scope.searchMovies = function () {
        var title = $scope.searchByTitle;
        $http.jsonp("http://www.myapifilms.com/imdb?title=" + title + "&format=JSONP&aka=0&business=0&seasons=0&seasonYear=0&technical=0&filter=N&exactFilter=0&limit=10&lang=en-us&actors=N&biography=0&trailer=0&uniqueName=0&filmography=0&bornDied=0&starSign=0&actorActress=0&actorTrivia=0&movieTrivia=0&awards=0&moviePhotos=N&movieVideos=N&callback=JSON_CALLBACK")
        .success(function (response) {
            $scope.movies = response;
        })
    }

    $scope.favoriteMovies = [];

    $scope.addToFavorites = function (movie) {
        $scope.favoriteMovies.push(movie);
    }

});
