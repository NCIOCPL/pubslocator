using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//adding
using PubEntAdmin.BLL;

namespace PubEntAdmin.UserControl
{
    public partial class FeaturedPubs : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //BindGrid();
        }

        private void BindGrid()
        {
            int paramActive = (this.chkActive.Checked == true) ? 1 : 0;
            string paramTitle = this.txtSearch.Text.Trim();
            this.gvStacks.DataSource = DAL.PE_DAL.GetFeaturedStacks(paramActive, paramTitle);
            this.gvStacks.DataBind();
            if (gvStacks.Rows.Count > 0)
            {
                gridPanel.Visible = true;
                lblMessage.Visible = false;
            }
            else
            {
                gridPanel.Visible = false;
                lblMessage.Text = "No records were returned by the search.";
                lblMessage.Visible = true;
            }
        }

        protected void gvStacks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Stack stackItem = (Stack)e.Row.DataItem;
                
                HiddenField hidStackId = (HiddenField)e.Row.FindControl("hidStackId");
                TextBox txtStackTitle = (TextBox)e.Row.FindControl("txtStackTitle");
                Label lblStackTitle = (Label)e.Row.FindControl("lblStackTitle");
                Button btnChangeStatus = (Button)e.Row.FindControl("btnChangeStatus");
                Button btnDelete = (Button)e.Row.FindControl("btnDelete");
                TextBox txtStackSequence = (TextBox)e.Row.FindControl("txtStackSequence");

                hidStackId.Value = stackItem.StackId.ToString();
                txtStackTitle.Text = stackItem.StackTitle;
                lblStackTitle.Text = stackItem.StackTitle;
                //Stack Id: 1 is New Publications Stack, Stack Id: 2 is Updated Publications
                if (string.Compare(hidStackId.Value, "1", true) == 0 || string.Compare(hidStackId.Value, "2", true) == 0)
                {
                    lblStackTitle.Visible = true;
                    txtStackTitle.Visible = false;
                    btnChangeStatus.Visible = false;
                    btnDelete.Visible = false;
                }
                //btnChangeStatus.Text = stackItem.StackStatusText;
                txtStackSequence.Text = stackItem.StackSequence.ToString();

                //Add Command Name and Arguments to the both buttons
                btnChangeStatus.CommandArgument = hidStackId.Value;

                Label lblConfirmA = (Label)e.Row.FindControl("lblConfirmA");
                if (stackItem.StackStatusText == "Inactive")
                {
                    btnChangeStatus.Text = "  Activate  ";
                    btnChangeStatus.CommandName = "Activate";
                    lblConfirmA.Text = "Are you sure you want to activate the stack \"" + txtStackTitle.Text + "\"";
                    if (stackItem.StackProdIds.Length == 0)
                        btnChangeStatus.Enabled = false;
                }
                else
                {
                    btnChangeStatus.Text = "Inactivate";
                    btnChangeStatus.CommandName = "Inactivate";
                    lblConfirmA.Text = "Are you sure you want to inactivate the stack \"" + txtStackTitle.Text + "\"";
                }

                btnDelete.CommandName = "DeleteRecord";
                btnDelete.CommandArgument = hidStackId.Value;
                
                Label lblConfirmDel = (Label)e.Row.FindControl("lblConfirmDel");
                lblConfirmDel.Text = "Are you sure you want to delete the stack \"" + txtStackTitle.Text + "\"";

            }
        }

        protected void gvStacks_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string StackId = e.CommandArgument.ToString();

            if (e.CommandName == "DeleteRecord")
            {
                //Delete code here
                DAL.PE_DAL.SetFeaturedStacks("DEL", StackId, "", "", "");
            }
            else //Change Status action
            {
                //Status change code here
                string ReqAction = e.CommandName;
                if (string.Compare(ReqAction, "Activate", true) == 0)
                {
                    DAL.PE_DAL.SetFeaturedStacks("ACT", StackId, "", "", "");
                }
                else if (string.Compare(ReqAction, "Inactivate", true) == 0)
                {
                    DAL.PE_DAL.SetFeaturedStacks("INA", StackId, "", "", "");
                }
            }
            BindGrid();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void btnSaveChanges_Click(object sender, EventArgs e)
        {
            #region Screen_Check
            System.Collections.ArrayList arrStackIdList = new System.Collections.ArrayList();
            System.Collections.ArrayList arrTitleList = new System.Collections.ArrayList();
            System.Collections.ArrayList arrSequenceList = new System.Collections.ArrayList();

            #region Add_to_ArrayList
            foreach (GridViewRow Item in gvStacks.Rows)
            {
                HiddenField hidStackId = (HiddenField)Item.FindControl("hidStackId");
                TextBox txtStackTitle = (TextBox)Item.FindControl("txtStackTitle");
                TextBox txtStackSequence = (TextBox)Item.FindControl("txtStackSequence");
               
                arrStackIdList.Add(hidStackId.Value);
                arrTitleList.Add(txtStackTitle.Text);
                arrSequenceList.Add(txtStackSequence.Text);
            }
            #endregion

            foreach (GridViewRow Item in gvStacks.Rows)
            {
                HiddenField hidStackId = (HiddenField)Item.FindControl("hidStackId");
                TextBox txtStackTitle = (TextBox)Item.FindControl("txtStackTitle");
                TextBox txtStackSequence = (TextBox)Item.FindControl("txtStackSequence");
                Label lblMessage = (Label)Item.FindControl("lblMessage");

                //Check for duplicate title on the grid
                for (int i = 0; i<arrStackIdList.Count; i++)
                {
                    if (    string.Compare(hidStackId.Value, arrStackIdList[i].ToString(), true) != 0
                            && string.Compare(txtStackTitle.Text.Trim(), arrTitleList[i].ToString().Trim(), true) == 0 
                       )
                    {
                        //lblMessage.Text = "Duplicate Title found. Please provide unique titles.";
                        lblMessage.Text = "Duplicate Title found.";
                        return;
                    }
                    else
                        lblMessage.Text = "";
                   
                }

                //Check for duplicate stack id on the grid
                for (int i = 0; i < arrStackIdList.Count; i++)
                { 
                    if (    string.Compare(hidStackId.Value, arrStackIdList[i].ToString(), true) != 0
                            && string.Compare(txtStackSequence.Text.Trim(), arrSequenceList[i].ToString().Trim(), true) == 0
                       )
                    {
                        //lblMessage.Text = "Duplicate Sequence Number found. Please provide unique numbers.";
                        lblMessage.Text = "Duplicate Sequence Number found.";
                        return;
                    }
                    else
                        lblMessage.Text = "";
                }

               
            }

            //int f;

            #endregion
            StackCollection StackColl = DAL.PE_DAL.GetFeaturedStacks(0, ""); //Original Stack

            string StackIds = "";
            string Titles = "";
            string Sequences = "";
            string Delim = PubEntAdminManager.stringDelim;

            foreach (GridViewRow Item in gvStacks.Rows)
            {
                HiddenField hidStackId = (HiddenField)Item.FindControl("hidStackId");
                TextBox txtStackTitle = (TextBox)Item.FindControl("txtStackTitle");
                TextBox txtStackSequence = (TextBox)Item.FindControl("txtStackSequence");
                Label lblMessage = (Label)Item.FindControl("lblMessage");

                foreach (Stack StackItem in StackColl)
                {
                    //Check for empty title
                    if (string.Compare(txtStackTitle.Text.Trim(), "", true) == 0)
                    {
                        lblMessage.Text = "Empty Title found.";
                        return;
                    }
                    else
                        lblMessage.Text = "";

                    //Check for empty sequence number
                    if (string.Compare(txtStackSequence.Text.Trim(), "", true) == 0)
                    {
                        lblMessage.Text = "Empty Sequence Number found.";
                        return;
                    }
                    else
                        lblMessage.Text = "";

                    ////Check for duplicate stack title
                    //if (string.Compare(txtStackTitle.Text.Trim(), StackItem.StackTitle, true) == 0
                    //    && string.Compare(hidStackId.Value, StackItem.StackId.ToString(), true) != 0)
                    //{
                    //    //lblMessage.Text = "Duplicate Title found. Please provide unique titles.";
                    //    lblMessage.Text = "Duplicate Title found.";
                    //    return;
                    //}
                    //else
                    //    lblMessage.Text = "";
                    
                    ////Check for duplicate sequence number
                    //if (string.Compare(txtStackSequence.Text.Trim(), StackItem.StackSequence.ToString(), true) == 0
                    //    && string.Compare(hidStackId.Value, StackItem.StackId.ToString(), true) != 0)
                    //{
                    //    //lblMessage.Text = "Duplicate Sequence Number found. Please provide unique numbers.";
                    //    lblMessage.Text = "Duplicate Sequence Number found.";
                    //    return;
                    //}
                    //else
                    //    lblMessage.Text = "";


                    //Check for duplicate stack title
                    if (string.Compare(txtStackTitle.Text.Trim(), StackItem.StackTitle, true) == 0
                        && !arrStackIdList.Contains(StackItem.StackId.ToString()))
                    {
                        //lblMessage.Text = "Duplicate Title found. Please provide unique titles.";
                        lblMessage.Text = "Duplicate Title found in the database table.";
                        return;
                    }
                    else
                        lblMessage.Text = "";

                    //Check for duplicate sequence number
                    if (string.Compare(txtStackSequence.Text.Trim(), StackItem.StackSequence.ToString(), true) == 0
                        && !arrStackIdList.Contains(StackItem.StackId.ToString()))
                    {
                        //lblMessage.Text = "Duplicate Sequence Number found. Please provide unique numbers.";
                        lblMessage.Text = "Duplicate Sequence Number found in the database table.";
                        return;
                    }
                    else
                        lblMessage.Text = "";
                        
                }



                if (StackIds.Length > 0)
                    StackIds += Delim + hidStackId.Value;
                else
                    StackIds = hidStackId.Value;

                if (Titles.Length > 0)
                    Titles += Delim + txtStackTitle.Text.Trim();
                else
                    Titles = txtStackTitle.Text.Trim();

                if (Sequences.Length > 0)
                    Sequences += Delim + txtStackSequence.Text.Trim();
                else
                    Sequences = txtStackSequence.Text.Trim();
            }

            //Execute the update
            DAL.PE_DAL.SetFeaturedStacks("UPD", StackIds, Titles, Sequences, Delim);
            BindGrid();
        }

        protected void btnCancelChanges_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string Titles = txtTitle.Text.Trim();
            string Sequences = txtSequence.Text.Trim();

            StackCollection StackColl = DAL.PE_DAL.GetFeaturedStacks(0, ""); //Original Stack
            foreach (Stack StackItem in StackColl)
            {
                if (string.Compare(Titles, StackItem.StackTitle, true) == 0)
                {
                    ReqFieldValTitle.Text = "The title already exists.";
                    ReqFieldValTitle.IsValid = false;
                    return;
                }
            }
            
            DAL.PE_DAL.SetFeaturedStacks("ADD", "", Titles, Sequences, "");
            BindGrid();
            txtTitle.Text = ""; txtSequence.Text = "";
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtTitle.Text = "";
            txtSequence.Text = "";
        }
    }
}