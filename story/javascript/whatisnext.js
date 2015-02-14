/*
    File: javascript/whatisnext.js
    
    Copyright 2011,
    Richard Rasala,
    College of Computer and Information Science
    Northeastern University, Boston, MA 02115
    rasala@ccs.neu.edu
    
    October 26, 2011
*/

/*
    The following must be loaded before loading this file:
        jQuery
        print.js
        queryparams.js
        storycore.js

    This Javascript drives whatisnext.htm.
*/


var $linkslist;


function initializeLinksList() {
    var html = "";
    var whatisnextRegExp = /whatisnext\.htm/;

    for (var i = 0; i < storyLength; i++) {
        if (i == 0) {
            if (storyList[0].search(whatisnextRegExp) >= 0)
                continue;
        }

        html += "<div class='list'>";
        html += "<a href='";
        html += storyList[i];
        html += "' target='_blank'>";
        html += titleList[i];
        html += "</a>";
        html += "</div>\n";
    }

    $(html).appendTo($linkslist);
}

function initializePage() {
    // Check errorMessage
    if (errorMessage.length > 0) {
        Print.print(errorMessage);
        return;
    }

    $linkslist = $("#linkslist");

    initializeLinksList();
}
