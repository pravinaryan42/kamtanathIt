using System;
using System.Web;
using System.IO;
using System.Net.Mail;
using KGS.Core;
using System.Threading.Tasks;
using KGS.Web.Code.LIBS;
using System.Collections.Generic;

namespace KGS.EmailSender
{
    public class FlexiMail
    {
        #region Constructors-Destructors
        public FlexiMail()
        {
            //set defaults 
            myEmail = new System.Net.Mail.MailMessage();
            _MailBodyManualSupply = false;
        }
        #endregion

        #region  Class Data
        private string _From;
        private string _FromName;
        private string _To;
        private string _ToList;
        private string _Subject;
        private string _CC;
        private string _CCList;
        private string _BCC;
        private string _TemplateDoc;
        private string[] _ArrValues;
        private string _BCCList;
        private bool _MailBodyManualSupply;
        private string _MailBody;
        //private string _Attachment;
        private string[] _Attachment;
        private Attachment _Attach;
        private List<ByteAttachment> _ByteAttchments;

        public MailPriority _MailPriority;

        private System.Net.Mail.MailMessage myEmail;

        #endregion

        #region Properties
        public string From
        {
            set { _From = value; }
        }
        public string FromName
        {
            set { _FromName = value; }
        }
        public string To
        {
            set { _To = value; }
        }
        public string Subject
        {
            set { _Subject = value; }
        }
        public string CC
        {
            set { _CC = value; }
        }
        public string BCC
        {

            set { _BCC = value; }
        }
        public bool MailBodyManualSupply
        {

            set { _MailBodyManualSupply = value; }
        }
        public string MailBody
        {
            set { _MailBody = value; }
        }
        public string EmailTemplateFileName
        {
            //FILE NAME OF TEMPLATE ( MUST RESIDE IN ../EMAILTEMPLAES/ FOLDER ) 
            set { _TemplateDoc = value; }
        }
        public string[] ValueArray
        {
            //ARRAY OF VALUES TO REPLACE VARS IN TEMPLATE 
            set { _ArrValues = value; }
        }

        public string[] AttachFile
        {
            set { _Attachment = value; }
        }
        public Attachment Attach
        {
            set { _Attach = value; }
        }

        public List<ByteAttachment> ByteAttchments { set { _ByteAttchments = value; } }


       
        public MailPriority MailPriority
        {
            set { _MailPriority = value; }
        }
        #endregion

        #region Send Email async

        public async Task SendEmail()
        {
            myEmail.IsBodyHtml = true;
            if (_FromName == "")
                _FromName = _From;
            myEmail.From = new MailAddress(_From, _FromName);
            myEmail.Subject = _Subject;

            //---Set recipients in To List 
            _ToList = _To.Replace(";", ",");
            if (_ToList != "")
            {
                string[] arr = _ToList.Split(',');
                myEmail.To.Clear();
                if (arr.Length > 0)
                {
                    foreach (string address in arr)
                    {
                        myEmail.To.Add(new MailAddress(address));
                    }
                }
                else
                {
                    myEmail.To.Add(new MailAddress(_ToList));
                }
            }

            //---Set recipients in CC List 
            if (!String.IsNullOrWhiteSpace(_CC))
            {
                _CCList = _CC.Replace(";", ",");
                if (_CCList != "")
                {
                    string[] arr = _CCList.Split(',');
                    myEmail.CC.Clear();
                    if (arr.Length > 0)
                    {
                        foreach (string address in arr)
                        {
                            myEmail.CC.Add(new MailAddress(address));
                        }
                    }
                    else
                    {
                        myEmail.CC.Add(new MailAddress(_CCList));
                    }
                }
            }

            //---Set recipients in BCC List 
            if (!String.IsNullOrWhiteSpace(_BCC))
            {
                _BCCList = _BCC.Replace(";", ",");
                if (_BCCList != "")
                {
                    string[] arr = _BCCList.Split(',');
                    myEmail.Bcc.Clear();
                    if (arr.Length > 0)
                    {
                        foreach (string address in arr)
                        {
                            myEmail.Bcc.Add(new MailAddress(address));
                        }
                    }
                    else
                    {
                        myEmail.Bcc.Add(new MailAddress(_BCCList));
                    }
                }
            }

            //set mail body 
            if (_MailBodyManualSupply)
            {
                myEmail.Body = _MailBody;
            }
            else
            {
                myEmail.Body = GetHtml(_TemplateDoc);
                //& GetHtml("EML_Footer.htm") 
            }

            // set attachment 
            if (_Attachment != null)
            {
                for (int i = 0; i < _Attachment.Length; i++)
                {
                    if (_Attachment[i] != null)
                        myEmail.Attachments.Add(new Attachment(_Attachment[i]));
                }

            }
            if (_ByteAttchments!=null)
            {
                for (int i = 0; i < _ByteAttchments.Count; i++)
                {
                    if (_ByteAttchments[i] != null)
                        myEmail.Attachments.Add(new Attachment(new MemoryStream(_ByteAttchments[i].bytes), _ByteAttchments[i].byteFlieName));
                }
              
            }
            var smtpClient = new SmtpClient();
            smtpClient.Host = SiteKeys.SMTPServer;
            if (SiteKeys.IsLive == "1")
            {
                smtpClient.Host = SiteKeys.SMTPLiveHost;
                smtpClient.Port = SiteKeys.SMTPLivePort;
            }
            else {
                if (smtpClient.Host != "localhost")
                {
                    smtpClient.Host = SiteKeys.SMTPServer;
                    smtpClient.Credentials = new System.Net.NetworkCredential(SiteKeys.SMTPUserName, SiteKeys.SMTPPassword);
                    smtpClient.Port = Convert.ToInt32(SiteKeys.SmtpPort);
                    //smtpClient.EnableSsl = true;
                }
            }
           
          
            await smtpClient.SendMailAsync(myEmail);
        }

