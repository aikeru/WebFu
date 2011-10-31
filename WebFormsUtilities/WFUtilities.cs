using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.IO;
using System.Web;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using WebFormsUtilities.Json;
using System.ComponentModel;
using WebFormsUtilities.DataAnnotations;

namespace WebFormsUtilities
{
    public static class WFUtilities
    {
        /// <summary>
        /// Build and return script and JSON objects to enable client validation.
        /// This should be called from WFPageBase or WFUserControlBase
        /// </summary>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public static string EnableClientValidationScript(WFModelMetaData metadata)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("if(!window.wfuClientValidationMetadata) { window.wfuClientValidationMetadata = []; }\r\n");
            sb.Append("window.wfuClientValidationMetadata.push(");

            List<JSONObject> fields = new List<JSONObject>();
            foreach (WFModelMetaProperty prop in metadata.Properties)
            {
                JSONObject field = new JSONObject();
                field.Attr("FieldName", prop.MarkupName);
                field.Attr("ReplaceValidationMessageContents", true);
                if (!String.IsNullOrEmpty(prop.OverriddenSpanID))
                {
                    field.Attr("ValidationMessageId", prop.OverriddenSpanID);
                }
                else
                {
                    field.Attr("ValidationMessageId", prop.MarkupName + "_validationMessage");
                }

                List<JSONObject> validationRules = new List<JSONObject>();
                foreach (object oVal in prop.ValidationAttributes)
                {
                    ValidationAttribute val = oVal as ValidationAttribute;
                    JSONObject valRule = new JSONObject();
                    JSONObject valParms = new JSONObject();
                    Type valType = oVal.GetType();
                    if (!String.IsNullOrEmpty(prop.OverriddenErrorMessage))
                    {
                        valRule.Attr("ErrorMessage", prop.OverriddenErrorMessage);
                    }
                    else
                    {
                        valRule.Attr("ErrorMessage", val.FormatErrorMessage(prop.DisplayName));
                    }

                    if (valType == typeof(StringLengthAttribute) || valType.IsSubclassOf(typeof(StringLengthAttribute)))
                    {
                        valParms.Attr("maximumLength", ((StringLengthAttribute)oVal).MaximumLength);
                        valParms.Attr("minimumLength", 0);
                        valRule.Attr("ValidationType", "stringLength");
                        valRule.Attr("ValidationParameters", valParms);
                    }
                    else if (valType == typeof(RequiredAttribute) || valType.IsSubclassOf(typeof(RequiredAttribute)))
                    {
                        valRule.Attr("ValidationType", "required");
                        valRule.Attr("ValidationParameters", new JSONObjectEmpty());
                    }
                    else if (valType == typeof(RangeAttribute) || valType.IsSubclassOf(typeof(RangeAttribute)))
                    {
                        valParms.Attr("minimum", ((RangeAttribute)oVal).Minimum);
                        valParms.Attr("maximum", ((RangeAttribute)oVal).Maximum);
                        valRule.Attr("ValidationType", "range");
                        valRule.Attr("ValidationParameters", valParms);

                        //Create an additional 'number' validation
                        JSONObject numRule = new JSONObject();
                        JSONObject numParms = new JSONObject();
                        numRule.Attr("ErrorMessage", "The field " + prop.DisplayName + " must be a number.");
                        numRule.Attr("ValidationParameters", new JSONObjectEmpty());
                        numRule.Attr("ValidationType", "number");
                        validationRules.Add(numRule);
                    }
                    else if (valType == typeof(RegularExpressionAttribute) || valType.IsSubclassOf((typeof(RegularExpressionAttribute))))
                    {
                        valRule.Attr("ValidationType", "regularExpression");
                        valRule.Attr("ValidationParameters", new JSONObject(new { pattern = ((RegularExpressionAttribute)oVal).Pattern }));
                    }
                    else //Custom Validator
                    {
                        if (val as IWFValidationAttribute == null)
                        {
                            throw new Exception("Custom validators must implement the IWFValidationAttribute interface so that the type of validator can be determined.\r\nThis type must inherit from WFDataAnnotationsModelValidatorBase.");
                        }
                        else
                        {
                            IWFValidationAttribute valAttr = (IWFValidationAttribute)val;
                            WFDataAnnotationsModelValidatorBase valBase = Activator.CreateInstance(valAttr.GetValidatorType(), new object[] { val, prop }) as WFDataAnnotationsModelValidatorBase;
                            var cvrs = valBase.GetClientValidationRules();
                            if (cvrs != null)
                            {
                                bool firstRule = true;
                                foreach (var cvr in valBase.GetClientValidationRules())
                                {
                                    if (firstRule)
                                    {
                                        valRule.Attr("ValidationType", cvr.ValidationType);
                                        foreach (KeyValuePair<string, object> kvp in cvr.ValidationParameters)
                                        {
                                            valParms.Attr(kvp.Key, kvp.Value);
                                        }
                                        valRule.Attr("ValidationParameters", valParms);
                                        firstRule = false;
                                    }
                                    else
                                    {
                                        JSONObject vrx = new JSONObject();
                                        JSONObject vrxParms = new JSONObject();
                                        vrx.Attr("ErrorMessage", cvr.ErrorMessage);
                                        vrx.Attr("ValidationType", cvr.ValidationType);

                                        foreach (KeyValuePair<string, object> kvp in cvr.ValidationParameters)
                                        {
                                            vrxParms.Attr(kvp.Key, kvp.Value);
                                        }
                                        vrx.Attr("ValidationParameters", vrxParms);
                                        validationRules.Add(vrx);
                                    }
                                }
                            }
                        }
                    }
                    validationRules.Add(valRule);
                }
                field.Attr("ValidationRules", validationRules);
                fields.Add(field);
            }
            JSONObject jo = new JSONObject(new { Fields = fields, FormId = "form1", ReplaceValidationSummary = false });

