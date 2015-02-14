/*
    File: javascript/storycore.js
    
    Copyright 2011,
    Richard Rasala,
    College of Computer and Information Science
    Northeastern University, Boston, MA 02115
    rasala@ccs.neu.edu
    
    October 5, 2011
*/

/*
    The following must be loaded before loading this file:
        jQuery
        print.js
        queryparams.js
    

    This file sets:

        window.onload = initializeStoryData;


    Expectation:

    Some javascript file loaded after this file must define a
    callback function

        initializePage()

    The code in this file references initializePage() when as
    much processing of the story file has been done as possible.

    If the string errorMessage is non-empty then the function
    initializePage() should use Print.print to print
    errorMessage and then do no more processing.
*/


/*
    Initialize an empty story list of url's.
    Initialize a parallel list of titles.

    storyList holds the list of URLs for the story.
    titleList holds the titles for each URL.
    storyLength is the length of the above arrays.

    storyLines is the array of all lines from the story file.
    storyLinesLength is the length of this array.

    The basic story utility uses storyList, titleList, and
    storyLength.

    Other utilities may use information extracted from the
    lines that start with whitespace.  That is why the array
    storyLines is maintained.
*/

var storyList = [];

var titleList = [];

var storyLength = 0;

var storyLines;

var storyLinesLength;


/*
    These lists will be populated via a text file
    whose own url is supplied as the query string.
  
    Each non-empty line of the text file that does
    not begin with whitespace should have the form:
  
        url whitespace title-text
  
    Each url will be appended to storyList.
  
    Each title-text will be appended to titleList.
  
    If there is no title-text on a line then the url
    will be used as the title text.  It is strongly
    recommended that title-text be provided.
    
    Lines that are empty are ignored.
    
    Lines that start with whitespace are viewed as
    comments by the basic story utility but may be
    used by other utilities.
*/


/* The default story file name: story.txt */
var storyName = "story.txt";

/* The variable to hold the story list url. */
var storyListUrl = null;

/* The variable to hold the story list url minus its file name. */
var storyListUrlBase = null;

/* The variable to hold the story list data as a string. */
var storyListData = null;


/* Error messages. */

var errorMessage = "";

var errorStart = 
    "<p class='bold red'>Error in query path to story file</p>\n";

var cannotLoad =
    "<p class='bold red'>Cannot find story file</p>\n";

var noData =
    "<p class='bold red'>Story file is empty</p>\n";

var noURLs =
    "<p class='bold red'>Story file does not contain a line that starts with a URL</p>\n";

var notTextFile =
    "<p class='bold red'>Story file is not a text file with extension <code>.txt</code></p>\n";


/*
    Define RegExp objects to search for url's
    of type http or https.
*/

var httpRegExp = new RegExp("http:\/\/");
var httpsRegExp = new RegExp("https:\/\/");


/* Whitespace is BLANK or TAB. */

var BLANK = " ";
var TAB = "\t";


/*
    Find the url of the text file with the story list
    from the query params.

    The url may be supplied as one of:
        url=...
        The first unnamed entity in the query string.
   
    If the url is not supplied defaults to storyName.

    Calls QueryParams.Parse() at the start.
*/
function findStoryListUrl() {
    QueryParams.Parse();

    var url = QueryParams.Params["url"]
        || QueryParams.List[0];

    if (url) {
        if (url.charAt(url.length - 1) == "/") {
            url += storyName;
        }

        storyListUrl = url;
    }
    else {
        storyListUrl = storyName;
    }
}


/* Fetch the data at the story list url as a string. */
function fetchStoryListData() {
    var length = storyListUrl.length;

    var error = (length < 5)
        || (storyListUrl.substring(length - 4, length) != ".txt");

    if (error) {
        errorMessage += errorStart;
        errorMessage += notTextFile;
        errorMessage += Print.preHtmlEncode("  " + storyListUrl);

        initializePage();

        return;
    }
    
    var settings = {
        url: storyListUrl,
        success: processStoryListData,
        error: storyListFailure,
        dataType: "text",
        cache: false
    };

    $.ajax(settings);
}


