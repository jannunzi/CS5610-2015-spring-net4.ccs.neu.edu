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

app.controller("ProfileController", function ($scope, UserService, $routeParams, CourseService) {
    var username = $routeParams.username;
    $scope.username = username;
    $scope.currentUser = UserService.getCurrentUser();
    console.log($scope.currentUser);
    if($scope.currentUser.role == "faculty")
    {
        $scope.authoring = CourseService.getCourseByIndices($scope.currentUser.authoring);
        $scope.teaching = CourseService.getCourseByIndices($scope.currentUser.teaching);
    }
    else if ($scope.currentUser.role == "student") {
        $scope.registered = CourseService.getCourseByIndices($scope.currentUser.registered);
    }
    $scope.removeCourse = function(index)
    {
        console.log(index);
        CourseService.removeCourseByIndex(index);
    }
});

app.controller("NavController", function ($scope, UserService) {
    $scope.currentUser = null;
    $scope.logout = function () {
        $scope.currentUser = null;
        UserService.logout();
        $scope.username = null;
        $scope.password = null;
    }
    $scope.login = function () {
        var username = $scope.username;
        var password = $scope.password;
        $scope.currentUser = UserService.login(username, password);
    }
});

app.factory("CourseService", function () {

    var courses = [
            { title: "Java 101" },
            { title: "NodeJS 101" },
            { title: "AngularJS 101" },
            { title: "MongoDB 101" },
            { title: "ASP.NET" },
            { title: "C#" },
            { title: "Entity Framework" }
    ];

    var addCourse = function(course)
    {
        courses.push(course);
    }

    var getCourseByIndex = function (index) {
        return courses[index];
    }

    var getCourseByIndices = function (indices) {
        var responseCourses = [];
        for (var i in indices)
        {
            var course = courses[indices[i]];
            responseCourses.push(course);
        }
        return responseCourses;
    }

    return {
        addCourse: addCourse,
        getCourseByIndex: getCourseByIndex,
        getCourseByIndices: getCourseByIndices
    }

});

app.factory("UserService", function () {
    var currentUser = null;

    var users = [
        { username: "alice", password: "alice", role: "admin" },
        { username: "bob", password: "bob", role: "faculty", authoring: [1, 2, 3], teaching : [3,4,5] },
        { username: "charlie", password: "charlie", role: "student", registered: [2, 3, 4] },
        { username: "dan", password: "dan", role: "faculty", authoring: [1,3,5,6], teaching: [1,2,4,6] },
    ];

    var logout = function()
    {
        currentUser = null;
    }
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
        logout: logout,
        getCurrentUser: getCurrentUser
    };
});
