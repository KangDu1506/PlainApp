using System.Collections.ObjectModel;
using PlainApp.Models;

namespace PlainApp.ViewModels
{
    public class MyPlansVM : BaseVM
    {
        private bool _showStatus = true;
        public bool ShowStatus
        {
            get => _showStatus;
            set => SetProperty(ref _showStatus, value);
        }

        private bool _showProgress = true;
        public bool ShowProgress
        {
            get => _showProgress;
            set => SetProperty(ref _showProgress, value);
        }

        private bool _showPriority = true;
        public bool ShowPriority
        {
            get => _showPriority;
            set => SetProperty(ref _showPriority, value);
        }

        private bool _showDueDate = true;
        public bool ShowDueDate
        {
            get => _showDueDate;
            set => SetProperty(ref _showDueDate, value);
        }

        private bool _showCreateDate = false;
        public bool ShowCreateDate
        {
            get => _showCreateDate;
            set => SetProperty(ref _showCreateDate, value);
        }

        private bool _showCreator = true;
        public bool ShowCreator
        {
            get => _showCreator;
            set => SetProperty(ref _showCreator, value);
        }

        public ObservableCollection<PlanModel> Plans { get; set; } = new();
    }
}
