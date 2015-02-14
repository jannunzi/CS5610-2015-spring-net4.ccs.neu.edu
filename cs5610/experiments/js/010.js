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

    var employees = [
        { fName: "Bob", lName: "Dylan" },
        { fName: "Charlie", lName: "Garcia" },
        { fName: "Dan", lName: "Akroyd" },
        { fName: "Edward", lName: "Norton" },
    ];

    var employeesDom = $("#employees");
    employeesDom.empty();
    for(var j in employees)
    {
        var e = employees[j];
        var tr = $("<tr>");
        var td = $("<td>");
        td.append(e.fName);
        tr.append(td);
        td = $("<td>");
        td.append(e.lName);
        tr.append(td);
        employeesDom.append(tr);
    }
});