        #endregion

        #region SEND EMAIL

        public void Send()
        {
            myEmail.IsBodyHtml = true;
            if (_FromName == "")
                _FromName = _From;
            myEmail.From = new MailAddress(_From, _FromName);
            myEmail.Subject = _Subject;

            //---Set recipients in To List 
            _ToList = _To.Replace(";", ",");
            if (_ToList != "")
            {
                string[] arr = _ToList.Split(',');
                myEmail.To.Clear();
                if (arr.Length > 0)
                {
                    foreach (string address in arr)
                    {
                        myEmail.To.Add(new MailAddress(address));
                    }
                }
                else
                {
                    myEmail.To.Add(new MailAddress(_ToList));
                }
            }

            //---Set recipients in CC List 
            if (!String.IsNullOrWhiteSpace(_CC))
            {
                _CCList = _CC.Replace(";", ",");
                if (_CCList != "")
                {
                    string[] arr = _CCList.Split(',');
                    myEmail.CC.Clear();
                    if (arr.Length > 0)
                    {
                        foreach (string address in arr)
                        {
                            myEmail.CC.Add(new MailAddress(address));
                        }
                    }
                    else
                    {
                        myEmail.CC.Add(new MailAddress(_CCList));
                    }
                }
            }

            //---Set recipients in BCC List 
            if (!String.IsNullOrWhiteSpace(_BCC))
            {
                _BCCList = _BCC.Replace(";", ",");
                if (_BCCList != "")
                {
                    string[] arr = _BCCList.Split(',');
                    myEmail.Bcc.Clear();
                    if (arr.Length > 0)
                    {
                        foreach (string address in arr)
                        {
                            myEmail.Bcc.Add(new MailAddress(address));
                        }
                    }
                    else
                    {
                        myEmail.Bcc.Add(new MailAddress(_BCCList));
                    }
                }
            }

            //set mail body 
            if (_MailBodyManualSupply)
            {
                myEmail.Body = _MailBody;
            }
            else
            {
                myEmail.Body = GetHtml(_TemplateDoc);
                //& GetHtml("EML_Footer.htm") 
            }

            // set attachment 
            if (_Attachment != null)
            {
                for (int i = 0; i < _Attachment.Length; i++)
                {
                    if (_Attachment[i] != null)
                        myEmail.Attachments.Add(new Attachment(_Attachment[i]));
                }

            }
            if (_ByteAttchments != null)
            {
                for (int i = 0; i < _ByteAttchments.Count; i++)
                {
                    if (_ByteAttchments[i] != null)
                        myEmail.Attachments.Add(new Attachment(new MemoryStream(_ByteAttchments[i].bytes), _ByteAttchments[i].byteFlieName));
                }

            }
          

            myEmail.Priority = _MailPriority;
            var smtpClient = new SmtpClient();
            smtpClient.Host = SiteKeys.SMTPServer;
            if (SiteKeys.IsLive == "1")
            {
                smtpClient.Host = SiteKeys.SMTPLiveHost;
                smtpClient.Port = SiteKeys.SMTPLivePort;
            }
            else
            {
                if (smtpClient.Host != "localhost")
                {
                    smtpClient.Host = SiteKeys.SMTPServer;
                    smtpClient.Credentials = new System.Net.NetworkCredential(SiteKeys.SMTPUserName, SiteKeys.SMTPPassword);
                    smtpClient.Port = Convert.ToInt32(SiteKeys.SmtpPort);
                    //smtpClient.EnableSsl = true;
                }
            }
            try
            {
                smtpClient.Send(myEmail);
            }
            catch
            {

                throw;
            }
        }
        #endregion

        #region GetHtml
        public string GetHtml(string argTemplateDocument)
        {
            int i;
            StreamReader filePtr;
            string fileData = "";

            filePtr = File.OpenText(HttpContext.Current.Server.MapPath("~/EmailTemplate/") + argTemplateDocument);
            //filePtr = File.OpenText(ConfigurationSettings.AppSettings["EMLPath"] + argTemplateDocument);
            fileData = filePtr.ReadToEnd();
            filePtr.Close();
            filePtr = null;
            if ((_ArrValues == null))
            {
                return fileData;
            }
            else
            {
                for (i = _ArrValues.GetLowerBound(0); i <= _ArrValues.GetUpperBound(0); i++)
                {
                    fileData = fileData.Replace("@v" + i.ToString() + "@", (string)_ArrValues[i]);
                }
                return fileData;
            }
        }
        #endregion
    }

    public class ByteAttachment {

        public byte[] bytes
        {
            get;set;
        }

        public string byteFlieName
        {
            get; set;
        }
    }
}