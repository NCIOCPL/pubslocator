using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Configuration;
using System.Text;
using PubEnt.BLL;
using System.Collections;
using PubEnt.FedExRateService;

namespace PubEnt
{

    public class FedEx
    {
        private static int boxcount;
        public static string ShipMethod(string method)  //***EAC This code is OBSOLETE
        {
            switch (method.ToUpper())
	        {
                case "STANDARD":
                    return "FEDEX_GROUND";
                    break;
                case "OVERNIGHT":
                    return "STANDARD_OVERNIGHT";
                    break;
                case "2DAY":
                    return "FEDEX_2_DAY";
                    break;
	            default:
                    return method;  //dont know what it is
            }
        }

        public static double FedExEstimatedRate(Person shipto, ProductCollection cart)
        {
            double temp = 0.0;  //return 0.0 if something is wrong

            RateRequest request = FedEx.FedExCreateRateRequest(shipto, cart);//TODO:  better to use shoppingcartV2 if it was available
            RateService service = new RateService(); // Initialize the service
            try
            {
                // Call the web service passing in a RateRequest and returning a RateReply
                RateReply reply = service.getRates(request);
                if (reply.HighestSeverity == NotificationSeverityType.SUCCESS || reply.HighestSeverity == NotificationSeverityType.NOTE || reply.HighestSeverity == NotificationSeverityType.WARNING) // check if the call was successful
                {
                    //ShowRateReply(reply);
                    for (int i = 0; i < reply.RateReplyDetails[0].RatedShipmentDetails.Count(); i++)
                    {
                        ShipmentRateDetail rateDetail = reply.RateReplyDetails[0].RatedShipmentDetails[i].ShipmentRateDetail;
                        if ((double)rateDetail.TotalNetCharge.Amount > temp)
                            temp = (double)rateDetail.TotalNetCharge.Amount;
                    }

                }
                FedEx.FedExShowNotifications(reply);
            }
            catch (Exception ex)
            {
                temp = 0.0;
            }
            return(temp);
        }
        public static RateRequest FedExCreateRateRequest(Person shipto, ProductCollection cart)
        {
            // Build a RateRequest
            RateRequest request = new RateRequest();
            //
            request.WebAuthenticationDetail = new WebAuthenticationDetail();
            request.WebAuthenticationDetail.UserCredential = new WebAuthenticationCredential();
            request.WebAuthenticationDetail.UserCredential.Key = ConfigurationManager.AppSettings["FedexKey"];
            request.WebAuthenticationDetail.UserCredential.Password = ConfigurationManager.AppSettings["FedexPassword"];
            //
            request.ClientDetail = new ClientDetail();
            request.ClientDetail.AccountNumber = ConfigurationManager.AppSettings["FedexLMAccountNumber"];
            request.ClientDetail.MeterNumber = ConfigurationManager.AppSettings["FedexLMMeterNumber"];
            //
            request.TransactionDetail = new TransactionDetail();
            request.TransactionDetail.CustomerTransactionId = "***Rate v10 Request using VC#***"; // This is a reference field for the customer.  Any value can be used and will be provided in the response.
            //
            request.Version = new VersionId(); // WSDL version information, value is automatically set from wsdl
            //
            request.ReturnTransitAndCommit = true;
            request.ReturnTransitAndCommitSpecified = true;
            //
            FedExSetShipmentDetails(request, shipto, cart);
            //
            return request;
        }

