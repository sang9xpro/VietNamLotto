//#region function detect element exists or not
//jQuery.fn.exists = function () { return this.length > 0; };
//#endregion

//#region test
//$.fn.serializeObject = function () {
//    var o = {};
//    var a = this.serializeArray();
//    $.each(a, function () {
//        if (o[this.name]) {
//            if (!o[this.name].push) {
//                o[this.name] = [o[this.name]];
//            }
//            o[this.name].push(this.value || '');
//        } else {
//            o[this.name] = this.value || '';
//        }
//    });
//    return o;
//};
//#endregion

//#region function write message in head
//function WriteCommendJS(type, message) {
//    if ($(".div-bubble").exists()) {
//        var divbubble = $(".div-bubble");
//        // display        
//        $(divbubble).fadeIn();        
//        $(divbubble).removeClass();
//        $(divbubble).addClass("alert div-bubble");
//        // content        
//        $("#error-div-content").html(message);
//        if (type == StatusTypeJS.Error) {
//            $(divbubble).addClass("alert-danger");            
//        }
//        else if (type == StatusTypeJS.Success) {
//            $(divbubble).addClass("alert-success");            
//        }
//        else if (type == StatusTypeJS.Warning) {
//            $(divbubble).addClass("alert-warning");            
//        }
//    }
//}
//#endregion

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
//#endregion

//#region setup ajax error
//$.ajaxSetup({
//    error: function (evt, xhr) {
//        if ($("#LoadingImage").exists()) {
//            $("#LoadingImage").hide();
//        }
//        var json = $.parseJSON(evt.responseText);
//        if (json != null)
//            WriteCommendJS(StatusTypeJS.Error, json.error);
//    }
//});
//#endregion

//#region function get controls
//function GetControl(controller, action) {
//    var j = new Object();
//    j.Controller = controller;
//    j.Action = action;
//    $.ajax({
//        url: '/Base/GetControls',
//        type: 'POST',
//        data: JSON.stringify(j),
//        contentType: 'application/json, charset=utf-8',
//        success: function (data) {
//            //alert(data);
//            $("#controls").html(data);
//        }
//    });
//}
////#endregion

//#region refresh dialog box
function RefreshDiadlog(objDiv) {
    //clear existing dialog content and show an ajax loader
    objDiv.html('<div class="loading" />');
    //show the dialog
    objDiv.dialog("option", "title", "Loading...");
    objDiv.dialog("open");
}
//#endregion

//#region refresh show messagebox
//function ShowMessageBox(message) {
//    $.alerts.okButton = "Close";
//    jAlert(message, $.IndoRepGlobalVariable.SysInfor);
//}

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
//#endregion

//#region show confirm messagebox with callback
//function ShowConfirmBox(message, callback) {
//    $.alerts.okButton = "Agree";
//    $.alerts.cancelButton = "Cancel";
//    jConfirm(message, $.IndoRepGlobalVariable.SysConfirm, function (r) {
//        if (r) {
//            callback();
//        }
//    });
//}
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
//#endregion

//#region Function get selected Id of table
//function GetListIdSelected(idtbl) {
//    var arr = new Array;
//    $('#' + idtbl + ' tr td input.chkItem:checked').each(function () {
//        arr.push($(this).val());
//    });
//    return arr;
//}
//#endregion

//#region Function get selected Id of table
//function CheckItemChoosed(idtbl) {
//    if (GetListIdSelected(idtbl).length > 0)
//        return true;
//    else return false;
//}
//#endregion

//#region Function clear checkbox table
//function ClearCheckbox(idtbl) {
//    $('#' + idtbl + ' .checkall').attr('checked', false);
//    $('#' + idtbl + ' tr td input.chkItem:checked').each(function () {
//        // unchecked
//        $(this).attr('checked', false);
//        // clear tr's class
//        $(this).parents('tr').toggleClass('row_selected');
//    });
//}
//#endregion

//#region Function disply notification
function ShowNotification(title, text) {
    var uniqueId = $.gritter.add({
        // (string | mandatory) the heading of the notification
        title: title,
        // (string | mandatory) the text inside the notification
        text: text,
        // (string | optional) the image to display on the left
        image: '',
        // (bool | optional) if you want it to fade out on its own or just sit there
        sticky: true,
        // (int | optional) the time you want it to be alive for before fading out
        time: '',
        // (string | optional) the class name you want to apply to that specific message
        class_name: 'my-sticky-class'
    });
    return uniqueId;
}
function CloseNotification(uniqueId, timer) {
    setTimeout(function () {
        $.gritter.remove(uniqueId, {
            fade: true,
            speed: 'slow'
        });
    }, timer);
}
//#endregion


//#region cookie
function setCookie(cname, cvalue, days) {
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        var expires = "; expires=" + date.toGMTString();
    } else {
        var expires = "";
    }
    document.cookie = cname + "=" + cvalue + expires+"; path=/";
    console.log(document.cookie);
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(";");
    console.log(ca);
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
      
        while (c.charAt(0) == " ") {
            c = c.substring(1);
            console.log(c);
        }
        if (c.indexOf(name) == 0) {
            console.log(c.substring(name.length, c.length));
            return c.substring(name.length, c.length);
        }
    }
    return null;
}
function eraseCookie(name) {
    setCookie(name, "", -1);
}
//#endregion
//#region Number formatMoney
Number.prototype.formatMoney = function formatMoney (decPlaces, thouSeparator, decSeparator) {
    decPlaces = isNaN(decPlaces = Math.abs(decPlaces)) ? 2 : decPlaces;
    thouSeparator = thouSeparator == undefined ? "," : thouSeparator;
    decSeparator = decSeparator == undefined ? "." : decSeparator;
    var n = this,
        sign = n < 0 ? "-" : "",
        i = parseInt(n = Math.abs(+n || 0).toFixed(decPlaces)) + "",
        j = (j = i.length) > 3 ? j % 3 : 0;
    return sign + (j ? i.substr(0, j) + thouSeparator : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + thouSeparator) + (decPlaces ? decSeparator + Math.abs(n - i).toFixed(decPlaces).slice(2) : "");
};
//#endregion