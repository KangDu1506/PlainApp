using System;
using System.Globalization;
using System.Windows.Data;

namespace PlainApp.Converters
{
    public class MonthYearFormatConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 3)
                return string.Empty;

            // values[0] = month, values[1] = year, values[2] = format string
            int month = 0;
            int year = 0;
            string format = values[2] as string ?? "{0} / {1}";

            try { month = System.Convert.ToInt32(values[0]); } catch { }
            try { year = System.Convert.ToInt32(values[1]); } catch { }

            try
            {
                return string.Format(CultureInfo.CurrentCulture, format, month, year);
            }
            catch
            {
                return $"{month} / {year}";
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
