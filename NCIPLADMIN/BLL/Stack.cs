using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//Added
using System.Collections;
using PubEntAdmin.DAL;

namespace PubEntAdmin.BLL
{
    public class Stack /*Note: The class definition is different from NCIPL*/
    {
        private int _stackid;
        private string _stacktitle;
        private string _stackprodids = "";
        private int _stackstatus = 0; //0 is not active, 1 is active
        private int _stacksequence;

        #region Properties
        public int StackId
        {
            get { return _stackid; }
            set { _stackid = value; }
        }
        public string StackTitle
        {
            get { return _stacktitle; }
            set { _stacktitle = value; }
        }
        public string StackProdIds
        {
            get { return _stackprodids; }
            set { _stackprodids = value; }
        }
        public int StackStatus
        {
            get { return _stackstatus; }
            set { _stackstatus = value; }
        }
        public string StackStatusText
        {
            get { 
                if (_stackstatus == 0)
                    return "Inactive";
                else
                    return "Active";
                }
            //set { _stackpublongtitle = value; }
        }
        public int StackSequence
        {
            get { return _stacksequence; }
            set { _stacksequence = value; }
        }
        #endregion

        #region Constructors
        public Stack()
        {
        }

        public Stack(   int StackId, string StackProdIds, 
                        string StackTitle, int StackStatus, 
                        int StackSequence   )
        {
            _stackid = StackId;
            _stackprodids = StackProdIds;
            _stacktitle = StackTitle;
            _stackstatus = StackStatus;
            _stacksequence = StackSequence;
        }
        #endregion

    }
}
