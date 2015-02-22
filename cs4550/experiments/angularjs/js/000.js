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
});
