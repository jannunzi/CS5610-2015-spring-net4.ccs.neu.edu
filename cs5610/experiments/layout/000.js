var app = angular.module("LayoutApp", []);

app.controller("LayoutController", function ($scope) {
    $scope.layout = {
        rows: [
            {
                cols: [
                    { content: "C11", span: 4 },
                    { content: "C12", span: 4 },
                    { content: "C13", span: 4 },
                ]
            }
        ]
    };
    $scope.addRow = function()
    {
        var newRow =
        {
            cols: [
                { content: "C"+$scope.layout.rows.length, span: 4 },
                { content: "C12", span: 4 },
                { content: "C13", span: 4 }
            ]
        };

        $scope.layout.rows.push(newRow);
    }

    $scope.removeCol = function(cols, index)
    {
        cols.splice(index, 1);
    }

    $scope.widenCol = function (col) {
        col.span++;
    }

    $scope.shrinkCol = function (col) {
        col.span--;
    }

    var selectedRow = null;
    var selectedRowIndex = null;
    var selectedCol = null;

    $scope.select = function (row) {
        selectedRow = row;
    }
    $scope.addColumn = function(row)
    {
        var newCol = { content: "New", span: 2 };
        row.cols.push(newCol);
    }
    $scope.removeRow = function(index)
    {
        $scope.layout.rows.splice(index, 1);
    }
    $scope.moveRowUp = function (index) {
        if (index > 0)
        {
            $scope.layout.rows.move(index, index - 1);
        }
    }
    $scope.moveRowDown = function (index) {
        if (index < $scope.layout.rows.length - 1) {
            $scope.layout.rows.move(index, index + 1);
        }
    }
    $scope.hoverIn = function (col) {
        col.edit = true;
    }
    $scope.hoverOut = function (col) {
        col.edit = false;
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