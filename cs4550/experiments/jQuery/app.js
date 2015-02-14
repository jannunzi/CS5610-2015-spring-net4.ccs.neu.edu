//console.log(jQuery);

$(function () {

    var title = "This is my Dynamic Title";
    $("#title").html(title);

//    alert("Hello from jQuery");
//    console.log("Document loaded");
    $("h1")
        .css({
            color: "red",
            "background-color": "yellow"
        });
    $("p#p2").remove();

    var p1 = $("p#p1");
    p1.css("color", "blue");
    var pclone = p1.clone();
    pclone.css("color", "red");

    $("body").append(pclone);



    var a = 1;
    console.log(a);

    // JSON
    var person = { fName: "Alice", lName: "Wonderland" };

    $("#fName").html(person.fName);
    $("#lName").html(person.lName);


    console.log(person);
    console.log(person.fName);
    var b = "lName";
    console.log(person[b]);
    person.middle = "G";
    console.log(person.middle);
    person["salary"] = "100000";
    console.log(person.salary);

    var genres = ["Drama", "Horror", "Comedy", "SciFi"];

    var ul = $("#genres");
    ul.empty();

    for (var i = 0; i < genres.length; i++)
    {
        var li = $("<li>" + genres[i] + "</li>");
        ul.append(li);
    }

    var employees = [
        { fName: "Bob", lName: "Dyllan" },
        { fName: "Charlie", lName: "Garcia" },
        { fName: "Dan", lName: "Akroyd" },
        { fName: "Edward", lName: "Norton" }
    ];

    var table = $("#employees");
    table.empty();
    for(var i in employees)
    {
        var emp = employees[i];
        var tr = $("<tr>");
        var td = $("<td>").append(emp.fName);
        tr.append(td);
        td = $("<td>").append(emp.lName);
        tr.append(td);
        table.append(tr);
        console.log(emp);
    }
});
