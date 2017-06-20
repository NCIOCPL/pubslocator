using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PubEntAdmin.BLL
{
    //Class represents a single Stack History Record
    [Serializable]
    public class StackHistRec
    {
        private int _refid = -99;
        private int _stackid = -99;
        private string _stacktitle = "";
        private int _pubid = 0;
        private string _productid = "";
        private string _longtitle = "";
        private DateTime _stackstartdate = DateTime.MinValue;
        private DateTime _stackenddate = DateTime.MinValue;
        private DateTime _pubstartdate = DateTime.MinValue;
        private DateTime _pubenddate = DateTime.MinValue;

        public int RefId
        {
            get { return _refid; }
            set { _refid = value; }
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
        public int PubId
        {
            get { return _pubid; }
            set { _pubid = value; }
        }
        public string ProductId
        {
            get { return _productid; }
            set { _productid = value; }
        }
        public string LongTitle
        {
            get { return _longtitle; }
            set { _longtitle = value; }
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
        public DateTime PubStartDate
        {
            get { return _pubstartdate; }
            set { _pubstartdate = value; }
        }
        public DateTime PubEndDate
        {
            get { return _pubenddate; }
            set { _pubenddate = value; }
        }

        public StackHistRec( int RefId,
                             int StackId,
                             string StackTitle,
                             int PubId,
                             string ProductId,
                             string LongTitle,
                             DateTime StackStartDate,
                             DateTime StackEndDate,
                             DateTime PubStartDate,
                             DateTime PubEndDate)
        {

            _refid = RefId;
            _stackid = StackId;
            _stacktitle = StackTitle;
            _pubid = PubId;
            _productid = ProductId;
            _longtitle = LongTitle;
            _stackstartdate = StackStartDate;
            _stackenddate = StackEndDate;
            _pubstartdate = PubStartDate;
            _pubenddate = PubEndDate;

        }



    }
}
