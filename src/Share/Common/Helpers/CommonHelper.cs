using System.Collections;
using System.Reflection;
using KarnelTravel.Share.Common.Contants;
using KarnelTravel.Share.Common.Models;
using Share.Common.Extensions;

namespace KarnelTravel.Share.Common.Helpers;
public static class CommonHelper
{
    public static string Str(int code)
    {
        return code.ToString();
    }

    public static string NullToEmpty(string input)
    {
        return If(!string.IsNullOrWhiteSpace(input), input, string.Empty);
    }

    public static T If<T>(bool condition, T valueIfTrue, T valueIfFalse = default(T))
    {
        return condition ? valueIfTrue : valueIfFalse;
    }

    public static string StrIf<T>(bool condition, T valueIfTrue, T valueIfFalse = default(T))
    {
        var ret = If(condition, valueIfTrue, valueIfFalse);
        var retStr = ret != null ? ret.ToString() : string.Empty;
        return retStr;
    }

    public static bool IsNullEmpty(string value)
    {
        return string.IsNullOrWhiteSpace(value);
    }

    public static bool IsNullEmpty<T>(IEnumerable<T> collection)
    {
        return collection == null || !collection.Any();
    }

    /// <summary>
    /// Objects to hashtable.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <returns></returns>
    public static Hashtable ObjectToHashtable(object obj)
    {
        var resultHash = new Hashtable();

        foreach (PropertyInfo prop in obj.GetType().GetProperties())
        {
            var propName = prop.Name;
            var val = obj.GetType().GetProperty(propName)?.GetValue(obj, null);

            resultHash.Add(propName, val?.ToString());
        }

        return resultHash;
    }


    /// <summary>
    /// Hastables the trim.
    /// </summary>
    /// <param name="hs">The hashtable</param>
    /// <returns></returns>
    public static Hashtable HastableTrim(Hashtable hs)
    {
        var tempHs = (Hashtable)hs.Clone();
        foreach (DictionaryEntry de in hs)
        {
            if (de.Value is string)
                tempHs[de.Key] = de.Value.ToString().Trim();

            if (de.Value is Hashtable hashtable)
                tempHs[de.Key] = HastableTrim(hashtable);
        }
        return tempHs;
    }


    /// <summary>
    /// Objects the class trim all properties is string.
    /// This function like HastableTrim
    /// </summary>
    /// <param name="obj">The object class</param>
    public static void ObjectClassTrimString(object obj)
    {
        if (!obj.GetType().GetTypeInfo().IsClass) return;

        // Trim for property is string
        var props = obj.GetType()
            // is public props 
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            // Ignore non-string properties
            .Where(prop => prop.PropertyType == typeof(string))
            // Ignore indexers
            .Where(prop => prop.GetIndexParameters().Length == 0)
            // Must be both readable and writable
            .Where(prop => prop.CanWrite && prop.CanRead);


        foreach (PropertyInfo prop in props)
        {
            var value = (string)prop.GetValue(obj, null);
            if (value != null)
            {
                value = value.Trim();
                prop.SetValue(obj, value, null);
            }
        }
    }

    /// <summary>
    /// Determines whether an object status is in valid range
    /// </summary>
    /// <param name="objStatus">object status</param>
    /// <returns>
    ///   <c>true</c> if object status is in valid range; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsObjectStatusValid(this int? objStatus)
    {
        return objStatus.HasValue && IsObjectStatusValid(objStatus.Value);
    }

    /// <summary>
    /// Determines whether an object status is in valid range
    /// </summary>
    /// <param name="objStatus">object status</param>
    /// <returns>
    ///   <c>true</c> if object status is in valid range; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsObjectStatusValid(this byte? objStatus)
    {
        return objStatus.HasValue && IsObjectStatusValid(objStatus.Value);
    }

    /// <summary>
    /// Determines whether the specified gender is valid
    /// </summary>
    /// <param name="gender">gender</param>
    public static bool IsValidGender(string gender)
    {
        return gender == GenderConstant.MALE
            || gender == GenderConstant.FEMALE
            || gender == GenderConstant.OTHER;
    }

    /// <summary>
    /// Determines whether the specified gender is valid
    /// </summary>
    /// <param name="deviceType">device type</param>
    public static bool IsValidDeviceType(int deviceType)
    {
        return deviceType == DeviceTypeConstant.ANDROID
            || deviceType == DeviceTypeConstant.IOS;
    }

    /// <summary>
    /// Extracts the merchant code to build information such as membership number
    /// </summary>
    /// <param name="merchantCode">merchant code</param>
    /// <param name="extractLength">length of the necessary part</param>
    /// <returns></returns>
    public static string ExtractMerchantCode(string merchantCode, int extractLength = 6)
    {
        return merchantCode.Substring(0, extractLength);
    }

    /// <summary>
    /// Generate card number (Membership card code)
    /// </summary>
    /// <param name="programCode">brand code</param>
    /// <param name="phoneNumber">phone number</param>
    /// <returns></returns>
    public static string GenerateCardNumber(string programCode, string phoneNumber)
    {
        var phoneNumberPart = phoneNumber.Substring(2, phoneNumber.Length - 2);
        return $"{programCode.Trim().Substring(0, 16 - phoneNumberPart.Length)}{phoneNumberPart}";
    }

    /// <summary>
    /// Formats money amount
    /// </summary>
    /// <param name="amount">amount</param>
    /// <returns></returns>
    public static string FormatMoney(this decimal amount)
    {
        return amount.ToString(CommonFormatConstant.DEFAULT_MONEY_FORMAT);
    }

    /// <summary>
    /// Calculates the age at the specified date
    /// </summary>
    /// <param name="dateOfBirth">user date of birth</param>
    /// <returns></returns>
    public static int ComputeAge(DateTime dateOfBirth)
    {
        var currentTime = DateTime.UtcNow;
        var age = currentTime.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > currentTime.AddYears(-age).Date)
            age--;
        return age;
    }

    public static AppActionResultData<IList<Guid>> ToGuidIds(this IEnumerable<string> strIds)
    {
        AppActionResultData<IList<Guid>> appActionResultData = new AppActionResultData<IList<Guid>>();
        List<Guid> list = new List<Guid>();
        if (strIds.IsNotNullNorEmpty())
        {
            foreach (string strId in strIds)
            {
                if (Guid.TryParse(strId, out Guid guidId))
                {
                    list.Add(guidId);
                    continue;
                }

                appActionResultData.BuildError(strId);
                return appActionResultData;
            }
        }

        appActionResultData.BuildResult(list, "Success");
        return appActionResultData;
    }
}
