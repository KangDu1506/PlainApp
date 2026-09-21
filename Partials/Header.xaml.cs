using PlainApp.Modules;
using PlainApp.Services;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace PlainApp.Partials
{
    public partial class Header : UserControl
    {
        private bool _isUpdatingUi = false;
        private string _currentTitleKey = "HomeTitle";

        public Header()
        {
            InitializeComponent();

            SyncComboBoxSelection();
        }

        private void SyncComboBoxSelection()
        {
            _isUpdatingUi = true;

            string currentLang = CultureInfo.CurrentUICulture.Name;
            bool isMatched = false;

            foreach (ComboBoxItem item in LanguageSelectorComboBox.Items)
            {
                if (item.Tag?.ToString()?.Equals(currentLang, StringComparison.OrdinalIgnoreCase) == true)
                {
                    LanguageSelectorComboBox.SelectedItem = item;
                    isMatched = true;
                    break;
                }
            }

            if (!isMatched && LanguageSelectorComboBox.Items.Count > 0)
            {
                LanguageSelectorComboBox.SelectedIndex = 0;
                ApplyLanguage("en-US");
            }

            var currentThemeUri = ThemeManager.GetCurrentThemeUri();
            if (currentThemeUri != null)
            {
                if (currentThemeUri.OriginalString.Contains("LightBluePalette.xaml"))
                    ThemeSelectorComboBox.SelectedIndex = 0;
                else if (currentThemeUri.OriginalString.Contains("DarkBluePalette.xaml"))
                    ThemeSelectorComboBox.SelectedIndex = 1;
            }

            _isUpdatingUi = false;
        }

        private void LanguageSelectorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isUpdatingUi) return;

            if (LanguageSelectorComboBox.SelectedItem is ComboBoxItem selectedItem && selectedItem.Tag != null)
            {
                ApplyLanguage(selectedItem.Tag.ToString()!);
            }
        }

        private void ApplyLanguage(string cultureCode)
        {
            LanguageManager.Instance.ChangeLanguage(cultureCode);
        }

        private void ThemeSelectorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isUpdatingUi) return;

            if (ThemeSelectorComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string tag = selectedItem.Tag?.ToString() ?? "";

                if (tag == "Light")
                {
                    ThemeManager.ChangeTheme(new Uri("/Assets/ColorPalettes/LightBluePalette.xaml", UriKind.Relative));
                }
                else if (tag == "Dark")
                {
                    ThemeManager.ChangeTheme(new Uri("/Assets/ColorPalettes/DarkBluePalette.xaml", UriKind.Relative));
                }
            }
        }

        public void SetTitleKey(string titleKey)
        {
            if (string.IsNullOrEmpty(titleKey)) return;

            _currentTitleKey = titleKey;
            Binding binding = new Binding($"[{_currentTitleKey}]")
            {
                Source = LanguageManager.Instance,
                Mode = BindingMode.OneWay
            };

            BindingOperations.SetBinding(WindowTitle, TextBlock.TextProperty, binding);
        }
    }
}