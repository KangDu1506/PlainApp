using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace PlainApp.Services
{
    public class DateTimeLocalizer : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 1 || values[0] == null)
                return DependencyProperty.UnsetValue;

            string? format = null;
            if (values.Length >= 2 && values[1] is string fmt && !string.IsNullOrEmpty(fmt))
                format = fmt;
            else if (parameter is string p && !string.IsNullOrEmpty(p))
                format = p;
            else
                format = culture?.DateTimeFormat.FullDateTimePattern ?? CultureInfo.CurrentCulture.DateTimeFormat.FullDateTimePattern;

            if (values[0] is DateTime dateTime)
            {
                var ci = (CultureInfo)(culture ?? CultureInfo.CurrentCulture).Clone();
                var dtfi = (DateTimeFormatInfo)ci.DateTimeFormat.Clone();

                try
                {
                    var lm = LanguageManager.Instance;
                    string Abbrev(string s) => string.IsNullOrEmpty(s) ? s : (s.Length > 3 ? s.Substring(0, 3) : s);

                    dtfi.DayNames = new[] {
                        lm["Sunday"],
                        lm["Monday"],
                        lm["Tuesday"],
                        lm["Wednesday"],
                        lm["Thursday"],
                        lm["Friday"],
                        lm["Saturday"]
                    };

                    dtfi.AbbreviatedDayNames = new[] {
                        Abbrev(lm["Sunday"]),
                        Abbrev(lm["Monday"]),
                        Abbrev(lm["Tuesday"]),
                        Abbrev(lm["Wednesday"]),
                        Abbrev(lm["Thursday"]),
                        Abbrev(lm["Friday"]),
                        Abbrev(lm["Saturday"])
                    };
                }
                catch { }

                return dateTime.ToString(format, dtfi);
            }
            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
