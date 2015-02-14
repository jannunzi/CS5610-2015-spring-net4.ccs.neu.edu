// jQuery wrappers
var $weburl;
var $create;
var $proxyurl;

function defineWrappers() {
    $weburl = $("#weburl");
    $create = $("#create");
    $proxyurl = $("#proxyurl");
}

function createProxyURL() {
    var weburl = $weburl.val();
    var proxyurl = "simpleproxy.aspx?url=|" + weburl + "|";

    $proxyurl.attr("href", proxyurl);
    $proxyurl.text(proxyurl);
}

function initialize() {
    defineWrappers();

    $create.click(createProxyURL);
}

$(initialize);