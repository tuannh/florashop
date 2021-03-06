/*
*   Developer: TuanNH
*   Date: 2013.10.18
*   Require: Jquery lib
*   Description: Captcha refresh script
*
*/

jQuery(document).ready(function($) {

    $('.refreshCaptcha').bind('click', refreshCaptcha);

    function refreshCaptcha(){
        $('.refreshCaptcha').unbind('click');

        var url = $('#captcha_RefreshUrl').val();
        var token = $('#' + $('#captcha_TokenElementId').val()).val();
        var tokenname = $('#captcha_TokenParameterName').val();
        var data = tokenname + '=' + token;

        $.ajax({
            type: "POST",
            url: url,
            data: data,
            success: function () {
                $('.refreshCaptcha').bind('click', refreshCaptcha);
            }
        });
    }

})