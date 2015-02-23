var app = angular.module("FormsApp", []);

app.controller("FormsController", function ($scope) {
    $scope.form = {
        id: "123",
        name: "Person",
        buttons: [
            { label: "Save", type: "save", clas: "btn-primary" },
            { label: "Cancel", type: "submit", clas: "btn-danger" },
            { label: "Ok", type: "submit", clas: "btn-success" }
        ],
        fields : [
            { name: "firstName", label: "First Name", type: "text", placeholder:"First Name" },
            { name: "lastName", label: "Last Name", type: "text", placeholder:"Last Name" },
            { name: "email", label: "Email", type: "email", placeholder:"someone@somewhere.com" },
            { name: "dateOfBirth", label: "Date of Birth", type: "date" },
            {
                name: "favoriteGenre", label: "Favorite", type: "select",
                options: [
                    { value: "HORROR", label: "Horror" },
                    { value: "COMEDY", label: "Comedy" }
                ]
            },
            {
                name: "gender", label: "Gender", type: "radios",
                options: [
                    { value: "MALE", label: "Male" },
                    { value: "FEMALE", label: "Female" }
                ]
            },
            {
                name: "favoriteSports", label: "Favorite Sports", type: "checkboxes",
                options: [
                    { value: "BASEBALL", label: "Baseball" },
                    { value: "FOOTBALL", label: "Footbal" }
                ]
            }
        ]
    };

    $scope.buttonClick = function(button)
    {
        console.log($scope.form);
//        console.log(button);
    }

    $scope.removeField = function(index)
    {
        $scope.form.fields.splice(index, 1);
    }
});

Array.prototype.move = function (old_index, new_index) {
    if (new_index >= this.length) {
        var k = new_index - this.length;
        while ((k--) + 1) {
            this.push(undefined);
        }
    }
    this.splice(new_index, 0, this.splice(old_index, 1)[0]);
    return this; // for testing purposes
};

/*
            {
                cols: [
                    { content: "C11", span: "col-xs-4" },
                    { content: "C12", span: "col-xs-4" },
                    { content: "C13", span: "col-xs-4" },
                ]
            }
*/