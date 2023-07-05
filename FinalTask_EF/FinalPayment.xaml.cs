using FinalTask_EF.Context;
using FinalTask_EF.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FinalTask_EF
{
    /// <summary>
    /// Interaction logic for FinalPayment.xaml
    /// </summary>
    public partial class FinalPayment : Window
    {
        public FinalPayment(Frontend front)
        {
            InitializeComponent();
            comboYear.ItemsSource = new List<int> { 2014, 2015, 2016, 2017, 2018, 2019, 2020, 2021, 2022, 2023, 2024 };
            comboMonth.ItemsSource = new List<int> { 01, 02, 03, 04, 05, 06, 07, 08, 09, 10, 11, 12 };
            Payment.ItemsSource = new List<string> { "Credit", "Debit" };
            this.front = front;

            double TotalWithTax = Convert.ToDouble(front.TotalBill) * 0.07;
            double FinalTotal = Convert.ToDouble(front.TotalBill) +TotalWithTax + front.FoodBill;
            CurrentBill.Text = " $ "+Convert.ToString(front.TotalBill)+" USD";
            FoodBill.Text = " $ "+Convert.ToString(front.FoodBill)+" USD";
            TaxBill.Text = " $ "+Convert.ToString(TotalWithTax)+" USD";
            Total.Text = " $ "+Convert.ToString(FinalTotal)+" USD";
            FinalTotalFinalized = FinalTotal;

        }

        ReservationDbContext context = new ReservationDbContext();
        private readonly Frontend front;

        #region FinalizeBill Property
        public int totalAmountFromFrontend { get; set; }
        public int foodBillF { get; set; }
        private double finalTotalFinalized = 0.0;
        private string paymentType;


        public double FinalTotalFinalized
        {
            get { return finalTotalFinalized; }
            set { finalTotalFinalized = value; }
        }

        public string PaymentType
        {
            get { return paymentType; }
            set { paymentType = value; }
        }
        private string paymentCardNumber;

        public string PaymentCardNumber
        {
            get { return paymentCardNumber; }
            set { paymentCardNumber = value; }
        }
        private string MM_YY_Of_Card;

        public string MM_YY_Of_Card1
        {
            get { return MM_YY_Of_Card; }
            set { MM_YY_Of_Card = value; }
        }
        private string CVC_Of_Card;

        public string CVC_Of_Card1
        {
            get { return CVC_Of_Card; }
            set { CVC_Of_Card = value; }
        }
        private string CardType;

        public string CardType1
        {
            get { return CardType; }
            set { CardType = value; } 
            #endregion
        }
        private void payBtnNext(object sender, RoutedEventArgs e)
        {
            PaymentType = Payment.Text;
            PaymentCardNumber = CardNum.Text;
            MM_YY_Of_Card1 = comboMonth.SelectedItem.ToString() + "/" + comboYear.SelectedItem.ToString();
            CVC_Of_Card1 = txtCVC.Text;
            CardType1 = TxtCardType.Text;



            front.PaymentType = PaymentType;
            front.CardNumber = PaymentCardNumber;
            front.CardDate = MM_YY_Of_Card1;
            front.CVC_Card = CVC_Of_Card1;  
            front.CardType = CardType1;
            front.TotalPayment = FinalTotalFinalized;

            this.Close();

            //Reservation r1 = new Reservation();

            //r1.payment_type = Payment.Text;
            //r1.card_cvc = txtCVC.Text;
            //r1.total_bill = int.Parse(LargeNum.Text);
            //r1.card_type = "Unkown";
            //r1.card_number = "5";
            //r1.card_exp = comboMonth.Text + "/" + comboYear.Text;
            //context.Reservations.Add(r1);
            //context.SaveChanges();
            //Close();
            
        }

        //public void InsertBill(Reservation r1)
        //{
        //    r1.payment_type = Payment.Text;
        //    r1.card_cvc = txtCVC.Text;
        //    r1.total_bill = int.Parse(CardNum.Text);
        //    r1.card_type = "Unkown";
        //    r1.card_number = "5";
        //    r1.card_exp = comboMonth.Text + "/" + comboYear.Text;
        //    //context.Reservations.Add(r1);
        //    //context.SaveChanges();
        //}
        
        private void SelectedMonthChange(object sender, SelectionChangedEventArgs e)
        {

            if (CardNum.Text.Substring(0, 3) == "011")
            {
                TxtCardType.Text = "AmericanExpress";
            }
            else if (CardNum.Text.Substring(0, 3) == "010")
            {
                TxtCardType.Text = "Visa";
            }
            else if (CardNum.Text.Substring(0, 3) == "012")
            {
                TxtCardType.Text = "MasterCard";
            }
            else if (CardNum.Text.Substring(0, 3) == "015")
            {
                TxtCardType.Text = "Discover";
            }
            else
                TxtCardType.Text = "Unknown";


        
    }
    }
}
