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

namespace PubEntAdmin.BLL
{
    public class Seq : MultiSelectListBoxItem
    {
        #region Fields
        //private int intSeqId;
        //private string strSeqName;
        private int intSeqValue;
        #endregion

        #region Constructors
        public Seq(int seqId, string seqname, int seqvalue)
            : base(seqId, seqname)
        {
            this.intSeqValue = seqvalue;
        }
        #endregion

        #region Properties
        public int SeqID
        {
            get { return this.ID; }
            set { this.ID = value; }
        }

        public string SeqName
        {
            get { return this.Name; }
            set { this.Name = value; }
        }

        public int SeqValue
        {
            get { return this.intSeqValue; }
            set { this.intSeqValue = value; }
        }
        #endregion
    }
}
