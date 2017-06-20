using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for cs_record
/// </summary>
namespace PubEntAdmin.BLL
{
    [Serializable]
    public class cs_record
    {
        private int _cannid = 0;
        private string _recid = "";
        private string _headertext = "";
        private string _cancertype = "";
        private string _subject = "";
        private string _pubformat = "";
        private string _race = "";
        private string _audience = "";
        private string _language = "";
        private string _collections = "";
        private int _active = 1;

        public cs_record()
        {
        }
        
        public cs_record
            (
                int cannid,
                string recid,
                string headertext,
                string audience,
                string cancertype,
                string language,
                string pubformat,
                string race,
                string collections,
                string subject,
                int active
            )
        {
            _cannid = cannid;
            _recid = recid;
            _headertext = headertext;
            _audience = audience.Replace("^~", ", ");
            _cancertype = cancertype.Replace("^~", ", "); 
            _language = language.Replace("^~", ", "); 
            _pubformat = pubformat.Replace("^~", ", "); 
            _race = race.Replace("^~", ", "); ;
            _collections = collections.Replace("^~", ", "); 
            _subject = subject.Replace("^~", ", "); 
            _active = active;
        }

        //Properties
        public int CannId
        {
            get { return _cannid; }
        }
        public string RecId
        {
            get { return _recid; }
        }
        public string HeaderText
        {
            get { return _headertext; }
        }
        public string Audience
        {
            get { return _audience; }
        }
        public string CancerType
        {
            get { return _cancertype; }
        }
        public string Language
        {
            get { return _language; }
        }
        public string PubFormat
        {
            get { return _pubformat; }
        }
        public string Race
        {
            get { return _race; }
        }
        public string Collections
        {
            get { return _collections; }
        }
        public string Subject
        {
            get { return _subject; }
        }
        public int Active
        {
            get { return _active; }
        }
    }
}