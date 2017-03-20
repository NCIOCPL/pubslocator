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

using PubEntAdmin.DAL;

namespace PubEntAdmin.BLL
{
    public class PubHist : MultiSelectListBoxItem
    {
        private int phdid;
        private string strNihNum1;
        private string strNihNum2;
        private int dtRevised_M;
        private int dtRevised_D;
        private int dtRevised_Y;
        private int dtLatestPrint_M;
        private int dtLatestPrint_D;
        private int dtLatestPrint_Y;
        private DateTime dtReceived;
        private int intQtyReceived;

        public PubHist(int phdid, int pubid, string nihNum1, string nihNum2, string name,
            int RevisedDate_M, int RevisedDate_D, int RevisedDate_Y,
            int LatestPrintDate_M, int LatestPrintDate_D, int LatestPrintDate_Y, DateTime ReceivedDate,
            int QtyReceived)
            : base(pubid, name)
        {
            this.phdid = phdid;
            this.strNihNum1 = nihNum1;
            this.strNihNum2 = nihNum2;
            this.dtRevised_M = RevisedDate_M;
            this.dtRevised_D = RevisedDate_D;
            this.dtRevised_Y = RevisedDate_Y;
            this.dtLatestPrint_M = LatestPrintDate_M;
            this.dtLatestPrint_D = LatestPrintDate_D;
            this.dtLatestPrint_Y = LatestPrintDate_Y;
            this.dtReceived = ReceivedDate;
            this.intQtyReceived = QtyReceived;
        }

        public PubHist(int pubid, string name)
            : base(pubid, name) { }

        public int PhdID
        {
            get { return this.phdid; }
            set { this.phdid = value; }
        }

        public int PubID
        {
            get { return this.ID; }
            set { this.ID = value; }
        }

        public string PubName
        {
            get { return this.Name; }
            set { this.Name = value; }
        }

        public string NIHNum1
        {
            get { return this.strNihNum1; }
            set { this.strNihNum1 = value; }
        }

        public string NIHNum2
        {
            get { return this.strNihNum2; }
            set { this.strNihNum2 = value; }
        }

        public int RevisedDate_M
        {
            get { return this.dtRevised_M; }
            set { this.dtRevised_M = value; }
        }

        public int RevisedDate_D
        {
            get { return this.dtRevised_D; }
            set { this.dtRevised_D = value; }
        }

        public int RevisedDate_Y
        {
            get { return this.dtRevised_Y; }
            set { this.dtRevised_Y = value; }
        }

        public int LatestPrintDate_M
        {
            get { return this.dtLatestPrint_M; }
            set { this.dtLatestPrint_M = value; }
        }

        public int LatestPrintDate_D
        {
            get { return this.dtLatestPrint_D; }
            set { this.dtLatestPrint_D = value; }
        }

        public int LatestPrintDate_Y
        {
            get { return this.dtLatestPrint_Y; }
            set { this.dtLatestPrint_Y = value; }
        }

        public DateTime ReceivedDate
        {
            get { return this.dtReceived; }
            set { this.dtReceived = value; }
        }

        public int QtyReceived
        {
            get { return this.intQtyReceived; }
            set { this.intQtyReceived = value; }
        }

        //public static PubHistCollection GetPubHist()
        //{
        //    return (PE_DAL.GetPubHist());
        //}
    }
}
