using System;
using System.Collections;
//using PubEnt.DAL;

namespace PubEntAdmin.BLL
{
    public class KVPairCollection:CollectionBase {

        public int Add( KVPair value )  {
            return( List.Add( value ) );
        }

  }
}
