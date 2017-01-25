using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//added
using System.Collections;

namespace NCICatalog.BLL
{
    [Serializable]
    public class CatalogPubsCollection:CollectionBase
    {
        public int Add(CatalogPub value)
        {
            return (List.Add(value));
        }
    }
}
