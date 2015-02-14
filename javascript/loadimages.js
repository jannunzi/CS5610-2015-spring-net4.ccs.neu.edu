/*
    File: javascript/loadimages.js
    
    Copyright 2011,
    Richard Rasala,
    College of Computer and Information Science
    Northeastern University, Boston, MA 02115
    rasala@ccs.neu.edu
    
    October 5, 2011
*/

/*
    jQuery should be loaded before this Javascript is loaded.


    LoadImages is a class that manages the loading of images
    from a web directory.

    Constructor parameters:

    1. imageDirectoryURL

        A string with the relative or absolute path to the web
        directory that contains the images.
        
        The string may be an empty string if the images are
        located in the same directory as the web page itself.

        If the parameter is null, it is replaced by the empty
        string.

    2. imageListFileName

        A string with the name of a text file that contains the
        list of the images to load from the imageDirectoryURL.
        The text file will be loaded via an ajax call.

        The text file should consist of one image file name per
        line.  This data will be parsed to create an array of
        image file names that will be stored in imageNames.

        The string may be an empty string if it is desired to
        use the next constructor parameter to directly specify
        the image file name array imageNames.
        
        If the parameter is null, it is replaced by the empty
        string.

    3. imageNames
        
        This parameter should be an array of image file names in
        the directory specified by imageDirectoryURL.

        If imageListFileName is an empty string, then this array
        will be used to initialize the internal imageNames.

        If imageListFileName is non-trivial but the ajax call
        fails then this parameter will be used to initialize the
        internal imageNames.  This behavior can be useful during
        debugging.

        If the parameter is null, it is replaced by [].

    4. callback

        A reference to a callback function to be invoked when the
        loading process has gone as far as possible.

        The callback function should expect one parameter that we
        will refer to as "data" in the discussion below.

    5. max
    
        Maximum milliseconds to wait for all images to load before
        transfer to callback.  Default = 10000.
    
    6. gap

        Time gap between successive attempts to test whether all
        images are loaded.  Default = 100.

    Details:

    The images will be supplied as an array of image objects
    that has been constructed by the loading process as follows.
    
    For each image file name in imageNames, an image object will
    be created by setting the "src" attribute to the concatenation
    of the imageDirectoryURL and the image file name.

    The image object array:

        * is passed as the "data" to the callback function

        * is also available by the method call getImages()

    The image object array may be empty if no images can be loaded
    but this array is guaranteed not to be null.  In particular,
    if the length of this array is positive, then images have been
    loaded.

    There is no attempt to check the validity of individual image
    file names used in the construction process.

    The array of image file names is available by the method call
    getImageNames().  This may be used for debugging purposes.
*/


/* Constructor */
function LoadImages
    (imageDirectoryURL, imageListFileName, imageNames,
     callback, max, gap)
{
    if (imageDirectoryURL == null)
        imageDirectoryURL = "";

    if (imageListFileName == null)
        imageListFileName = "";

    if (imageNames == null)
        imageNames = [];

    this.imageDirectoryURL = imageDirectoryURL;
    this.imageListFileName = imageListFileName;
    this.imageNames = imageNames;
    this.callback = callback;
    this.max = max ? max : 10000;
    this.gap = gap ? gap : 100;

    this.images = [];

    this._load();
}


/*
    Returns the array of image objects.
*/
LoadImages.prototype.getImages = function() {
    return this.images;
}


/*
    Returns the array of image file names.
*/
LoadImages.prototype.getImageNames = function () {
    return this.imageNames;
}


/*
    Returns true if the complete property of all
    image objects is true.
*/
LoadImages.prototype.imagesLoaded = function () {
    var images = this.images;
    var length = images.length;

    if (length == 0)
        return true;

    var i;

    for (i = 0; i < length; i++) {
        if (!images[i].complete) {
            return false;
        }
    }

    return true;
}


/*
    Wait for the complete property of all images to become true.

    1. callback

        A reference to a callback function to be invoked when the
        loading process has gone as far as possible.

        The callback function should expect one parameter that we
        will refer to as "data" in the discussion below.

    2. max
    
        Maximum milliseconds to wait for all images to load before
        transfer to callback.  Default = 10000.
    
    3. gap

        Time gap between successive attempts to test whether all
        images are loaded.  Default = 100.

    The max property is set so that the application does
    not hang waiting for all images to load.

    The gap property is set to define the granularity of
    the testing.

    Since the images may fail to load in max milliseconds,
    the application must test if the images are loaded if
    that is crucial to the success of the application. 
*/
LoadImages.prototype.waitForImagesLoaded =
    function (callback, max, gap) {
        var self = this;

        if (self.imagesLoaded()) {
            if (callback) {
                callback(self.images);
            }

            return;
        }

        if (!max)
            max = 10000;

        if (!gap)
            gap = 100;

        var waiter = function () {
            if (self.imagesLoaded() || (max <= 0)) {
                if (callback) {
                    callback(self.images);
                }

                return;
            }

            max -= gap;

            setTimeout(waiter, gap);
        }

        setTimeout(waiter, gap);
    }


/*
    Load the file imageDirectoryURL + imageListFileName
    to obtain the image file names.

    Uses jQuery.ajax.
*/
LoadImages.prototype._load = function () {
    var imageListFileName = this.imageListFileName;

    if (imageListFileName.length > 0) {
        var imageDirectoryURL = this.imageDirectoryURL;
        var self = this;

        var ajaxData = {
            url: imageDirectoryURL + imageListFileName,
            dataType: "text",
            context: self,
            success: self._load_success,
            error: self._load_continuation,
            cache: false
        };

        jQuery.ajax(ajaxData);
    }
    else {
        this._load_continuation();
    }

}


/*
    Function to execute if the jQuery.ajax call in _load
    is a success.

    Extracts the image file names from the text file that
    was loaded.  Stores these name in imageNames.
*/
LoadImages.prototype._load_success = function (data) {
    if (data.length > 0) {
        // Replace default names
        this.imageNames = data.split("\n");

        // Eliminate possibility that last entry is empty
        var length = this.imageNames.length;

        if (this.imageNames[length - 1] == "")
            this.imageNames.length = length - 1;
    }

    this._load_continuation();
}


/*
    Function to execute to continue processing once
    the array imageNames is set.

    This may happen either through _load_success or
    through a constructor parameter.
*/
LoadImages.prototype._load_continuation = function () {
    var imageDirectoryURL = this.imageDirectoryURL;
    var imageNames = this.imageNames;
    var length = imageNames.length;
    var i, name, image;

    for (i = 0; i < length; i++) {
        name = imageNames[i];

        image = jQuery("<img />")
            .attr("src", imageDirectoryURL + name)
            .attr("alt", name)
            .attr("title", name)[0];

        this.images.push(image);
    }

    this.waitForImagesLoaded(this.callback, this.max, this.gap);
}
