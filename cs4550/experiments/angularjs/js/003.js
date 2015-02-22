var app = angular.module("HelloApp", []);

app.controller("HelloController", function ($scope)
{
    $scope.hello = "Hello World";
    $scope.course = { title: "AngularJS 101", description: "Awesome" };
    $scope.courses = [
        { title: "Java 101", description: "Java Description" },
        { title: "C# 101", description: "C# Description" },
        { title: "JavaScript 101", description: "JavaScript Description" },
        { title: "MongoDB 101", description: "MongoDB Description" },
        { title: "NodeJS 101", description: "NodeJS Description" }
    ];

    $scope.favorites = [];

    $scope.favorite = function(course)
    {
        if ($scope.favorites.indexOf(course) >= 0)
        {
            return;
        }
        $scope.favorites.push(course);
        console.log($scope.favorites);
    }

    $scope.addCourse = function (newCourse)
    {
        var obj = {
            title: newCourse.title,
            description: newCourse.description
        };
        $scope.courses.push(obj);
    }

    $scope.removeCourse = function(course)
    {
        var index = $scope.courses.indexOf(course);
        $scope.courses.splice(index, 1);
    }
});
