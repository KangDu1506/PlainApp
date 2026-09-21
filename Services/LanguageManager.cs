using System.Windows;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace PlainApp.Services
{
    public class LanguageManager : INotifyPropertyChanged
    {
        private static LanguageManager? _instance;
        public static LanguageManager Instance => _instance ??= new LanguageManager();

        public string this[string key]
        {
            get
            {
                var translation = Languages.Strings.ResourceManager.GetString(key, Languages.Strings.Culture);
                return translation ?? $"[{key}]";
            }
        }

        public void ChangeLanguage(string cultureCode)
        {
            if (CultureInfo.CurrentUICulture.Name.Equals(cultureCode, StringComparison.OrdinalIgnoreCase))
                return;

            var newCulture = new CultureInfo(cultureCode);
            CultureInfo.CurrentCulture = newCulture;
            CultureInfo.CurrentUICulture = newCulture;
            CultureInfo.DefaultThreadCurrentCulture = newCulture;
            CultureInfo.DefaultThreadCurrentUICulture = newCulture;

            Languages.Strings.Culture = newCulture;

            OnPropertyChanged(string.Empty);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