        public static void FedExSetShipmentDetails(RateRequest request, Person shipto, ProductCollection cart)
        {
            request.RequestedShipment = new RequestedShipment();
            request.RequestedShipment.ShipTimestamp = DateTime.Now; // Shipping date and time
            request.RequestedShipment.ShipTimestampSpecified = true;
            request.RequestedShipment.DropoffType = DropoffType.REGULAR_PICKUP; //Drop off types are BUSINESS_SERVICE_CENTER, DROP_BOX, REGULAR_PICKUP, REQUEST_COURIER, STATION
            request.RequestedShipment.ServiceType = (ServiceType)Enum.Parse(typeof(ServiceType), DAL2.DAL.GetShippingMethodValuebyID(cart.ShipMethod)); // Service types are STANDARD_OVERNIGHT, PRIORITY_OVERNIGHT, FEDEX_GROUND ...
            request.RequestedShipment.ServiceTypeSpecified = true;
            request.RequestedShipment.PackagingType = PackagingType.YOUR_PACKAGING; // Packaging type FEDEX_BOK, FEDEX_PAK, FEDEX_TUBE, YOUR_PACKAGING, ...
            request.RequestedShipment.PackagingTypeSpecified = true;
            //
            FedExSetOrigin(request, shipto, cart);
            //
            FedExSetDestination(request, shipto, cart);
            //
            FedExSetPackageLineItems(request, shipto, cart);
            //
            //request.RequestedShipment.TotalInsuredValue = new Money();
            //request.RequestedShipment.TotalInsuredValue.Amount = 100;
            //request.RequestedShipment.TotalInsuredValue.Currency = "USD";
            //
            request.RequestedShipment.RateRequestTypes = new RateRequestType[1];
            request.RequestedShipment.RateRequestTypes[0] = RateRequestType.LIST;
            //request.RequestedShipment.RateRequestTypes[1] = RateRequestType.LIST; 
            request.RequestedShipment.PackageCount = boxcount.ToString();
        }

        public static void FedExSetOrigin(RateRequest request, Person shipto, ProductCollection cart)
        {
            request.RequestedShipment.Shipper = new Party();
            request.RequestedShipment.Shipper.Address = new Address();
            request.RequestedShipment.Shipper.Address.StreetLines = new string[1] { ConfigurationManager.AppSettings["LMStreet"] };
            request.RequestedShipment.Shipper.Address.City = ConfigurationManager.AppSettings["LMCity"];
            request.RequestedShipment.Shipper.Address.StateOrProvinceCode = ConfigurationManager.AppSettings["LMState"];
            request.RequestedShipment.Shipper.Address.PostalCode = ConfigurationManager.AppSettings["LMZip"];
            request.RequestedShipment.Shipper.Address.CountryCode = ConfigurationManager.AppSettings["LMCountry"];
        }

        public static void FedExSetDestination(RateRequest request, Person shipto, ProductCollection cart)
        {
            request.RequestedShipment.Recipient = new Party();
            request.RequestedShipment.Recipient.Address = new Address();
            request.RequestedShipment.Recipient.Address.StreetLines = new string[1] { shipto.Addr1 };
            request.RequestedShipment.Recipient.Address.City = shipto.City;
            request.RequestedShipment.Recipient.Address.StateOrProvinceCode = shipto.State;
            request.RequestedShipment.Recipient.Address.PostalCode = shipto.Zip5;
            request.RequestedShipment.Recipient.Address.CountryCode = shipto.Country;

            //request.RequestedShipment.Recipient.AccountNumber = cart.ShipAcctNum ;//EAC appears useless atm
        }

        public static void FedExSetPackageLineItems(RateRequest request, Person shipto, ProductCollection cart)
        {
            double MaxWeightPerBox = Double.Parse(ConfigurationManager.AppSettings["MaxWeightPerBox"]);
            boxcount = (int)(cart.TotalWeight / MaxWeightPerBox) + 1;
            request.RequestedShipment.RequestedPackageLineItems = new RequestedPackageLineItem[boxcount];

            for (int i = 0; i < boxcount; i++)
            {
                request.RequestedShipment.RequestedPackageLineItems[i] = new RequestedPackageLineItem();
                request.RequestedShipment.RequestedPackageLineItems[i].SequenceNumber = (i+1).ToString(); // package sequence number
                request.RequestedShipment.RequestedPackageLineItems[i].GroupPackageCount = "1";
                // package weight
                request.RequestedShipment.RequestedPackageLineItems[i].Weight = new Weight();
                request.RequestedShipment.RequestedPackageLineItems[i].Weight.Units = WeightUnits.LB;
                if (i == (boxcount - 1))//last box
                    request.RequestedShipment.RequestedPackageLineItems[i].Weight.Value = (decimal)((cart.TotalWeight - (i * MaxWeightPerBox)) * 1.1);           //plus 10%      
                else
                    request.RequestedShipment.RequestedPackageLineItems[i].Weight.Value = (decimal)(MaxWeightPerBox * 1.1);   //plus 10%   
                // package dimensions
                //***EAC per Andy's instructions
                request.RequestedShipment.RequestedPackageLineItems[i].Dimensions = new Dimensions();
                request.RequestedShipment.RequestedPackageLineItems[i].Dimensions.Length = "18";    //per Andy
                request.RequestedShipment.RequestedPackageLineItems[i].Dimensions.Width = "12";     //per Andy
                request.RequestedShipment.RequestedPackageLineItems[i].Dimensions.Height = "10";    //per Andy
                request.RequestedShipment.RequestedPackageLineItems[i].Dimensions.Units = LinearUnits.IN;
            }
        }


