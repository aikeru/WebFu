//WFUtilitiesJquery.js

// WFUtilities
function WFSubmitForm(methodName) {
    var elem = $('#JSMethod');
    var elemStr = '<input type="hidden" name="JSMethod" id="JSMethod" value="' + methodName + '" />';
    if (elem[0] === undefined) {
        elem = $("form").append(elemStr);
    } else {
        elem = $('#JSMethod').replaceWith(elemStr);
    }
    (document.forms[0]).submit();
    return false;
}

function WFEnableUpload() {
    $('form').attr("enctype", "multipart/form-data");
    $('form').attr("encoding", "multipart/form-data");
}

//Syntax is like this:
//  PageName.aspx/MethodName
//This works against AJAX in 3.5 but not 2.0 (2.0 has no .d property)
function WFCallPage(pageMethodURL, args) {
    if (args.data === undefined || args.data === null || args.data === "") { args.data = "{}"; }
    $.ajax({ type: "POST", url: pageMethodURL, data: args.data, contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function(a, b, c) {
            if (args.success != undefined && args.success != null) {
                args.success(a, b, c);
            }
        },
        error: function(a, b, c) {
            if (args.error != undefined && args.error != null) {
                args.error(a, b, c);
            }
        }
    });
}

//Uses jQuery's .serialize() method and excludes ASP.net's extra hidden fields
//Returns what looks like a raw post body that is URL encoded (ie: name=value&name2=value2...) and has double-quotes escaped " -> \"
function WFSerializeForm() {
    var serStr = $('form :input').not('#__VIEWSTATE,#__EVENTVALIDATION').serialize();
    serStr = serStr.replace(/"/g, "\\\"");
    return serStr;
}