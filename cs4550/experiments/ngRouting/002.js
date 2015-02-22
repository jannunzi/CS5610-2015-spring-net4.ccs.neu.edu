var app = angular.module("MyFilmsApiApp", ["ngRoute"]);

app.config(['$routeProvider',
  function ($routeProvider) {
      $routeProvider.
        when('/home', {
            templateUrl: 'pages/home.html',
            controller: 'HomeController'
        }).
        when('/profile', {
            templateUrl: 'pages/profile.html'
        }).
        when('/details', {
            templateUrl: 'pages/details.html'
        }).
        otherwise({
            redirectTo: '/home'
        });
  }]);

app.controller("HomeController", function ($scope, $http) {
    $scope.search = function () {
        $http.jsonp("http://www.myapifilms.com/imdb?title="+$scope.searchTitle+"&format=JSONP&aka=0&business=0&seasons=0&seasonYear=0&technical=0&filter=N&exactFilter=0&limit=10&lang=en-us&actors=N&biography=0&trailer=0&uniqueName=0&filmography=0&bornDied=0&starSign=0&actorActress=0&actorTrivia=0&movieTrivia=0&awards=0&moviePhotos=N&movieVideos=N&callback=JSON_CALLBACK")
        .success(function (movies) {
            console.log(movies);
            $scope.movies = movies;
        });
    }
    var favorites = [];
    $scope.addToFavorites = function(movie)
    {
        favorites.push(movie);
        console.log(favorites);
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