            sb.Append(jo.Render());

            sb.Append(");");
            return sb.ToString();
        }
        /// <summary>
        /// This method should only be called by Html.&lt;control&gt;For() methods.
        /// It registers metadata for model properties if they do not exist for controls on the form and informs the control if there is an error.
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="Model"></param>
        /// <param name="tag"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static bool CheckPropertyError(WFModelMetaData metadata, object Model, HtmlTag tag, string markupName, string reflectName)
        {
            if (reflectName == "") { reflectName = markupName; }
            if (Model == null || String.IsNullOrEmpty(markupName) || tag == null || metadata == null) { return false; }
            WFModelMetaProperty metaprop = null;
            string lcName = markupName.ToLower();
            for (int i = 0; i < metadata.Properties.Count; i++)
            {
                //if (metadata.Properties[i].ModelObject == Model && metadata.Properties[i].MarkupName.ToLower() == lcName)
                if (metadata.Properties[i].MarkupName.ToLower() == lcName)
                {
                    metaprop = metadata.Properties[i];
                    break;
                }
            }
            if (metaprop == null)
            {
                //Create a meta property if it does not exist  
                metaprop = GetMetaProperty(metadata, markupName, Model, reflectName, null);
                metadata.Properties.Add(metaprop);
            }
            if (metaprop.HasError)
            {
                tag.AddClass("input-validation-error");
                return true;
            }
            return false;
        }

        /// <summary>
        /// This method is used internally to find or create meta properties in validation data.
        /// </summary>
        /// <param name="metadata">The metadata object.</param>
        /// <param name="markupName">The name of the property in markup.</param>
        /// <param name="model">The model object in memory the property belongs to.</param>
        /// <param name="reflectName">The reflected property name.</param>
        /// <param name="context">The current HTTP context.</param>
        /// <returns>Returns a WFModelMetaProperty class.</returns>
        public static WFModelMetaProperty GetMetaProperty(WFModelMetaData metadata, string markupName, object model, string reflectName, HttpContext context)
        {

            Type tx = model.GetType();
            WFModelMetaProperty metaprop = null;
            foreach (PropertyInfo pi in tx.GetProperties())
            {
                if (pi.Name == reflectName)
                {
                    object[] attribs = pi.GetCustomAttributes(false);
                    var displayNameAttr = pi.GetCustomAttributes(typeof(DisplayNameAttribute), true).OfType<DisplayNameAttribute>().FirstOrDefault();
                    string displayName = displayNameAttr == null ? pi.Name : displayNameAttr.DisplayName;
                    foreach (object o in attribs)
                    {
                        Type oType = o.GetType();
                        if (oType.IsSubclassOf(typeof(ValidationAttribute)))
                        {
                            var oVal = (ValidationAttribute)o;
                            if (metaprop == null) // Try to find it ...
                            {
                                foreach (WFModelMetaProperty mx in metadata.Properties)
                                {
                                    if (mx.ModelObject == model && pi.Name.ToLower() == mx.PropertyName.ToLower())
                                    {
                                        metaprop = mx;
                                        break;
                                    }
                                }
                            }
                            if (metaprop == null) // Make a new one ...
                            {
                                metaprop = new WFModelMetaProperty();
                            }
                            metaprop.ModelObject = model;
                            metaprop.PropertyName = pi.Name;
                            metaprop.DisplayName = displayName;
                            metaprop.MarkupName = markupName;
                            metaprop.ValidationAttributes.Add(oVal);

                            if (context != null && !oVal.IsValid(context.Request[markupName]))
                            {
                                metaprop.HasError = true;
                                metaprop.Errors.Add(oVal.FormatErrorMessage(displayName));
                            }
                        }
                    }
                }
            }
            return metaprop;
        }


