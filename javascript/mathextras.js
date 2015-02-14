/*
    File: javascript/mathextras.js
    
    Copyright 2011,
    Richard Rasala,
    College of Computer and Information Science
    Northeastern University, Boston, MA 02115
    rasala@ccs.neu.edu
    
    August 11, 2011
*/


/*
    Introduces extras to the built-in Math class
    using a class MathExtras.

    Includes:

      * trunc function
      * conversions between degrees and radians 
      * assorted random functions
      * assorted permutation functions
*/


MathExtras = {};


/*
    Returns x rounded towards 0.
*/
MathExtras.trunc = function (x) {
    return (x >= 0) ? Math.floor(x) : Math.ceil(x);
}


MathExtras.degrees_to_radians = Math.PI / 180;

MathExtras.radians_to_degrees = 180 / Math.PI;


/*
    Convert the degrees parameter to radians.
*/
MathExtras.radians = function (degrees) {
    return degrees * MathExtras.degrees_to_radians;
}


/*
    Convert the radians parameter to degrees.
*/
MathExtras.degrees = function (radians) {
    return radians * MathExtras.radians_to_degrees;
}


/*
    Returns x * Math.random().

    Therefore:
    Inclusive of 0
    Exclusive of x unless x == 0
*/
MathExtras.random = function (x) {
    return x * Math.random();
}


/*
    Returns a + MathExtras.random(b - a).

    Therefore:
        Inclusive of a
        Exclusive of b unless b == a
*/
MathExtras.randomBetween = function (a, b) {
    return a + MathExtras.random(b - a);
}


/*
    Replaces x by MathExtras.trunc(x).
    This will do nothing if x is an integer.

    Then returns MathExtras.trunc(x * Math.random()).
*/
MathExtras.randomInt = function (x) {
    x = MathExtras.trunc(x);

    return MathExtras.trunc(x * Math.random());
}


/*
    Replaces a by MathExtras.trunc(a).
    Replaces b by MathExtras.trunc(b).
    This will do nothing if a and b are integers.

    Then returns a + MathExtras.trunc((b - a) * Math.random()).
*/
MathExtras.randomIntBetween = function (a, b) {
    a = MathExtras.trunc(a);
    b = MathExtras.trunc(b);

    return a + MathExtras.trunc((b - a) * Math.random());
}


/*
    Returns the array with start, (start + 1), ..., (start + n - 1).

    Assumes n is an integer and start is a number.
    If start is omitted then start is assumed to be 0.
*/
MathExtras.sequence = function (n, start) {
    if (start == null)
        start = 0;

    var data = new Array(n);
    var i;

    for (i = 0; i < n; i++)
        data[i] = start + i;

    return data;
}


/*
    Returns a random permutation of the array with
        start, (start + 1), ..., (start + n - 1).

    Assumes n is an integer and start is a number.
    If start is omitted then start is assumed to be 0.
*/
MathExtras.permutation = function (n, start) {
    var data = MathExtras.sequence(n, start);
    var i, k, m, temp;

    for (i = n; i > 1; i--) {
        m = i - 1;
        k = MathExtras.randomInt(i);

        if (k != m) {
            temp = data[m];
            data[m] = data[k];
            data[k] = temp;
        }
    }

    return data;
}


/*
    Returns a random even permutation of the array with
    start, (start + 1), ..., (start + n - 1).

    Assumes n is an integer and start is a number.
    If start is omitted then start is assumed to be 0.
*/
MathExtras.evenPermutation = function (n, start) {
    var data = MathExtras.sequence(n, start);
    var swap = 0;

    var i, k, m, temp;

    for (i = n; i > 2; i--) {
        m = i - 1;
        k = MathExtras.randomInt(i);

        if (k != m) {
            temp = data[m];
            data[m] = data[k];
            data[k] = temp;

            swap = (swap == 0) ? 1 : 0;
        }
    }

    if (swap == 1) {
        temp = data[1];
        data[1] = data[0];
        data[0] = temp;
    }

    return data;
}


/*
    Expects array of items comparable by >.

    Returns -1 if any array cells have equal values.

    Otherwise returns the count of pair (i, j) with
        i < j and data[i] > data[j]
*/
MathExtras.reversals = function (data) {
    var n = data.length;

    if (n <= 1)
        return 0;

    var m = n - 1;
    var r = 0;

    var i, j;
    for (i = 0; i < n; i++) {
        for (j = (i + 1); j < n; j++) {
            if (data[i] == data[j])
                return -1;

            if (data[i] > data[j])
                r++;
        }
    }

    return r;
}
