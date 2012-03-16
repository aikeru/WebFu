//WFUtilitiesJquery.js

// WFUtilities
function WFSubmitForm(methodName) {
    /// <summary>Invoke a full postback, passing an extra JSMethod form variable to indicate which method the server should invoke.
    /// &#10;You can use WFPageUtilities.CallJSMethod() on the server side during a postback to call this method automatically.
    /// &#10;The target method should have two arguments: object sender and EventArgs e, which will both be null.</summary>
    /// <param name="methodName" type="String">The name of the method on the page class to invoke.</param>
    /// <returns>Returns false.</returns>
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
    ///<summary>Add enctype and encoding attributes to the &lt;form&gt; tag so that WebForms will see the files uploaded by &lt;input type=file&gt;</summary>
    $('form').attr("enctype", "multipart/form-data");
    $('form').attr("encoding", "multipart/form-data");
}


//Syntax is like this:
//  PageName.aspx/MethodName
//This works against AJAX in 3.5 but not 2.0 (2.0 has no .d property)
function WFCallPage(pageMethodURL, args) {
    ///<summary>Shortcut to invoke a [WebMethod] using jQuery's $.ajax()
    ///&#10;[WebMethod]'s from any WebForms page can be called.</summary>
    ///<param name="pageMethodURL" type="String">Syntax: PageName.aspx/WebMethodName</param>
    ///<param name="args" type="Object">Magic arguments object.
    /// &#10;{data: a STRING in the form of a JSON object which must match the signature of the webmethod
    /// &#10;success: function(a, b, c),
    /// &#10;error: function(a, b, c)}</param>
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
    ///<summary>Serialize the form into a post 'body' string. (ie: name=value&amp;name2=value2...) Will escape double-quotes.</summary>
    ///<returns>Returns a string in the form of (ie: name=value&amp;name2=value2...)</returns>
    var serStr = $('form :input').not('#__VIEWSTATE,#__EVENTVALIDATION').serialize();
    serStr = serStr.replace(/"/g, "\\\"");
    return serStr;
}