using System.Globalization;
using KarnelTravel.Share.Common.Models;
using Share.Common.Extensions;

namespace Share.Common.Helpers;
public static class DateTimeHelper
{
    public const string DEFAULT_DATE_FORMAT = "yyyy-MM-dd";
    public const string DEFAULT_DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
    public const string DEFAULT_LONG_DATE_TIME_FORMAT = "dd/MM/yyyy HH:mm:ss.fff";
    public const string SHORT_DATE_TIME_FORMAT = "dd/MM/yyyy HH:mm:ss";
    public const string VN_DATE_FORMAT = "dd/MM/yyyy";
    public const string POSTFIX_DATETIME_FORMAT = "yyyyMMddHHmmssfff";
    public const string KEY_DATETIME_FORMAT = "yyyyMMddHHmmssfff";
    public const string DEFAULT_DATETIME_FORMAT_IN_REPORT = "yyyyMMddHHmmss";

    public const string SHORT_TIME_FORMAT = "c";
    public const string TIME_SPAN_FORMAT = @"d\.hh\:mm\:ss";
    public const string DEFAULT_TIME_SPAN_FORMAT = @"hh\:mm\:ss";
    public const string DEFAULT_TIME_FORMAT = "HH:mm:ss";
    public const string DEFAULT_SHORT_TIME_FORMAT = "HH:mm";

    public const string DEFAULT_DATETIME_OFFSET_FORMAT = "yyyy-MM-ddTHH:mm:sszzz";

    public const string UNKNOWN_DAY = "XX";
    public const string UNKNOWN_MONTH = "XX";
    public const string UNKNOWN_YEAR = "XXXX";
    public const string UNKNOWN_DATE = "XXXXXXXX";

    public const string DAY_FORMAT = "00";
    public const string MONTH_FORMAT = "00";
    public const string YEAR_FORMAT = "0000";

    public const string MILLISECOND = "Millisecond";
    public const string SECOND = "Second";
    public const string MINUTE = "Minute";
    public const string HOUR = "Hour";
    public const string DAY = "Day";
    public const string MONTH = "Month";
    public const string YEAR = "Year";

    public const int HOUR_TO_MINUTES = 60;
    public const int HOUR_TO_SECONDS = 3600;
    public const int HOUR_TO_MILISECONDS = 3600000;

    public const int MINUTE_TO_SECONDS = 60;
    public const int MINUTE_TO_MILISECONDS = 60000;

    public const int SECOND_TO_MILISECONDS = 1000;

    public static readonly DateTime MIN_VALUE = new DateTime(1980, 1, 1, 0, 0, 0, 0);
    public static readonly DateTime MAX_VALUE = new DateTime(2300, 1, 1, 0, 0, 0, 0);

    #region Get Date-Time Strings

