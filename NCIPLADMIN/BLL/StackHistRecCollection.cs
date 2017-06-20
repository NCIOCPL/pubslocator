using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//added
using System.Collections;

namespace PubEntAdmin.BLL
{
    //Class represents a Stack History Record Collection
    [Serializable]
    public class StackHistRecCollection:CollectionBase
    {
        private int _id = -99;
        private int _stackid = -99;
        private string _stacktitle = "";
        private DateTime _stackstartdate = DateTime.MinValue;
        private DateTime _stackenddate = DateTime.MinValue;

        #region Properties
        
            public int Id
            {
                get { return _id; }
                set { _id = value; }
            }
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
            public DateTime StackStartDate
            {
                get { return _stackstartdate; }
                set { _stackstartdate = value; }
            }
            public DateTime StackEndDate
            {
                get { return _stackenddate; }
                set { _stackenddate = value; }
            }

        #endregion

            #region Methods
            
            public int Add(StackHistRec value)
            {
                return (List.Add(value));
            }
        
        #endregion

    }
}
