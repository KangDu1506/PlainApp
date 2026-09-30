using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using PlainApp.Services;
using PlainApp.Models;

namespace PlainApp.Views
{
    public partial class MyPlansView : UserControl
    {
        public MyPlansView()
        {
            InitializeComponent();

            if (FilterComboBox != null)
            {
                FilterComboBox.DropDownOpened += FilterComboBox_DropDownOpened;
            }
            this.Loaded += MyPlansView_Loaded;
        }

        private void MyPlansView_Loaded(object? sender, RoutedEventArgs e)
        {
            if (PlansDataGrid == null)
                return;

            var rng = new Random();

            var creators = new[]
            {
                new UserModel { Name = "Alice" },
                new UserModel { Name = "Bob" },
                new UserModel { Name = "Carol" }
            };

            for (int i = PlansDataGrid.Items.Count - 1; i >= 0; i--)
            {
                if (PlansDataGrid.Items[i] is PlanModel)
                    PlansDataGrid.Items.RemoveAt(i);
            }

            for (int i = 0; i < 10; i++)
            {
                var creator = creators[rng.Next(creators.Length)];
                var plan = new PlanModel
                {
                    Title = $"Sample Plan {i + 1}",
                    Note = "Generated sample",
                    Priority = (i % 5) + 1,
                    Progress = (float)rng.Next(0, 101),
                    Status = PlanModel.StatusStrings[rng.Next(PlanModel.StatusStrings.Count)],
                    CreatedDate = DateTime.Now.AddDays(-rng.Next(0, 30)),
                    DueDate = DateTime.Now.AddDays(rng.Next(1, 60)),
                    Creator = creator
                };

                PlansDataGrid.Items.Add(plan);
            }
        }

        private void FilterComboBox_DropDownOpened(object? sender, EventArgs e)
        {
            if (sender is ComboBox combo)
            {
                if (combo.Template.FindName("PART_Popup", combo) is Popup popup)
                {
                    popup.Placement = PlacementMode.Custom;
                    popup.CustomPopupPlacementCallback = PopupPlacementHelper.AlignRightCallback;
                    popup.PlacementTarget = combo;
                }
            }
        }
    }
}