    /// <summary>
    /// Gets the formatted date.
    /// </summary>
    /// <param name="date">date</param>
    /// <param name="format">format</param>
    /// <returns></returns>
    public static string GetFormattedDate(this DateTime date, string format)
    {
        return date.ToString(format, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Gets the formatted date.
    /// </summary>
    /// <param name="date">date</param>
    /// <param name="format">format</param>
    /// <returns></returns>
    public static string GetFormattedDate(this DateTime? date, string format)
    {
        return date.HasValue ? date.Value.GetFormattedDate(format) : string.Empty;
    }

    /// <summary>
    /// Gets the formatted date.
    /// </summary>
    /// <param name="date">date</param>
    /// <param name="format">format</param>
    /// <returns></returns>
    public static string GetFormattedDate(this DateTimeOffset date, string format)
    {
        return date.ToString(format, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Gets the formatted date.
    /// </summary>
    /// <param name="date">date</param>
    /// <param name="format">format</param>
    /// <returns></returns>
    public static string GetFormattedDate(this DateTimeOffset? date, string format)
    {
        return date.HasValue ? date.Value.GetFormattedDate(format) : string.Empty;
    }

    /// <summary>
    /// Gets the formatted date.
    /// </summary>
    /// <param name="date">date</param>
    /// <returns></returns>
    public static string GetFormattedDate(this DateTime date)
    {
        return GetFormattedDate(date, DEFAULT_DATE_FORMAT);
    }

    /// <summary>
    /// Gets the formatted date.
    /// </summary>
    /// <param name="date">date</param>
    /// <returns></returns>
    public static string GetFormattedDate(this DateTime? date)
    {
        return GetFormattedDate(date, DEFAULT_DATE_FORMAT);
    }

    /// <summary>
    /// Gets the formatted date with long format
    /// </summary>
    /// <param name="date">date</param>
    /// <returns></returns>
    public static string GetFormattedLongDate(this DateTime date)
    {
        return GetFormattedDate(date, DEFAULT_LONG_DATE_TIME_FORMAT);
    }

    /// <summary>
    /// Gets the formatted date with long format
    /// </summary>
    /// <param name="date">date</param>
    /// <returns></returns>
    public static string GetFormattedLongDate(this DateTime? date)
    {
        return GetFormattedDate(date, DEFAULT_LONG_DATE_TIME_FORMAT);
    }

    /// <summary>
    /// Gets the formatted time span.
    /// </summary>
    /// <param name="ts">time</param>
    /// <param name="format">format</param>
    /// <returns></returns>
    public static string GetFormattedTimeSpan(this TimeSpan ts, string format)
    {
        return ts.ToString(format, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Formats the date/time
    /// </summary>
    /// <param name="dateTime">date-time</param>
    /// <returns></returns>
    public static string GetFormattedDateTime(this DateTime? dateTime)
    {
        return dateTime.GetFormattedDate(SHORT_DATE_TIME_FORMAT);
    }

    /// <summary>
    /// Formats the date/time
    /// </summary>
    /// <param name="dateTime">date-time</param>
    /// <returns></returns>
    public static string GetFormattedDateTime(this DateTime dateTime)
    {
        return dateTime.GetFormattedDate(SHORT_DATE_TIME_FORMAT);
    }

    /// <summary>
    /// Gets the formatted time span.
    /// </summary>
    /// <param name="ts">ts</param>
    /// <returns></returns>
    public static string GetFormattedTimeSpan(this TimeSpan ts)
    {
        return ts.ToString(DEFAULT_TIME_SPAN_FORMAT, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Gets the postfix date time.
    /// </summary>
    /// <param name="datetime">datetime</param>
    /// <returns></returns>
    public static string GetPostfixDateTime(this DateTime? datetime)
    {
        return GetFormattedDate(datetime, POSTFIX_DATETIME_FORMAT);
    }

    /// <summary>
    /// Gets the postfix date time.
    /// </summary>
    /// <param name="datetime">datetime</param>
    /// <returns></returns>
    public static string GetPostfixDateTime(this DateTime datetime)
    {
        return GetFormattedDate(datetime, POSTFIX_DATETIME_FORMAT);
    }

    /// <summary>
    /// Gets the short time string.
    /// </summary>
    /// <param name="ts">ts</param>
    /// <returns></returns>
    public static string GetShortTimeString(this TimeSpan? ts)
    {
        return ts.HasValue ? $"{ts.Value.Hours:D2}:{ts.Value.Minutes:D2}:{ts.Value.Seconds:D2}" : string.Empty;
    }

    /// <summary>
    /// Res the format date.
    /// </summary>
    /// <param name="strDate">string date</param>
    /// <param name="expectedDateFormat">expected date format</param>
    /// <returns></returns>
    public static string ReFormatDate(this string strDate, string expectedDateFormat = DEFAULT_DATE_FORMAT)
    {
        var strFormattedDate = strDate;
        if (TryParseDateTime(strDate, out DateTime date))
        {
            strFormattedDate = GetFormattedDate(date, expectedDateFormat);
        }
        return strFormattedDate;
    }

    /// <summary>
    /// Gets the formatted datetime of now as date-part of a key
    /// </summary>
    /// <returns></returns>
    public static string GetDateTimeKeyPart()
    {
        return DateTime.Now.ToString(KEY_DATETIME_FORMAT, CultureInfo.InvariantCulture);
    }

    #endregion Get Date-Time Strings
    #region Parse Date-Time

    /// <summary>
    /// Tries the parse date time.
    /// </summary>
    /// <param name="strDateTime">string date time</param>
    /// <param name="datetime">datetime</param>
    /// <param name="exactFormat">exact date format</param>
    /// <returns></returns>
    public static bool TryParseDateTime(this string strDateTime, out DateTime datetime, string exactFormat = null)
    {
        datetime = DateTime.MinValue;
        if (!string.IsNullOrEmpty(strDateTime))
        {
            var dateFormat = exactFormat.IsNotNullNorEmpty() ? exactFormat : DEFAULT_DATE_FORMAT;
            return DateTime.TryParseExact(strDateTime, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out datetime);
        }
        else
        {
            return false;
        }
    }

    #endregion Parse Date-Time
    #region Time

    /// <summary>
    /// Tries the parse short time.
    /// </summary>
    /// <param name="strTime">string time</param>
    /// <param name="time">time</param>
    /// <returns></returns>
    public static bool TryParseShortTime(this string strTime, out TimeSpan time)
    {
        return TimeSpan.TryParseExact(strTime, SHORT_TIME_FORMAT, CultureInfo.InvariantCulture, TimeSpanStyles.None, out time);
    }

    /// <summary>
    /// Tries the parse time.
    /// </summary>
    /// <param name="strTime">string time</param>
    /// <param name="exactFormat">exact time format</param>
    /// <returns></returns>
    public static TimeSpan? TryParseTime(this string strTime, string exactFormat = null)
    {
        TimeSpan? time = null;
        if (!string.IsNullOrEmpty(strTime))
        {
            var timeFormat = exactFormat.IsNotNullNorEmpty() ? exactFormat : DEFAULT_TIME_FORMAT;
            if (DateTime.TryParseExact(strTime, timeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
            {
                time = dt.TimeOfDay;
            }
        }
        return time;
    }

    /// <summary>
    /// Determines whether [is time period valid].
    /// </summary>
    /// <param name="timePeriod">time period</param>
    /// <returns>
    ///   <c>true</c> if [is time period valid] [the specified time period]; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsTimePeriodValid(this TimeSpan?[] timePeriod)
    {
        var isValid = false;
        if (timePeriod != null && timePeriod.Length == 2)
        {
            isValid = timePeriod[0].HasValue && timePeriod[1].HasValue;
        }
        return isValid;
    }

    /// <summary>
    /// Merges the date time.
    /// </summary>
    /// <param name="date">date</param>
    /// <param name="time">time</param>
    /// <returns></returns>
    public static DateTime MergeDateTime(DateTime date, TimeSpan time)
    {
        return date.Date.AddHours(time.Hours).AddMinutes(time.Minutes).AddSeconds(time.Seconds).AddMilliseconds(time.Milliseconds);
    }

    /// <summary>
    /// [31-Dec-2015 Tuan] EndTime less than StartTime means that EndTime in the next date
    /// </summary>
    /// <param name="date">date</param>
    /// <param name="timePeriod">time period</param>
    /// <returns></returns>
    public static DateTime[] GetDateTimePeriod(DateTime date, TimeSpan?[] timePeriod)
    {
        DateTime[] dateTimePeriod = null;
        if (IsTimePeriodValid(timePeriod))
        {
            dateTimePeriod = new DateTime[2];
            dateTimePeriod[0] = MergeDateTime(date, timePeriod[0].Value);
            dateTimePeriod[1] = MergeDateTime(date, timePeriod[1].Value);
            if (dateTimePeriod[0] > dateTimePeriod[1])
            {
                dateTimePeriod[1] = dateTimePeriod[1].AddDays(1);
            }
        }
        return dateTimePeriod;
    }

    #endregion Time
    #region DateTimeOffset

    /// <summary>
    /// Gets the formatted date time offset.
    /// </summary>
    /// <param name="datetimeOffset">datetime offset</param>
    /// <param name="format">format</param>
    /// <returns></returns>
    public static string GetFormattedDateTimeOffset(this DateTimeOffset? datetimeOffset, string format)
    {
        return datetimeOffset.HasValue ? GetFormattedDate(datetimeOffset.Value.DateTime, format) : string.Empty;
    }

    /// <summary>
    /// Tries the parse date time offset.
    /// </summary>
    /// <param name="strDatetimeOffset">string datetime offset</param>
    /// <param name="exactFormat">additional formats</param>
    /// <returns></returns>
    public static DateTimeOffset? TryParseDateTimeOffset(string strDatetimeOffset, string exactFormat = null)
    {
        if (TryParseDateTime(strDatetimeOffset, out DateTime dt, exactFormat))
        {
            return new DateTimeOffset(dt);
        }
        return null;
    }

    #endregion DateTimeOffset
    #region Approximate DOB

    /// <summary>
    /// Gets the approximate date part.
    /// </summary>
    /// <param name="datePartVal">date part value</param>
    /// <param name="format">format</param>
    /// <param name="defaultValue">default value</param>
    /// <returns></returns>
    private static string GetApproximateDatePart(int? datePartVal, string format, string defaultValue)
    {
        return (datePartVal.HasValue && datePartVal.Value != 0) ? datePartVal.Value.ToString(format) : defaultValue;
    }

    /// <summary>
    /// Gets the approximate date.
    /// </summary>
    /// <param name="day">day</param>
    /// <param name="month">month</param>
    /// <param name="year">year</param>
    /// <param name="outputDateFormat">output date format</param>
    /// <returns></returns>
    public static string GetApproximateDate(int? day, int? month, int? year, string outputDateFormat)
    {
        var strDay = GetApproximateDatePart(day, DAY_FORMAT, UNKNOWN_DAY);
        var strMonth = GetApproximateDatePart(month, MONTH_FORMAT, UNKNOWN_MONTH);
        var strYear = GetApproximateDatePart(year, YEAR_FORMAT, UNKNOWN_YEAR);
        return string.Format(outputDateFormat, strDay, strMonth, strYear);
    }

    /// <summary>
    /// Gets the approximate date.
    /// </summary>
    /// <param name="strOriginalDate">string original date</param>
    /// <param name="dtPartPositions">dt part positions</param>
    /// <param name="outputDateFormat">output date format</param>
    /// <param name="exactFormat">exact datetime format</param>
    /// <returns></returns>
    public static string GetApproximateDate(string strOriginalDate, int[] dtPartPositions, string outputDateFormat, string exactFormat = null)
    {
        string strApproximateDate = null;
        if (!string.IsNullOrEmpty(strOriginalDate))
        {
            var isNormalDob = TryParseDateTime(strOriginalDate, out DateTime dob, exactFormat);
            if (isNormalDob)
            {
                strApproximateDate = GetApproximateDate(dob.Day, dob.Month, dob.Year, outputDateFormat);
            }
            else
            {
                if (dtPartPositions.Length == 6)
                {
                    var iDay = NumberHelper.TryParse<int>(strOriginalDate.Substring(dtPartPositions[0], dtPartPositions[1]), int.TryParse);
                    var iMonth = NumberHelper.TryParse<int>(strOriginalDate.Substring(dtPartPositions[2], dtPartPositions[3]), int.TryParse);
                    var iYear = NumberHelper.TryParse<int>(strOriginalDate.Substring(dtPartPositions[4], dtPartPositions[5]), int.TryParse);
                    strApproximateDate = GetApproximateDate(iDay, iMonth, iYear, outputDateFormat);
                }
            }
        }
        return !string.IsNullOrEmpty(strApproximateDate) ? strApproximateDate : UNKNOWN_DATE;
    }

    #endregion Approximate DOB
    #region Unix Epoch Time

    /// <summary>
    /// Convert a datetime from unknown time zone to UTC time
    /// </summary>
    /// <param name="dateTime">dateTime</param>
    /// <returns></returns>
    public static DateTime ToUtcTime(this DateTime dateTime)
    {
        var utcDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond, DateTimeKind.Utc);
        return utcDateTime;
    }

    /// <summary>
    /// Convert Unix Epoch Time to UTC time.
    /// </summary>
    /// <param name="unixTimeStamp">unix time stamp</param>
    /// <returns></returns>
    public static DateTime UnixTimestampToUTCTime(double unixTimeStamp)
    {
        var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddSeconds(unixTimeStamp);
        return dtDateTime;
    }

    /// <summary>
    /// Convert datetime to Unix timestamp
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static double ConvertToUnixTimestamp(this DateTime date)
    {
        var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        var diff = date - origin;
        return diff.TotalSeconds;
    }

    /// <summary>
    /// Convert datetime to epoch time in seconds
    /// </summary>
    /// <param name="datetime">date and time</param>
    /// <returns></returns>
    public static double ToEpoch(this DateTime datetime)
    {
        return ConvertToUnixTimestamp(datetime);
    }

    /// <summary>
    /// Convert datetime to epoch time in seconds
    /// </summary>
    /// <param name="datetime">date and time</param>
    /// <returns></returns>
    public static double? ToEpoch(this DateTime? datetime)
    {
        return datetime.HasValue ? ConvertToUnixTimestamp(datetime.Value) : default;
    }

    #endregion Unix Epoch Time
    #region Compute

    /// <summary>
    /// Computes the months and days difference between start time and end time
    /// </summary>
    /// <param name="endTime">end time (>= start time)</param>
    /// <param name="startTime">start time</param>
    /// <returns></returns>
    public static MonthDayDiff ComputeMonthDayDiff(this DateTime endTime, DateTime startTime)
    {
        var monthDayDiff = new MonthDayDiff();
        if (startTime <= endTime)
        {
            var startDate = startTime.Date;
            var endDate = endTime.Date;
            var beginDate = startDate;

            while (beginDate < endDate)
            {
                var nextBeginDate = beginDate.AddMonths(1);
                if (nextBeginDate <= endDate)
                {
                    monthDayDiff.NumOfMonths++;
                    beginDate = nextBeginDate;
                }
                else
                {
                    monthDayDiff.NumOfDays = (int)endDate.Subtract(beginDate).TotalDays;
                    beginDate = endDate;
                }
            }
        }
        return monthDayDiff;
    }

    /// <summary>
    /// Computes time from miliseconds
    /// </summary>
    /// <param name="timeInMiliseconds">time in miliseconds</param>
    /// <returns></returns>
    public static TimeSpan ComputeTime(this long timeInMiliseconds)
    {
        var hours = (int)(timeInMiliseconds / HOUR_TO_MILISECONDS);
        var minutes = (int)((timeInMiliseconds - hours * HOUR_TO_MILISECONDS) / MINUTE_TO_MILISECONDS);
        var seconds = (int)((timeInMiliseconds - hours * HOUR_TO_MILISECONDS - minutes * MINUTE_TO_MILISECONDS) / SECOND_TO_MILISECONDS);
        var miliseconds = (int)(timeInMiliseconds - hours * HOUR_TO_MILISECONDS - minutes * MINUTE_TO_MILISECONDS - seconds * SECOND_TO_MILISECONDS);
        return new TimeSpan(hours, minutes, seconds, miliseconds);
    }

    /// <summary>
    /// Computes time from seconds
    /// </summary>
    /// <param name="timeInSeconds">time in seconds</param>
    /// <returns></returns>
    public static TimeSpan ComputeTime(this int timeInSeconds)
    {
        var hours = timeInSeconds / HOUR_TO_SECONDS;
        var minutes = (timeInSeconds - hours * HOUR_TO_SECONDS) / MINUTE_TO_SECONDS;
        var seconds = (timeInSeconds - hours * HOUR_TO_SECONDS - minutes * MINUTE_TO_SECONDS);
        return new TimeSpan(hours, minutes, seconds);
    }

    /// <summary>
    /// Computes the date time range till now.
    /// </summary>
    /// <param name="period">period</param>
    /// <param name="periodType">Type of the period.</param>
    /// <returns></returns>
    public static DateTime[] ComputeDateTimeRangeTillNow(int? period, string periodType)
    {
        var startTime = MIN_VALUE;
        var endTime = DateTime.UtcNow;

        if (period.HasValue)
        {
            var periodValue = period.Value;
            var finalPeriodType = periodType ?? string.Empty;
            switch (finalPeriodType)
            {
                case MILLISECOND:
                    startTime = endTime.AddMilliseconds(-periodValue);
                    break;
                case SECOND:
                    startTime = endTime.AddSeconds(-periodValue);
                    break;
                case MINUTE:
                    startTime = endTime.AddMinutes(-periodValue);
                    break;
                case HOUR:
                    startTime = endTime.AddHours(-periodValue);
                    break;
                case DAY:
                    startTime = endTime.AddDays(-periodValue);
                    break;
                case MONTH:
                    startTime = endTime.AddMonths(-periodValue);
                    break;
                case YEAR:
                    startTime = endTime.AddYears(-periodValue);
                    break;
                default:
                    break;
            }
        }

        return new DateTime[] { startTime, endTime };
    }

    /// <summary>
    /// Computes the date time after adding a period
    /// </summary>
    /// <param name="datetime">datetime</param>
    /// <param name="period">period</param>
    /// <param name="periodType">Type of the period.</param>
    /// <returns></returns>
    public static DateTime ComputeDateTimeAfterPeriod(this DateTime datetime, int? period, string periodType)
    {
        if (period.HasValue)
        {
            var periodValue = period.Value;
            var finalPeriodType = periodType ?? string.Empty;
            return finalPeriodType switch
            {
                MILLISECOND => datetime.AddMilliseconds(periodValue),
                SECOND => datetime.AddSeconds(periodValue),
                MINUTE => datetime.AddMinutes(periodValue),
                HOUR => datetime.AddHours(periodValue),
                DAY => datetime.AddDays(periodValue),
                MONTH => datetime.AddMonths(periodValue),
                YEAR => datetime.AddYears(periodValue),
                _ => datetime,
            };
        }

        return datetime;
    }

    /// <summary>
    /// Gets the period type sequence.
    /// </summary>
    /// <param name="periodType">type of the period</param>
    /// <returns></returns>
    public static int GetPeriodTypeSequence(this string periodType)
    {
        return periodType switch
        {
            MILLISECOND => 1,
            SECOND => 2,
            MINUTE => 3,
            HOUR => 4,
            DAY => 5,
            MONTH => 6,
            YEAR => 7,
            _ => 0,
        };
    }

    #endregion Compute
}
