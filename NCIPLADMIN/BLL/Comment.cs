using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PubEntAdmin.DAL;

namespace PubEntAdmin.BLL
{
    public class Comment : MultiSelectListBoxItem
    {
        #region Fields
        private int intPubId;
        private int intCreatorUserId;
        private string strCreatorUsername;
        private string strComment;
        private DateTime dtDateCreated;
        #endregion

        #region Constructors
        public Comment(int commentId, int PubId, string comment, int creatorUserId, 
            string creatorUsername, DateTime created) :base(commentId,""){
            if (comment == null ||comment.Length==0 )
                throw (new ArgumentOutOfRangeException("comment"));

            //this.intId = commentId;
            this.intPubId = PubId;
            this.intCreatorUserId = creatorUserId;
            this.strCreatorUsername = creatorUsername;
            this.strComment = comment;
            this.dtDateCreated = created;
        }

        public Comment(int PubId, string comment, int creatorUserId, string creatorUsername)
            : base(0, "")
        {
            if (comment == null || comment.Length == 0)
                throw (new ArgumentOutOfRangeException("comment"));

            this.intPubId = PubId;
            this.intCreatorUserId = creatorUserId;
            this.strCreatorUsername = creatorUsername;
            this.strComment = comment;
        }
        #endregion

        #region Properties
        public string CommentContent
        {
            get
            {
                if (strComment == null || strComment.Length == 0)
                    return string.Empty;
                else
                    return strComment;
            }

            set { strComment = value; }
        }

        public int CreatorUserId
        {
            get
            {
                if (intCreatorUserId == null || intCreatorUserId == 0)
                    return 0;
                else
                    return this.intCreatorUserId;
            }
        }

        public string CreatorUsername
        {
            get
            {
                if (strCreatorUsername == null || strCreatorUsername.Length == 0)
                    return string.Empty;
                else
                    return strCreatorUsername;
            }
        }

        public DateTime DateCreated
        {
            get { return dtDateCreated; }
        }

        public int CommentId
        {
            get { return this.ID; }
        }

        public int PubId
        {
            get { return this.intPubId; }
            set
            {
                this.intPubId = value;
            }
        }
        #endregion

        #region Methods
        public bool Save()
        {
            int TempId = PE_DAL.SetComment(this.PubId, this.CreatorUserId,this.CreatorUsername,this.CommentContent);
            if (TempId > 0)
            {
                this.ID = TempId;
                return true;
            }
            else
                return false;
        }
        #endregion

    }
}
