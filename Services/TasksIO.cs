using System.Collections.ObjectModel;
using PlainApp.Models;
using PlainApp.Enums;

namespace PlainApp.Services
{
    public static class TasksIO
    {
        public static ObservableCollection<TaskOnCalendarModel> GetTasks(TaskDisplayOption displayOption, TaskSortOption sortOption, int maxCount)
        {
            var tasks = new ObservableCollection<TaskOnCalendarModel>();
            // Need Implement logic to retrieve tasks based on displayOption and sortOption, and limit to maxCount
            return tasks;
        }

        public static void SortTasks(List<TaskOnCalendarModel> rawList, ObservableCollection<TaskOnCalendarModel> targetList, TaskSortOption sortOption)
        {
            var baseList = rawList;
            targetList.Clear();
            switch (sortOption)
            {
                default:
                    break;
                case TaskSortOption.ByStartDate:
                    foreach (var item in baseList.OrderBy(t => t.StartDate))
                    {
                        targetList.Add(item);
                    }
                    break;
                case TaskSortOption.ByEndDate:
                    foreach (var item in baseList.OrderBy(t => t.EndDate))
                    {
                        targetList.Add(item);
                    }
                    break;
                case TaskSortOption.ByProgress:
                    foreach (var item in baseList.OrderBy(t => t.Progress))
                    {
                        targetList.Add(item);
                    }
                    break;
                case TaskSortOption.ByImportance:
                    foreach (var item in baseList.OrderByDescending(t => (int)t.Flags))
                    {
                        targetList.Add(item);
                    }
                    break;
            }

        }

        public static void FilterTasks(List<TaskOnCalendarModel> rawList, ObservableCollection<TaskOnCalendarModel> targetList, TaskDisplayOption displayOption)
        {
            var baseList = rawList;
            targetList.Clear();
            foreach (var item in baseList)
            {
                switch (displayOption)
                {
                    default:
                        break;
                    case TaskDisplayOption.Today:
                        if (item.EndDate.Date == DateTime.Today)
                        {
                            targetList.Add(item);
                        }
                        break;
                    case TaskDisplayOption.InSevenDays:
                        if (item.EndDate.Date <= DateTime.Today.AddDays(7))
                        {
                            targetList.Add(item);
                        }
                        break;
                    case TaskDisplayOption.InThirtyDays:
                        if (item.EndDate.Date <= DateTime.Today.AddDays(30))
                        {
                            targetList.Add(item);
                        }
                        break;
                    case TaskDisplayOption.ThisMonth:
                        if (item.EndDate.Month == DateTime.Today.Month && item.EndDate.Year == DateTime.Today.Year)
                        {
                            targetList.Add(item);
                        }
                        break;
                    case TaskDisplayOption.ThisYear:
                        if (item.EndDate.Year == DateTime.Today.Year)
                        {
                            targetList.Add(item);
                        }
                        break;
                }
            }
        }
    }
}