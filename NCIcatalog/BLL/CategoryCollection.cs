using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//added
using System.Collections;

namespace NCICatalog.BLL
{
    [Serializable]
    public class CategoryCollection:CollectionBase
    {
        public int Add(Category value)
        {
            return (List.Add(value));
        }
    }
}