        /// <summary>
        /// Render a Control (.aspx, .ascx) and return it as a string
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string RenderControl(string path)
        {
            return RenderControl(path, null);
        }
        /// <summary>
        /// Render a Control (.aspx, .ascx) and return it as a string, setting the Model property to Model
        /// </summary>
        /// <param name="path"></param>
        /// <param name="Model"></param>
        /// <returns></returns>
        public static string RenderControl(string path, object Model)
        {
            return RenderControl(path, Model, null);
        }
        /// <summary>
        /// Use discretion when specifying the pageInstance, as some server controls will throw an error when WFPageHolder is not used.
        /// You should not pass the current page as the pageInstance. This method assumes you know what you're doing.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pageInstance"></param>
        /// <returns></returns>
        public static string RenderControl(string path, System.Web.UI.Page pageInstance)
        {
            return RenderControl(path, null, pageInstance);
        }
        /// <summary>
        /// Use discretion when specifying the pageInstance, as some server controls will throw an error when WFPageHolder is not used.
        /// !! You should not pass the current page as the pageInstance !! This method assumes you know what you're doing.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="Model"></param>
        /// <param name="pageInstance"></param>
        /// <returns></returns>
        public static string RenderControl(string path, object Model, System.Web.UI.Page pageInstance)
        {
            Page wph = pageInstance == null ? new WFPageHolder() as Page : pageInstance;
            Control ctrl = wph.LoadControl(path);
            if (ctrl == null) { return ""; }

            PropertyInfo pi = ctrl.GetType().GetProperties().FirstOrDefault(p => p.Name == "Model");
            if (pi == null && Model != null)
            {
                throw new Exception("Make sure 'Model' is a public property with a 'setter'");
            }
            else if (pi != null && Model != null)
            {
                pi.SetValue(ctrl, Model, null);
            }

            wph.Controls.Add(ctrl);

            StringWriter output = new StringWriter();
            HttpContext.Current.Server.Execute(wph, output, false);
            return output.ToString();
        }

        /// <summary>
        /// This method can be used to run validation from a [WebMethod] or other AJAX call where a Page instance is not available.
        /// Controls on the page will not be updated by this method.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="prefix"></param>
        /// <param name="context"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static bool TryValidateModel(object model, string prefix, Dictionary<string, string> frmValues, out List<string> errors)
        {
            WFModelMetaData meta = new WFModelMetaData();
            meta.PageModel = model;
            meta.Properties = new List<WFModelMetaProperty>();

            WFPageHolder ph = new WFPageHolder();
            ph.Model = model;
            return TryValidateModel(model, prefix, frmValues, out errors, meta);
        }

        /// <summary>
        /// This method is used to validate class-level attributes
        /// Returns object[] of the model's custom attributes
        /// </summary>
        /// <param name="model"></param>
        /// <param name="modelType"></param>
        /// <param name="errors"></param>
        private static object[] validateModelAttributes(object model, Type modelType, ref List<string> errors)
        {
            object[] modelAttribs = model.GetType().GetCustomAttributes(false);
            var modelDisplayAttr = model.GetType().GetCustomAttributes(typeof(DisplayNameAttribute), true).OfType<DisplayNameAttribute>().FirstOrDefault();
            string modelDisplayName = modelDisplayAttr == null ? model.GetType().Name : modelDisplayAttr.DisplayName;

