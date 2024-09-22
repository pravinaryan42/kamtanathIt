using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

public static class Extensions
{
    #region "Dictionary"

    public static void AddOrReplace(this IDictionary<string, object> DICT, string key, object value)
    {
        if (DICT.ContainsKey(key))
            DICT[key] = value;
        else
            DICT.Add(key, value);
    }

    public static dynamic GetObjectOrDefault(this IDictionary<string, object> DICT, string key)
    {
        if (DICT.ContainsKey(key))
            return DICT[key];
        else
            return null;
    }

    public static T GetObjectOrDefault<T>(this IDictionary<string, object> DICT, string key)
    {
        if (DICT.ContainsKey(key))
            return (T)Convert.ChangeType(DICT[key], typeof(T));
        else
            return default(T);
    }

    #endregion "Dictionary"

    #region "String"

    public static string ToSelfURL(this string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;

        string outputStr = text.Trim().Replace(":", "").Replace("&", "").Replace(" ", "-").Replace("'", "").Replace(",", "").Replace("(", "").Replace(")", "").Replace("--", "").Replace(".", "");
        return Regex.Replace(outputStr.Trim().ToLower().Replace("--", ""), "[^a-zA-Z0-9_-]+", "", RegexOptions.Compiled);
    }

    public static string TrimLength(this string input, int length, bool Incomplete = true)
    {
        if (String.IsNullOrEmpty(input)) { return String.Empty; }
        return input.Length > length ? String.Concat(input.Substring(0, length), Incomplete ? "..." : "") : input;
    }

    public static string ToTitle(this string input)
    {
        return String.IsNullOrEmpty(input) ? String.Empty : CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
    }

    public static bool ContainsAny(this string input, params string[] values)
    {
        return String.IsNullOrEmpty(input) ? false : values.Any(S => input.Contains(S));
    }

    public static string ToXML<T>(this T xmlObject)
    {
        var stringwriter = new System.IO.StringWriter();
        var serializer = new XmlSerializer(xmlObject.GetType());
        serializer.Serialize(stringwriter, xmlObject);
        return stringwriter.ToString().Replace("_x003A_", ":");
    }

    #endregion "String"

