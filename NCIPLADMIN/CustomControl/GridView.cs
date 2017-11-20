using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace PubEntAdmin.CustomControl
{
    public class GridView : System.Web.UI.WebControls.GridView
    {
        #region Properties
        #region Style properties
        private TableItemStyle _sortAscendingStyle = null;
        private TableItemStyle _sortDescendingStyle = null;

        [Description("The style applied to the header cell that is sorted in ascending order."), Themeable(true), Category("Styles"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
        public TableItemStyle SortAscendingStyle
        {
            get
            {
                if (_sortAscendingStyle == null)
                {
                    _sortAscendingStyle = new TableItemStyle();
                    if (base.IsTrackingViewState)
                        ((IStateManager)_sortAscendingStyle).TrackViewState();
                }

                return _sortAscendingStyle;
            }
        }

        [Description("The style applied to the header cell that is sorted in descending order."), Themeable(true), Category("Styles"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(true), PersistenceMode(PersistenceMode.InnerProperty)]
        public TableItemStyle SortDescendingStyle
        {
            get
            {
                if (_sortDescendingStyle == null)
                {
                    _sortDescendingStyle = new TableItemStyle();
                    if (base.IsTrackingViewState)
                        ((IStateManager)_sortDescendingStyle).TrackViewState();
                }

                return _sortDescendingStyle;
            }
        }
        #endregion

        #region ArrowUpImageUrl
        [Description("The url of the image shown when a column is sorted in ascending order."), DefaultValue(""), Themeable(true), Category("Appearance")]
        public string ArrowUpImageUrl
        {
            get
            {
                string str = ViewState["ArrowUpImageUrl"] as string;
                if (str == null)
                    return string.Empty;
                else
                    return str;
            }
            set
            {
                ViewState["ArrowUpImageUrl"] = value;
            }
        }

        protected virtual string ArrowUpImageUrlInternal
        {
            get
            {
                if (string.IsNullOrEmpty(ArrowUpImageUrl))
                    return Page.ClientScript.GetWebResourceUrl(this.GetType(), "skmControls2.ArrowUp.gif");
                else
                    return ArrowUpImageUrl;
            }
        }
        #endregion

        #region ArrowDownImageUrl
        [Description("The url of the image shown when a column is sorted in descending order."), DefaultValue(""), Themeable(true), Category("Appearance")]
        public string ArrowDownImageUrl
        {
            get
            {
                string str = ViewState["ArrowDownImageUrl"] as string;
                if (str == null)
                    return string.Empty;
                else
                    return str;
            }
            set
            {
                ViewState["ArrowDownImageUrl"] = value;
            }
        }

        protected virtual string ArrowDownImageUrlInternal
        {
            get
            {
                if (string.IsNullOrEmpty(ArrowDownImageUrl))
                    return Page.ClientScript.GetWebResourceUrl(this.GetType(), "skmControls2.ArrowDown.gif");
                else
                    return ArrowDownImageUrl;
            }
        }
        #endregion
        #endregion

        #region "Methods"
        protected override void PrepareControlHierarchy()
        {
            base.PrepareControlHierarchy();

            if (this.HasControls())
            {
                if (!string.IsNullOrEmpty(this.SortExpression) && this.ShowHeader)
                {
                    Table table = this.Controls[0] as Table;

                    if (table != null && table.Rows.Count > 0)
                    {
                        // Need to check first TWO rows because the first row may be a
                        // pager row... 
                        GridViewRow headerRow = table.Rows[0] as GridViewRow;
                        if (headerRow.RowType != DataControlRowType.Header && table.Rows.Count > 1)
                            headerRow = table.Rows[1] as GridViewRow;

                        if (headerRow.RowType == DataControlRowType.Header)
                        {
                            foreach (TableCell cell in headerRow.Cells)
                            {
                                DataControlFieldCell gridViewCell = cell as DataControlFieldCell;
                                if (gridViewCell != null)
                                {
                                    DataControlField cellsField = gridViewCell.ContainingField;
                                    if (cellsField != null && cellsField.SortExpression == this.SortExpression)
                                    {
                                        // Add the sort arrows for this cell
                                        CreateSortArrows(cell);

                                        // We're done!
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        protected virtual void CreateSortArrows(TableCell sortedCell)
        {
            // Add the appropriate arrow image and apply the appropriate state, depending on whether we're
            // sorting the results in ascending or descending order
            TableItemStyle sortStyle = null;
            string imgUrl = null;

            if (this.SortDirection == SortDirection.Ascending)
            {
                imgUrl = this.ArrowUpImageUrlInternal;
                sortStyle = _sortAscendingStyle;
            }
            else
            {
                imgUrl = this.ArrowDownImageUrlInternal;
                sortStyle = _sortDescendingStyle;
            }

            Image arrow = new Image();
            arrow.ImageUrl = imgUrl;
            arrow.BorderStyle = BorderStyle.None;
            sortedCell.Controls.Add(arrow);

            if (sortStyle != null)
                sortedCell.MergeStyle(sortStyle);
        }

        #region State Management Methods
        protected override object SaveViewState()
        {
            // We need to save any programmatic changes to the SortAscendingStyle or SortDescendingStyle
            // properties to view state...
            object[] state = new object[3];
            state[0] = base.SaveViewState();
            if (_sortAscendingStyle != null)
                state[1] = ((IStateManager)_sortAscendingStyle).SaveViewState();
            if (_sortDescendingStyle != null)
                state[2] = ((IStateManager)_sortDescendingStyle).SaveViewState();

            return state;
        }

        protected override void LoadViewState(object savedState)
        {
            object[] state = (object[])savedState;

            base.LoadViewState(state[0]);

            if (state[1] != null)
                ((IStateManager)this.SortAscendingStyle).LoadViewState(state[1]);
            if (state[2] != null)
                ((IStateManager)this.SortDescendingStyle).LoadViewState(state[2]);
        }

        protected override void TrackViewState()
        {
            base.TrackViewState();

            if (_sortAscendingStyle != null)
                ((IStateManager)_sortAscendingStyle).TrackViewState();
            if (_sortDescendingStyle != null)
                ((IStateManager)_sortDescendingStyle).TrackViewState();
        }
        #endregion


        /* This method was deprecated with the code update on February 6th, 2008.
         * For more information, see:
         *      http://aspnet.4guysfromrolla.com/articles/020608-1.aspx
         */
        //protected override void OnSorted(EventArgs e)
        //{
        //    string imgArrowUp = string.Format(@" <img border=""0"" src=""{0}"" />", Page.ResolveUrl(ArrowUpImageUrlInternal));
        //    string imgArrowDown = string.Format(@" <img border=""0"" src=""{0}"" />", Page.ResolveUrl(ArrowDownImageUrlInternal));

        //    foreach (DataControlField field in this.Columns)
        //    {
        //        // strip off the old ascending/descending icon
        //        int iconPosition = field.HeaderText.IndexOf(@" <img border=""0"" src=""");
        //        if (iconPosition > 0)
        //            field.HeaderText = field.HeaderText.Substring(0, iconPosition);

        //        // See where to add the sort ascending/descending icon
        //        if (field.SortExpression == this.SortExpression)
        //        {
        //            if (this.SortDirection == SortDirection.Ascending)
        //                field.HeaderText += imgArrowUp;
        //            else
        //                field.HeaderText += imgArrowDown;
        //        }
        //    }

        //    base.OnSorted(e);
        //}
        #endregion
    }
}
