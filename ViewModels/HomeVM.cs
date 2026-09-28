using PlainApp.Models;
using PlainApp.Enums;
using PlainApp.Services;
using System.Collections.ObjectModel;

namespace PlainApp.ViewModels
{
    public class HomeVM : BaseVM
    {
        private List<TaskOnCalendarModel> rawTasks = new();
        private ObservableCollection<TaskOnCalendarModel> tasks = new();
        public ObservableCollection<TaskOnCalendarModel> Tasks
        {
            get => tasks;
            set => SetProperty(ref tasks, value);
        }

        public HomeVM()
        {
            LoadInitialData();
        }

        private TaskDisplayOption displayOption = TaskDisplayOption.Today;
        public TaskDisplayOption DisplayOption
        {
            get => displayOption;
            set
            {
                if (SetProperty(ref displayOption, value))
                {
                    TasksIO.FilterTasks(rawTasks, Tasks, displayOption);
                }
            }
        }

        private TaskSortOption sortOption = TaskSortOption.ByStartDate;
        public TaskSortOption SortOption
        {
            get => sortOption;
            set
            {
                if (SetProperty(ref sortOption, value))
                {
                    TasksIO.SortTasks(rawTasks, Tasks, sortOption);
                }
            }
        }

        public const int MaxTaskToDisplay = 30;

        private void LoadInitialData()
        {
            rawTasks = TasksIO.GetTasks(DisplayOption, SortOption, MaxTaskToDisplay).ToList();
            Tasks = new ObservableCollection<TaskOnCalendarModel>(rawTasks);
        }

        public void RefreshTasksContainerData()
        {
        }
    }
}
