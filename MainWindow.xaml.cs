using System.Windows;
using System.Windows.Input;

namespace PlainApp
{
    public partial class MainWindow : Window
    {
        private readonly ViewModels.MainVM _mainVM = new ViewModels.MainVM();

        public MainWindow()
        {
            InitializeComponent();
            MainSideMenu.OnNavigationRequested += SideMenu_OnNavigationRequested;

            _mainVM = new ViewModels.MainVM();
            this.DataContext = _mainVM;

            MainHeader.SetTitleKey("HomeTitle");
        }

        private void SideMenu_OnNavigationRequested(string title)
        {
            MainHeader.SetTitleKey(title);

            _mainVM.NavigateTo(title);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.D1 && Keyboard.Modifiers == ModifierKeys.Control)
            {
                SideMenu_OnNavigationRequested("HomeTitle");
            }
            else if (e.Key == Key.D2 && Keyboard.Modifiers == ModifierKeys.Control)
            {
                SideMenu_OnNavigationRequested("MyPlansTitle");
            }
            else if (e.Key == Key.D3 && Keyboard.Modifiers == ModifierKeys.Control)
            {
                SideMenu_OnNavigationRequested("NewTitle");
            }
            else if (e.Key == Key.D4 && Keyboard.Modifiers == ModifierKeys.Control)
            {
                SideMenu_OnNavigationRequested("CollaboratorsTitle");
            }
            else if (e.Key == Key.W && Keyboard.Modifiers == ModifierKeys.Control)
            {
                Application.Current.Shutdown();
            }
        }
    }
}