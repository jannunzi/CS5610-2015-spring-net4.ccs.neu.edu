/*
    jQuery must be loaded before this Javascript file.

    Using fixed positioning, this tracker will place a small block
    in the lower right corner of the viewport that will show the
    width and height of the viewport.

    This block will be made visible when the page opens and when
    the page is resized.

    Click on the block to make it disappear.

    The package self initialzes.
*/


(function () {

    var $ = jQuery;

    var $window_size_tracker_div;
    var $window_size_tracker_w;
    var $window_size_tracker_h;
    var $window;

    var install_tracker = function() {
        var html = "<div id='window_size_tracker_div' "
        + "style='position:fixed; bottom:10px; right:10px; padding:0 10px; display:inline-block; border:1px solid black; background-color: white'>\n"
        + "<pre>w=<span id='window_size_tracker_w'></span>\n"
        + "h=<span id='window_size_tracker_h'></span></pre>\n"
        + "<p>Click to Close</p>\n"
        + "</div>\n";

        $("body").append(html);

        $window_size_tracker_div = $("#window_size_tracker_div");
        $window_size_tracker_w = $("#window_size_tracker_w");
        $window_size_tracker_h = $("#window_size_tracker_h");
        $window = $(window);
    }

    var show = function () {
        $window_size_tracker_w.html($window.width() + "");
        $window_size_tracker_h.html($window.height() + "");
        $window_size_tracker_div.show();
    }

    var hide = function () {
        $window_size_tracker_div.hide();
    }

    var initialize = function () {
        install_tracker();

        $window_size_tracker_div.on("click", hide);

        $window.on("resize", show);

        show();
    }

    $(initialize);
})();