            foreach (object o in modelAttribs)
            {
                Type oxType = o.GetType();
                if (oxType.IsSubclassOf(typeof(ValidationAttribute)))
                {
                    var oVal = (ValidationAttribute)o;
                    if (!oVal.IsValid(model))
                    {
                        errors.Add(oVal.FormatErrorMessage(modelDisplayName));
                    }
                }
            }
            return modelAttribs;
        }

        /// <summary>
        /// This method is used to validate the model against itself
        /// </summary>
        /// <param name="model"></param>
        /// <param name="modelType"></param>
        /// <param name="prefix">Optional. A prefix to prepend to property names when searching for them on the model.</param>
        /// <returns></returns>
        public static bool TryValidateModel(object model, string prefix, Type modelType, out List<string> errors, ref WFModelMetaData metadata)
        {
            IWFValueProvider cp = new WFObjectValueProvider(model, prefix);
            return TryValidateModel(model, "", cp, out errors, metadata, null);
        }

        /// <summary>
        /// This method is used by WFPageBase and WFUserControlBase TryValidateModel() method
        /// </summary>
        /// <param name="pageinstance"></param>
        /// <param name="model"></param>
        /// <param name="prefix"></param>
        /// <param name="context"></param>
        /// <param name="errors"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public static bool TryValidateModel(object model, string prefix, HttpContext context, out List<string> errors, WFModelMetaData metadata)
        {
            IWFValueProvider cp = new WFHttpContextValueProvider(context);
            return TryValidateModel(model, prefix, cp, out errors, metadata, null);
        }
        public static bool TryValidateModel(object model, string prefix, Dictionary<string, string> frmValues, out List<string> errors, WFModelMetaData metadata)
        {
            IWFValueProvider cp = new WFDictionaryValueProvider(frmValues);
            return TryValidateModel(model, prefix, cp, out errors, metadata, null);
        }