        //public static void FedExSetPackageLineItemsOriginal(RateRequest request, Person shipto, ProductCollection cart)
        //{
        //    request.RequestedShipment.RequestedPackageLineItems = new RequestedPackageLineItem[1];
        //    request.RequestedShipment.RequestedPackageLineItems[0] = new RequestedPackageLineItem();
        //    request.RequestedShipment.RequestedPackageLineItems[0].SequenceNumber = "1"; // package sequence number
        //    request.RequestedShipment.RequestedPackageLineItems[0].GroupPackageCount = "1";
        //    // package weight
        //    request.RequestedShipment.RequestedPackageLineItems[0].Weight = new Weight();
        //    request.RequestedShipment.RequestedPackageLineItems[0].Weight.Units = WeightUnits.LB;
        //    request.RequestedShipment.RequestedPackageLineItems[0].Weight.Value = (decimal)cart.TotalWeight;
            
        //    // package dimensions
        //    //***EAC TODO: need to follow box rules from Andy's email
        //    request.RequestedShipment.RequestedPackageLineItems[0].Dimensions = new Dimensions();
        //    request.RequestedShipment.RequestedPackageLineItems[0].Dimensions.Length = "10";
        //    request.RequestedShipment.RequestedPackageLineItems[0].Dimensions.Width = "13";
        //    request.RequestedShipment.RequestedPackageLineItems[0].Dimensions.Height = "4";
        //    request.RequestedShipment.RequestedPackageLineItems[0].Dimensions.Units = LinearUnits.IN;
        //    //***EAC insured value - does not appear to be useful
        //    //request.RequestedShipment.RequestedPackageLineItems[0].InsuredValue = new Money();
        //    //request.RequestedShipment.RequestedPackageLineItems[0].InsuredValue.Amount = 100;
        //    //request.RequestedShipment.RequestedPackageLineItems[0].InsuredValue.Currency = "USD";
        //    //
        //    //request.RequestedShipment.RequestedPackageLineItems[1] = new RequestedPackageLineItem();
        //    //request.RequestedShipment.RequestedPackageLineItems[1].SequenceNumber = "2"; // package sequence number
        //    //request.RequestedShipment.RequestedPackageLineItems[1].GroupPackageCount = "1";
        //    //// package weight
        //    //request.RequestedShipment.RequestedPackageLineItems[1].Weight = new Weight();
        //    //request.RequestedShipment.RequestedPackageLineItems[1].Weight.Units = WeightUnits.LB;
        //    //request.RequestedShipment.RequestedPackageLineItems[1].Weight.Value = 25.0M;
        //    //// package dimensions
        //    //request.RequestedShipment.RequestedPackageLineItems[1].Dimensions = new Dimensions();
        //    //request.RequestedShipment.RequestedPackageLineItems[1].Dimensions.Length = "20";
        //    //request.RequestedShipment.RequestedPackageLineItems[1].Dimensions.Width = "13";
        //    //request.RequestedShipment.RequestedPackageLineItems[1].Dimensions.Height = "4";
        //    //request.RequestedShipment.RequestedPackageLineItems[1].Dimensions.Units = LinearUnits.IN;
        //    //// insured value
        //    //request.RequestedShipment.RequestedPackageLineItems[1].InsuredValue = new Money();
        //    //request.RequestedShipment.RequestedPackageLineItems[1].InsuredValue.Amount = 500;
        //    //request.RequestedShipment.RequestedPackageLineItems[1].InsuredValue.Currency = "USD";
        //}

