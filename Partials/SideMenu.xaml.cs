using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace PlainApp.Partials
{
    public partial class SideMenu : UserControl
    {
        private DispatcherTimer hoverTimer;

        public SideMenu()
        {
            InitializeComponent();
            Tag = "Collapsed";
            SideMenuContainer.Width = 80;
            OverlayBackground.IsHitTestVisible = false;

            hoverTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(0.2)
            };
            hoverTimer.Tick += (s, e) => HoverTimer_Tick();
        }

        private void SideMenuContainer_MouseEnter(object sender, MouseEventArgs e)
        {
            hoverTimer.Start();
        }

        private void SideMenuContainer_MouseLeave(object sender, MouseEventArgs e)
        {
            hoverTimer.Stop();
            CollapseMenu();
        }

        private void HoverTimer_Tick()
        {
            hoverTimer.Stop();
            ExpandMenu();
        }

        private void ExpandMenu()
        {
            AnimateWidth(SideMenuContainer.ActualWidth, 300, TimeSpan.FromSeconds(0.3));
            Tag = "Expanded";

            AnimateOverlayOpacity(OverlayBackground.Opacity, 0.7, TimeSpan.FromSeconds(0.3));
            OverlayBackground.IsHitTestVisible = true;
        }

        private void CollapseMenu()
        {
            AnimateWidth(SideMenuContainer.ActualWidth, 80, TimeSpan.FromSeconds(0.25));
            Tag = "Collapsed";

            AnimateOverlayOpacity(OverlayBackground.Opacity, 0, TimeSpan.FromSeconds(0.25));
            OverlayBackground.IsHitTestVisible = false;
        }

        private void AnimateWidth(double fromValue, double toValue, TimeSpan duration)
        {
            var animation = new DoubleAnimation
            {
                From = fromValue,
                To = toValue,
                Duration = new Duration(duration),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            SideMenuContainer.BeginAnimation(FrameworkElement.WidthProperty, animation);
        }

        public void AnimateOverlayOpacity(double fromValue, double toValue, TimeSpan duration)
        {
            var opacityAnimation = new DoubleAnimation
            {
                From = fromValue,
                To = toValue,
                Duration = new Duration(duration),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };

            OverlayBackground.BeginAnimation(UIElement.OpacityProperty, opacityAnimation);
        }

        public event Action<string>? OnNavigationRequested;

        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string key)
            {
                OnNavigationRequested?.Invoke(key);
            }

            CollapseMenu();
        }
    }
}