using System.Windows;
using System.Windows.Controls.Primitives;

namespace PlainApp.Services
{
    public static class PopupPlacementHelper
    {
        public static readonly CustomPopupPlacementCallback AlignRightCallback = PlacePopupAlignRight;

        // Align popup so its right edge matches the target's right edge, and appears below the target.
        public static CustomPopupPlacement[] PlacePopupAlignRight(Size popupSize, Size targetSize, Point offset)
        {
            // x = targetWidth - popupWidth ; y = targetHeight
            double x = targetSize.Width - popupSize.Width;
            double y = targetSize.Height;
            var placement = new CustomPopupPlacement(new Point(x, y), PopupPrimaryAxis.Horizontal);
            return new[] { placement };
        }
    }
}
