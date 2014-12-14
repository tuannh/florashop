$(document).ready(function () {
    executeLazyLoad();
})

function executeLazyLoad() {
    if (typeof (jQuery.fn.customSort) != "undefined" &&
        typeof (LazyLoad) != "undefined") {
        cssLazyLoad();
        jsLazyLoad();
    }
    else{
        setTimemout(executeLazyLoad, 200);
    }
}

function cssLazyLoad() {
    var cssList = $('script[data-type=css][data-href]').customSort();

    if (cssList.length > 0) {

        var initOrder = cssList.eq(0).attr('data-order');
        var arrCss = new Array();

        for (var i = 0; i < cssList.length; i++) {
            if (cssList.eq(i).attr('data-order') == initOrder) {

                var href = cssList.eq(i).attr('data-href');
                arrCss.push(href);
                cssList.eq(i).attr('data-del', 1);
            }
        }

        if (window.console && console.log)
            console.log('css lazy: ' + $('script[data-type=css][data-del]').length + ', order: ' + initOrder);
        // del processed css from dom object
        $('script[data-type=css][data-del]').remove();

        LazyLoad.css(arrCss, function () {
            cssLazyLoad();
        })
    }
}

function jsLazyLoad() {
    // sort all lazy script
    var jsList = $('script[data-src]').customSort();

    if (jsList.length > 0) {

        var initOrder = jsList.eq(0).attr('data-order');
        var arrJs = new Array();

        for (var i = 0; i < jsList.length; i++) {
            if (jsList.eq(i).attr('data-order') == initOrder) {
                var src = jsList.eq(i).attr('data-src');
                arrJs.push(src);
                jsList.eq(i).attr('data-del', 1);
            }
        }

        if (window.console && console.log)
            console.log('js lazy: ' + $('script[data-src][data-del]').length + ', order: ' + initOrder);

        // del processed css from dom object
        $('script[data-src][data-del]').remove();

        LazyLoad.js(arrJs, function () {
            jsLazyLoad();
        })
    }
}