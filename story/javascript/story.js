/*
    File: javascript/story.js
    
    Copyright 2012,
    Richard Rasala,
    College of Computer and Information Science
    Northeastern University, Boston, MA 02115
    rasala@ccs.neu.edu
    
    January 27, 2012
*/

/*
The following must be loaded before loading this file:
    jQuery
    print.js
    queryparams.js
    storycore.js
*/


/*
    These Javascript variables correspond to the
    items in the body with the same id.
    
    $window is the jQuery wrapper for window.
*/

var $window;

var $pageHead;

var $storyFrame;
var storyFrame;

var $backArrow;
var $nextArrow;

var newtabLink;

var $slideDropdown;
var slideDropdown;

var $slideTotal;


/*
Error messages.
*/

var errorAlert =
    "<span class='bold red'>Error</span>";

var instructions =
    "<hr /><p>Go to <a href='index.htm'>Story Utility Instructions</a></p>";


/*
    The initial slide number is -1
    to show that no slide is initially selected.
*/
var slideNumber = -1;


/*
    Initialize Javascript variables that correspond
    to items in the body.

    Also initialize $window.
*/
function initializeVariables() {
    $window = $(window);

    $pageHead = $("#pageHead");

    $storyFrame = $("#storyFrame");
    storyFrame = $storyFrame[0];

    $backArrow = $("#backArrow");
    $nextArrow = $("#nextArrow");

    newtabLink = $("#newtabLink")[0];

    $slideDropdown = $("#slideDropdown");
    slideDropdown = $slideDropdown[0];

    $slideTotal = $("#slideTotal");
}


/*
    Show the slide with the given number.
  
    By modular arithmetic, ensure that:
        0 <= number < storyLength
  
    If the storyList was never populated
    then this method will do nothing.
*/
function showSlide(number) {
    if (storyLength <= 0)
        return;
        
    number = number % storyLength;

    if (number < 0)
        number += storyLength;

    slideNumber = number;

    storyFrame.src = storyList[number];
    newtabLink.href = storyList[number];
    slideDropdown.selectedIndex = number;
}


/* Go back one slide */
function backSlide() { showSlide(slideNumber - 1); }


/* Go to next slide */
function nextSlide() { showSlide(slideNumber + 1); }


/*
    Initialize the slide dropdown menu
*/
function initializeSlideDropdown() {
    var html = "";

    for (var i = 0; i < storyLength; i++) {
        html += "<option>" + (i+1) + ": " + titleList[i] + "</option>\n";
    }

    $slideDropdown.append(html);
}


/*
    Function that encapsulates the slide dropdown behavior.
*/
function slideDropdownBehavior() {
    showSlide(slideDropdown.selectedIndex);
}


/*
    Function to resize the slide frame height so it fills
    most of the available space in the browser window.
  
    The internal gap value allows for some spacing between
    the bottom of the iframe and the bottom of the window.

    Also sets the width of slide frame to the width of the
    page.  One would think that this would be automatic
    but some mobile device browsers do not do this.
*/
function resizeStoryFrame() {
    var h = $window.height();

    var h1 = $pageHead.height();

    var gap = 30;

    var h2 = h - h1 - gap;

    $storyFrame.height(h2);

    $storyFrame.width($pageHead.width());
}


/*
    Function to initialize page.
*/
function initializePage() {
    // Initialize variables that correspond to
    // DOM objects in the user interface plus
    // $window.
    initializeVariables();

    // Check errorMessage
    if (errorMessage.length > 0) {
        Print.print(errorMessage);
        Print.print(instructions);
        return;
    }

    // Provide the link behavior for the arrows.
    $backArrow.click(backSlide);
    $nextArrow.click(nextSlide);

    // Initialize the slide dropdown menu
    initializeSlideDropdown();

    // Provide the behavior for the slide dropdown menu
    slideDropdown.onchange = slideDropdownBehavior;
    
    // Do the initial sizing of the story frame.
    resizeStoryFrame();

    // Show the total number of slides
    $slideTotal.text("" + storyLength);

    // Initialize by showing slide 0 or show the slide
    // corresponding to the query parameter start=N.
    //
    // Keep in mind that user indexing starts at 1 so
    // we need to adjust here by subtracting 1.

    var start = 0;

    if (QueryParams.Params["start"]) {
        start = parseInt(QueryParams.Params["start"]);

        if (isNaN(start))
            start = 0;
        else
            start--;
    }

    showSlide(start);

    // Set up auto resize on the story frame.
    window.onresize = resizeStoryFrame;
}
