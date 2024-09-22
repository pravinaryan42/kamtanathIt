using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace KGS.Web.Code.PayPal
{
    public class PayPalStandardPayment
    {
        public bool UseSandbox { get; set; }
        public bool SendAllProduct { get; set; }
        public bool enableIpn { get; set; }

        private string BusinessEmail;
        private string PDTToken;
        

        public PayPalStandardPayment()
        {
            UseSandbox = true;
            BusinessEmail = Convert.ToString(ConfigurationManager.AppSettings["PayPalUsername"]);
            PDTToken = Convert.ToString(ConfigurationManager.AppSettings["PDTTokenId"]);
        }

        private string GetPaypalUrl()
        {
            return UseSandbox ? "https://www.sandbox.paypal.com/us/cgi-bin/webscr" :
                "https://www.paypal.com/us/cgi-bin/webscr";
        }
        private string GetIpnPaypalUrl()
        {
            return UseSandbox ? "https://ipnpb.sandbox.paypal.com/cgi-bin/webscr" :
                "https://ipnpb.paypal.com/cgi-bin/webscr";
        }
        //public string GenerateRedirectionUrl(TransactionOrderViewModel model,string PlanName)
        //{
        //    StringBuilder paypalUrl = new StringBuilder();
        //    SubscriptionPackage subscriptionProduct = new SubscriptionPackage();          
        //    if (subscriptionProduct != null)
        //    {
        //        paypalUrl.Append(GetPaypalUrl());
        //        var cmd = SendAllProduct ? "_cart" : "_xclick";

        //        paypalUrl.AppendFormat("?cmd={0}&business={1}", cmd, HttpUtility.UrlEncode(BusinessEmail));
        //        paypalUrl.AppendFormat("&item_name=" + HttpUtility.UrlEncode("SB Subscription Plan:" + PlanName));
        //        if (model.DiscountAmount > 0M)
        //        {
        //            paypalUrl.AppendFormat("&discount_amount_cart={0}", model.DiscountAmount.ToString("0.00", CultureInfo.InvariantCulture));
        //        }
        //        var orderTotal = Math.Round(Convert.ToDouble(model.Amount), 2);
        //        paypalUrl.AppendFormat("&Package={0}", PlanName);
        //        paypalUrl.AppendFormat("&amount={0}", orderTotal.ToString("0.00", CultureInfo.InvariantCulture));
        //        paypalUrl.AppendFormat("&custom={0}", model.GroupOrderId);
        //        paypalUrl.AppendFormat("&charset={0}", "utf-8");
        //        paypalUrl.AppendFormat("&currency_code={0}", "USD");
        //        paypalUrl.AppendFormat("&invoice={0}", Guid.NewGuid());
        //        string returnUrl = SiteKeys.Domain + "users/payment/PDTHandler";
        //        string cancelReturnUrl = SiteKeys.Domain + "users/payment/FailedTransaction";
        //        paypalUrl.AppendFormat("&return={0}&cancel_return={1}", HttpUtility.UrlEncode(returnUrl), HttpUtility.UrlEncode(cancelReturnUrl));
        //        string ipnUrl = SiteKeys.Domain + "users/payment/IPNHandler";
        //        paypalUrl.AppendFormat("&notify_url={0}", ipnUrl);

        //        if (model.UserData != null)
        //        {
        //            paypalUrl.AppendFormat("&address_override={0}", 0);   // Set to 0 and then issue fixed http://www.webassist.com/forums/posts.php?id=3434
        //            paypalUrl.AppendFormat("&first_name={0}", HttpUtility.UrlEncode(model.UserData.FirstName));
        //            paypalUrl.AppendFormat("&last_name={0}", HttpUtility.UrlEncode(model.UserData.LastName));
        //            if (model.UserData.UserRoleId == (byte)UserRoles.Doctor)
        //            {
        //                paypalUrl.AppendFormat("&address1={0}", HttpUtility.UrlEncode(model.UserData.DoctorDetail?.Location));
        //            }
        //            else if (model.UserData.UserRoleId == (byte)UserRoles.Patient)
        //            {
        //                paypalUrl.AppendFormat("&address1={0}", HttpUtility.UrlEncode(model.UserData.PatientDetail?.Location));
        //            }
        //            paypalUrl.AppendFormat("&address2={0}", String.Empty);
        //            paypalUrl.AppendFormat("&city={0}", HttpUtility.UrlEncode("N/A"));
        //            paypalUrl.AppendFormat("&state={0}", HttpUtility.UrlEncode("N/A"));
        //            paypalUrl.AppendFormat("&country={0}", "USA");
        //            //paypalUrl.AppendFormat("&zip={0}", HttpUtility.UrlEncode(sale.Property.PropertySignBoard.PostCode));
        //            paypalUrl.AppendFormat("&email={0}", HttpUtility.UrlEncode(model.UserData.Email));
        //            if (model.PaymentMethodType == 1)
        //            {
        //                paypalUrl.AppendFormat("&landing_page={0}", HttpUtility.UrlEncode("billing"));
        //            }

        //        }

        //    }
        //    return paypalUrl.ToString();
        //}


        //public bool GetPdtDetails(string tx, out Dictionary<string, string> values, out string response)
        //{
        //    var req = (HttpWebRequest)WebRequest.Create(GetPaypalUrl());           
        //    ServicePointManager.Expect100Continue = true;
        //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;           
        //    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        //    req.Method = WebRequestMethods.Http.Post;
        //    req.ContentType = "application/x-www-form-urlencoded";
        //    req.UserAgent = HttpContext.Current.Request.UserAgent;
        //    string formContent = string.Format("cmd=_notify-synch&at={0}&tx={1}", PDTToken, tx);
        //    req.ContentLength = formContent.Length;
        //    using (var sw = new StreamWriter(req.GetRequestStream(), Encoding.ASCII))
        //        sw.Write(formContent);
        //    using (var sr = new StreamReader(req.GetResponse().GetResponseStream()))
        //        response = HttpUtility.UrlDecode(sr.ReadToEnd());
        //    values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        //    bool firstLine = true, success = false;
        //    foreach (string l in response.Split('\n'))            {
        //        string line = l.Trim();
        //        if (firstLine)
        //        {
        //            success = line.Equals("SUCCESS", StringComparison.OrdinalIgnoreCase);
        //            firstLine = false;
        //        }
        //        else
        //        {
        //            int equalPox = line.IndexOf('=');
        //            if (equalPox >= 0)
        //                values.Add(line.Substring(0, equalPox), line.Substring(equalPox + 1));
        //        }
        //    }

        //    return success;



        //}

        public bool GetPdtDetails(string tx, out Dictionary<string, string> values, out string response)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var req = (HttpWebRequest)WebRequest.Create(GetPaypalUrl());

            req.Method = WebRequestMethods.Http.Post;
            req.ContentType = "application/x-www-form-urlencoded";
            req.UserAgent = HttpContext.Current.Request.UserAgent;
            string formContent = string.Format("cmd=_notify-synch&at={0}&tx={1}", PDTToken, tx);
            req.ContentLength = formContent.Length;

            using (var sw = new StreamWriter(req.GetRequestStream(), Encoding.ASCII))
                sw.Write(formContent);

            using (var sr = new StreamReader(req.GetResponse().GetResponseStream()))
                response = HttpUtility.UrlDecode(sr.ReadToEnd());

            values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            bool firstLine = true, success = false;
            foreach (string l in response.Split('\n'))
            {
                string line = l.Trim();
                if (firstLine)
                {
                    success = line.Equals("SUCCESS", StringComparison.OrdinalIgnoreCase);
                    firstLine = false;
                }
                else
                {
                    int equalPox = line.IndexOf('=');
                    if (equalPox >= 0)
                        values.Add(line.Substring(0, equalPox), line.Substring(equalPox + 1));
                }
            }

            return success;
        }

        public bool VerifyIpn(string formString, out Dictionary<string, string> values)
        {
            var req = (HttpWebRequest)WebRequest.Create(GetIpnPaypalUrl());
            req.Method = WebRequestMethods.Http.Post;
            req.ContentType = "application/x-www-form-urlencoded";
            req.UserAgent = HttpContext.Current.Request.UserAgent;

            var formContent = string.Format("cmd=_notify-validate&{0}", formString);
            req.ContentLength = formContent.Length;

            using (var sw = new StreamWriter(req.GetRequestStream(), Encoding.ASCII))
            {
                sw.Write(formContent);
            }

            string response;
            using (var sr = new StreamReader(req.GetResponse().GetResponseStream()))
            {
                response = HttpUtility.UrlDecode(sr.ReadToEnd());
            }
            bool success = response.Trim().Equals("VERIFIED", StringComparison.OrdinalIgnoreCase);

            values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (string l in formString.Split('&'))
            {
                string line = l.Trim();
                int equalPox = line.IndexOf('=');
                if (equalPox >= 0)
                    values.Add(line.Substring(0, equalPox), line.Substring(equalPox + 1));
            }

            return success;
        }
    }
}