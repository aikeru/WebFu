using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Reflection;

namespace WebFormsUtilities.WebControls
{
    [ParseChildren(true)]
    [ToolboxData("<{0}:GridControl runat=server></{0}:GridControl>")]
    public class GridControl : WebControl
    {
        private TableItemStyle _HeaderStyle = null;
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public TableItemStyle HeaderStyle
        {
            get { return _HeaderStyle; }
            set { _HeaderStyle = value; }
        }
        
        

        private object _DataSource = null;
        public object DataSource
        { 
            get
            {
                return _DataSource;
            }
            set
            {
                if (value as IEnumerable != null || value as IListSource != null)
                {
                    _DataSource = value;
                }
                else
                {
                    throw new Exception("An invalid data source is being used for " + this.ID + ". A valid data source must implement either IListSource or IEnumerable.");
                }
            }
        }

        public override void DataBind()
        {
            //Whatever we need to do here...
            base.DataBind();
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            HtmlTag tblGrid = new HtmlTag("table");
            tblGrid.Attr("cellspacing", "0");
            tblGrid.Attr("rules", "all");
            tblGrid.Attr("border", "1");
            tblGrid.Attr("id", this.ClientID);
            tblGrid.Attr("style", "border-collapse:collapse;");
            
            HtmlTag trHeader = new HtmlTag("tr");
            WebControlUtilities.ApplyTableItemStyleToTag(trHeader, HeaderStyle);

            IEnumerable ds = DataSource as IEnumerable;
            if (ds == null)
            {
                ds = (DataSource as IListSource).GetList() as IEnumerable;
            }
            //asp.net's datagrid requires all objects be the same type as the first object
            Type dataType = null;
            PropertyInfo[] properties = null;
            IEnumerator ie = ds.GetEnumerator();
            List<GridControlRow> rowDef = new List<GridControlRow>();
            //Get property data from the first element
            if (ie.MoveNext())
            {
                dataType = ie.Current.GetType();
                properties = dataType.GetProperties();

                HtmlTag firstRow = new HtmlTag("tr");

                properties = properties ?? ie.Current.GetType().GetProperties();
                //Create rows   
                foreach (PropertyInfo pi in properties)
                {
                    rowDef.Add(new GridControlRow() { ColumnName = pi.Name });
                    HtmlTag tdHdr = new HtmlTag("td") { InnerText = pi.Name };
                    trHeader.Children.Add(tdHdr);
                    firstRow.Children.Add(new HtmlTag("td") { InnerText = pi.GetValue(ie.Current, null).ToString() });
                }

                tblGrid.Children.Add(trHeader);
                tblGrid.Children.Add(firstRow);

                //Get the rest
                while (ie.MoveNext())
                {
                    HtmlTag currentRow = new HtmlTag("tr");
                    foreach (PropertyInfo pi in properties)
                    {
                        currentRow.Children.Add(new HtmlTag("td") { InnerText = pi.GetValue(ie.Current, null).ToString() });
                    }
                    tblGrid.Children.Add(currentRow);
                }

            }

            tblGrid.MergeObjectProperties(this.Attributes);
            
            output.Write(tblGrid.Render());
        }

        protected override void Render(HtmlTextWriter writer)
        {
            RenderContents(writer);
        }

    }

    public class GridControlRow
    {
        public string ColumnName { get; set; }
    }
}
