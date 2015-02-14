/*
    File: javascript/links.js
    
    Copyright 2011,
    Richard Rasala,
    College of Computer and Information Science
    Northeastern University, Boston, MA 02115
    rasala@ccs.neu.edu
    
    September 28, 2011
*/

/*
    The following must be loaded before loading this file:
        jQuery
        print.js
        queryparams.js
        storycore.js

    This Javascript drives links.htm.
*/


var $pagetitle;
var $linkslist;

var tag_title = "!title=";
var tag_pagetitle = "!pagetitle=";
var tag_heading = "!heading=";
var tag_comment = "!comment=";
var tag_br = "!br";
var tag_hr = "!hr";


function initializeTitle() {
    var i;

    for (var i = 0; i < storyLinesLength; i++) {
        var s = storyLines[i];

        if (s.length == 0)
            continue;

        var c = s.charAt(0);

        if ((c != BLANK) && (c != TAB))
            continue;

        var title = getTagContents(s, tag_title);

        if (title.length != 0) {
            $("title").text(title);
            return;
        }
    }
}


function initializePageTitle() {
    var i;

    for (var i = 0; i < storyLinesLength; i++) {
        var s = storyLines[i];

        if (s.length == 0)
            continue;

        var c = s.charAt(0);

        if ((c != BLANK) && (c != TAB))
            continue;

        var pagetitle = getTagContents(s, tag_pagetitle);

        if (pagetitle.length != 0) {
            var div = "<div class='title'>" + pagetitle + "</div>";
            $(div).appendTo($pagetitle);
        }
    }
}


function initializeLinksList() {
    var html = "";
    var index = 0;
    var i;

    for (var i = 0; i < storyLinesLength; i++) {
        var s = storyLines[i];

        if (s.length == 0)
            continue;

        var c = s.charAt(0);

        if ((c != BLANK) && (c != TAB)) {
            html += "<div class='list'>";
            html += "<a href='";
            html += storyList[index];
            html += "' target='_blank'>";
            html += titleList[index];
            html += "</a>";
            html += "</div>\n";

            index++;
        }
        else {
            if (isBreak(s)) {
                html += "<br />\n";
                continue;
            }

            if (isHorizontalRule(s)) {
                html += "<hr />\n";
                continue;
            }

            var heading = getTagContents(s, tag_heading);

            if (heading.length != 0) {
                html += "<div class='heading'>";
                html += heading;
                html += "</div>\n";
                continue;
            }

            var comment = getTagContents(s, tag_comment);

            if (comment.length != 0) {
                html += "<div class='comment'>";
                html += comment;
                html += "</div>\n";
                continue;
            }
        }
    }

    $(html).appendTo($linkslist);
}


function initializePage() {
    // Check errorMessage
    if (errorMessage.length > 0) {
        Print.print(errorMessage);
        return;
    }

    $pagetitle = $("#pagetitle");
    $linkslist = $("#linkslist");

    initializeTitle();
    initializePageTitle();
    initializeLinksList();
}


/*
    Searches for a the tag within the string s.
    If the tag is not found, returns "".

    Otherwise, returns the contents within s
    that follows the tag.

    Leading and trailing whitespace is removed.
*/
function getTagContents(s, tag) {
    if (!s || (s.length == 0))
        return "";

    if (!tag || (tag.length == 0))
        return "";

    var spot = s.search(tag);

    if (spot == -1)
        return "";
    
    var length = s.length;
    var size = tag.length;

    spot += size;

    if (spot >= length)
        return "";

    return s.substring(spot).trim();
}


function isBreak(s) {
    if (!s || (s.length == 0))
        return false;

    var spot = s.search(tag_br);

    return (spot != -1);
}


function isHorizontalRule(s) {
    if (!s || (s.length == 0))
        return false;

    var spot = s.search(tag_hr);

    return (spot != -1);
}