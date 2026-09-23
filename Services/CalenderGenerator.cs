namespace PlainApp.Services
{
    class CalenderGenerator
    {
        public static (int weekCount, int offset) GetWeekCountOfMonth(int month, int year)
        {
            DateTime firstDate = new DateTime(year, month, 1);
            int daysInMonth = DateTime.DaysInMonth(year, month);
            int offset = ((int)firstDate.DayOfWeek + 6) % 7;
            int weekCount = (int)Math.Ceiling((daysInMonth + offset) / 7.0);

            return (weekCount, offset);
        }
    }
}
