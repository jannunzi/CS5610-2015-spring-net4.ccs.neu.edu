/*
    File: expandable.js
        
    Copyright 2010,
    Richard Rasala,
    College of Computer and Information Science
    Northeastern University, Boston, MA 02115
    rasala@ccs.neu.edu
    
    September 25, 2010
*/

/*
    jQuery must be loaded before loading this file.
    
    When jQuery determines that the document is ready,
    the function loadExpandable will be called to
    install the plus/minus icons and the behavior.
    
    See expandableX.js if you need to delay loading of
    this Javascript because expandable sections are
    being created dynamically by other Javascript.
*/

/*
    Conventions in the associated HTML.
    
    
    Each expandable/collapsible section should be enclosed
    in a <div> tag or a <span> tag.
    
    
    The header for the expandable/collapsible section is the
    HTML entity that immediately precedes the section.
    
    The header should be a simple tag that contains only text
    and that has CSS class "expandable".  It is fine if the
    header has other CSS class names as well.
    
    The header will have the plus/minus icons inserted that
    will then control the expand/collapse behavior.
    

    A button with class "expandbutton" will expand all sections.
    
    A button with class "collapsebutton" will collapse all sections.
    
    
    A button with class "collapsesectionbutton" will collapse the
    section that contains it.  Place such a button at the end of
    section within a <p> tag or <div> tag.  The button definition
    should be:
    
        <input type="button" class="collapsesectionbutton" />

    The button text will be dynamically computed from the text of
    the header that immediately precedes the enclosing section.
*/


var Expandable = {};


Expandable.loadExpandable = function() {
    // The collapse section buttons must be initialized
    // before the headers for the expandable sections
    // have been altered with the plus/minus icons.
    jQuery(".collapsesectionbutton").each(Expandable.collapsesectionInit);
    jQuery(".expandable").each(Expandable.expandableInit);

    // Now add the click behavior.
    jQuery(".expandbutton").click(Expandable.expandall);
    jQuery(".collapsebutton").click(Expandable.collapseall);
    jQuery(".collapsesectionbutton").click(Expandable.collapsesection);

    // Collapse all to start.
    Expandable.collapseall();
};


Expandable.expandableInit = function(index) {
    this.innerHTML = "<span></span> " + this.innerHTML;
    jQuery("> span:nth-child(1)", this).click(Expandable.expandorcollapse);
};


Expandable.expandone = function(index) {
    var wrap = jQuery(this);
    wrap.addClass("collapsenode")
        .removeClass("expandnode")
        .attr("title", "collapse");
    wrap.parent().next().show();
};


Expandable.collapseone = function(index) {
    var wrap = jQuery(this);
    wrap.addClass("expandnode")
        .removeClass("collapsenode")
        .attr("title", "expand");
    wrap.parent().next().hide();
};


Expandable.expandorcollapse = function() {
    var wrap = jQuery(this);
    if (wrap.hasClass("expandnode")) {
        wrap.addClass("collapsenode")
            .removeClass("expandnode")
            .attr("title", "collapse");
        wrap.parent().next().show();
    }
    else if (wrap.hasClass("collapsenode")) {
        wrap.addClass("expandnode")
            .removeClass("collapsenode")
            .attr("title", "expand");
        wrap.parent().next().hide();
    };
}


Expandable.expandall = function() {
    jQuery(".expandable > span:nth-child(1)").each(Expandable.expandone);
};


Expandable.collapseall = function() {
    jQuery(".expandable > span:nth-child(1)").each(Expandable.collapseone);
};


Expandable.collapsesection = function() {
    var wrap = jQuery(this);

    var section = wrap.parent().parent();
    section.hide();

    var node = jQuery("> span:nth-child(1)", section.prev());
    node.addClass("expandnode")
        .removeClass("collapsenode")
        .attr("title", "expand");

    var nodeoffset = node.offset().top - 10;
    var scrolltop = $(document).scrollTop();

    if (scrolltop > nodeoffset) {
        $(document).scrollTop(nodeoffset);
    }
};


Expandable.collapsesectionInit = function() {
    var wrap = jQuery(this);
    var header = wrap.parent().parent().prev();
    var text = "Collapse: " + header.text();
    wrap.attr("value", text);
};


/* Load this code once the page is loaded. */
jQuery(Expandable.loadExpandable);