        /// <summary>
        /// All other TryValidateModel() methods call this method. Returns false if any validation errors are found.
        /// </summary>
        /// <param name="model">A pointer to the current model object</param>
        /// <param name="prefix">A prefix to search and filter values from the value provider</param>
        /// <param name="values">A class implementing IWFValueProvider (examples include WFObjectValueProvider and WFHttpContextValueProvider)</param>
        /// <param name="errors">A simple string list of errors.</param>
        /// <param name="metadata">Pointer to the metadata object where error information is stored.</param>
        /// <param name="buddyClass">Can be null. Will be used for validation instead of model.GetType() if specified.</param>
        /// <returns></returns>
        public static bool TryValidateModel(object model, string prefix, IWFValueProvider values, out List<string> errors, WFModelMetaData metadata, Type buddyClass)
        {
            bool validated = true;
            errors = new List<string>();
            Type tx = model.GetType();
            Type metaDataType = null;
            object[] modelAtts = validateModelAttributes(model, tx, ref errors);
            PropertyInfo[] buddyProps = null;
            if (buddyClass == null) //If there is no 'buddy class' then use the model's own type
            {
                if (modelAtts != null && modelAtts.Length > 0)
                {
                    var metaDataAtt = model.GetType().GetCustomAttributes(typeof(MetadataTypeAttribute), true).OfType<MetadataTypeAttribute>().FirstOrDefault();
                    if (metaDataAtt != null)
                    {
                        metaDataType = metaDataAtt == null ? null : metaDataAtt.MetadataClassType;
                        buddyProps = metaDataType.GetProperties();
                    }
                }
            }
            else
            {
                metaDataType = buddyClass;
            }

            if (errors.Count > 0) { validated = false; }

            foreach (PropertyInfo pi in tx.GetProperties())
            {
                if (values.ContainsKey(prefix + pi.Name))
                {
                    object[] attribs = null;

                    //Find out if we need to check attributes from the buddy/proxy class
                    //..if so, contatenate them to attribs, otherwise just use the PropertyInfo from the class.
                    if (metaDataType != null)
                    {
                        PropertyInfo buddyPI = buddyProps.FirstOrDefault(p => p.Name == (prefix + pi.Name));
                        if (buddyPI != null)
                        {
                            object[] custAtts = pi.GetCustomAttributes(false);
                            object[] buddyAtts = buddyPI.GetCustomAttributes(false);
                            attribs = new object[buddyAtts.Length + custAtts.Length];
                            buddyAtts.CopyTo(attribs, 0);
                            custAtts.CopyTo(attribs, buddyAtts.Length);
                        }
                        else
                        {
                            attribs = pi.GetCustomAttributes(false);
                        }
                    }
                    else
                    {
                        attribs = pi.GetCustomAttributes(false);
                    }

                    if (attribs.Length == 0) { continue; } //no attributes to validate

                    var displayNameAttr = pi.GetCustomAttributes(typeof(DisplayNameAttribute), true).OfType<DisplayNameAttribute>().FirstOrDefault();
                    string displayName = displayNameAttr == null ? pi.Name : displayNameAttr.DisplayName;
                    WFModelMetaProperty metaprop = null;
                    foreach (object o in attribs)
                    {
                        Type oType = o.GetType();
                        if (oType.IsSubclassOf(typeof(ValidationAttribute)))
                        {
                            var oVal = (ValidationAttribute)o;
                            bool isValid = false;

                            //Range attribute may throw a "FormatException" if passing a string to be validated
                            if (oType == typeof(RangeAttribute))
                            {
                                try { isValid = oVal.IsValid(values.KeyValue(prefix + pi.Name)); }
                                catch (Exception ex)
                                {
                                    if (ex.GetType() == typeof(FormatException))
                                    {
                                        isValid = false;
                                    }
                                    else { throw ex; }
                                }
                            }
                            else
                            {
                                isValid = oVal.IsValid(values.KeyValue(prefix + pi.Name));
                            }


                            if (!isValid)
                            {
                                validated = false;
                                errors.Add(oVal.FormatErrorMessage(displayName));

                                if (metaprop == null) // Try to find it ...
                                {
                                    foreach (WFModelMetaProperty mx in metadata.Properties)
                                    {
                                        if (mx.ModelObject == model && pi.Name.ToLower() == mx.PropertyName.ToLower())
                                        {
                                            metaprop = mx;
                                            break;
                                        }
                                    }
                                }
                                if (metaprop == null) // Make a new one ...
                                {
                                    metaprop = new WFModelMetaProperty();
                                    metaprop.ModelObject = model;
                                    metaprop.PropertyName = pi.Name;
                                    metaprop.DisplayName = displayName;
                                    metaprop.MarkupName = prefix + pi.Name;
                                    metaprop.ValidationAttributes.Add(oVal);

                                    metadata.Properties.Add(metaprop);
                                }
                                metaprop.HasError = true;
                                metaprop.Errors.Add(oVal.FormatErrorMessage(displayName));

                            }
                        }
                    }
                }
            }
            return validated;
        }

