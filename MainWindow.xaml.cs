using System.Windows;

namespace PlainApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainSideMenu.OnNavigationRequested += SideMenu_OnNavigationRequested;

            MainHeader.SetTitleKey("HomeTitle");
        }

        private void SideMenu_OnNavigationRequested(string title)
        {
            MainHeader.SetTitleKey(title);
        }
    }
}