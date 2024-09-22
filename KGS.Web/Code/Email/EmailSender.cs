using KGS.Data;
using KGS.EmailSender;
using KGS.Service;
using KGS.Web.Code.LIBS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using KGS.Core;

namespace KGS.Web.Code.Email
{
    public class EmailSender
    {
      
        private FlexiMail flexiEmail;

        public EmailSender()
        {
            this.flexiEmail = new FlexiMail();
     
        }

       

        //#region Verification

        //public async Task SendVerificationEmailAsync(User user)
        //{
        //    if (user != null)
        //    {
        //        EmailTemplate emailTemplate = emailTemplateService.GetEmailTemplatebyId((byte)EmailType.VerificationEmail);
        //        if (emailTemplate != null && emailTemplate.IsActive == true)
        //        {
        //            flexiEmail.To = user.Email;
        //            flexiEmail.CC = SiteKeys.MailCC;
        //            flexiEmail.BCC = SiteKeys.MailCC;
        //            flexiEmail.From = SiteKeys.SMTPUserName;
        //            flexiEmail.MailBodyManualSupply = true;
        //            flexiEmail.Subject = emailTemplate.Subject;
        //            string messageBody = emailTemplate.Content.Replace("##UserName##", user.FirstName)
        //            .Replace("##LinkButton##", "<a href='" + SiteKeys.Domain + "Home/VerificationLink/" + user.ActivationKey + "' target='_blank'>VerficationLink</a>");

        //            flexiEmail.MailBody = messageBody;
        //            await flexiEmail.SendEmail();
        //        }
        //    }


        //}


        //#endregion
        //public async Task SendSecretoryActivationMail(SecretoryActivationDto secretoryActivationModel)
        //{
        //    EmailTemplate emailTemplate = emailTemplateService.GetEmailTemplatebyId((byte)EmailType.SecretoryActivation);
        //    if (emailTemplate != null && emailTemplate.IsActive == true)
        //    {
        //        flexiEmail.To = secretoryActivationModel.SecretoryEmail;
        //        //flexiEmail.CC = SiteKeys.MailCC;
        //        //flexiEmail.BCC = SiteKeys.MailCC;
        //        flexiEmail.From = SiteKeys.SMTPUserName;
        //        flexiEmail.MailBodyManualSupply = true;
        //        flexiEmail.Subject = emailTemplate.Subject;
        //        string messageBody = emailTemplate.Content.Replace("##SecretaryName##", secretoryActivationModel.SecretoryName)
        //        .Replace("##DoctorName##", secretoryActivationModel.DrName).Replace("##Email##", secretoryActivationModel.SecretoryEmail).Replace("##Password##", secretoryActivationModel.SecretoryPassword).Replace("##UrlLink##", SiteKeys.Domain);

        //        flexiEmail.MailBody = messageBody;
        //        await flexiEmail.SendEmail();
        //    }

        //}

        /// <summary>
        

        

    }
}