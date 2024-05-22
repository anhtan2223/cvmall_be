using System.Globalization;

namespace Framework.Core.Extensions
{
    public static class DateTimeExtensions
    {
        private static string _dateFormat = "yyyy-MM-dd";
        private static string _dateTimeFormat = "yyyy-MM-dd hh:mm:ss";
        private static string _dateTimeStampFormat = "yyyyMMddHHmmssffff";
        private static string _dateTimeDatabaseFormat = "yyyy-MM-dd HH:mm:ss.fff";
        private static string _datetimeSampleFormat = "yyyy-MM-dd";

        public static string ToDateStringJP(this DateTime dateTime)
        {
            CultureInfo culture = new CultureInfo("ja-JP", true);
            culture.DateTimeFormat.Calendar = new JapaneseCalendar();

            string currDate = dateTime.ToString("ggyy年M月d日", culture);
            return currDate;
        }

        public static DateTime? ToDate(this string dateStr)
        {
            var res = DateTime.Now;
            var trypa = DateTime.TryParse(dateStr, out res);
            return trypa ? res : null;
        }
        public static DateTime? ToDateTime(this string datetimeStr)
        {
            var resTime = DateTime.Now;
            var trypa = DateTime.TryParse(datetimeStr, out resTime);
            return trypa ? resTime : null;
        }

        public static string ToDateString(this DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.ToString(_dateFormat) : string.Empty;
        }

        public static string ToDateTimeString(this DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.ToString(_dateTimeFormat) : string.Empty;
        }

        public static string ToDateString(this DateTimeOffset? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.ToString(_dateFormat) : string.Empty;
        }
        
        public static string ToDateTimeString(this DateTimeOffset? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.ToString(_dateTimeFormat) : string.Empty;
        }

        

        public static string ToDateString(this DateTimeOffset dateTime)
        {
            return dateTime.ToString(_dateFormat);
        }
        
        public static string ToDateTimeString(this DateTimeOffset dateTime)
        {
            return dateTime.ToString(_dateTimeFormat);
        }

        public static string ToDateString(this DateTime dateTime)
        {
            return dateTime.ToString(_dateFormat);
        }
        
        public static string ToDateTimeString(this DateTime dateTime)
        {
            return dateTime.ToString(_dateTimeFormat);
        }

        public static string ToDateTimeStampString(DateTime? dateTime = null)
        {
            return dateTime.HasValue ? dateTime.Value.ToString(_dateTimeStampFormat) : DateTime.Now.ToString(_dateTimeStampFormat);
        }
        public static string ToDateTimeDatabaseString(DateTime? dateTime = null)
        {
            return dateTime.HasValue ? dateTime.Value.ToString(_dateTimeDatabaseFormat) : DateTime.Now.ToString(_dateTimeDatabaseFormat);
        }

        public static string ToDateTimeSample(this DateTime dateTime)
        {
            return dateTime.ToString(_datetimeSampleFormat);
        }
    }
}
