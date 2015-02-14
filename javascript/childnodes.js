// The code below drills down into the child nodes of an XML
// element node by directly matching the tag name.
//
// This code is needed because jQuery does NOT work properly
// with XML namespaced tags.
//
// All hacks suggested in web blogs to get around this fault
// in jQuery seem too fragile to use.

// Returns an array of child nodes of the given xmlNode that
// have the given tagName

function getChildNodesByTagName(xmlNode, tag) {
    // Warning!
    // Do not use a "for in" style loop with the childNodes
    // object that is associated with a node.
    // This kind of loop fails.
    // Write the loop in the classic fashion.

    var result = [];

    if (!xmlNode) {
        return result;
    }

    var length = xmlNode.childNodes.length;

    for (var i = 0; i < length; i++) {
        child = xmlNode.childNodes[i];

        if (child.nodeType == 1) {
            if (child.tagName == tag)
                result.push(child);
        }
    }

    return result;
}
