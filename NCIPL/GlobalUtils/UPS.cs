using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Configuration;
using System.Text;
using System.Collections;
using System.ServiceModel;
using System.Net;

using PubEnt.BLL;
using PubEnt.UPSRateService;

namespace PubEnt
{

    public class UPS
    {
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

        public static double UPSEstimatedRate(Person shipto, ProductCollection cart)
        {
            double temp = 0.0;  //return 0.0 if something is wrong
            try
            {

                RateService rate = new RateService();
                RateRequest rateRequest = new RateRequest();
                UPSSecurity upss = new UPSSecurity();
                UPSSecurityServiceAccessToken upssSvcAccessToken = new UPSSecurityServiceAccessToken();
                upssSvcAccessToken.AccessLicenseNumber = ConfigurationManager.AppSettings["UPSAccessLicenseNumber"];
                upss.ServiceAccessToken = upssSvcAccessToken;
                UPSSecurityUsernameToken upssUsrNameToken = new UPSSecurityUsernameToken();
                upssUsrNameToken.Username = ConfigurationManager.AppSettings["UPSUserName"];
                upssUsrNameToken.Password = ConfigurationManager.AppSettings["UPSPassword"];
                upss.UsernameToken = upssUsrNameToken;
                rate.UPSSecurityValue = upss;
                RequestType request = new RequestType();
                String[] requestOption = { "Rate" };
                request.RequestOption = requestOption;
                rateRequest.Request = request;
                ShipmentType shipment = new ShipmentType();
                ShipperType shipper = new ShipperType();
                shipper.ShipperNumber = "54A177";   //***EAC Intentionally hard-coded.  DO NOT REPLACE WITH ONE IN WEB.CONFIG!

                UPSRateService.AddressType shipperAddress = new UPSRateService.AddressType();
                String[] addressLine = { ConfigurationManager.AppSettings["LMStreet"] };
                shipperAddress.AddressLine = addressLine;
                shipperAddress.City = ConfigurationManager.AppSettings["LMCity"];
                shipperAddress.StateProvinceCode = ConfigurationManager.AppSettings["LMState"];
                shipperAddress.PostalCode = ConfigurationManager.AppSettings["LMZip"];
                shipperAddress.CountryCode = ConfigurationManager.AppSettings["LMCountry"];
                shipperAddress.AddressLine = addressLine;
                shipper.Address = shipperAddress;
                shipment.Shipper = shipper;

                ShipFromType shipFrom = new ShipFromType();
                //UPSRateService.AddressType shipFromAddress = new UPSRateService.AddressType();
                //shipFromAddress.AddressLine = shipperAddress.AddressLine;
                //shipFromAddress.City = shipperAddress.City;
                //shipFromAddress.StateProvinceCode = shipperAddress.StateProvinceCode;
                //shipFromAddress.PostalCode = shipperAddress.PostalCode;
                //shipFromAddress.CountryCode = shipperAddress.CountryCode;
                shipFrom.Address = shipperAddress;
                shipment.ShipFrom = shipFrom;


                ShipToType shipTo = new ShipToType();
                ShipToAddressType shipToAddress = new ShipToAddressType();
                String[] addressLine1 = { shipto.Addr1 };
                shipToAddress.AddressLine = addressLine1;
                shipToAddress.City = shipto.City;
                shipToAddress.StateProvinceCode = shipto.State;
                shipToAddress.PostalCode = shipto.Zip5;
                shipToAddress.CountryCode = shipto.Country;
                shipTo.Address = shipToAddress;
                shipment.ShipTo = shipTo;


                CodeDescriptionType service = new CodeDescriptionType();
                //Below code uses dummy date for reference. Please udpate as required.
                //service.Code = "03"; //01:nextdayair 02;2ndday 03:ground 12:3dayselect
                service.Code = DAL2.DAL.GetShippingMethodValuebyID(cart.ShipMethod);
                shipment.Service = service;
                PackageType package = new PackageType();
                PackageWeightType packageWeight = new PackageWeightType();
                packageWeight.Weight = cart.TotalWeight.ToString();
                CodeDescriptionType uom = new CodeDescriptionType();
                uom.Code = "LBS";
                uom.Description = "pounds";
                packageWeight.UnitOfMeasurement = uom;
                package.PackageWeight = packageWeight;
                CodeDescriptionType packType = new CodeDescriptionType();
                packType.Code = "02"; //02:pkgcustomer
                package.PackagingType = packType;
                PackageType[] pkgArray = { package };
                shipment.Package = pkgArray;
                rateRequest.Shipment = shipment;
                System.Net.ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy();

                RateResponse rateResponse = rate.ProcessRate(rateRequest);
                UPSNoelWrite("The transaction was a " + rateResponse.Response.ResponseStatus.Description);
                UPSNoelWrite("Total Shipment Charges " + rateResponse.RatedShipment[0].TotalCharges.MonetaryValue + rateResponse.RatedShipment[0].TotalCharges.CurrencyCode);
                temp = Double.Parse( rateResponse.RatedShipment[0].TotalCharges.MonetaryValue);
            }
            catch (Exception ex)
            {
                temp = 0.0;
            }
            return(temp);
            //catch (System.Web.Services.Protocols.SoapException ex)
            //{
            //    UPSNoelWrite("");
            //    UPSNoelWrite("---------Rate Web Service returns error----------------");
            //    UPSNoelWrite("---------\"Hard\" is user error \"Transient\" is system error----------------");
            //    UPSNoelWrite("SoapException Message= " + ex.Message);
            //    UPSNoelWrite("");
            //    UPSNoelWrite("SoapException Category:Code:Message= " + ex.Detail.LastChild.InnerText);
            //    UPSNoelWrite("");
            //    UPSNoelWrite("SoapException XML String for all= " + ex.Detail.LastChild.OuterXml);
            //    UPSNoelWrite("");
            //    UPSNoelWrite("SoapException StackTrace= " + ex.StackTrace);
            //    UPSNoelWrite("-------------------------");
            //    UPSNoelWrite("");
            //}
            //catch (System.ServiceModel.CommunicationException ex)
            //{
            //    UPSNoelWrite("");
            //    UPSNoelWrite("--------------------");
            //    UPSNoelWrite("CommunicationException= " + ex.Message);
            //    UPSNoelWrite("CommunicationException-StackTrace= " + ex.StackTrace);
            //    UPSNoelWrite("-------------------------");
            //    UPSNoelWrite("");

            //}
            //catch (Exception ex)
            //{
            //    UPSNoelWrite("");
            //    UPSNoelWrite("-------------------------");
            //    UPSNoelWrite(" Generaal Exception= " + ex.Message);
            //    UPSNoelWrite(" Generaal Exception-StackTrace= " + ex.StackTrace);
            //    UPSNoelWrite("-------------------------");

            //}
            //finally
            //{
            //    //Console.ReadKey();
            //}
        }

        public class TrustAllCertificatePolicy : System.Net.ICertificatePolicy
        {
            public TrustAllCertificatePolicy()
            { }

            public bool CheckValidationResult(ServicePoint sp,
             System.Security.Cryptography.X509Certificates.X509Certificate cert, WebRequest req, int problem)
            {
                return true;
            }
        }
        public static void UPSNoelWrite(string s)
        {
            //do nothing
        }     
    }
}
