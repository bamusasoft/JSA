using System;
using System.Drawing;

namespace Jsa.ViewsModel.Helpers
{
    public static class PrintingHelper
    {
        public static int ScaleCm(float cm)
        {
            return (int)Math.Ceiling(cm * 40);
        }
        public static Size ScaleSize(Graphics g, Size si)
        {

            int wi = (int)Math.Ceiling(si.Width / 100f * g.DpiX);
            int hi = (int)Math.Ceiling(si.Height / 100f * g.DpiY);


            return new Size(wi, hi);
        }

        public static Point ScalePoint(Graphics g, int x, int y)
        {
            x = (int)Math.Ceiling((x / 100f * g.DpiX));
            y = (int)Math.Ceiling((y / 100f * g.DpiY));
            return new Point(x, y);
        }
        public static Font ScaleFont(Graphics g, Font font)
        {
            return new Font(font.FontFamily,
                            font.SizeInPoints / 72f * g.DpiY,
                            font.Style, GraphicsUnit.Pixel, font.GdiCharSet, font.GdiVerticalFont);
        }
    }
}
