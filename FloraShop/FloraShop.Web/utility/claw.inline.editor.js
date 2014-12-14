(function () {
    // integrate ckfinder to ckeditor
    if (typeof (CKFinder) != 'undefined') CKFinder.setupCKEditor(null, '/utility/ckfinder/');

    CKEDITOR.on('instanceCreated', function (event) {
        var editor = event.editor,
            element = editor.element;
        // Customize editors for headers and tag list.
        // These editors don't need features like smileys, templates, iframes etc.

        editor.on('instanceReady', function (e) {
            $(e.editor.element.$).removeAttr("title");
        });

        editor.config.fillEmptyBlocks = true;
        editor.config.tabSpaces = 0;
        editor.config.forcePasteAsPlainText = true;

        if (element.getAttribute('contenteditable') == 'true') {
            // Customize the editor configurations on "configLoaded" event,
            // which is fired after the configuration file loading and
            // execution. This makes it possible to change the
            // configurations before the editor initialization takes place.
            //if (element.getAttribute('class') == 'ckeditor_div')

            if (element.getAttribute('data-islabel') == 'True') {
                editor.on('configLoaded', function () {
                    editor.config.allowedContent = true;
                    editor.config.extraPlugins = 'onchange,clawsave,clawcancel,imageresize';//,image2,testplugin',clawedit;

                    // Remove unnecessary plugins to make the editor simpler.
                    editor.config.removePlugins = 'find,flash,link,forms,iframe,newpage,stylescombo,templates,magicline';

                    editor.config.toolbar = [
                        ['Undo', 'Redo'],
                        ['clawsave', 'clawcancel']
                    ];

                });
            }
            else //if (element.getAttribute('data-islabel') == 'False')
            {
                editor.on('configLoaded', function () {
                    //editor.config.specialChars = CKEDITOR.config.specialChars.concat([['&omega;', 'Omega']]);
                    //editor.config.fillEmptyBlocks = false;
                    editor.config.allowedContent = true;

                    editor.config.extraPlugins = 'onchange,clawsave,clawcancel,imageresize,simpleLink,imageedit';//,image2,testplugin';,internpage,clawedit,

                    // Remove unnecessary plugins to make the editor simpler.
                    editor.config.removePlugins = 'find,flash,forms,iframe,newpage,stylescombo,templates,magicline';

                    editor.config.toolbar = [
                        //['Source'],
                        ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord'],
                        ['Undo', 'Redo', '-', 'Find', 'Replace', '-', 'SelectAll', 'RemoveFormat'],
                        ['TextColor', 'BGColor'],
                        ['Bold', 'Italic', 'Underline'],
                        ['JustifyLeft', 'JustifyCenter', 'JustifyRight'],
                        ['NumberedList', 'BulletedList'],
                        '/',
                        ['SimpleLink', 'Unlink'],//'Link', 'Unlink', 'Anchor',
                        ['Image', 'Table'],// 'Flash',
                        ['Outdent', 'Indent'],
                        ['Styles', 'Font', 'Format', 'FontSize', 'HorizontalRule', 'Smiley', 'SpecialChar'],
                        ['clawsave', 'clawedit', 'clawcancel']
                    ];

                });
            }

        }
    });

})();

