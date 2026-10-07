using PlainApp.Models;
using System.Windows;

namespace PlainApp.Views
{
    /// <summary>
    /// Interaction logic for PlanEditorView.xaml
    /// </summary>
    public partial class PlanEditorView : Window
    {
        public PlanEditorView()
        {
            InitializeComponent();
        }

        public PlanEditorView(PlanModel plan) : this()
        {
            DataContext = plan;
        }
    }
}
