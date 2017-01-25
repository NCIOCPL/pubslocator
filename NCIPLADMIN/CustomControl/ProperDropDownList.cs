using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace PubEntAdmin.CustomControl
{
    public class ProperDropDownList : DropDownList
    {
        public ProperDropDownList() : base() { }

        //override the control state methods to see if can get the selected index
        //set prior to the post back happening
        protected override void OnInit(EventArgs e)
        {
            Page.RegisterRequiresControlState(this);
            base.OnInit(e);
        }
        protected override void LoadControlState(object savedState)
        {
            object[] controlState = (object[])savedState;
            base.LoadControlState(controlState[0]);
            base.SelectedIndex = (int)controlState[1];
        }
        protected override object SaveControlState()
        {
            object[] controlState = new object[2];
            controlState[0] = base.SaveControlState();
            controlState[1] = base.SelectedIndex;
            return controlState;
        }
    }
}
