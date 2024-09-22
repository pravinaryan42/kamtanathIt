using System;
using System.Configuration;

namespace KGS.Web.Code.LIBS
{
    public static class SiteKeys
    {
        public static string Domain
        {
            get { return ConfigurationManager.AppSettings["Domain"]; }
        }
        public static string SMTPServer
        {
            get { return ConfigurationManager.AppSettings["SMTPServer"].ToLower(); }
        }

        public static string GooglePlaceApi
        {
            get { return ConfigurationManager.AppSettings["GooglePlaceApi"]; }
        }

        public static string SMTPUserName
        {
            get { return ConfigurationManager.AppSettings["SMTPUserName"]; }
        }

        public static string SMTPPassword
        {
            get { return ConfigurationManager.AppSettings["SMTPPassword"]; }
        }

        public static string SmtpHost
        {
            get { return ConfigurationManager.AppSettings["SMTPHost"]; }
        }

        public static string MailBCC
        {
            get { return ConfigurationManager.AppSettings["BCC"]; }
        }
        public static string MailCC
        {
            get { return ConfigurationManager.AppSettings["CC"]; }
        }
        public static string MailFrom
        {
            get { return ConfigurationManager.AppSettings["MailFrom"]; }
        }
        public static string FromName
        {
            get { return ConfigurationManager.AppSettings["FromName"]; }
        }
        public static string AdminEmail
        {
            get { return ConfigurationManager.AppSettings["AdminEmail"]; }
        }
        public static string AdminResetPasswordTemplate
        {
            get { return ConfigurationManager.AppSettings["ResetPasswordTemplatePathForAdmin"]; }
        }

        public static string SecretKey
        {
            get { return ConfigurationManager.AppSettings["SecretKey"]; }
        }
        public static string DataSiteKey
        {
            get { return ConfigurationManager.AppSettings["DataSiteKey"]; }
        }
        public static string SBReportPath
        {
            get { return ConfigurationManager.AppSettings["SBReportPath"]; }
        }
        public static string WKHTMLDirectoryPath
        {
            get { return ConfigurationManager.AppSettings["WKHTMLDirectoryPath"]; }
        }
        public static string WKHTMLFileName
        {
            get { return ConfigurationManager.AppSettings["WKHTMLFileName"]; }
        }
        public static string ImportedFilePath
        {
            get { return ConfigurationManager.AppSettings["ImportedFilePath"]; }
        }

        public static string TemplateFilePath
        {
            get { return ConfigurationManager.AppSettings["TemplateFilePath"]; }
        }
        public static string SmtpPort
        {
            get { return ConfigurationManager.AppSettings["SMTPPort"]; }
        }

        public static string CopyrightYear
        {
            get { return ConfigurationManager.AppSettings["CopyrightYear"]; }
        }
        public static string IsLive
        {
            get { return ConfigurationManager.AppSettings["IsLive"]; }
        }

        public static string SMTPLiveHost
        {
            get { return ConfigurationManager.AppSettings["SMTPLiveHost"]; }
        }
        public static int SMTPLivePort
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["SMTPLivePort"]); }
        }
    }
}
