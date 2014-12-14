/*
*---utilities.js---
*Created by Nguyen Tran
*Created date: 2014.06.20
*Common utility functions
*/

function checkIsAbsoluteUrl(url) {
    var r = new RegExp('^(?:[a-z]+:)?//', 'i');
    return r.test(url);
}

function escapeHtml(s) {
    if (s != null && s.length > 0) {
        return s.split('&').join('&amp;')
            .split('<').join('&lt;')
            .split('"').join('&quot;');
    }
    return "";
}

function qualifyUrl(url) {
    var el = document.createElement('div');
    el.innerHTML = '<img src="' + escapeHtml(url) + '">';
    return el.firstChild.src;
}
function loadScriptFile(filename) {
    var fileref = document.createElement('script')
    fileref.setAttribute("type", "text/javascript")
    fileref.setAttribute("src", filename)
    if (typeof fileref != "undefined")
        document.getElementsByTagName("body")[0].appendChild(fileref)
}
function loadCssFile(filename, filetype) {
    var fileref = document.createElement("link")
    fileref.setAttribute("rel", "stylesheet")
    fileref.setAttribute("type", "text/css")
    fileref.setAttribute("href", filename)
    if (typeof fileref != "undefined")
        document.getElementsByTagName("head")[0].appendChild(fileref)
}
function appendBodyTag(tagName, id, attrName, attrValue) {
    if (id != null && id.length > 0) {
        var m = document.getElementById(id);
        if (m) {
            m.parentNode.removeChild(m);
        }
    }

    var fileref = document.createElement(tagName)
    if (id != null && id.length > 0) {
        fileref.setAttribute("id", id);
    }
    if (attrName != null && attrName.length > 0) {
        fileref.setAttribute(attrName, attrValue);
    }
    if (typeof fileref != "undefined" && document.getElementsByTagName("body").length > 0) {
        document.getElementsByTagName("body")[0].appendChild(fileref);
        return fileref;
    }
    return null;
}


function closePopup() {
    try {
        parent.jQuery.fancybox.close();
    }
    catch (err) { }
}
