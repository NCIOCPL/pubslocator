using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PubEntAdmin.BLL;
using PubEntAdmin.DAL;

namespace PubEntAdmin.BLL
{
    public class Attachment : MultiSelectListBoxItem
    {
        #region Fields
        private int intPubId;
        private int intCreatorUserId;
        private string strCreatorUsername;
        private int intFileSize;
        private string strContentType;
        private byte[] arrFileData;
        private DateTime dtDateCreated;

        #endregion

        #region Constructors
        public Attachment(int pubId, int creatorUserid, string creatorUsername, 
            string fileName, int fileSize, string contentType,  byte[] fileData) 
            : this(0, pubId, creatorUserid, creatorUsername, fileName, fileSize, contentType, DateTime.MinValue)
        {
            this.arrFileData = fileData;
        }

        public Attachment(int fileId, int pubId, int creatorUserid, string creatorUsername,
            string fileName, int fileSize, string contentType, DateTime dateCreated, byte[] fileData)
            : this (fileId, pubId, creatorUserid, creatorUsername,
            fileName, fileSize, contentType, dateCreated)
        {
            this.arrFileData = fileData;
        }

        public Attachment(int fileId, int pubId, int creatorUserid, string creatorUsername,
            string fileName, int fileSize, string contentType, DateTime dateCreated): base(fileId,fileName)
		{
            this.intPubId = pubId;
            this.intCreatorUserId = creatorUserid;
            this.strCreatorUsername = creatorUsername;
            this.intFileSize = fileSize;
            this.strContentType = contentType;
            this.dtDateCreated = dateCreated;
        }
        #endregion

        #region Methods
        public bool Save()
        {
            int TempId = PE_DAL.SetAttachment(this.PubId, this.CreatorUserId, this.CreatorUserName,
                this.FileName, this.FileSize, this.ContentType, this.arrFileData);
            if (TempId > 0)
            {
                this.ID = TempId;
                return true;
            }
            else
                return false;
        }

        public string FormatFileSize(int displayNumber)
        {
            int dwKB = 1024;          // Kilobyte
            int dwMB = 1024 * dwKB;   // Megabyte
            int dwGB = 1024 * dwMB;   // Gigabyte

            int dwNumber, dwRemainder;
            string strNumber = String.Empty;

            if (displayNumber < dwKB)
            {
                strNumber = String.Format("{0:n0} B", displayNumber);
            }
            else
            {
                if (displayNumber < dwMB)
                {
                    dwNumber = displayNumber / dwKB;
                    dwRemainder = (displayNumber * 100 / dwKB) % 100;
                    strNumber = String.Format("{0:n0}.{1} KB", dwNumber, dwRemainder);
                }
                else
                {
                    if (displayNumber < dwGB)
                    {
                        dwNumber = displayNumber / dwMB;
                        dwRemainder = (displayNumber * 100 / dwMB) % 100;
                        strNumber = String.Format("{0:n0}.{1} MB", dwNumber, dwRemainder);
                    }
                    else
                    {
                        if (displayNumber >= dwGB)
                        {
                            dwNumber = displayNumber / dwGB;
                            dwRemainder = (displayNumber * 100 / dwGB) % 100;
                            strNumber = String.Format("{0:n0}.{1} GB", dwNumber, dwRemainder);
                        }
                    }
                }
            }

            if (strNumber == null)
            {
                return String.Empty;
            }
            else
            {
                return strNumber;
            }
        }
        #endregion

        #region Properties

        public int CreatorUserId
        {
            get { return this.intCreatorUserId; }
        }

        public string CreatorUserName
        {
            get { return this.strCreatorUsername; }
        }

        public DateTime DateCreated
        {
            get { return this.dtDateCreated; }
        }

        public int AttachmentId
        {
            get { return this.ID; }
        }

        public string FileName
        {
            get { return this.Name; }
        }

        public string ContentType
        {
            get { return this.strContentType; }
        }

        public int FileSize
        {
            get { return this.intFileSize; }
        }

        public int PubId
        {
            get { return this.intPubId; }
            set
            {
                this.intPubId = value;
            }
        }

        public byte[] FileData
        {
            get
            {
                if (this.arrFileData != null && this.arrFileData.Length > 0)
                {
                    return this.arrFileData;
                }
                else
                {
                    //TODO: Get File Data from DB
                    return this.arrFileData;
                }
            }
            set { this.arrFileData = value; }
        }
        #endregion
    }
}
