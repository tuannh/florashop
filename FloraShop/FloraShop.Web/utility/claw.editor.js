/*
*---claw.editor.js---
*Created by Nguyen Tran
*Created date: 2014.06.25
*jQuery plugin - Custom ckeditor for claw3
*/

(function ($) {

    // integrate ckfinder to ckeditor
    if (typeof (CKFinder) != 'undefined')
        CKFinder.setupCKEditor(null, '/utility/ckfinder/');

    $.fn.clawEditor = function (toolbar, customConfig) {
        try {
            var supprtToolbars = ["simple", "basic", "standard", "full"];
            var loadToolbar = "standard";

            for (var i = 0; i < supprtToolbars.length; i++) {
                if (toolbar.toLowerCase() == supprtToolbars[i]) {
                    loadToolbar = supprtToolbars[i];
                    break;
                }
            }

            var config = { toolbar: loadToolbar };
            if (customConfig)
                $.extend(config, customConfig);

            return this.each(function () {
                CKEDITOR.replace($(this).attr('name'), config);
            });

        }
        catch (err) {
            return this.each(function () {
                CKEDITOR.replace($(this).attr('name'));
            });
        }
    };
}(jQuery));