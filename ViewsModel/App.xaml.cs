using Jsa.DomainModel;
using Jsa.ViewsModel.Helpers;
using AppSettings = Jsa.ViewsModel.Properties.Settings;
using System.Linq;
using System.Threading;
using System.Windows;

namespace Jsa.ViewsModel
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ar-SA");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ar-SA");
            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            if (e.Args.Count() == 0)
            {
                GlobalConst.ServerName = ".";
            }
            else
            {
                GlobalConst.ServerName = e.Args[0];
            }
            CheckUpgrade();
            base.OnStartup(e);
        }
        private void CheckUpgrade()
        {
            if (AppSettings.Default.UpgradedVersion)
            {
                AppSettings.Default.Upgrade();
                AppSettings.Default.UpgradedVersion = false;
                AppSettings.Default.Save();
            }
        }
        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string msg = Helper.ProcessExceptionMessages(e.Exception);
            Logger.Log(LogMessageTypes.Error,
                msg,
                e.Exception.TargetSite.ToString(),
                e.Exception.StackTrace);
        }
       

    }
}
