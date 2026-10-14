using PlainApp.Models;
using System;
using System.Windows;
using System.Windows.Interop;

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

        const int WM_NCLBUTTONDOWN = 0x00A1;
        const int WM_NCLBUTTONDBLCLK = 0x00A3;
        const int WM_SYSCOMMAND = 0x0112;
        const int SC_MOVE = 0xF010;
        const int HTCAPTION = 2;

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var src = (HwndSource)PresentationSource.FromVisual(this);
            src?.AddHook(WndProc);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            int wp = wParam.ToInt32();
            if ((msg == WM_NCLBUTTONDOWN || msg == WM_NCLBUTTONDBLCLK) && wp == HTCAPTION)
            {
                handled = true; // block clicks on title bar from starting window drag/restore
                return IntPtr.Zero;
            }
            if (msg == WM_SYSCOMMAND)
            {
                int cmd = wp & 0xFFF0;
                if (cmd == SC_MOVE)
                {
                    handled = true; // block explicit SC_MOVE requests
                    return IntPtr.Zero;
                }
            }
            return IntPtr.Zero;
        }
    }
}
