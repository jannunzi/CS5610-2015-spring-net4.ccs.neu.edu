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
        when('/profile/:username', {
            templateUrl: 'courseApp/profile.html',
            controller: 'ProfileController'
        }).
        otherwise({
            redirectTo: '/home'
        });
  }]);

app.controller("ProfileController", function ($scope, UserService, $routeParams) {
    var username = $routeParams.username;
    $scope.username = username;
    console.log(username);
});

app.controller("NavController", function ($scope, UserService) {
    $scope.currentUser = null;
    $scope.login = function () {
        var username = $scope.username;
        var password = $scope.password;
        $scope.currentUser = UserService.login(username, password);
    }
});

app.factory("UserService", function () {
    var currentUser = null;

    var users = [
        { username: "alice", password: "alice" },
        { username: "bob", password: "bob" },
        { username: "charlie", password: "charlie" }
    ]

    var login = function (username, password) {
        for(var u in users)
        {
            if(users[u].username == username)
            {
                currentUser = users[u];
                return users[u];
            }
        }
        return null;
    };

    var getCurrentUser = function()
    {
        return currentUser;
    }

    return {
        login: login,
        getCurrentUser: getCurrentUser
    };
});
