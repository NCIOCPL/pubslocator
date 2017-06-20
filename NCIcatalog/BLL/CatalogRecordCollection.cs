using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//added
using System.Collections;
namespace NCICatalog.BLL
{
    [Serializable]
    public class CatalogRecordCollection:CollectionBase
    {
        public enum CatalogSortFields
        {
            CatalogWYNTKCancerType
        }
        private sealed class CancerTypeComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                CatalogRecord first = (CatalogRecord)x;
                CatalogRecord second = (CatalogRecord)y;
                return first.CatalogWYNTKCancerTypeDesc.CompareTo(second.CatalogWYNTKCancerTypeDesc);
                
            }
        }
        public void Sort(CatalogSortFields sortField, bool isAscending)
        {
            switch (sortField)
            {
                case CatalogSortFields.CatalogWYNTKCancerType:
                    InnerList.Sort(new CancerTypeComparer());
                    break;
            }
            if (!isAscending) InnerList.Reverse();
        }
        public int Add(CatalogRecord value)
        {
            return (List.Add(value));
        }
    }
}
