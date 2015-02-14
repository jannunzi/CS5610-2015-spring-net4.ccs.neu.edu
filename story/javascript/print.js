/*
    File: javascript/print.js
    
    Copyright 2011,
    Richard Rasala,
    College of Computer and Information Science
    Northeastern University, Boston, MA 02115
    rasala@ccs.neu.edu
    
    September 27, 2011
*/


/*
    This file is similar to the older file

        jQueryPrint.js

    but is independent of the jQuery namespace.

    This file uses an object Print for the namespace 
    within which all functions are defined.

    Keep in mind that this file does use jQuery so jQuery
    must be loaded before this file.

    This file provides 4 print functions that will print
    into div-blocks appended at the bottom of a web page:

        Print.print(message)
        Print.printEncoded(message)
        Print.printEncodedInPreTag(message)
        Print.printLines(message, start, stop)

    This file also exposes 5 helper functions that may be
    as important as the print functions:

        Print.htmlEncode(message)
        Print.pre(message)
        Print.preHtmlEncode(message)
        Print.getLines(s, start, stop)
        Print.getLinesAsArray(s, start, stop)

    The next comment describes all functions in more detail.
*/


/*
    The Print.print function is taken almost verbatim from
    jQuery 1.4 Reference Guide
    Karl Swedberg and Jonathan Chaffer
    PACKT Publishing
    pp. 260-261

    The Print.print function behaves as follows:

    Print.print(message)
        First the function searches for a wrapper div with the
        id 'print-output'.  If this wrapper does not exist, it
        is created at the end of the body.
    
        Then the message is placed in its own div with CSS class
        'output-message' and this div is appended to the wrapper.


    This file also has three more specialized print functions
    that are not in Swedberg and Chaffer:

    Print.printEncoded(message)
        HTML encode the message.
        Then pass the result to Print.print.
  
    Print.printEncodedInPreTag(message)
        HTML encode the message.
        Wrap the encoded message in a pre tag pair.
        Then pass the result to Print.print.
  
    Print.printLines(message, start, stop)
        Print the lines in the message starting at
            line = start up to but not including
            line = stop.

        To be consistent with end user conventions,
        the first line will have index 1.

        Print.printEncodedInPreTag will be used to
        print.
  

    This file also exposes its helper functions for other
    possible uses:

    Print.htmlEncode(message)
        Return the HTML encoded message.
  
    Print.pre(message)
        Return the message wrapped in a pre tag pair.
  
    Print.preHtmlEncode(message)
        Return Print.pre(Print.htmlEncode(message))
  
    Print.getLines(s, start, stop)
        Return the lines in the string s starting at
            line = start up to but not including
            line = stop.
        The return value is a substring of s.

    Print.getLinesAsArray(s, start, stop)
        Return the lines in the string s starting at
            line = start up to but not including
            line = stop.
        The return value is an array of strings.  Each
        string in the array represents one line in the
        string s.
*/


// Initialize the Print namespace object.

Print = {};


/*
    The foundation print function that appends
    the message as html in a div with class
    output-message in a wrapper div with class
    print-output.
*/
Print.print = function (message) {
    if (!message)
    return;

    if (typeof message != "string")
        message = message.toString();

    var $output = jQuery('#print-output');

    if ($output.length === 0) {
        $output = jQuery('<div id="print-output"></div>')
            .appendTo('body');
    }

    jQuery('<div class="output-message"></div>')
        .html(message)
        .appendTo($output);
}


/*
    The Print.printEncoded  function will:
        HTML encode the message.
        Then pass the result to Print.print.
*/
Print.printEncoded = function (message) {
    if (!message)
        return;

    if (typeof message != "string")
        message = message.toString();

    Print.print(Print.htmlEncode(message));
}


/*
    The Print.printEncodedInPreTag function will:
        HTML encode the message.
        Wrap the encoded message in a pre tag pair.
        Then pass the result to Print.print.
*/
Print.printEncodedInPreTag = function (message) {
    if (!message)
        return;

    if (typeof message != "string")
        message = message.toString();

    Print.print(Print.preHtmlEncode(message));
}


/*
    The Print.printLines function will:
    Print the lines in the message starting at
        line = start up to but not including
        line = stop.

    To be consistent with end user conventions,
    the first line will have index 1.

    Print.printEncodedInPreTag will be used to print.
*/
Print.printLines = function (message, start, stop) {
    if (!message)
        return;

    if (typeof message != "string")
        message = message.toString();

    Print.printEncodedInPreTag(Print.getLines(message, start, stop));
}


