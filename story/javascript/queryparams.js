/*
    File: javascript/queryparams.js
    
    Copyright 2010,
    Richard Rasala,
    College of Computer and Information Science
    Northeastern University, Boston, MA 02115
    rasala@ccs.neu.edu
    
    September 16, 2010
*/


/*
Adapted from a discussion on Parsing URLs
David Flanagan, Javascript: The Definitive Guide,
5th Edition, pp. 272-273.

Creates a namespace variable QueryParams.

Call QueryParams.Parse() to parse the query params.

As a result of this call the following items will
be created.


>>> QueryParams.String

If there are non-empty query params, then this string
holds the original query string minus the leading ?

Otherwise this string is "".


>>> QueryParams.Params

This acts as a property list and captures name=value
pairs among the query items.


>>> QueryParams.List

Query items that are not name=value pairs are put
into this array in order.


The data is populated as follows.

The original query string is split by "&" into items.

For each item, a split is attempted by "=".  If this
split leads to two non-empty strings, we consider
this as a pair:

    name=value

In that case, we set

    QueryParams.Params[name] = value;

No attempt is made to test if a name occurs more than
once.  If this happens the last value is the one that
is stored.


If the split by "=" fails then we put the item into a
new cell in the array QueryParams.List.


Unlike Flanaghan, we do not sanitize the strings in any
way.  Such decisions are left to the caller.
*/


// Create a QueryParams namespace
var QueryParams = {}


// Parse the query parameters
QueryParams.Parse = function() {
    var s = location.search;

    QueryParams.String = s ? s.substring(1) : "";

    QueryParams.Params = {};
    QueryParams.List = [];

    if (!QueryParams.String)
        return;

    var items = QueryParams.String.split("&");
    var count = items.length;

    for (var i = 0; i < count; i++) {
        var item = items[i];

        if (!item)
            continue;

        var pairs = item.split("=");

        if (pairs.length == 2) {
            if (pairs[0] && pairs[1]) {
                QueryParams.Params[pairs[0]] = pairs[1];
                continue;
            }
        }

        QueryParams.List[QueryParams.List.length] = item;
    }
}
