/*******/
CKEDITOR_BASEPATH = "/utility/ckeditor/";

// register console if not exit
if (!window.console || typeof console === "undefined") {
    console = { log: function (logMsg) { } };
}

function OpenWindow(target) {
    window.open(target, "_Child", "toolbar=no,scrollbars=yes,resizable=yes,width=400,height=400");
}

function OpenPostWindow(target) {
    window.open(target, "_Child", "resizable=yes,width=500,height=700");
}

/* cookie utility function */
function getCookie(sName) {
    var cookie = "" + document.cookie;
    var start = cookie.indexOf(sName);

    if (cookie == "" || start == -1)
        return "";

    var end = cookie.indexOf(';', start);
    if (end == -1)
        end = cookie.length;

    return unescape(cookie.substring(start + sName.length + 1, end));
}

function setCookie(sName, value) {
    document.cookie = sName + "=" + escape(value) + ";path=/;";
}

function setCookieForever(sName, value) {
    document.cookie = sName + "=" + escape(value) + ";path=/;expires=Wed, 1 Jan 2020 00:00:00 GMT;";
}

// Alias helper
function getObjectById(id) {
    if (document.getElementById)
        var returnVar = document.getElementById(id);
    else if (document.all)
        var returnVar = document.all[id];
    else if (document.layers)
        var returnVar = document.layers[id];
    return returnVar;
}

//This method check the inputText is meet the alias requirement or not
function IsAliasFormat(inputText) {
    return inputText.match(/^[a-zA-Z0-9][a-zA-Z0-9_-]+$/);
}

//This method generate an alias from an input text, return empty string if no valid character presents.
function GenerateAlias(inputText) {
    return GenerateAlias(inputText, 250);
}

function GenerateAlias(inputText, maxLength) {
    //lower chars
    inputText = inputText.replace(/æ|å|ä|à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
    inputText = inputText.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
    inputText = inputText.replace(/ì|í|ị|ỉ|ĩ/g, "i");
    inputText = inputText.replace(/ø|ö|ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
    inputText = inputText.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
    inputText = inputText.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
    inputText = inputText.replace(/đ/g, "d");
    inputText = inputText.replace(/ñ/g, "n");

    //upper chars
    inputText = inputText.replace(/Æ|Å|Ä|À|Á|Ạ|Ả|Ã|Â|Ầ|Ấ|Ậ|Ẩ|Ẫ|Ă|Ằ|Ắ|Ặ|Ẳ|Ẵ/g, "A");
    inputText = inputText.replace(/È|É|Ẹ|Ẻ|Ẽ|Ê|Ề|Ế|Ệ|Ể|Ễ/g, "E");
    inputText = inputText.replace(/Ì|Í|Ị|Ỉ|Ĩ/g, "I");
    inputText = inputText.replace(/Ø|Ö|Ó|Ò|Ỏ|Õ|Ọ|Ô|Ố|Ồ|Ổ|Ỗ|Ộ|Ơ|Ớ|Ờ|Ở|Ỡ|Ợ/g, "O");
    inputText = inputText.replace(/Ú|Ù|Ủ|Ũ|Ụ|Ư|Ứ|Ừ|Ử|Ữ|Ự/g, "U");
    inputText = inputText.replace(/Ý|Ỳ|Ỷ|Ỹ|Ỵ/g, "Y");
    inputText = inputText.replace(/Đ/g, "D");
    inputText = inputText.replace(/Ñ/g, "N");

    inputText = inputText.replace(/!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'| |\"|\&|\#|\[|\]|~|$|_/g, " ");
        
    // Remove invalid character
    var result = inputText.replace(/(^[^a-zA-Z0-9]+)|([^a-zA-Z0-9\-_\s])/g, "");
    //Replace all space with _
    result = result.replace(/\s+[\-_]*/g, "-");
    result = result.replace(/-+-/g, "-"); //thay thế 2- thành 1-
    result = result.replace(/^\-+|\-+$/g, "");
    // trim space, - and _ of strim
    result = result.replace(/^[\s\-_]+|[\s\-_]+$/g, "");    
    
    return result.substring(0, maxLength).toLowerCase();
}

function AutoGenerateAlias(inputText, targetObjectId, isOverwrite) {
    var aliasTextbox = getObjectById(targetObjectId);

    if (aliasTextbox == null || aliasTextbox == undefined)
        return;

    // the target text does not allow to change value
    if (aliasTextbox.disabled == true || aliasTextbox.attributes["readonly"] != undefined)
        return;

    var currentAliasText = aliasTextbox.value.replace(/^[\s\-_]+|[\s\-_]+$/g, "");

    if (currentAliasText.length > 0 && !isOverwrite)
        return; //Do not allow overwrite on current alias

    currentAliasText = GenerateAlias(inputText);

    if (currentAliasText.length > 0)
        aliasTextbox.value = currentAliasText;
}

function ReplaceSingleQuote(targetTextBoxId) {    
    var TargetTextBox = $(targetTextBoxId);    
    if (TargetTextBox == undefined || TargetTextBox == null)
        return;
    var ValidateRegEx = /'/ig;
    TargetTextBox.value = TargetTextBox.value.replace(ValidateRegEx, "%27");
}

function removeHTMLTags(s){       
    var strInputCode = s;
    /*
            This line is optional, it replaces escaped brackets with real ones,
            i.e. &lt; is replaced with < and &gt; is replaced with >
    */
    strInputCode = strInputCode.replace(/&(lt|gt);/g, function (strMatch, p1){
            return (p1 == "lt")? "<" : ">";
    });
    var strTagStrippedText = strInputCode.replace(/<\/?[^>]+(>|$)/g, "");
    return strTagStrippedText.trim();
  // Use the alert below if you want to show the input and the output text
  //           alert("Input code:\n" + strInputCode + "\n\nOutput text:\n" + strTagStrippedText);       
}

function clog(message) {
    try {
        console.log(message);
    }
    catch (ex) { }
}

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexString = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexString);
    var found = regex.exec(window.location.search);
    if (found == null)
        return "";
    else
        return decodeURIComponent(found[1].replace(/\+/g, " "));
}