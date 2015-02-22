var app = angular.module("MyFilmsApiApp", ["ngRoute"]);

app.config(['$routeProvider',
  function ($routeProvider) {
      $routeProvider.
        when('/home', {
            templateUrl: 'pages/home.html',
            controller: 'HomeController'
        }).
        when('/profile', {
            templateUrl: 'pages/profile.html',
            controller: 'ProfileController'
        }).
        when('/details/:index', {
            templateUrl: 'pages/details.html',
            controller: 'DetailsController'
        }).
        otherwise({
            redirectTo: '/home'
        });
  }]);


app.factory('MovieService', function MovieService($http) {

    var favorites = [];

    var searchForMovie = function(searchTitle, callback)
    {
        $http.jsonp("http://www.myapifilms.com/imdb?title=" + searchTitle + "&format=JSONP&aka=0&business=0&seasons=0&seasonYear=0&technical=0&filter=N&exactFilter=0&limit=10&lang=en-us&actors=N&biography=0&trailer=0&uniqueName=0&filmography=0&bornDied=0&starSign=0&actorActress=0&actorTrivia=0&movieTrivia=0&awards=0&moviePhotos=N&movieVideos=N&callback=JSON_CALLBACK")
        .success(callback);
    }

    var addToFavorites = function(movie)
    {
        favorites.push(movie);
    }

    var getFavorites = function()
    {
        return favorites;
    }

    return {
        searchForMovie: searchForMovie,
        addToFavorites: addToFavorites,
        getFavorites: getFavorites
    };
});

app.controller("DetailsController", function ($scope, $http, MovieService, $routeParams) {
    var index = $routeParams.index;
    favorites = MovieService.getFavorites();
    $scope.movie = favorites[index];
})

app.controller("ProfileController", function ($scope, $http, MovieService) {
    $scope.favorites = MovieService.getFavorites();
})

app.controller("HomeController", function ($scope, $http, MovieService) {
    $scope.search = function () {
        MovieService.searchForMovie($scope.searchTitle, function (movies) {
            console.log(movies);
            $scope.movies = movies;
        });
    }
    $scope.addToFavorites = function(movie)
    {
        MovieService.addToFavorites(movie);
    }
});

/*
app.controller("MyFilmsApiController",
function ($scope, $http)
{
    $scope.favorites = [];

    $scope.addToFavorites = function(movie)
    {
        $scope.favorites.push(movie);
    }

    $scope.searchTitle = function()
    {        
        $http.jsonp("http://www.myapifilms.com/imdb?title=" + $scope.titleSearch + "&format=JSONP&aka=0&business=0&seasons=0&seasonYear=0&technical=0&filter=N&exactFilter=0&limit=10&lang=en-us&actors=N&biography=0&trailer=0&uniqueName=0&filmography=0&bornDied=0&starSign=0&actorActress=0&actorTrivia=0&movieTrivia=0&awards=0&moviePhotos=N&movieVideos=N&callback=JSON_CALLBACK")
        .success(function (response) {
            $scope.movies = response;
        });
    }

});
*/