using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//Added
using System.Collections;
using PubEnt.DAL;

namespace PubEnt.BLL
{
    public class Stack
    {
        private int _stackid;
        private string _stacktitle;
        private int _stackpubpubid;
        private string _stackpubprodid;
        private string _stackpublongtitle = "";
        private string _stackpubshorttitle = "";
        private string _stackpubfeaturedimage = "";
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
        public int StackPubPubId
        {
            get { return _stackpubpubid; }
            set { _stackpubpubid = value; }
        }
        public string StackPubProdId
        {
            get { return _stackpubprodid; }
            set { _stackpubprodid = value; }
        }
        public string StackPubLongTitle
        {
            get { return _stackpublongtitle; }
            set { _stackpublongtitle = value; }
        }
        public string StackPubShortTitle
        {
            get { return _stackpubshorttitle; }
            set { _stackpubshorttitle = value; }
        }
        public string StackPubFeaturedImage
        {
            get { return _stackpubfeaturedimage; }
            set { _stackpubfeaturedimage = value; }
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

        public Stack(   int StackId, string StackTitle, int StackPubId, 
                        string StackPubProdId, string StackPubLongTitle, string StackPubShortTitle,
                        string StackPubFeaturedImage, int StackSequence)
        {
            _stackid = StackId;
            _stacktitle = StackTitle;
            _stackpubpubid = StackPubId;
            _stackpubprodid = StackPubProdId;
            _stackpublongtitle = StackPubLongTitle;
            _stackpubshorttitle = StackPubShortTitle;
            _stackpubfeaturedimage = StackPubFeaturedImage;
            _stacksequence = StackSequence;
        }
        #endregion

    }
}
