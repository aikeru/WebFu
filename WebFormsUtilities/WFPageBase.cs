using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace WebFormsUtilities
{
    //Inheriting from WFPageBase and WFUserControlBase now only serves as a shortcut to declaring HtmlHelper (Html functions),
    //Model, and WFModelMetaData objects as well as bringing CallJSMethod() and EnableClientValidation() methods to markup without
    //registering namespaces.



    /// <summary>
    /// This class is no longer necessary for any WebFormsUtilities functionality.
    /// Call methods from the WFPageUtilities static class. Inheriting from this class can simplify some aspects.
    /// (ie: declaring HtmlHelper)
    /// </summary>
    public class WFPageBase : Page, IWebFormsView<WFPageBase>
    {
        private WFModelMetaData _WFMetaData = new WFModelMetaData();
        private HtmlHelper<WFPageBase> _Html = null;
        public virtual object Model { get; set; }
        public string EnableClientValidation()
        {
            return WFPageUtilities.EnableClientValidation(WFMetaData);
        }
        public void CallJSMethod()
        {
            WFPageUtilities.CallJSMethod(this, Request);
        }

        #region IWebFormsView<WFPageBase> Members

        public object GetModel()
        {
            return Model;
        }

        public void SetModel(object model)
        {
            Model = model;
        }

        public WFModelMetaData WFMetaData
        {
            get
            {
                if (_WFMetaData == null) { _WFMetaData = new WFModelMetaData(); }
                return _WFMetaData;
            }
            set
            {
                _WFMetaData = value;
            }
        }

        public HtmlHelper<WFPageBase> Html
        {
            get
            {
                if (_Html == null)
                { _Html = new HtmlHelper<WFPageBase>(this, Model, WFMetaData); }
                return _Html;
            }
            set
            {
                _Html = value;
            }
        }

        #endregion
    }
    /// <summary>
    /// This class is no longer necessary for any WebFormsUtilities functionality.
    /// Call methods from the WFPageUtilities static class. Inheriting from this class can simplify some aspects.
    /// (ie: declaring HtmlHelper)
    /// </summary>
    public class WFUserControlBase : UserControl
    {
        private WFModelMetaData _WFMetaData = new WFModelMetaData();
        private HtmlHelper<WFPageBase> _Html = null;
        public virtual object Model { get; set; }
        public string EnableClientValidation()
        {
            return WFPageUtilities.EnableClientValidation(WFMetaData);
        }
        public void CallJSMethod()
        {
            WFPageUtilities.CallJSMethod(this, Request);
        }

        #region IWebFormsView<WFPageBase> Members

        public object GetModel()
        {
            return Model;
        }

        public void SetModel(object model)
        {
            Model = model;
        }

        public WFModelMetaData WFMetaData
        {
            get
            {
                if (_WFMetaData == null) { _WFMetaData = new WFModelMetaData(); }
                return _WFMetaData;
            }
            set
            {
                _WFMetaData = value;
            }
        }

        public HtmlHelper<WFPageBase> Html
        {
            get
            {
                if (_Html == null)
                { _Html = new HtmlHelper<WFPageBase>(this, Model, WFMetaData); }
                return _Html;
            }
            set
            {
                _Html = value;
            }
        }

        #endregion
    }
}