    #region "Collection"

    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        if (source != null && source.Count() >= 0)
        {
            foreach (T element in source)
            {
                action(element);
            }
        }
    }

    public static bool IsNotNullAndNotEmpty<T>(this ICollection<T> source)
    {
        return source != null && source.Count() > 0;
    }

    #endregion "Collection"

    #region "DateTime"

    //public static DateTime ToDateTime(this string str, bool isWithTime = false)
    //{
    //    if (string.IsNullOrWhiteSpace(str))
    //        return DateTime.Now;

    //    if (isWithTime)
    //    {
    //        return DateTime.ParseExact(str, "dd-MMM-yyyy hh:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None);
    //    }

    //    return DateTime.ParseExact(str, "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
    //}
    //public static DateTime ToDateTime(this string str, string format)
    //{
    //    if (string.IsNullOrWhiteSpace(str))
    //        return DateTime.Now;

    //    return DateTime.ParseExact(str, format, CultureInfo.InvariantCulture, DateTimeStyles.None);
    //}
    public static DateTime ToDateTime(this string str, bool isWithTime = false)
    {
        if (string.IsNullOrWhiteSpace(str))
            return DateTime.Now;

        string[] formats = { "dd/MM/yyyy", "d/MM/yyyy", "dd/M/yyyy", "dd/MM/yyyy h:mm:ss tt", "d/MM/yyyy h:mm:ss tt", "dd/M/yyyy h:mm:ss tt", "yyyy-MM-dd", "yyyy-M-dd", "yyyy-MM-d", "yyyy-MM-dd h:mm:ss tt", "yyyy-M-dd h:mm:ss tt", "yyyy-MM-d h:mm:ss tt", "dd-MM-yyyy", "dd-MMM-yyyy", "d-MM-yyyy", "dd-M-yyyy", "dd-MM-yyyy h:mm:ss tt", "d-MM-yyyy h:mm:ss tt", "dd-M-yyyy h:mm:ss tt", "yyyy/MM/dd", "yyyy/M/dd", "yyyy/MM/d", "yyyy/MM/dd  h:mm:ss tt", "yyyy/M/dd  h:mm:ss tt", "yyyy/MM/d  h:mm:ss tt", "d/M/yyyy h:mm:ss tt", "M/dd/yyyy h:mm:ss tt", "MM/dd/yyyy h:mm:ss tt", "MM/d/yyyy h:mm:ss tt", "M/dd/yyyy", "MM/dd/yyyy", "MM/d/yyyy", "yyyyMMdd", "d/M/yy" };
        if (isWithTime)
        {
            return DateTime.ParseExact(str, formats, CultureInfo.InvariantCulture, DateTimeStyles.None);
        }

        return DateTime.ParseExact(str, formats, CultureInfo.InvariantCulture, DateTimeStyles.None);
    }

    public static string ToFormatString(this DateTime? date)
    {
        if (!date.HasValue)
            return string.Empty;

        return date.Value.ToString("dd MMM, yyyy");
    }

    public static string ToFormatString(this DateTime date)
    {
        return date.ToString("dd MMM, yyyy");
    }

    public static string ToFormatCustomString(this DateTime date)
    {
        return date.ToString("dd-MMM-yyyy");
    }

    public static string ToFormatCustomString(this DateTime date, string dateformat)
    {
        return date.ToString(dateformat);
    }

    public static string ToFormatCustomString(this DateTime? date)
    {
        if (!date.HasValue)
            return string.Empty;

        return date.Value.ToString("dd-MMM-yyyy");
    }

    public static string ToFormatDateString(this DateTime? date, bool isIncludeTime = false)
    {
        if (!date.HasValue)
            return string.Empty;

        if (isIncludeTime)
            return date.Value.ToString("dd-MMM-yyyy hh:mm tt");

        return date.Value.ToString("dd-MMM-yyyy");
    }
    public static string ToFormatDateUSString(this DateTime? date)
    {
        if (!date.HasValue)
            return string.Empty;
          return date.Value.ToString("MM/dd/yyyy");
    }

    public static string ToFormatString(this TimeSpan? time)
    {
        if (!time.HasValue)
        {
            return string.Empty;
        }
        else {
            DateTime timeInString = DateTime.Today.Add(time.Value);
           return timeInString.ToString("hh:mm tt");
        }
          
    }


    public static string ToFormatDateString(this DateTime date, bool isIncludeTime = false)
    {
        if (isIncludeTime)
            return date.ToLocalTime().ToString("dd-MMM-yyyy hh:mm tt");

        return date.ToLocalTime().ToString("dd-MMM-yyyy");
    }

    public static string ToFormatDateTimeString(this DateTime date, bool isIncludeTime = false)
    {
        string formatedDate = string.Empty;
        if (isIncludeTime)
        {
            formatedDate = date.ToString("dd/MM/yyyy hh:mm tt", System.Globalization.CultureInfo.InvariantCulture);
        }
        else
            formatedDate = date.ToLocalTime().ToString("dd/MM/yyyy");
        return formatedDate;
    }

    public static string ToFormatDateString(this DateTime date, string format)
    {
        return date.ToLocalTime().ToString(format);
    }

    #endregion "DateTime"

    #region EnumValue

    public static T GetValueFromDescription<T>(string description)
    {
        try
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description?.ToLower() == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name?.ToLower() == description)
                        return (T)field.GetValue(null);
                }
            }
            return default(T);
        }


        catch (Exception ex)
        {
            return default(T);
        }

    }


    #endregion

    public static string RandomPassword()
    {
        int length = 15;
        // Create a string of characters, numbers, special characters that allowed in the password  
        string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
        Random random = new Random();

        // Select one random character at a time from the string  
        // and create an array of chars  
        char[] chars = new char[length];
        for (int i = 0; i < length; i++)
        {
            chars[i] = validChars[random.Next(0, validChars.Length)];
        }
        return new string(chars);
    }




    public static string GenerateRandomString(int size)
    {
        var temp = Guid.NewGuid().ToString().Replace("-", string.Empty);
        var barcode = Regex.Replace(temp, "[a-zA-Z]", string.Empty).Substring(0, size);
        return barcode;
    }


}



