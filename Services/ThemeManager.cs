using System;
using System.Linq;
using System.Windows;

namespace PlainApp.Modules
{
    public static class ThemeManager
    {
        public static void ChangeTheme(Uri themeUri)
        {
            ResourceDictionary newTheme = new ResourceDictionary { Source = themeUri };
            var appResources = Application.Current.Resources.MergedDictionaries;

            var currentTheme = appResources.FirstOrDefault(d =>
                d.Source != null && d.Source.ToString().Contains("ColorPalettes"));

            if (currentTheme != null)
            {
                int index = appResources.IndexOf(currentTheme);
                appResources.RemoveAt(index);
                appResources.Insert(index, newTheme);
            }
            else
            {
                appResources.Add(newTheme);
            }
        }

        public static Uri? GetCurrentThemeUri()
        {
            var appResources = Application.Current.Resources.MergedDictionaries;
            var currentTheme = appResources.FirstOrDefault(d =>
                d.Source != null && d.Source.ToString().Contains("ColorPalettes"));

            return currentTheme?.Source;
        }
    }
}