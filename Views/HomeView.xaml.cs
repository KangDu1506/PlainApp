using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PlainApp.Services;

namespace PlainApp.Views
{
    public partial class HomeView : UserControl
    {
        public static readonly DependencyProperty CurrentMonthProperty =
            DependencyProperty.Register(nameof(CurrentMonth), typeof(int), typeof(HomeView),
                new FrameworkPropertyMetadata(DateTime.Now.Month, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public int CurrentMonth
        {
            get => (int)GetValue(CurrentMonthProperty);
            set => SetValue(CurrentMonthProperty, value);
        }

        public static readonly DependencyProperty CurrentYearProperty =
            DependencyProperty.Register(nameof(CurrentYear), typeof(int), typeof(HomeView),
                new FrameworkPropertyMetadata(DateTime.Now.Year, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public int CurrentYear
        {
            get => (int)GetValue(CurrentYearProperty);
            set => SetValue(CurrentYearProperty, value);
        }

        public HomeView()
        {
            InitializeComponent();
            RenderCalendarSkeleton(CurrentMonth, CurrentYear);
        }

        private void RenderCalendarSkeleton(int month, int year)
        {
            BackgroundGridLayer.Children.Clear();
            BackgroundGridLayer.RowDefinitions.Clear();
            BackgroundGridLayer.ColumnDefinitions.Clear();
            TaskOverlayCanvasLayer.Children.Clear();

            (int weeksInMonth, int offset) = CalenderGenerator.GetWeekCountOfMonth(month, year);

            for (int c = 0; c < 7; c++)
            {
                BackgroundGridLayer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            for (int r = 0; r < weeksInMonth; r++)
            {
                BackgroundGridLayer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            double rowPixelHeight = 120.0;
            double totalHeight = weeksInMonth * rowPixelHeight;
            BackgroundGridLayer.Height = totalHeight;
            TaskOverlayCanvasLayer.Height = totalHeight;

            int daysInMonth = DateTime.DaysInMonth(year, month);
            int currentDay = 1;

            for (int row = 0; row < weeksInMonth; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    int cellIndex = row * 7 + col;

                    if (cellIndex >= offset && currentDay <= daysInMonth)
                    {
                        Border dayCell = CreateDayCellBorder(currentDay);
                        Grid.SetRow(dayCell, row);
                        Grid.SetColumn(dayCell, col);
                        BackgroundGridLayer.Children.Add(dayCell);

                        currentDay++;
                    }
                    else
                    {
                        Border emptyCell = CreateEmptyCellBorder();
                        Grid.SetRow(emptyCell, row);
                        Grid.SetColumn(emptyCell, col);
                        BackgroundGridLayer.Children.Add(emptyCell);
                    }
                }
            }
        }

        private Border CreateDayCellBorder(int dayNumber)
        {
            Border border = new Border
            {
                BorderBrush = new SolidColorBrush(Color.FromArgb(40, 240, 240, 240)),
                BorderThickness = new Thickness(1.0),
                Background = Brushes.Transparent,
                Padding = new Thickness(4)                
            };

            Grid cellGrid = new Grid();
            cellGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            cellGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            TextBlock dayText = new TextBlock
            {
                Text = dayNumber.ToString(),
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Right,
                Foreground = Brushes.White
            };

            Grid.SetRow(dayText, 0);
            cellGrid.Children.Add(dayText);
            border.Child = cellGrid;

            return border;
        }

        private Border CreateEmptyCellBorder()
        {
            return new Border
            {
                BorderBrush = new SolidColorBrush(Color.FromArgb(15, 240, 240, 240)),
                BorderThickness = new Thickness(1.0),
                Background = new SolidColorBrush(Color.FromArgb(5, 0, 0, 0))
            };
        }

        private void PreviousMonthButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime dt = new DateTime(CurrentYear, CurrentMonth, 1).AddMonths(-1);
            CurrentMonth = dt.Month;
            CurrentYear = dt.Year;
            RenderCalendarSkeleton(CurrentMonth, CurrentYear);
        }

        private void NextMonthButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime dt = new DateTime(CurrentYear, CurrentMonth, 1).AddMonths(1);
            CurrentMonth = dt.Month;
            CurrentYear = dt.Year;
            RenderCalendarSkeleton(CurrentMonth, CurrentYear);
        }

        private void TaskOverlayCanvasLayer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModels.HomeVM homeVM)
            {
                homeVM.RefreshTasksContainerData();
            }
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}