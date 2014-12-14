/**
 * @license Copyright (c) 2003-2013, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.html or http://ckeditor.com/license
 */
var aviaryLoaded = false;

CKEDITOR.editorConfig = function (config, type) {
    config.pasteFromWordRemoveFontStyles = true;
    config.pasteFromWordRemoveStyles = true;

    config.disableNativeSpellChecker = false;
    config.scayt_autoStartup = false;
    //config.allowedContent = true; // fix auto clean all a tag when view source
    // config.autoParagraph = false;
    // config.enterMode = CKEDITOR.ENTER_BR;

    config.removeButtons = 'scayt,Underline,Subscript,Superscript,magicline';

    // define simple, basic, standard and full toolbar. ckeditor is belong to 4 toobars
    config.toolbar_simple =
    [
             ['Source'],
             ['Bold', 'Italic', '-', 'SimpleLink', 'Unlink', 'Anchor'],
             ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord']
    ];

    config.toolbar_basic =
    [
             ['Source'],
             ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'],
             ['SimpleLink', 'Unlink', 'Anchor'],
             ['Image', 'Table', 'HorizontalRule'],
             '/',
             ['Bold', 'Italic', 'Strike', '-', 'RemoveFormat'],
             ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote'],
             ['Styles', 'Format']
    ];

    config.toolbar_standard =
    [
            ['Source'],
            ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'],
            ['Image', 'Table', 'HorizontalRule', 'SpecialChar', 'PageBreak', 'Iframe'],
            '/',
            ['basicstyles', 'cleanup'],
            ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', '-', 'RemoveFormat'],
            ['list', 'indent', 'blocks', 'align', 'bidi'],
            ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote', 'CreateDiv', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
            ['SimpleLink', 'Unlink', 'Anchor'],
            '/',
            ['Styles', 'Format', 'Font', 'FontSize'],
            ['TextColor', 'BGColor']
    ];

    config.toolbar_full =
    [
            ['Source', '-', 'Preview', 'Print', '-', 'Templates'],
            ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'],
            ['find', 'selection', 'spellchecker'],
            ['Image', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar', 'PageBreak', 'Iframe'],
             ['Maximize', 'ShowBlocks'],
            '/',
            ['basicstyles', 'cleanup'],
            ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', '-', 'RemoveFormat'],
            ['list', 'indent', 'blocks', 'align', 'bidi'],
            ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote', 'CreateDiv', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
            ['SimpleLink', 'Unlink', 'Anchor'],
            ['Find', 'Replace', '-', 'SelectAll', '-', 'Scayt'],
            '/',
            ['Styles', 'Format', 'Font', 'FontSize'],
            ['TextColor', 'BGColor']           
    ];
};

