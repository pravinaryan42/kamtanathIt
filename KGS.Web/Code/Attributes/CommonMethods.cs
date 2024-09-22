using System;

namespace KGS.Web.Code.Attributes
{
    public static class CommonMethods
    {
       
        public static void SaveAudit(string moduleName,string actionName,string pageTitle,string text,Guid userId,string referenceId)
        {
            try
            {
                //SiteAudit audit = new SiteAudit();
                //audit.AuditId = Guid.NewGuid();
                //audit.CreatedOn = DateTime.Now;
                //audit.ActionName = actionName;
                //audit.ModuleName = moduleName;
                //audit.PageTitle = pageTitle;
                //audit.UserId = userId;
                //audit.ReferenceId = referenceId;
                //var browser = System.Web.HttpContext.Current.Request.Browser;
                //audit.Web_Agent = browser.Browser + " v" + browser.Version;
                //audit.IP_Address = GetUserIP();
                //audit.AuditText = text;
                //SmartBillingEntities dbcontext = new SmartBillingEntities();
                //dbcontext.SiteAudits.Add(audit);
                //dbcontext.SaveChanges();
            }
            catch (Exception ex) {
               
            }
           
        }
        /// <summary>
        /// Method to get system IP
        /// </summary>
        /// <returns></returns>
        private static string GetUserIP()
        {
            string ipList = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipList))
            {
                return ipList.Split(',')[0];
            }

            return System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        }

       
    }
}