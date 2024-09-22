namespace KGS.Core
{
    public class ApplicationConstant
    {
        public const string PasswordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,15}$";
        public const string MOBILENUMBER_REGEX_VALIDATOR = @"^[0-9]{4,16}$";

        //public const string PHONENUMBER_VALIDATION_MESSAGE = "Min should be 4 and max should be 16 digits only";
        //public const string ZIPCODE_REGEX_VALIDATOR = @"/^\d{6}$/";
        //public const string ZIPCODE_REGEX_VALIDATOR = @"^[0-9]{5}(?:-[0-9]{4})?$";
        public const string ZIPCODE_VALIDATION_MESSAGE = "Zip code is invalid.";

        public const string DateFormat = "MM/dd/yyyy";
        public const string DateTimeFormat = "MM/dd/yyyy hh:mm tt";
        public const string TimeFormat = "hh:mm tt";
        public const string DatePlaceHolder = "MM/DD/YYYY";
        public const string TimePlaceHolder = "HH:MM TT";
        public const string DateTimePlaceHolder = "MM/DD/YYYY HH:MM tt";
        public const string NON_NEGATIVE_NUMBER_REGEX = @"/^\d+$/";
    }
}