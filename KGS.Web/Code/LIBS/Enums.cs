
using System.ComponentModel;

namespace KGS.Web.Code.LIBS
{

    //public static class EnumHelper
    //{
    //    public static string GetDescription<TEnum>(this TEnum value)
    //    {
    //        var fi = value.GetType().GetField(value.ToString());

    //        if (fi != null)
    //        {
    //            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

    //            if (attributes.Length > 0)
    //            {
    //                return attributes[0].Description;
    //            }
    //        }

    //        return value.ToString();
    //    }

    //}
    public enum MessageType
    {
        Warning,
        Success,
        Danger,
        Info
    }

    public enum ModalSize
    {
        Small,
        Large,
        Medium
    }

 

    public enum Gender
    {
        [Description("Male")]
        Male = 'M',
        [Description("Female")]
        Female = 'F',
        [Description("Other")]
        Other = 'O'
    }
    public enum EmailType
    {
        General = 1,
        AdminForgotPassword = 2,
        ResetPassword = 3,
        Enquiry = 4,
        VerificationEmail = 5,
        SecretoryActivation = 6,
        UserActivation = 7,
        SubmitIntakeForm = 8,
        Rejection = 9,
        SecretaryVerified = 10,
        NewIntakeRecieved = 11,
        AppointmentEmailToPatient = 12,
        AppointmentEmailToDoctor = 13,
        AppointmentRejectEmailToPatient = 14,
        PatientFeedbackToDoctor = 16,
        DoctorReplyToPatient = 17,
        PatientAccountDeleteRequest = 18,
        PatientAccountDeletedMessage = 19,
            ReplyToAccountDeleteRequest = 20,
        DoctorRegistrationEmailForDoctor = 21,
        DoctorRegistrationEmailForAdmin= 22,
        DoctorAccountStatusChanges = 23,

        RecommendationStatusChanged = 24,
        OtherDiagnosisApproval = 25,
        DeleteAccount = 26,
        IntakeProcessChanged = 27,
        DoctorReplyReceived = 28,
        RecommendationReceived=30,
        RecommendationBrandUpdated = 31,
        SendMessageNotification = 32,
        SupportRequestReceived = 34,
        SupportAcknowledgement = 35,
        SupportReplyReceived = 36,
        SendNotificationToDelegateCaregiver=37,
        SendNotificationToSharedCaregiver = 38,
        StopSharing = 39,
        ChangePasswordByAdmin = 42,
        ExistingUserAdded = 43,
        ExistingUserUpdated= 44,
        AppointmentRequestRecieved = 45,
        AppointmentRequestStatusChange = 46,
        AssignSecretaryToSameClinic = 47,
        AppointmentScheduled = 48,
        AppointmentStatusChanged =49,
        AvailabilityChanged = 50,
        AppointmentAvailabilityDelete=51,
        HealthRecordSubmitted = 52,
        HealthRecordStatusChanged = 53,
        AppointmentDeleteEmailToDoctor = 54,
        AppointmentDeleteEmailToPatient = 55,
        EmailToUserForSubscriptionChange = 56,
        EmailToDoctorForRenewable = 57,
    };

    public enum LanguageType
    {
        [Description("English")]
        English = 1,
        [Description("Hindi")]
        Hindi = 2,

    }

    public enum SpecialityTypes
    {
        [Description("MD")]
        MD = 1,
        [Description("Physician")]
        Physician = 2,
        [Description("Surgeon")]
        Surgeon = 3,
    }



   

   
    public enum RatioType
    {
        HighRatio = 1,
        LowRatio = 2,
        Custom = 3
    }

    public enum CBDHighRatioTypes
    {
        [Description("27:1")]
        Ratio27 = 27,
        [Description("25:1")]
        Ratio25 = 25,
        [Description("20:1")]
        Ratio20 = 20,
        [Description("18:1")]
        Ratio18 = 18,
        [Description("15:1")]
        Ratio15 = 15,
        [Description("12:1")]
        Ratio12 = 12,

    }
    public enum CBDLowRatioTypes
    {
        [Description("10:1")]
        Ratio10 = 9,
        [Description("8:1")]
        Ratio8 = 8,
        [Description("4:1")]
        Ratio4 = 4,
        [Description("3:1")]
        Ratio3 = 3,
        [Description("2:1")]
        Ratio2 = 2,
        [Description("1:1")]
        Ratio1 = 1,

    }

