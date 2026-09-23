using System;
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

        // 2. Khai báo DependencyProperty cho CurrentYear
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
            // Reset dữ liệu Lưới Layer 1 & Canvas Layer 2
            BackgroundGridLayer.Children.Clear();
            BackgroundGridLayer.RowDefinitions.Clear();
            BackgroundGridLayer.ColumnDefinitions.Clear();
            TaskOverlayCanvasLayer.Children.Clear();

            // Lấy số tuần và offset bằng ValueTuple[cite: 1]
            (int weeksInMonth, int offset) = CalenderGenerator.GetWeekCountOfMonth(month, year);

            // 1. Dựng 7 Cột cố định (Thứ 2 -> Chủ Nhật)
            for (int c = 0; c < 7; c++)
            {
                BackgroundGridLayer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            // 2. Dựng N Hàng động tương ứng số tuần của tháng
            for (int r = 0; r < weeksInMonth; r++)
            {
                BackgroundGridLayer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            // Set explicit pixel height so the ScrollViewer can scroll when content is larger than viewport.
            // Use a reasonable default row height; adjust as needed for your UI.
            double rowPixelHeight = 120.0;
            double totalHeight = weeksInMonth * rowPixelHeight;
            BackgroundGridLayer.Height = totalHeight;
            TaskOverlayCanvasLayer.Height = totalHeight;

            // 3. Render các ô Ngày vào Layer 1
            int daysInMonth = DateTime.DaysInMonth(year, month);
            int currentDay = 1;

            for (int row = 0; row < weeksInMonth; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    int cellIndex = (row * 7) + col;

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
                BorderThickness = new Thickness(0.5),
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
                BorderThickness = new Thickness(0.5),
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
            // Sẽ dùng ở bước vẽ Task Spanning Bars lên Canvas
        }
    }
}