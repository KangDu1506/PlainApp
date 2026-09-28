namespace PlainApp.ViewModels
{
    class MainVM : BaseVM
    {
        private BaseVM _currentVM = null;

        public BaseVM CurrentVM
        {
            get => _currentVM;
            set => SetProperty(ref _currentVM, value);
        }
        public MainVM()
        {
            CurrentVM = new HomeVM();
        }

        public void NavigateTo(string title)
        {
            CurrentVM = title switch
            {
                "HomeTitle" => new HomeVM(),
                "MyPlansTitle" => new MyPlansVM(),
                _ => CurrentVM
            };
        }
    }
}