/*
    Return the HTML encoding of the given message.

    Replaces each &, <, >, " with their HTML encodings.
    All other characters pass through as is.

    Return "" if the given message is null, undefined,
    or empty.
*/
Print.htmlEncode = function (message) {
    if (!message)
        return "";

    if (typeof message != "string")
        message = message.toString();

    var encoding = "";
    var length = message.length;

    for (var index = 0; index < length; index++) {
        var c = message.charAt(index);

        if (c == "&")
            encoding = encoding + "&amp;";
        else if (c == "<")
            encoding = encoding + "&lt;";
        else if (c == ">")
            encoding = encoding + "&gt;";
        else if (c == '"')
            encoding = encoding + "&quot;";
        else
            encoding = encoding + c;
    }

    return encoding;
}


/*
    Return the message wrapped in a pre tag pair.

    Return "" if the given message is null, undefined,
    or empty.
*/
Print.pre = function (message) {
    if (!message)
        return "";

    if (typeof message != "string")
        message = message.toString();

    return "<pre>" + message + "</pre>";
}


/*
    Return Print.pre(Print.htmlEncode(message))
*/
Print.preHtmlEncode = function (message) {
    if (!message)
        return "";

    if (typeof message != "string")
        message = message.toString();

    return Print.pre(Print.htmlEncode(message));
}


/*
    Return the substring of s consisting of the
    lines with index i where start <= i < stop.

    To be consistent with end user conventions,
    the first line will have index 1.

    If start is undefined, set start = 1.

    If stop is undefined or stop <= 0, then
    return all lines starting at start.
*/
Print.getLines = function (s, start, stop) {
    if (!s || !(typeof s == "string") || (s.length == 0))
        return "";

    if (!start || !(typeof start == "number") || isNaN(start) || (start < 1))
        start = 1;

    if (!stop || !(typeof stop == "number") || isNaN(stop) || (stop < 1))
        stop = 0;

    if ((stop <= start) && (stop != 0))
        return "";

    var RETURN = "\r";
    var LINEFEED = "\n";

    var length = s.length;
    var position = 0;
    var line = 1;
    var c;

    // Find the first character of the line with index start
    while (line < start) {
        if (position >= length)
            break;

        c = s.charAt(position);

        if (c == RETURN) {
            line++;
            position++;

            if (position < length) {
                c = s.charAt(position);

                if (c == LINEFEED) {
                    position++;
                }
            }
        }
        else if (c == LINEFEED) {
            line++;
            position++;
        }
        else {
            position++;
        }
    }

    if (line < start)
        return "";

    if (stop == 0) {
        return s.substring(position);
    }

    var anchor = position;

    // Find the first character of the line with index stop
    while (line < stop) {
        if (position >= length)
            break;

        c = s.charAt(position);

        if (c == RETURN) {
            line++;
            position++;

            if (position < length) {
                c = s.charAt(position);

                if (c == LINEFEED) {
                    position++;
                }
            }
        }
        else if (c == LINEFEED) {
            line++;
            position++;
        }
        else {
            position++;
        }
    }

    return s.substring(anchor, position);
}


/*
    Return the array of lines in s consisting of
    the lines with index i where start <= i < stop.

    To be consistent with end user conventions,
    the first line will have index 1.

    If start is undefined, set start = 1.

    If stop is undefined or stop <= 0, then return
    the array of all lines starting at start.

    If no lines satisfy the conditions then the
    array that is return is empty.
*/
Print.getLinesAsArray = function (s, start, stop) {
    var array = [];

    if (!s || !(typeof s == "string") || (s.length == 0))
        return array;

    if (!start || !(typeof start == "number") || isNaN(start) || (start < 1))
        start = 1;

    if (!stop || !(typeof stop == "number") || isNaN(stop) || (stop < 1))
        stop = 0;

    if ((stop <= start) && (stop != 0))
        return array;

    var RETURN = "\r";
    var LINEFEED = "\n";

    var length = s.length;
    var position = 0;
    var line = 1;
    var c;
    var anchor;
    var last;

    // Find the first character of the line with index start
    while (line < start) {
        if (position >= length)
            break;

        c = s.charAt(position);

        if (c == RETURN) {
            line++;
            position++;

            if (position < length) {
                c = s.charAt(position);

                if (c == LINEFEED) {
                    position++;
                }
            }
        }
        else if (c == LINEFEED) {
            line++;
            position++;
        }
        else {
            position++;
        }
    }

    if (line < start)
        return array;

    // Find each line and push it onto the array
    while ((stop == 0) || (line < stop)) {
        if (position >= length)
            break;

        anchor = position;
        last = position;

        while (true) {
            last = position;

            if (position >= length)
                break;

            c = s.charAt(position);

            if (c == RETURN) {
                line++;
                position++;

                if (position < length) {
                    c = s.charAt(position);

                    if (c == LINEFEED) {
                        position++;
                    }
                }

                break;
            }
            else if (c == LINEFEED) {
                line++;
                position++;

                break;
            }
            else {
                position++;
            }
        }

        array.push(s.substring(anchor, last));
    }

    return array;
}