using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Collections;
using PubEntAdmin.BLL;

/// <summary>
/// Summary description for cs_recordcollection
/// </summary>
namespace PubEntAdmin.BLL
{
    [Serializable]
    public class cs_recordcollection:CollectionBase
    {
        public int Add(cs_record value)
        {
            return List.Add(value);
        }
    }
}