$(function () {
    $(document).on("click", ".downloadbutton",
        function () {
            var nextdata = $(this).next()[0].innerHTML;
            document.location = "download.aspx?" + nextdata;
        });
});
