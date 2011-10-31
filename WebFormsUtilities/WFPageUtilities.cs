using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;
using System.Web.UI;

namespace WebFormsUtilities
{
    public static class WFPageUtilities
    {
        /// <summary>
        /// This method can be used if WFUtilitiesJquery.js is not included in the project.<br/>
        /// Returns a &lt;script&gt; tag which should be placed in &lt;head&gt; after jQuery.
        /// </summary>
        /// <returns>Returns a &lt;script&gt; tag which should be placed in &lt;head&gt; after jQuery.</returns>
        public static string ScriptRegisterClientFunctions()
        {
            return JSResources.WFUtilitiesJquery;
        }
        /// <summary>
        /// This method can be used if WFUtilitiesJquery.js is not included in the project.<br/>
        /// Returns a minified &lt;script&gt; tag which should be placed in &lt;head&gt; after jQuery.
        /// </summary>
        /// <returns>Returns a minified &lt;script&gt; tag which should be placed in &lt;head&gt; after jQuery.</returns>
        public static string ScriptRegisterClientFunctionsMinified()
        {
            return JSResources.WFUtilitiesJqueryMin;
        }
        /// <summary>
        /// Call this method when posting back from the JavaScript WFCallPage(); function<br/>
        /// Use Request["JSMethod"] to find a matching Page method that is decorated with WFJScriptMethod<br/>
        /// This prevents methods from being called by JavaScript unless explicitly opt-in.<br/>
        /// The method must accept an [object] and an [EventArgs]. As of this compile, these will be empty.<br/>
        /// </summary>
        public static void CallJSMethod(Control pageControl, HttpRequest request)
        {
            if (!String.IsNullOrEmpty(request["JSMethod"]))
            {
                MethodInfo mi = pageControl.GetType().GetMethod(request["JSMethod"], BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (mi != null)
                {
                    if (mi.GetCustomAttributes(typeof(WFJScriptMethodAttribute), false).Length > 0)
                    {
                        mi.Invoke(pageControl, new object[] { null, new EventArgs() });
                    }
                }
                else
                {
                    throw new Exception("No matching method " + request["JSMethod"] + " that is marked with [WFJScriptMethod] attribute.");
                }
            }
        }
        /// <summary>
        /// Validate the model against form values (not against itself).
        /// </summary>
        /// <param name="WFMetaData">The metadata object which stores validation information.</param>
        /// <param name="model">The model being validated.</param>
        /// <param name="context">The HTTP context where form values are stored to validate.<br/>
        /// HttpContext.Current can be used.
        /// </param>
        /// <returns>Returns 'true' if the values in the form data validate successfully.</returns>
        public static bool TryValidateModel(WFModelMetaData WFMetaData, object model, HttpContext context)
        {
            return TryValidateModel(WFMetaData, model, "", context);
        }
        /// <summary>
        /// Validate the model against form values (not against itself).
        /// </summary>
        /// <param name="WFMetaData">The metadata object which stores validation information.</param>
        /// <param name="model">The model being validated.</param>
        /// <param name="prefix">The prefix to separate different objects in form data.<br/>
        /// ie: object1_FirstName=John, object2_FirstName=Joe</param>
        /// <param name="context">The HTTP context where form values are stored to validate.<br/>
        /// HttpContext.Current can be used.
        /// </param>
        /// <returns></returns>
        public static bool TryValidateModel(WFModelMetaData WFMetaData, object model, string prefix, HttpContext context)
        {
            List<string> errors = new List<string>();

            return TryValidateModel(WFMetaData, model, prefix, context, out errors);
        }
        /// <summary>
        /// Validate the model against form values (not against itself).
        /// </summary>
        /// <param name="WFMetaData">The metadata object which stores validation information.</param>
        /// <param name="model">The model being validated.</param>
        /// <param name="prefix">The prefix to separate different objects in form data.<br/>
        /// ie: object1_FirstName=John, object2_FirstName=Joe</param>
        /// <param name="context">he HTTP context where form values are stored to validate.<br/>
        /// HttpContext.Current can be used.</param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static bool TryValidateModel(WFModelMetaData WFMetaData, object model, string prefix, HttpContext context, out List<string> errors)
        {
            errors = new List<string>();
            return WFUtilities.TryValidateModel(model, prefix, context, out errors, WFMetaData);
        }

        /// <summary>
        /// !! MUST be run at the end of the form in markup. !!
        /// Outputs a script tag containing JavaScript code to enable validation on client side.
        /// </summary>
        /// <returns></returns>
        public static string EnableClientValidation(WFModelMetaData WFMetaData)
        {
            return new HtmlTag("script", new { type = "text/javascript", language = "javascript" }) { InnerText = WFUtilities.EnableClientValidationScript(WFMetaData) }.Render();
        }

        /// <summary>
        /// Updates the specified model instance using values from the value provider.
        /// </summary>
        /// <typeparam name="TModel">The type of model object</typeparam>
        /// <param name="model">The model instance to update</param>
        public static void UpdateModel<TModel>(HttpRequest request, TModel model)
        {
            UpdateModel<TModel>(request, model, "");
        }
        /// <summary>
        /// Updates the specified model instance using values from the value provider.
        /// </summary>
        /// <typeparam name="TModel">The type of model object</typeparam>
        /// <param name="model">The model instance to update</param>
        /// <param name="prefix">The prefix to use when looking up values in the value provider</param>
        public static void UpdateModel<TModel>(HttpRequest request, TModel model, string prefix)
        {
            WFHttpContextValueProvider vp = new WFHttpContextValueProvider(request);
            UpdateModel<TModel>(vp, model, prefix, null, new string[] { });
        }
        /// <summary>
        /// Updates the specified model instance using values from the value provider.
        /// </summary>
        /// <typeparam name="TModel">The type of model object</typeparam>
        /// <param name="model">The model instance to update</param>
        /// <param name="includeProperties">A list of properties of the model to update</param>
        public static void UpdateModel<TModel>(HttpRequest request, TModel model, string[] includeProperties)
        {
            WFHttpContextValueProvider vp = new WFHttpContextValueProvider(request);
            UpdateModel<TModel>(vp, model, "", includeProperties, new string[] { });
        }
        /// <summary>
        /// Updates the specified model instance using values from the value provider.
        /// </summary>
        /// <typeparam name="TModel">The type of model object</typeparam>
        /// <param name="model">The model instance to update</param>
        /// <param name="prefix">The prefix to use when looking up values in the value provider</param>
        /// <param name="includeProperties">A list of properties of the model to update</param>
        public static void UpdateModel<TModel>(HttpRequest request, TModel model, string prefix, string[] includeProperties)
        {
            WFHttpContextValueProvider vp = new WFHttpContextValueProvider(request);
            UpdateModel<TModel>(vp, model, prefix, includeProperties, new string[] { });
        }
        /// <summary>
        /// Updates the specified model instance using values from the value provider.
        /// </summary>
        /// <typeparam name="TModel">The type of the model object.</typeparam>
        /// <param name="model">The model instance to update.</param>
        /// <param name="prefix">The prefix to use when looking up values in the value provider.</param>
        /// <param name="includeProperties">A list of properties of the model to update.</param>
        /// <param name="excludeProperties">A list of properties to explicitly exclude from the update. These are excluded even if they are listed in the includeProperties parameter list.</param>
        public static void UpdateModel<TModel>(IWFValueProvider request, TModel model, string prefix, string[] includeProperties, string[] excludeProperties)
        {
            Type t = typeof(TModel);
            PropertyInfo[] props = t.GetProperties();

            var query = props.AsQueryable();
            if (includeProperties != null)
            { query = query.Where(p => includeProperties.Contains(p.Name)); }
            if (excludeProperties != null && excludeProperties.Length > 0)
            { query = query.Where(p => !excludeProperties.Contains(p.Name)); }

            foreach (PropertyInfo pi in query)
            {
                if (request.ContainsKey(prefix + pi.Name))
                {
                    try
                    {
                        if (pi.PropertyType == typeof(Int32?))
                        {
                            if (String.IsNullOrEmpty(request.KeyValue(prefix + pi.Name).ToString()))
                            {
                                pi.SetValue(model, null, null);
                            }
                            else
                            {
                                pi.SetValue(model, int.Parse(request.KeyValue(prefix + pi.Name) as String), null);
                            }
                        }
                        else if (pi.PropertyType == typeof(Double?))
                        {
                            if (String.IsNullOrEmpty(request.KeyValue(prefix + pi.Name) as String))
                            {
                                pi.SetValue(model, null, null);
                            }
                            else
                            {
                                pi.SetValue(model, double.Parse(request.KeyValue(prefix + pi.Name) as String), null);
                            }
                        }
                        else if (pi.PropertyType == typeof(DateTime?))
                        {
                            if (String.IsNullOrEmpty(request.KeyValue(prefix + pi.Name) as String))
                            {
                                pi.SetValue(model, null, null);
                            }
                            else
                            {
                                pi.SetValue(model, DateTime.Parse(request.KeyValue(prefix + pi.Name) as String), null);
                            }
                        }
                        else if (pi.PropertyType == typeof(Boolean?) || pi.PropertyType == typeof(bool))
                        {
                            string[] trueValues = new string[] { "true", "true,false", "on" };

                            if (String.IsNullOrEmpty(request.KeyValue(prefix + pi.Name) as String)) //If the value passed is empty...
                            {
                                if (pi.PropertyType == typeof(Boolean?)) //..and it is nullable, set to null.
                                { pi.SetValue(model, null, null); }
                                else if (pi.PropertyType == typeof(bool)) //..and not nullable, set to false.
                                {
                                    pi.SetValue(model, false, null);
                                }
                            }
                            else if ((request.KeyValue(prefix + pi.Name) as String) == "off" || (request.KeyValue(prefix + pi.Name) as String) == "false") //If the value passed is false/off...
                            {
                                pi.SetValue(model, false, null); //...set to false.
                            }
                            else if (trueValues.Contains((request.KeyValue(prefix + pi.Name) as String))) //If the value passed is "true"...
                            {
                                pi.SetValue(model, true, null); //...set to true
                            }
                            else
                            {
                                //If all else fails, at least try to convert it to boolean
                                pi.SetValue(model, Convert.ChangeType((request.KeyValue(prefix + pi.Name) as String), pi.PropertyType), null);
                            }
                        }
                        else
                        {
                            Type[] defaultTypes = new Type[] 
                            { typeof(byte), typeof(short), typeof(int), typeof(long), typeof(float), typeof(double), typeof(decimal),
                              typeof(DateTime) };
                            if (defaultTypes.Contains(pi.PropertyType) && String.IsNullOrEmpty((request.KeyValue(prefix + pi.Name) as String)))
                            {
                                pi.SetValue(model, Activator.CreateInstance(pi.PropertyType), null); //Set to default value
                            }
                            else
                            {
                                pi.SetValue(model, Convert.ChangeType((request.KeyValue(prefix + pi.Name) as String), pi.PropertyType), null);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Can't cast type of property [" + pi.Name + "] value supplied [" + (request.KeyValue(prefix + pi.Name) ?? "null/empty").ToString() + "]");
                    }
                }
            }
        }
    }
}
