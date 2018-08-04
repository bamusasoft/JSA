using Jsa.DomainModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Jsa.ViewsModel
{
    /// <summary>
    /// Interaction logic for TestWriteOnContractImage.xaml
    /// </summary>
    public partial class TestWriteOnContractImage : Window, INotifyPropertyChanged
    {
        public TestWriteOnContractImage()
        {
            InitializeComponent();
            DataContext = this;
        }
        BitmapImage _image;
        public BitmapImage ContractImage
        {
            get { return _image; }
            set
            {
                _image = value;
                RaisePropertyChanged();

            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Contract c = null;
            using (IUnitOfWork unit = new UnitOfWork())
            {
                c = unit.Contracts.GetById(3420596);
           
            Bitmap bitmap = new Bitmap(@"C:\BaMusaSoft\JeddahStation\UnSignedContract.jpg");
            Graphics graphicImage = Graphics.FromImage(bitmap);
            graphicImage.SmoothingMode = SmoothingMode.AntiAlias;
            PrintOnImage(c, graphicImage);
            //var hbitmap = bitmap.GetHbitmap();
            //var imageSource = Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(bitMapImage.Width, bitMapImage.Height));
            //imgContract.Source = imageSource;

            ContractImage = ConvertToBitmapImage(bitmap);
            
                
            }


        }
       
        BitmapImage ConvertToBitmapImage(Bitmap bm)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bm.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ms.Position = 0;
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = ms;
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.EndInit();
                return bi;
            }
        }

        private void PrintOnImage(Contract c, Graphics g)
        {
            using (g)
            {
                g.PageUnit = GraphicsUnit.Millimeter;
                g.PageScale = 1;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = TextRenderingHint.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;


                using (var sf = StringFormat.GenericTypographic)
                {
                    sf.FormatFlags = StringFormatFlags.DirectionRightToLeft;
                    sf.Trimming = StringTrimming.Word;


                    //Note: Don't mix StringFormatFlags.DirectionRightToLeft with
                    //StringAlignment as using any of StringAlignment enums will
                    //cancel the effect of DirectionRightToLeft.
                    //sf.Alignment = StringAlignment.Far;

                    using (var font = new Font("Simplified Arabic", 12.5f, System.Drawing.FontStyle.Regular))
                    {
                        System.Drawing.Brush brush = new SolidBrush(System.Drawing.Color.Black);
                        //Contract No
                        float x = g.VisibleClipBounds.Right - XCmToMm(10f);
                        float y = g.VisibleClipBounds.Top + YCmToMm(2.6f);
                        g.DrawString(c.ContractNo.ToString(), font, brush, x, y, sf);
                        //Sign Day
                        x = g.VisibleClipBounds.Right - XCmToMm(1f);
                        y = g.VisibleClipBounds.Top + YCmToMm(4f);
                        g.DrawString(c.SignDay, font, brush, x, y, sf);
                        //Sign Hijri Date
                        x = g.VisibleClipBounds.Right - XCmToMm(4.5f);
                        y = g.VisibleClipBounds.Top + YCmToMm(4f);
                        g.DrawString(Helper.PutMask(c.SignHijriDate), font, brush, x, y, sf);
                        //Sign Greg Date
                        x = g.VisibleClipBounds.Right - XCmToMm(9f);
                        y = g.VisibleClipBounds.Top + YCmToMm(4f);
                        g.DrawString(Helper.PutMask(c.SignGregDate), font, brush, x, y, sf);
                        //Customer Name
                        x = g.VisibleClipBounds.Right - XCmToMm(1f);
                        y = g.VisibleClipBounds.Top + YCmToMm(6.2f);
                        g.DrawString(c.Customer.Name, font, brush, x, y, sf);
                        //ID Number
                        x = g.VisibleClipBounds.Right - XCmToMm(3.5f);
                        y = g.VisibleClipBounds.Top + YCmToMm(7f);
                        g.DrawString(c.Customer.IdNumber, font, brush, x, y, sf);
                        //ID Date
                        x = g.VisibleClipBounds.Right - XCmToMm(9f);
                        y = g.VisibleClipBounds.Top + YCmToMm(7f);
                        g.DrawString(Helper.PutMask(c.Customer.IdDate), font, brush, x, y, sf);
                        //ID Issue
                        x = g.VisibleClipBounds.Right - XCmToMm(14.5f);
                        y = g.VisibleClipBounds.Top + YCmToMm(7f);
                        g.DrawString(c.Customer.IdIssue, font, brush, x, y, sf);
                        //Address
                        x = g.VisibleClipBounds.Right - XCmToMm(2.5f);
                        y = g.VisibleClipBounds.Top + YCmToMm(7.5f);
                        g.DrawString(c.Customer.AddressLine1, font, brush, x, y, sf);
                        //Property Type
                        x = g.VisibleClipBounds.Right - XCmToMm(6.5f);
                        y = g.VisibleClipBounds.Top + YCmToMm(9.5f);
                        g.DrawString(c.Property.Type, font, brush, x, y, sf);
                        //Property No
                        x = g.VisibleClipBounds.Right - XCmToMm(11f);
                        y = g.VisibleClipBounds.Top + YCmToMm(9.5f);
                        g.DrawString(c.Property.PropertyNo, font, brush, x, y, sf);
                        //Property Location
                        x = g.VisibleClipBounds.Right - XCmToMm(13.8f);
                        y = g.VisibleClipBounds.Top + YCmToMm(9.5f);
                        g.DrawString(c.Property.Location, font, brush, x, y, sf);
                        //Location District
                        x = g.VisibleClipBounds.Right - XCmToMm(18.5f);
                        y = g.VisibleClipBounds.Top + YCmToMm(9.5f);
                        g.DrawString(c.Property.District, font, brush, x, y, sf);
                        //Activity Description
                        x = g.VisibleClipBounds.Right - XCmToMm(4f);
                        y = g.VisibleClipBounds.Top + YCmToMm(10.3f);
                        g.DrawString("بيع الذهب والمحوهرات", font, brush, x, y, sf);
                        //Start Date
                        x = g.VisibleClipBounds.Right - XCmToMm(8.2f);
                        y = g.VisibleClipBounds.Top + YCmToMm(11f);
                        g.DrawString(Helper.PutMask(c.StartDate), font, brush, x, y, sf);
                        //End Date
                        x = g.VisibleClipBounds.Right - XCmToMm(13f);
                        y = g.VisibleClipBounds.Top + YCmToMm(11f);
                        g.DrawString(Helper.PutMask(c.EndDate), font, brush, x, y, sf);
                        //Agreed Rent
                        x = g.VisibleClipBounds.Right - XCmToMm(5f);
                        y = g.VisibleClipBounds.Top + YCmToMm(11.5f);
                        g.DrawString(c.AgreedRent.ToString("#,#"), font, brush, x, y, sf);
                        //Agreed Rent Words
                        x = g.VisibleClipBounds.Right - XCmToMm(10.3f);
                        y = g.VisibleClipBounds.Top + YCmToMm(11.5f);
                        //g.DrawString(AgreedRentWords, font, brush, x, y, sf);
                        //Agreed Deposit
                        x = g.VisibleClipBounds.Right - XCmToMm(6.8f);
                        y = g.VisibleClipBounds.Top + YCmToMm(14f);
                        g.DrawString(c.AgreedDeposit.ToString("#,#"), font, brush, x, y, sf);
                        //Agreed Deposit Words
                        x = g.VisibleClipBounds.Right - XCmToMm(10f);
                        y = g.VisibleClipBounds.Top + YCmToMm(14f);
                        //g.DrawString(AgreedDepositWords, font, brush, x, y, sf);
                        
                    }
                }
            }
        }
        private float XCmToMm(float cm)
        {
            //return (int)Math.Round((mm / 25.4) * dpi);
            return (cm * 10.00f);
        }
        private float YCmToMm(float cm)
        {
            return (cm * 10.00f);
        }
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;



        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
