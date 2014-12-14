var product = function () {
    addToLastView : function(id){
        var lastView = $.cookie('LastView');
        var newid = '&'+ id + ';';
        lastView = lastView.replace(newid,'');
        lastView += newid;

        $.cookie('LastView', lastView);
    }
}