/* Process the story list data. */
function processStoryListData(data) {
    if (!data) {
        errorMessage += errorStart;
        errorMessage += noData;
        errorMessage += Print.preHtmlEncode("  " + storyListUrl);

        initializePage();

        return;
    }

    // Get the array of all lines in the story file

    storyLines = Print.getLinesAsArray(data);
    storyLinesLength = storyLines.length;

    // Compute the story list url base, that is, the
    // relative urlfrom the story code directory to
    // the story text file directory
    //
    // This will be used to adjust a relative URL in
    // a story file to become relative to the story
    // code directory where it will be used.

    var last = storyListUrl.lastIndexOf("/");

    storyListUrlBase = (last < 0)
        ? ""
        : storyListUrl.substring(0, last + 1);

    // Extract the url's and corresponding title-text

    var i;

    for (i = 0; i < storyLinesLength; i++) {
        var s = storyLines[i];

        if (s.length == 0)
            continue;

        var c = s.charAt(0);

        if ((c == BLANK) || (c == TAB))
            continue;

        var url = getURL(s);
        var title = getTitleText(s);

        if (title.length == 0)
            title = url;

        storyList.push(url);
        titleList.push(title);
    }

    storyLength = storyList.length;

    if (storyLength == 0) {
        errorMessage += errorStart;
        errorMessage += noURLs;
        errorMessage += Print.preHtmlEncode("  " + storyListUrl);
    }

    initializePage();
}


/*
    Get the URL portion of a line s from a story file.

    Assumes the URL starts s and is terminated either
    by whitespace or by the end of s.
    
    If the URL does not begin with http:// or https://
    then storyListUrlBase will be prepended to the URL
    so the URL becomes relative to the story code
    directory.

    Returns an empty string if an error is detected. 
*/
function getURL(s) {
    if (!s || (s.length == 0))
        return "";

    var length = s.length;
    var position = 0;

    while (position < length) {
        var c = s.charAt(position);

        if (c == BLANK)
            break;

        if (c == TAB)
            break;

        position++;
    }

    if (position == 0)
        return "";
    
    var url = s.substring(0, position);

    // Adjust a relative url so it becomes relative
    // to the directory of the story web page which
    // is the location where it will be launched

    var a = url.search(httpRegExp);
    var b = url.search(httpsRegExp);

    if ((a != 0) && (b != 0)) {
        url = storyListUrlBase + url;
    }

    return url;
}


/*
    Get the title-text portion of a line s from a story
    file.

    Assumes the URL starts s and is terminated either
    by whitespace or by the end of s.
    
    The title-text is the portion of the line s after
    the initial whitespace and up to but not including
    any trailing whitespace.

    Returns an empty string if an error is detected or
    there is no title-text.
*/
function getTitleText(s) {
    if (!s || (s.length == 0))
        return "";

    var length = s.length;
    var position = 0;

    // Skip non-whitespace

    while (position < length) {
        var c = s.charAt(position);

        if (c == BLANK)
            break;

        if (c == TAB)
            break;

        position++;
    }

    if (position == 0)
        return "";

    // Skip whitespace

    while (position < length) {
        var c = s.charAt(position);

        if ((c != BLANK) && (c != TAB))
            break;

        position++;
    }

    if (position == length)
        return "";

    var stop = length;

    // Skip trailing whitespace
    while (stop > position) {
        var c = s.charAt(stop - 1);

        if ((c != BLANK) && (c != TAB))
            break;

        stop--;
    }

    return s.substring(position, stop);
}


/* Handle story list data failure. */
function storyListFailure() {
    errorMessage += errorStart;
    errorMessage += cannotLoad;
    errorMessage += Print.preHtmlEncode("  " + storyListUrl);

    initializePage();
}


/* Function to initialize data. */
function initializeStoryData() {
    findStoryListUrl();
    fetchStoryListData();
}


window.onload = initializeStoryData;
