using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//Manually added
using System.Collections;
using PubEnt.BLL;

namespace PubEnt.BLL
{
    [Serializable]
    public class ProductCollection:CollectionBase
    {
        private double _shipcost; //***ESTIMATED
        private string _shipvendor;
        private string _shipmethod;
        private string _shipacctnum;
        private string _ordercreator;
        private string _ordermedia;


		public enum ProductFields //Used for sorting
		{
			LongTitle,
			RecordUpdateDate,
            RevisedDate
		}
		public void Sort(ProductFields sortField, bool isAscending) 
		{
			switch (sortField) 
			{
                case ProductFields.LongTitle:
					InnerList.Sort(new LongTitleComparer());
					break;
                case ProductFields.RecordUpdateDate:
                    InnerList.Sort(new RecordUpdateDateComparer());
                    break;
                case ProductFields.RevisedDate:
                    InnerList.Sort(new RevisedDateComparer());
                    break;
                
			}
			if (!isAscending) InnerList.Reverse();
		}

		private sealed class LongTitleComparer : IComparer 
		{
			public int Compare(object x, object y) 
			{
				Product first = (Product) x;
				Product second = (Product) y;
				return first.LongTitle.CompareTo(second.LongTitle);
			}
		}
        private sealed class RecordUpdateDateComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                Product first = (Product)x;
                Product second = (Product)y;
                return first.dtRecordUpdateDate.CompareTo(second.dtRecordUpdateDate); //Compare using the date fields
            }
        }
        private sealed class RevisedDateComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                Product first = (Product)x;
                Product second = (Product)y;
                return first.dtRevisedDate.CompareTo(second.dtRevisedDate); //Compare using the date fields
            }
        }
        public Product this[int index]
        {
            get { return ((Product) List[index] ); }
            set { List[index] = value; }
        }

        public int Add(Product value)
        {
            //dedupe items and possibly ignore numqty=0 - DONE
            if (value.NumQtyOrdered > 0)
            {
                bool found = false;
                foreach (Product item in List)
                {                    
                    if (item.PubId == value.PubId)
                    {
                        found = true;
                        item.NumQtyOrdered += value.NumQtyOrdered;
                        break;
                    }
                }
                if (found)
                {
                    return (-1);
                }
                else
                {
                    return(List.Add(value));
                }
            }
            else
                return (-1);
        }

        public int IndexOf(Product value)
        {
            return (List.IndexOf(value));
        }

        public void Insert(int index, Product value)
        {
            List.Insert(index, value);
        }

        public void Remove(Product value)
        {
            List.Remove(value);
        }

        public bool Contains(Product value)
        {
            return (List.Contains(value));
        }
        public int NumItems()//Number of Line items
        {
            //TODO: modify logic when KITS involved
            return (List.Count);
        }

        public int TotalQty//Total count of pubs
        {
            get
            {
                //TODO: modify logic when KITS/NERDO involved
                int t = 0;
                foreach (Product p in this)
                {
                    t += p.NumQtyOrdered;
                }
                return (t);
            }
        }
        public double Cost
        {
            get { return _shipcost; }

        }
        public double ShipCost
        {
            get { return _shipcost; }
            set { _shipcost = value; }
        }
        public string ShipVendor
        {
            get { return _shipvendor; }
            set { _shipvendor = value; }
        }
        public string ShipMethod
        {
            get { return _shipmethod; }
            set { _shipmethod = value; }
        }
        public string ShipAcctNum
        {
            get { return _shipacctnum; }
            set { _shipacctnum = value; }
        }
        public string OrderCreator
        {
            get { return _ordercreator; }
            set { _ordercreator = value; }
        }
        public string OrderMedia
        {
            get { return _ordermedia; }
            set { _ordermedia = value; }
        }
        ////***EAC "isFree" will be replaced by "isFree2Order" below to support XPO (HITT 12054)
        //public bool isFree
        //{
        //    get
        //    {
        //        int t = 0;
        //        foreach (Product p in this)
        //        {
        //            if (p.BookStatus != "X") t += p.NumQtyOrdered;
        //        }
        //        if (t > 20)
        //            return (false);
        //        else
        //            return (true);
        //    }
        //}
        //***EAC Overrides "isFree" to support XPO (HITT 12054)
        public bool isFree2Order(string state)
        {
            int t = 0;
            int max = 20;
            if (Zipcode.isXPO(state))
                return (true);

            foreach (Product p in this)
            {
                if (p.BookStatus != "X") t += p.NumQtyOrdered;
            }
            if (t > max)
                return (false);
            else
                return (true);
        }
        //***EAC Cart+Shipto objects good to go?
        public bool isOrderAllowed(string state)
        {
            if (Zipcode.isXPO(state))
            {
                if (this.TotalQty > GlobalUtils.Const.XPOMaxQuantity)
                    return false;
                else
                    return true;
            }
            else
            {
                return true;
            }
        }
        public void UpdateQty(int pubid, int qty)
        {
            foreach (Product p in this)
            {
                if (p.PubId == pubid)
                {
                    p.NumQtyOrdered = qty;
                    break;
                }
              
            }
        }
        public string Pubs
        {
            get
            {
                string temp = "";
                foreach (Product item in this)
                {
                    temp += item.PubId.ToString() + ",";
                }
                return (temp);
            }

        }
        public string Qtys
        {
            get
            {
                string temp = "";
                foreach (Product item in this)
                {
                    temp += item.NumQtyOrdered.ToString() + ",";
                }
                return (temp);
            }

        }
        public double TotalWeight
        {
            get {
                double t = 0.0;
                
                foreach (Product p in this)
                {
                    t += p.Weight * p.NumQtyOrdered;
                }
                if (t == 0) t = 0.1; //***EAC something is not right with our inventory...give a default weight of 0.1 lbs!.

                return (t);
            }
        }

        protected override void OnInsert(int index, Object value)
        {
            if (value.GetType() != Type.GetType("PubEnt.BLL.Product"))
                throw new ArgumentException("value must be of type Product.", "value");
        }

        protected override void OnRemove(int index, Object value)
        {
            if (value.GetType() != Type.GetType("PubEnt.BLL.Product"))
                throw new ArgumentException("value must be of type Product.", "value");
        }

        protected override void OnSet(int index, Object oldValue, Object newValue)
        {
            if (newValue.GetType() != Type.GetType("PubEnt.BLL.Product"))
                throw new ArgumentException("newValue must be of type Product.", "newValue");
        }

        protected override void OnValidate(Object value)
        {
            if (value.GetType() != Type.GetType("PubEnt.BLL.Product"))
                throw new ArgumentException("value must be of type Product.");
        }

        /*******************************************************************************/
        //Function that enforces business rules and adds individual product items to the 
        //products collection
        //Any new business rules or any change in business rules should be addressed here
        /*******************************************************************************/
        public void AddItemsToCollWithRules(Product pItem)
        {
            //foreach (Product p in this)
            //{
            //    //To remove an item from the collection
            //    //this.Remove(p);
            //}

            //We get all pubs from the database with ISSEARCHABLE_NCIPL flag set to true and then 
            //check for business rules pertaining to Book Status and Display Status
            //before adding the item to the collection.

            //Keep in mind that the MAXQTY_NCIPL is not taken care of here, 
            //but it is to be handled in the presentation layer code when a product is ordered.
            
            //Available Book Status Values
            //Active (A), Out of Stock/No Backorder (I), Out of Stock/Backorder (J),
            //Not Yet Taking Orders (N), Purge/Archive (P), Reserve (R), Web (W), 
            //Virtual (V) (kits), Exempt from Cost Recovery (X)

            //Book Status values I, P, R do not appear in search results 
           //if ((string.Compare(pItem.BookStatus, "I", true) == 0) ||
           //        (string.Compare(pItem.BookStatus, "P", true) == 0) ||
           //            (string.Compare(pItem.BookStatus, "R", true) == 0))
           //     return;

           //05-04-09 Changed business rules to disallow only "P" publications
           if (string.Compare(pItem.BookStatus, "P", true) == 0)
                return;

           //Initializing values
           pItem.CanView = 0;
           pItem.CanOrder = 0;
           pItem.OrderMsg = "";
           pItem.CanOrderCover = 0;
           pItem.CoverMsg = "";
            
            //Read Online - The simplest one, same logic for all book status codes
           //CR 28 if (pItem.Url.Length > 0 &&
           if ((pItem.Url.Length > 0 || pItem.PDFUrl.Length > 0) &&
               string.Compare(pItem.OnlineDisplayStatus, "ONLINE", true) == 0)
               pItem.CanView = 1;
           else
               pItem.CanView = 0;


           if (string.Compare(pItem.BookStatus, "A", true) == 0)
           {

               //Order
               if (pItem.NumQtyAvailable > 0 &&
                   string.Compare(pItem.OrderDisplayStatus, "ORDER", true) == 0)
                   pItem.CanOrder = 1;
               else if (string.Compare(pItem.OrderDisplayStatus, "ORDER", true) != 0)
                   pItem.CanOrder = 0;
               else if (string.Compare(pItem.OrderDisplayStatus, "ORDER", true) == 0 &&
                   pItem.NumQtyAvailable <= 0)
               {
                    pItem.CanOrder = 0;
                    pItem.OrderMsg = "Not available for ordering.";
               }
            

               ////Nerdo
               //if (pItem.NumQtyAvailableCover > 0 && pItem.UrlNerdo.Length > 0 &&
               //    string.Compare(pItem.NerdoDisplayStatus, "NERDO", true) == 0)
               //    pItem.CanOrderCover = 1;
               //else if (pItem.UrlNerdo.Length == 0 || string.Compare(pItem.NerdoDisplayStatus, "NERDO", true) != 0)
               //    pItem.CanOrderCover = 0;
               //else if (pItem.UrlNerdo.Length > 0 && string.Compare(pItem.NerdoDisplayStatus, "NERDO", true) == 0 &&
               //         pItem.NumQtyAvailableCover <= 0)
               //{
               //    pItem.CanOrderCover = 0;
               //    pItem.CoverMsg = "Not available for ordering.";
               //}


           }


           if (string.Compare(pItem.BookStatus, "J", true) == 0)
           {

               //Order
               if (string.Compare(pItem.OrderDisplayStatus, "ORDER", true) == 0)
               {
                   pItem.CanOrder = 1;
                   pItem.OrderMsg = "Publication is on backorder.";
               }
               else
                   pItem.CanOrder = 0;
               

               ////Nerdo
               //if (pItem.UrlNerdo.Length > 0 &&
               //    string.Compare(pItem.NerdoDisplayStatus, "NERDO", true) == 0 &&
               //    pItem.ProductIdCover.Length > 0)
               //{
               //    pItem.CanOrderCover = 1;
               //    pItem.CoverMsg = "Publication is on backorder.";
               //}
               //else if (pItem.UrlNerdo.Length == 0 || string.Compare(pItem.NerdoDisplayStatus, "NERDO", true) != 0 ||
               //    pItem.ProductIdCover.Length == 0)
               //    pItem.CanOrderCover = 0;

           }

           if (string.Compare(pItem.BookStatus, "N", true) == 0)
           {
               //Order
               if (string.Compare(pItem.OrderDisplayStatus, "ORDER", true) == 0)
               {
                   pItem.CanOrder = 0;
                   pItem.OrderMsg = "Not yet available for ordering.";
               }
               else
                   pItem.CanOrder = 0;

               ////Nerdo
               //if (string.Compare(pItem.NerdoDisplayStatus, "NERDO", true) == 0)
               //{
               //    pItem.CanOrderCover = 0;
               //    pItem.CoverMsg = "Not yet available for ordering.";
               //}
               //else 
               //    pItem.CanOrderCover = 0;
           }

           if (string.Compare(pItem.BookStatus, "V", true) == 0)
           {
               //Order
               if (string.Compare(pItem.OrderDisplayStatus, "ORDER", true) == 0)
                   pItem.CanOrder = 1;
               else
                   pItem.CanOrder = 0;

               //To Do Nerdo
               pItem.CanOrderCover = 0;
           }

           if (string.Compare(pItem.BookStatus, "X", true) == 0)
           {
               //Order
               if (pItem.NumQtyAvailable > 0 &&
                 string.Compare(pItem.OrderDisplayStatus, "ORDER", true) == 0)
                   pItem.CanOrder = 1;
               else if (string.Compare(pItem.OrderDisplayStatus, "ORDER", true) != 0)
                   pItem.CanOrder = 0;
               else if (string.Compare(pItem.OrderDisplayStatus, "ORDER", true) == 0 &&
                        pItem.NumQtyAvailable <= 0)
               {
                   pItem.CanOrder = 0;
                   pItem.OrderMsg = "Not available for ordering.";
               }

               ////Nerdo
               //if (pItem.NumQtyAvailableCover > 0 && pItem.UrlNerdo.Length > 0 &&
               //    string.Compare(pItem.NerdoDisplayStatus, "NERDO", true) == 0)
               //    pItem.CanOrderCover = 1;
               //else if (pItem.UrlNerdo.Length == 0 || string.Compare(pItem.NerdoDisplayStatus, "NERDO", true) != 0)
               //    pItem.CanOrderCover = 0;
               //else if (pItem.UrlNerdo.Length > 0 && string.Compare(pItem.NerdoDisplayStatus, "NERDO", true) == 0 &&
               //         pItem.NumQtyAvailableCover <= 0)
               //{
               //    pItem.CanOrderCover = 0;
               //    pItem.CoverMsg = "Not available for ordering.";
               //}
           }

           if (string.Compare(pItem.BookStatus, "W", true) == 0)
           {
                //Order
               pItem.CanOrder = 0;
               pItem.OrderMsg = "Not available for ordering.";

               //Nerdo
               pItem.CanOrderCover = 0;
           }

           #region NerdoRules
           //05-05-09 -  - Use Cover Pub Book Status for Nerdo Cover Pubs
           if (string.Compare(pItem.BookStatusCover, "A", true) == 0)
           {
               //Nerdo
               if (pItem.NumQtyAvailableCover > 0 && pItem.UrlNerdo.Length > 0 &&
                   string.Compare(pItem.NerdoDisplayStatus, "NERDO", true) == 0 && string.Compare(pItem.OrderDisplayStatusCover, "ORDER", true) == 0
                   && pItem.ProductIdCover.Length > 0)
                   pItem.CanOrderCover = 1;
               else if (pItem.UrlNerdo.Length == 0 || string.Compare(pItem.NerdoDisplayStatus, "NERDO", true) != 0)
                   pItem.CanOrderCover = 0;
               else if (pItem.UrlNerdo.Length > 0 && string.Compare(pItem.NerdoDisplayStatus, "NERDO", true) == 0 &&
                        pItem.NumQtyAvailableCover <= 0)
               {
                   pItem.CanOrderCover = 0;
                   pItem.CoverMsg = "Not available for ordering.";
               }

           }

           if (string.Compare(pItem.BookStatusCover, "J", true) == 0)
           {
               //Nerdo
               if (pItem.UrlNerdo.Length > 0 &&
                   string.Compare(pItem.NerdoDisplayStatus, "NERDO", true) == 0 &&
                   pItem.ProductIdCover.Length > 0 && string.Compare(pItem.OrderDisplayStatusCover, "ORDER", true) == 0)
               {
                   pItem.CanOrderCover = 1;
                   pItem.CoverMsg = "Publication is on backorder.";
               }
               else if (pItem.UrlNerdo.Length == 0 || string.Compare(pItem.NerdoDisplayStatus, "NERDO", true) != 0 ||
                   pItem.ProductIdCover.Length == 0)
                   pItem.CanOrderCover = 0;

           }

           if (string.Compare(pItem.BookStatusCover, "N", true) == 0)
           {
               //Nerdo
               if (string.Compare(pItem.NerdoDisplayStatus, "NERDO", true) == 0)
               {
                   pItem.CanOrderCover = 0;
                   pItem.CoverMsg = "Not yet available for ordering.";
               }
               else
                   pItem.CanOrderCover = 0;
           }

           if (string.Compare(pItem.BookStatusCover, "X", true) == 0)
           {
               //Nerdo
               if (pItem.NumQtyAvailableCover > 0 && pItem.UrlNerdo.Length > 0 &&
                   string.Compare(pItem.NerdoDisplayStatus, "NERDO", true) == 0 && string.Compare(pItem.OrderDisplayStatusCover, "ORDER", true) == 0
                   && pItem.ProductIdCover.Length > 0)
                   pItem.CanOrderCover = 1;
               else if (pItem.UrlNerdo.Length == 0 || string.Compare(pItem.NerdoDisplayStatus, "NERDO", true) != 0)
                   pItem.CanOrderCover = 0;
               else if (pItem.UrlNerdo.Length > 0 && string.Compare(pItem.NerdoDisplayStatus, "NERDO", true) == 0 &&
                        pItem.NumQtyAvailableCover <= 0)
               {
                   pItem.CanOrderCover = 0;
                   pItem.CoverMsg = "Not available for ordering.";
               }
           }

           #endregion

           List.Add(pItem);
        }

        //This method is used in the order upload class
        public int AddForOrderUpload(Product value)
        {
            return (List.Add(value));
        }
    }
}
