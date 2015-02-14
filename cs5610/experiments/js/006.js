$(function () {
    console.log("Hello from jQuery");
    $("#second").remove();
    $("p").append(" New Content");
    $("h1").html("Append, Remove, and Change Content");
    $("body").append("<p>This is a new paragraphs</p>");
    $("body").prepend("<p>This is a new paragraphs</p>");

    var newParagraph = $("<p>New Paragraph</p>");
    $("body").prepend(newParagraph);
    newParagraph.css('background-color', "green");
});
