using System;
using System.Text;
using System.Windows;
using Jsa.DomainModel;
using Jsa.ViewsModel.Printers;
using Jsa.ViewsModel.Properties;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Data;

using WpfToolkit = Xceed.Wpf.Toolkit;
using Jsa.ViewsModel.Helpers;
using System.Threading;
using System.Globalization;


namespace Jsa.ViewsModel
{
    public static class Helper
    {
        /// <summary>
        /// Gets all exception messages in the supplied exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <returns></returns>
        public static string ProcessExceptionMessages(Exception ex)
        {
            var sb = new StringBuilder();
            return GetAllExceptionMessages(ex, sb);
        }

        private static string GetAllExceptionMessages(Exception exception, StringBuilder sb)
        {
            if (null == sb) throw new ArgumentNullException("sb");
            if (exception != null)
            {
                sb.AppendLine(exception.Message);
                GetAllExceptionMessages(exception.InnerException, sb);
            }
            return sb.ToString();
        }
        public static void ShowMessage(string msg)
        {
            MessageBox.Show(msg, "JSA", MessageBoxButton.OK,
                             MessageBoxImage.Information,
                             MessageBoxResult.OK,
                             MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }
        public static int StartNewIncrement(string activeYear, string branch)
        {
            string s = activeYear + branch + "0001";
            return int.Parse(s);
        }
        public static bool DateInYearRange(string dateString)
        {

            bool result = false;
            if (string.IsNullOrEmpty(dateString)) { return false; }
            if (dateString.Length != 8) { return false; }
            int currentYear;
            if (!int.TryParse(Settings.Default.CurrentYear, out currentYear))
            {
                return false;
            }
            string d = dateString.Substring(6, 2);
            string m = dateString.Substring(4, 2);
            string y = dateString.Substring(0, 4);

            int day;
            int month;
            int year;

            if (int.TryParse(d, out day) &&
                int.TryParse(m, out month) &&
                int.TryParse(y, out year))
            {
                if (
                     (day > 0 && day < 31)
                     &&
                     (month > 0 && month <= 12)
                     &&
                     (year == currentYear))
                {
                    result = true;
                }
            }
            return result;
        }

        public static bool ValidDate(string dateString)
        {

            bool result = false;
            if (string.IsNullOrEmpty(dateString)) { return false; }
            if (dateString.Length != 8) { return false; }
            string d = dateString.Substring(6, 2);
            string m = dateString.Substring(4, 2);
            string y = dateString.Substring(0, 4);

            int day;
            int month;
            int year;

            if (int.TryParse(d, out day) &&
                int.TryParse(m, out month) &&
                int.TryParse(y, out year))
            {
                if (
                     (day > 0 && day < 31)
                     &&
                     (month > 0 && month <= 12)
                     &&
                     (year <= 1500))
                {
                    result = true;
                }
            }
            return result;
        }
        public static bool UserConfirmed(string msg)
        {
            var result = MessageBox.Show(msg, "JSA", MessageBoxButton.YesNo,
                                          MessageBoxImage.Question, MessageBoxResult.No,
                                          MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
            if (result == MessageBoxResult.Yes) return true;
            return false;

        }
        public static string DateMiror(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            if (s.Length == 10)
            {

                return new StringBuilder().Append(s.Substring(s.Length - 2, 2))
                                           .Append("/")
                                           .Append(s.Substring(s.Length - 5, 2))
                                           .Append("/")
                                           .Append(s.Substring(0, 4))
                                           .ToString();
            }
            if (s.Length == 8)
            {
                return new StringBuilder().Append(s.Substring(s.Length - 2, 2))
                    .Append("/")
                    .Append(s.Substring(s.Length - 4, 2))
                    .Append("/")
                    .Append(s.Substring(0, 4))
                    .ToString();
            }
            return s;
        }
        public static string PutMask(string dateString)
        {
            if (!string.IsNullOrEmpty(dateString))
            {
                string y = dateString.Substring(0, 4);
                string m = dateString.Substring(4, 2);
                string d = dateString.Substring(6, 2);
                string f = y + "/" + m + "/" + d;
                return f;
            }
            return "";

        }
        public static void ExplicitUpdateBinding()
        {
            IInputElement uie = Keyboard.FocusedElement;
            if (uie is TextBox)
            {
                var focusedText = uie as TextBox;

                BindingExpression be = null;
                if (focusedText is WpfToolkit.MaskedTextBox)
                {
                    be = focusedText.GetBindingExpression(WpfToolkit.MaskedTextBox.ValueProperty);
                }
                else
                {
                    be = focusedText.GetBindingExpression(TextBox.TextProperty);
                }

                if (be != null)
                {
                    be.UpdateSource();
                }
            }
        }
        public static void MoveFocus(System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;

            var uie = e.OriginalSource as UIElement;
            var textbox = uie as TextBox;
            if (textbox == null || textbox.AcceptsReturn)
            {
                return;
            }
            e.Handled = true;
            uie.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }
        public static bool ValidYear(string year)
        {
            if (string.IsNullOrEmpty(year))
            {
                return false;
            }
            int y;
            if (!(int.TryParse(year, out y)))
            {
                return false;
            }
            if (y < 1431)
            {
                return false;
            }

            return true;
        }
        public static string ApplyDateMask(string s)
        {
            if (string.IsNullOrEmpty(s)) return ""; //Fail silently
            string d = s.Substring(6, 2);
            string m = s.Substring(4, 2);
            string y = s.Substring(0, 4);
            var mask = y + "/" + m + "/" + d;
            return mask;
        }
        public static void LogShowError(Exception ex)
        {
            string msg = Helper.ProcessExceptionMessages(ex);
            Logger.Log(LogMessageTypes.Error, msg, ex.TargetSite.ToString(), ex.StackTrace);
            Helper.ShowMessage(msg);

        }
        public static void LogInfo(string msg)
        {
            Logger.Log(LogMessageTypes.Info, msg);
        }
        public static string GetCurrentDate()
        {
            if (Thread.CurrentThread.CurrentCulture.Name != "ar-SA")
            {
                CultureInfo c = new CultureInfo("ar-SA");
                Thread.CurrentThread.CurrentCulture = c;
            }
            return DateTime.Today.Date.ToString("yyyyMMdd");
        }

        public static string GetDayOfWeek()
        {
            if (Thread.CurrentThread.CurrentCulture.Name != "ar-SA")
            {
                CultureInfo c = new CultureInfo("ar-SA");
                Thread.CurrentThread.CurrentCulture = c;
            }
            return DateTime.Today.DayOfWeek.ToString();
        }

        public static string GetCurrentGregDate()
        {
            CultureInfo c = new CultureInfo("ar-SA");
            if (Thread.CurrentThread.CurrentCulture.Name != "ar-SA")
            {
                Thread.CurrentThread.CurrentCulture = c;
            }
            CultureInfo enCulture = new CultureInfo("en-US");
            GregorianCalendar greg = new GregorianCalendar(GregorianCalendarTypes.USEnglish);
            return DateTime.Now.ToString("yyyyMMdd", enCulture.DateTimeFormat);

        }


        public static DateTime ConvertToGregDate(string hijriDate)
        {
            CultureInfo c = new CultureInfo("ar-SA");
            if (Thread.CurrentThread.CurrentCulture.Name != "ar-SA")
            {
                Thread.CurrentThread.CurrentCulture = c;
            }
            DateTime hij = DateTime.ParseExact(hijriDate, "yyyyMMdd", c.DateTimeFormat);
            CultureInfo enCulture = new CultureInfo("en-US");
            GregorianCalendar greg = new GregorianCalendar(GregorianCalendarTypes.USEnglish);
            var  t = hij.ToString("yyyyMMdd", enCulture.DateTimeFormat);
            var nn = DateTime.ParseExact(t, "yyyyMMdd", enCulture.DateTimeFormat);
            return nn;
        }



        public static string GetCurrentYear
        {
            get
            {
                if (Thread.CurrentThread.CurrentCulture.Name != "ar-SA")
                {
                    CultureInfo c = new CultureInfo("ar-SA");
                    Thread.CurrentThread.CurrentCulture = c;
                }
                var arCult = Thread.CurrentThread.CurrentCulture;
                var yea = arCult.Calendar.GetYear(DateTime.Today);
                return yea.ToString();
            }
        }
        public static void SetCurrentYear()
        {
            var s = Helper.GetCurrentYear;
            Properties.Settings.Default.CurrentYear = s;
            Properties.Settings.Default.Save();
        }
        public static string CreateContractPhotoPath(int contractNo)
        {
            string path = Properties.Settings.Default.ContractsPhotosPath + "\\" + contractNo.ToString() + ".jpg";
            if (string.IsNullOrEmpty(path)) throw new InvalidOperationException(Properties.Resources.ContractView_ContPhoFolderMissingMsg);
            return path;
        }

        public static string GenerateId(string maxNo)
        {
            string currentYear = Settings.Default.CurrentYear.Substring(2, 2);
            string branch = Settings.Default.Branch.ToString(CultureInfo.InvariantCulture);


            if (!string.IsNullOrEmpty(maxNo))
            {
                string yearPortion = maxNo.Substring(0, 2);
                if (yearPortion.Equals(currentYear))
                {
                    string incrementedPortion = maxNo.Substring(3, 4);
                    int incrementedNo;

                    if (int.TryParse(incrementedPortion, out incrementedNo))
                    {
                        incrementedNo++;
                    }
                    return currentYear + branch + DecorateNo(incrementedNo); ;
                }
            }
            return StartNewIncrement(currentYear, branch).ToString(CultureInfo.InvariantCulture);
        }
        private static string DecorateNo(int i)
        {
            string s = i.ToString(CultureInfo.InvariantCulture);
            switch (s.Length)
            {
                case 1:
                    return "000" + s;
                case 2:
                    return "00" + s;
                case 3:
                    return "0" + s;
                case 4:
                    return s;
                default:
                    throw new IndexOutOfRangeException("Schedule No. can't be greater than 9999");
            }

        }
        private static string Decorate(int arg)
        {
            string s = arg.ToString(CultureInfo.InvariantCulture);
            switch (s.Length)
            {
                case 1:
                    return "0" + s;
                case 2:
                    return s;
                default:
                    throw new IndexOutOfRangeException("Argument out of range.");
            }

        }
        public static DateTime ConvertStringToDate(string str)
        {
            try
            {
                string copy = str.DeepClone<string>();
                if (!ValidDate(copy)) throw new InvalidCastException("Invalid Date Format");
                copy = PutMask(copy);
                //return Convert.ToDateTime(str);
                return DateTime.ParseExact(copy, "yyyy/MM/dd", null);
            }
            catch (FormatException ex)
            {
                if (ex.Message.Contains("The DateTime represented by the string is not supported in calendar System.Globalization.UmAlQuraCalendar"))
                {
                    string udjestedDate = GetYear(str) + GetMonth(str) + SubstractDays(GetDay(str), 1);
                   return ConvertStringToDate(udjestedDate);
                    
                }
                throw;

            }
            catch
            {
                throw;
            }

        }
        public static DateTime NextMonth(DateTime date)
        {
            if (date.Day != DateTime.DaysInMonth(date.Year, date.Month))
            {
                return date.AddMonths(1);
            }
            else
            {
                return date.AddDays(1).AddMonths(1).AddDays(-1);
            }
        }
        public static string GetDay(string dateString)
        {
            if (!ValidDate(dateString))
            {
                throw new FormatException("Can not recognize the date format");
            }
            string day = dateString.Substring(6, 2);
            return day;
        }
        public static string GetMonth(string dateString)
        {
            if (!ValidDate(dateString))
            {
                throw new FormatException("Can not recognize the date format");
            }
            string month = dateString.Substring(4, 2);
            return month;
        }
        public static string GetYear(string dateString)
        {
            if (!ValidDate(dateString))
            {
                throw new FormatException("Can not recognize the date format");
            }
            string year = dateString.Substring(0, 4);
            return year;
        }
        public static int GetDays(string startDate, string endDate)
        {
            DateTime start = Helper.ConvertStringToDate(startDate);
            DateTime end = Helper.ConvertStringToDate(endDate);
            int daysNum = (int)(end - start).TotalDays;
            return daysNum;
        }
        public static int GetMonths(string startDate, string endDate)
        {
            DateTime start = Helper.ConvertStringToDate(startDate);
            DateTime end = Helper.ConvertStringToDate(endDate);
            int monthsNum = (end.Month - start.Month);
            return monthsNum;
        }
        /// <summary>
        /// Increase a month by number you supplied
        /// </summary>
        /// <param name="month">The current monthe</param>
        /// <param name="numAdd">Number of months to add</param>
        /// <returns></returns>
        public static string AddMonths(string month, int numAdd)
        {
            int result;
            if (int.TryParse(month, out result))
            {
                int newMonth = result + numAdd;
                return Decorate(newMonth);
            }
            throw new FormatException("Can not recogonize format of the month");

        }
        public static string SubstractDays(string day, int numSubs)
        {
            int result;
            if (int.TryParse(day, out result))
            {
                int newDay = result - numSubs;
                return Decorate(newDay);
            }
            throw new FormatException("Can not recogonize format of the day");

        }
        /// <summary>
        /// <para>Calculate difference in days and months between two dates.</para> 
        /// <para>Dates must be in the same year.</para>
        /// <para>Note that this method doesn't account for different in years, and it will not check for them.</para>
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>
        /// <para>Method will return a Tuble./></para>
        /// <para>Item1 in the Tuble will represent difference in number of days.</para>
        /// <para>Item2 in the Tuble will represent difference in number on months.</para>
        /// </returns>
        public static Tuple<int, int> CaculateDate(string startDate, string endDate)
        {
            int startDay = int.Parse(GetDay(startDate));
            int endDay = int.Parse(GetDay(endDate)) + 1; //Add one,so end day can be calculated

            int startMonth = int.Parse(GetMonth(startDate));
            int endMonth = int.Parse(GetMonth(endDate));
            if (endDay < startDay)
            {
                endDay = endDay + 30;
                endMonth = endMonth - 1;

            }
            int numberOfDays = endDay - startDay;
            int numberOfMonths = endMonth - startMonth;
            return new Tuple<int, int>(numberOfDays, numberOfMonths);



        }
        public static string GenerateOutboxNo(string maxNo)
        {
            string currentYear = Settings.Default.CurrentYear;
            if (!string.IsNullOrEmpty(maxNo))
            {
                string yearPortion = maxNo.Substring(0, 4);
                if (yearPortion.Equals(currentYear))
                {
                    string incrementedPortion = maxNo.Substring(4, 4);
                    int incrementedNo;

                    if (int.TryParse(incrementedPortion, out incrementedNo))
                    {
                        incrementedNo++;
                    }
                    return currentYear + DecorateNo(incrementedNo); ;
                }
            }
            return currentYear + "0001"; ;
        }
        public static string AppendGeneratedNo(string baseNo, int startIndex)
        {
            if (string.IsNullOrEmpty(baseNo)) throw new ArgumentNullException("baseNo", "Base No cannot be null");
            string basePortion = baseNo.Substring(0, startIndex);
            string incrementedPortion = baseNo.Substring(basePortion.Length);

                int incrementedNo;

                if (int.TryParse(incrementedPortion, out incrementedNo))
                {
                    incrementedNo++;
                }
            return baseNo + AddMissingZeros(incrementedNo);
            
        }
        private static string AddMissingZeros(int no)
        {
            string s = no.ToString(CultureInfo.InvariantCulture);
            switch (s.Length)
            {
                case 1:
                    return "00" + s;
                case 2:
                    return "0" + s;
                case 3:
                    return s;
                default:
                    throw new IndexOutOfRangeException("No. can't be greater than 999");
            }
        }
        public static void PrintSchedule(Schedule schedule)
        {
            string path = Settings.Default.ScheduleTemplatePath;
            SchedulePrinter printer = new SchedulePrinter(schedule, path);
            printer.Print();

        }
       

    }
}