        public static Dictionary<string, string> UrlDecodeDictionary(string decode)
        {
            if (String.IsNullOrEmpty(decode)) { return new Dictionary<string, string>(); }
            if (decode.Contains('&'))
            {
                string[] strs = decode.Split('&');
                Dictionary<string, string> reqDict = new Dictionary<string, string>();
                foreach (string s in strs)
                {
                    string[] sx = s.Split('=');
                    if (reqDict.ContainsKey(HttpContext.Current.Server.UrlDecode(sx[0])))
                    {
                        if (!String.IsNullOrEmpty((reqDict[HttpContext.Current.Server.UrlDecode(sx[0])] ?? "").Trim()))
                        {
                            reqDict[HttpContext.Current.Server.UrlDecode(sx[0])] =
                                reqDict[HttpContext.Current.Server.UrlDecode(sx[0])] + "," + HttpContext.Current.Server.UrlDecode(sx[1]);
                        }
                        else
                        {
                            reqDict[HttpContext.Current.Server.UrlDecode(sx[0])] = HttpContext.Current.Server.UrlDecode(sx[1]);

                        }
                    }
                    else
                    {
                        reqDict.Add(HttpContext.Current.Server.UrlDecode(sx[0]), HttpContext.Current.Server.UrlDecode(sx[1]));
                    }
                }
                return reqDict;
            }
            else
            {
                if (decode.Contains('='))
                {
                    string[] strs = decode.Split('=');
                    return new Dictionary<string, string>() { { HttpContext.Current.Server.UrlDecode(strs[0]), 
                                                                 HttpContext.Current.Server.UrlDecode(strs[1]) } };
                }
                else
                {
                    string d = HttpContext.Current.Server.UrlDecode(decode);
                    return new Dictionary<string, string>() { { d, d } };
                }
            }

        }
    }


    public interface IWFValueProvider
    {
        bool ContainsKey(string keyName);
        object KeyValue(string keyName);
    }

    /// <summary>
    /// Caches the page controls on constructor (ByRef) and then provides their values when needed.
    /// Methods like UpdateModel() use IWFValueProvider's.
    /// </summary>
    public class WFPageControlsValueProvider : IWFValueProvider
    {
        private Control _Page = null;
        private string _Prefix = "";
        private Dictionary<string, Control> _WebControls = new Dictionary<string, Control>();
        public WFPageControlsValueProvider(Control page, string prefix)
        {
            _Page = page;
            _Prefix = prefix;
            _WebControls = WebControlUtilities.FlattenPageControls(_Page);
        }


        #region IWFValueProvider Members

        public bool ContainsKey(string keyName)
        {
            return _WebControls.ContainsKey(keyName);
        }

        public object KeyValue(string keyName)
        {
            return WebControlUtilities.GetControlValue(_WebControls[keyName]);
        }

        #endregion
    }

    public class WFObjectValueProvider : IWFValueProvider
    {
        private object _Model = null;
        private string _Prefix = "";
        private PropertyInfo[] _Properties = null;
        /// <summary>
        /// Provide values to validate against from an object (rather than, say, an HTTP request)
        /// </summary>
        /// <param name="model">The object which already has values populated</param>
        /// <param name="prefix">An optional prefix to prepend when checking property names</param>
        public WFObjectValueProvider(object model, string prefix)
        {
            _Model = model;
            _Properties = _Model.GetType().GetProperties();
            _Prefix = prefix;
        }
        #region IWFValueProvider Members

        public bool ContainsKey(string keyName)
        {
            if (!String.IsNullOrEmpty(_Prefix))
            {
                if (_Properties.FirstOrDefault(p => (_Prefix.ToLower() + p.Name.ToLower()) == keyName.ToLower()) != null)
                {
                    return true;
                }
            }
            else
            {
                if (_Properties.FirstOrDefault(p => p.Name.ToLower() == keyName.ToLower()) != null)
                {
                    return true;
                }
            }
            return false;
        }

        public object KeyValue(string keyName)
        {
            if (_Model == null) { return null; }
            PropertyInfo pi = null;
            if (!String.IsNullOrEmpty(_Prefix))
            {
                pi = _Properties.First(p => (_Prefix.ToLower() + p.Name.ToLower()) == keyName.ToLower());
            }
            else
            {
                pi = _Properties.First(p => p.Name.ToLower() == keyName.ToLower());
            }
            return pi.GetValue(_Model, null);
        }

        #endregion
    }
    public class WFDictionaryValueProvider : IWFValueProvider
    {
        private Dictionary<string, string> StringDictionary = null;
        public WFDictionaryValueProvider(Dictionary<string, string> dict)
        {
            StringDictionary = dict;
        }
        #region IWFValueProvider Members

        public bool ContainsKey(string keyName)
        {
            return StringDictionary.ContainsKey(keyName);
        }

        public object KeyValue(string keyName)
        {
            return StringDictionary[keyName];
        }

        #endregion
    }
    public class WFHttpContextValueProvider : IWFValueProvider
    {
        private HttpContext _HttpContext = null;
        private HttpRequest _Request = null;
        public WFHttpContextValueProvider(HttpContext context)
        {
            _HttpContext = context;
            _Request = _HttpContext.Request;
        }
        public WFHttpContextValueProvider(HttpRequest request)
        {
            _Request = request;
        }

        #region IWFValueProvider Members

        public bool ContainsKey(string keyName)
        {
            return _Request.Form.AllKeys.Contains(keyName);
        }

        public object KeyValue(string keyName)
        {
            return _Request.Form[keyName];
        }

        #endregion
    }
}