        public static void FedExShowRateReply(RateReply reply, Person shipto, ProductCollection cart)
        {
            FedExNoelWrite("RateReply details:");
            foreach (RateReplyDetail rateReplyDetail in reply.RateReplyDetails)
            {
                if (rateReplyDetail.ServiceTypeSpecified)
                    FedExNoelWrite("Service Type: " + rateReplyDetail.ServiceType);
                if (rateReplyDetail.PackagingTypeSpecified)
                    FedExNoelWrite("Packaging Type: " + rateReplyDetail.PackagingType);

                foreach (RatedShipmentDetail shipmentDetail in rateReplyDetail.RatedShipmentDetails)
                {
                    FedExRate(shipmentDetail, shipto, cart);
                    //FedExNoelWrite();
                }
                FedExShowDeliveryDetails(rateReplyDetail);
                //FedExNoelWrite("**********************************************************");
            }
        }

        //public static void FedExShowShipmentRateDetails(RatedShipmentDetail shipmentDetail, Person shipto, ProductCollection cart)
        public static double FedExRate(RatedShipmentDetail shipmentDetail, Person shipto, ProductCollection cart)
        {
            double temp = 0;
            if (shipmentDetail == null) return -1;//***EAC something wrong?
            if (shipmentDetail.ShipmentRateDetail == null) return -1;//***EAC something wrong?
            ShipmentRateDetail rateDetail = shipmentDetail.ShipmentRateDetail;
            FedExNoelWrite("--- Shipment Rate Detail ---");
            //
            FedExNoelWrite("RateType: " + rateDetail.RateType);
            if (rateDetail.TotalBillingWeight != null) FedExNoelWrite("Total Billing Weight: " + rateDetail.TotalBillingWeight.Value + " " + shipmentDetail.ShipmentRateDetail.TotalBillingWeight.Units);
            if (rateDetail.TotalBaseCharge != null) FedExNoelWrite("Total Base Charge: " + rateDetail.TotalBaseCharge.Amount + " " + rateDetail.TotalBaseCharge.Currency);
            if (rateDetail.TotalFreightDiscounts != null) FedExNoelWrite("Total Freight Discounts: " + rateDetail.TotalFreightDiscounts.Amount + " " + rateDetail.TotalFreightDiscounts.Currency);
            if (rateDetail.TotalSurcharges != null) FedExNoelWrite("Total Surcharges: " + rateDetail.TotalSurcharges.Amount + " " + rateDetail.TotalSurcharges.Currency);
            if (rateDetail.Surcharges != null)
            {
                // Individual surcharge for each package
                foreach (Surcharge surcharge in rateDetail.Surcharges)
                    FedExNoelWrite(" surcharge " + surcharge.SurchargeType + " " + surcharge.Amount.Amount + " " + surcharge.Amount.Currency);
            }
            if (rateDetail.TotalNetCharge != null)
            {
                FedExNoelWrite("Total Net Charge: " + rateDetail.TotalNetCharge.Amount + " " + rateDetail.TotalNetCharge.Currency);
                temp = (double)rateDetail.TotalNetCharge.Amount;
            }
            return temp;
        }

        public static void FedExShowDeliveryDetails(RateReplyDetail rateDetail)
        {
            if (rateDetail.DeliveryTimestampSpecified)
                FedExNoelWrite("Delivery timestamp: " + rateDetail.DeliveryTimestamp.ToString());
            if (rateDetail.TransitTimeSpecified)
                FedExNoelWrite("Transit time: " + rateDetail.TransitTime);
        }

        public static void FedExShowNotifications(RateReply reply)
        {
            FedExNoelWrite("Notifications");
            for (int i = 0; i < reply.Notifications.Length; i++)
            {
                Notification notification = reply.Notifications[i];
                FedExNoelWrite("Notification no. " + i);
                FedExNoelWrite(" Severity:" + notification.Severity);
                FedExNoelWrite(" Code: " + notification.Code);
                FedExNoelWrite(" Message: " + notification.Message);
                FedExNoelWrite(" Source: " + notification.Source);
            }
        }

        public static void FedExNoelWrite(string s)
        {
            //do nothing
        }          
    }
}
