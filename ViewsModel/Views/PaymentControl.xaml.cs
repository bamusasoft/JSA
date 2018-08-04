using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Jsa.ViewsModel.Helpers;

namespace Jsa.ViewsModel.Views
{
    
    /// <summary>
    /// Interaction logic for ContractPaymentsControl.xaml
    /// </summary>
    public partial class PaymentControl : UserControl
    {
        public PaymentControl()
        {
            InitializeComponent();
            layoutGrid.DataContext = this;
        }
        public static readonly DependencyProperty CustomerNameProperty =
            DependencyProperty.Register("CustomerName", typeof(string), typeof(PaymentControl)
            , new FrameworkPropertyMetadata(null));
        public string CustomerName
        {
            get { return (string)GetValue(CustomerNameProperty); }
            set { SetValue(CustomerNameProperty, value); }
        }
        //

        public static readonly DependencyProperty PropertyLocationProperty =
            DependencyProperty.Register("PropertyLocation", typeof(string), typeof(PaymentControl)
            , new FrameworkPropertyMetadata(null));
        public string PropertyLocation
        {
            get { return (string)GetValue(PropertyLocationProperty); }
            set { SetValue(PropertyLocationProperty, value); }
        }
        //
        public static readonly DependencyProperty ContractStartDateProperty =
            DependencyProperty.Register("ContractStartDate", typeof(string), typeof(PaymentControl)
            , new FrameworkPropertyMetadata(null));
        public string ContractStartDate
        {
            get { return (string)GetValue(ContractStartDateProperty); }
            set { SetValue(ContractStartDateProperty, value); }
        }
        //
        public static readonly DependencyProperty ContractEndDateProperty =
            DependencyProperty.Register("ContractEndDate", typeof(string), typeof(PaymentControl)
            , new FrameworkPropertyMetadata(null));
        public string ContractEndDate
        {
            get { return (string)GetValue(ContractEndDateProperty); }
            set { SetValue(ContractEndDateProperty, value); }
        }
        
        //
        public static readonly DependencyProperty AgreedRentProperty =
           DependencyProperty.Register("AgreedRent", typeof(int), typeof(PaymentControl)
           , new FrameworkPropertyMetadata(0));
        public int AgreedRent
        {
            get { return (int)GetValue(AgreedRentProperty); }
            set { SetValue(AgreedRentProperty, value); }
        }
        //
        public static readonly DependencyProperty RentDueProperty =
           DependencyProperty.Register("RentDue", typeof(int), typeof(PaymentControl)
           , new FrameworkPropertyMetadata(0));
        public int RentDue
        {
            get { return (int)GetValue(RentDueProperty); }
            set { SetValue(RentDueProperty, value); }
        }
        //
        public static readonly DependencyProperty RentPaidProperty =
           DependencyProperty.Register("RentPaid", typeof(int), typeof(PaymentControl)
           , new FrameworkPropertyMetadata(0));
        public int RentPaid
        {
            get { return (int)GetValue(RentPaidProperty); }
            set { SetValue(RentPaidProperty, value); }
        }
        //
        public static readonly DependencyProperty RentBalanceProperty =
           DependencyProperty.Register("RentBalance", typeof(int), typeof(PaymentControl)
           , new FrameworkPropertyMetadata(0));
        public int RentBalance
        {
            get { return (int)GetValue(RentBalanceProperty); }
            set { SetValue(RentBalanceProperty, value); }
        }
        ///****************************************************
        public static readonly DependencyProperty MentDueProperty =
           DependencyProperty.Register("MentDue", typeof(int), typeof(PaymentControl)
           , new FrameworkPropertyMetadata(0));
        public int MentDue
        {
            get { return (int)GetValue(MentDueProperty); }
            set { SetValue(MentDueProperty, value); }
        }
        //
        public static readonly DependencyProperty MentPaidProperty =
          DependencyProperty.Register("MentPaid", typeof(int), typeof(PaymentControl)
          , new FrameworkPropertyMetadata(0));
        public int MentPaid
        {
            get { return (int)GetValue(MentPaidProperty); }
            set { SetValue(MentPaidProperty, value); }
        }
        //
        public static readonly DependencyProperty MentBalanceProperty =
          DependencyProperty.Register("MentBalance", typeof(int), typeof(PaymentControl)
          , new FrameworkPropertyMetadata(0));
        public int MentBalance
        {
            get { return (int)GetValue(MentBalanceProperty); }
            set { SetValue(MentBalanceProperty, value); }
        }
        //**************************************
        public static readonly DependencyProperty DepositDueProperty =
          DependencyProperty.Register("DepositDue", typeof(int), typeof(PaymentControl)
          , new FrameworkPropertyMetadata(0));
        public int DepositDue
        {
            get { return (int)GetValue(DepositDueProperty); }
            set { SetValue(DepositDueProperty, value); }
        }
        //
        public static readonly DependencyProperty DepositPaidProperty =
          DependencyProperty.Register("DepositPaid", typeof(int), typeof(PaymentControl)
          , new FrameworkPropertyMetadata(0));
        public int DepositPaid
        {
            get { return (int)GetValue(DepositPaidProperty); }
            set { SetValue(DepositPaidProperty, value); }
        }
        //
        public static readonly DependencyProperty DepositBalanceProperty =
          DependencyProperty.Register("DepositBalance", typeof(int), typeof(PaymentControl)
          , new FrameworkPropertyMetadata(0));
        public int DepositBalance
        {
            get { return (int)GetValue(DepositBalanceProperty); }
            set { SetValue(DepositBalanceProperty, value); }
        }
        //*************************
        public static readonly DependencyProperty DueTotalsProperty =
          DependencyProperty.Register("DueTotals", typeof(int), typeof(PaymentControl)
          , new FrameworkPropertyMetadata(0));
        public int DueTotals
        {
            get { return (int)GetValue(DueTotalsProperty); }
            set { SetValue(DueTotalsProperty, value); }
        }
        //
        public static readonly DependencyProperty PaidTotalsProperty =
         DependencyProperty.Register("PaidTotals", typeof(int), typeof(PaymentControl)
         , new FrameworkPropertyMetadata(0));
        public int PaidTotals
        {
            get { return (int)GetValue(PaidTotalsProperty); }
            set { SetValue(PaidTotalsProperty, value); }
        }
        //
        public static readonly DependencyProperty BalanceTotalsProperty =
         DependencyProperty.Register("BalanceTotals", typeof(int), typeof(PaymentControl)
         , new FrameworkPropertyMetadata(0));
        public int BalanceTotals
        {
            get { return (int)GetValue(BalanceTotalsProperty); }
            set { SetValue(BalanceTotalsProperty, value); }
        }

        //
        public static readonly DependencyProperty ContractPaymentsProperty =
          DependencyProperty.Register("ContractPayments", typeof(IList<ContractPaymentDetails>), typeof(PaymentControl)
          , new FrameworkPropertyMetadata(null));
        public IList<ContractPaymentDetails> ContractPayments
        {
            get { return (IList<ContractPaymentDetails>)GetValue(ContractPaymentsProperty); }
            set { SetValue(ContractPaymentsProperty, value); }
        }
    }
}
