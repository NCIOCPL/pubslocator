using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//added
using NCICatalog.BLL;
using NCICatalog.DAL;

namespace NCICatalog.BLL
{
    [Serializable]
    public class Category
    {
        private int _subjectid;
        private int _subjectcode; //same as _subjectid
        private string _subjectdesc;
        private int _hassubcategoriesflag;
        private int _subcategoryid;
        private int _subcategorycode; //same as _subcategoryid
        private string _subcategorydesc;
        private int _catalogsubcategorytype;
        private int _hassusbsubcategoriesflag;
        private int _subsubcategoryid;
        private string _subsubcategorydesc;

        public Category()
        {
        }

        //For sp_CATALOG_getCategories
        public Category(int subjectid,
                        string subjectdesc,
                        int catalogsubcategorytype,
                        int hassubcategoriesflag)
        {
            _subjectid = subjectid;
            _subjectdesc = subjectdesc;
            _catalogsubcategorytype = catalogsubcategorytype;
            _hassubcategoriesflag = hassubcategoriesflag;
        }

        //For sp_CATALOG_getSubCategoriesBySubjectId
        public Category(int subcategoryid,
                        string subcategorydesc,
                        int subjectcode,
                        int hassubcategoriesflag,
                        int hassusbsubcategoriesflag
            )
        {
            _subcategoryid = subcategoryid;
            _subcategorydesc = subcategorydesc;
            _subjectcode = subjectcode;
            _hassubcategoriesflag = hassubcategoriesflag;
            _hassusbsubcategoriesflag = hassusbsubcategoriesflag;
        }

        //For sp_CATALOG_getSubSubCategoriesBySubCategoryId
        public Category(int subcategorycode,
                        int subsubcategoryid,
                        string subsubcategorydesc
            )
        {
            _subcategorycode = subcategorycode;
            _subsubcategoryid = subsubcategoryid;
            _subsubcategorydesc = subsubcategorydesc;
        }

        public int SubjectId
        {
            get { return _subjectid; }
            //set { _subjectid = value; }
        }
        public int SubjectCode { get { return _subjectid; } }
        public string SubjectDesc {get { return _subjectdesc; ; }}
        public int SubCategoryId{get { return _subcategoryid; }}
        public int SubCategoryCode{get { return _subcategoryid; }}
        public string SubCategoryDesc{ get { return _subcategorydesc; }}
        public int SubSubCategoryId { get { return _subsubcategoryid; } }
        public string SubSubCategoryDesc { get { return _subsubcategorydesc; } }

    }
}
