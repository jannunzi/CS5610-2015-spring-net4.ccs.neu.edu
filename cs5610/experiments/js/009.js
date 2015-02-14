$(function () {
    var mainTitle = "This is a dynamice title";
    $("#mainTitle").html(mainTitle);

    var genres = ["Horror", "Comedy", "Drama"];

    var ul = $("ul#genres");
    ul.empty();
    for(var i in genres)
    {
        var li = $("<li>" + genres[i] + "</li>");
        ul.append(li);
    }

    var employee = {
        fName: "Alice",
        lName: "Wonderland"
    };

    var employeeDom = $("#employee");
    console.log(employeeDom);
    var fName = employeeDom.find("#fName");
    var lName = employeeDom.find("#lName");
    console.log(fName);
    fName.html(employee.fName);
    lName.html(employee.lName);
});