    public enum DosesTypes
    {
        [Description("CBD")]
        CBD = 1,
        [Description("CBDA")]
        CBDA = 2,
        [Description("CBDV")]
        CBDV = 3,
        [Description("CBG")]
        CBG = 4,
        [Description("CBN")]
        CBN = 5,
        [Description("THC")]
        THC = 6,
        [Description("THCA")]
        THCA = 7,
        [Description("THCV ")]
        THCV = 8
    }


    public enum THCDoseTypes
    {
        [Description("1 mg")]
        THC1 = 1,
        [Description("2.5 mg")]
        THC2 = 2,
        [Description("5 mg")]
        THC5 = 5,
        [Description("7.5 mg")]
        THC7 = 7,
        [Description("10 mg")]
        THC10 = 10,
        [Description("12.5 mg")]
        THC12 = 12,
        [Description("15 mg")]
        THC15 = 15,

    }





    public enum FrequencyTypes
    {
        [Description("Every 8 hours")]
        Frequency8 = 1,
        [Description("Every 12 hours")]
        Frequency12 = 2,
        [Description("Once a day")]
        Frequency1 = 3,
        [Description("Twice a day")]
        Frequency2 = 4,
        [Description("Three times a day")]
        Frequency3 = 5,
        [Description("Once a week")]
        FrequencyOnceWeek = 6,
        [Description("Three times a week")]
        FrequencyThreeWeek = 7,
        [Description("Other")]
        Other = 0
    }


    public enum LongIncreasingDose
    {
        [Description("One day")]
        OneDay = 1,
        [Description("One week")]
        OneWeek = 2,
        [Description("Two weeks")]
        TwoWeeks = 3,
        [Description("Other")]
        Other = 0
    }

    public enum MuchIncreasingDose
    {
        [Description("0.1 ml per dose")]
        OnePerDose = 1,
        [Description("0.2 ml per dose")]
        TwoPerDose = 2,
        [Description("10 mg per dose")]
        TenPerDose = 3,
        [Description("10-20 mg per dose")]
        TwentyPerDose = 4,
        [Description("Other")]
        Other = 0
    }

    public enum WatchFor
    {
        [Description("CBG")]
        CBG = 1,
        [Description("CBD")]
        CBD = 2,
        [Description("THC")]
        THC = 3
    }

   


    public enum SupplementBrandTypes
    {
        [Description("Treatwell (http://truefarma.com/)")]
        Treatwell = 1,
        [Description("Myriam’s Hope (https://myriamshope.org/)")]
        Myriam = 2,
        [Description("Fiddler’s Greens (http://fiddlers-greens.com/)")]
        Fiddler = 3,
        [Description("CannaKids (http://cannakids.org/)")]
        CannaKids = 4,
        [Description("Care By Design (https://www.cbd.org/)")]
        Care = 5,
        [Description("Other")]
        Other = 6
    }
    public enum SupplementPurchasePlace
    {
        [Description("Flower Child Herbals")]
        Flower = 1,
        [Description("CannaKids")]
        CannaKids = 2,
        [Description("Care By Design")]
        Carebydesign = 3,
        [Description("Fiddler’s Greens")]
        Feddlergreen = 4,
        [Description("Myriam’s Hope")]
        Marianshope = 5,
        [Description("TrueFarma")]
        TrueFarma = 6,
        [Description("Other")]
        Other = 7       
    }





    public enum GenderType
    {
        Male = 1,
        Female = 2
    }

    public enum Days
    {
        Mo = 1,
        Tu = 2,
        Wd = 3,
        Th = 4,
        Fr = 5,
        Sa = 6,
        Su = 7
    }

   

    public enum GroupRadioButtons
    {
        [Description("Yes")]
        Yes = 1,
        [Description("No")]
        No = 2,
        [Description("Something Else")]
        SomethingElse =3,

       
    }

}
