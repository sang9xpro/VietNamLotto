function ShowMessageBox(message, type) {
    if (type == StatusTypeJS.Error) {
        $.msgBox({
            title: $.H3CGlobalVariable.SystemTitle,
            content: message,
            type: "error"
        });
    }
    else if (type == StatusTypeJS.Success) {
        $.msgBox({
            title: $.H3CGlobalVariable.SystemTitle,
            content: message,
            type: "info"
        });
    }
    else if (type == StatusTypeJS.Warning) {
        $.msgBox(message);
    }
}

function ShowConfirmMessageBox(message, callback) {
    $.msgBox({
        title: $.H3CGlobalVariable.SystemTitle,
        content: message,
        type: "confirm",
        buttons: [{ value: "Yes" }, { value: "No" }],
        success: function (result) {
            if (result == "Yes")
                callback();
        }
    });
}
//#region function fade out success div
function FadeOutSuccessDiv() {
    if ($(".div-bubble").exists()) {
        var divbubble = $(".div-bubble");
        var divchild = $(divbubble).children(":first");
        var className = $(divchild).attr('class');
        if (className.toLowerCase().indexOf("alert-success") >= 0
            || className.toLowerCase().indexOf("alert-warning")) {
            setTimeout(function () {
                $(".div-bubble").fadeOut("slow", function () {
                });
            }, 5000);
        }
    }
}
//#endregion

//#region OpenLoading
function OpenLoadingAjax() {
    $("#LoadingImage").show();
}
//#endregion

//#region ClostLoading
function CloseLoadingAjax() {
    $("#LoadingImage").hide();
}

function replaceVietnameseString(str) {
    var from = "àáãảạẳăằắặẵẩâầấậẫẻèéẹẽềếệễêểủùúụũừứựưữỏòóõọổôồốỗộờớợỡởìíĩịỉ";
    var to =   "aaaaaaaaaaaaaaaaaeeeeeeeeeeeuuuuuuuuuuooooooooooooooooiiiii";
    for(var i=0,l=from.length;i<l;i++){
        str=str.replace(RegExp(from[i],"gi"),to[i]);
    }

    str = str.toLowerCase().trim().replace(/\s/g,"");
    return str;

}