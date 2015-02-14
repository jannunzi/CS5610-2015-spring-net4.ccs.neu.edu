var app = angular.module("HelloWorldApp", []);

app.controller("HelloWorldController",
function ($scope) {

    var employee = {
        first: "Alice",
        last: "Wonderland"
    };

    $scope.employee = employee;

    var employees = [
        { first: "Bob", last: "Dylan" },
        { first: "Jerry", last: "Garcia" },
    ];

    $scope.employees = employees;

    $scope.hello = "Hello From Angular JS";
    console.log("HelloWorldController");
});