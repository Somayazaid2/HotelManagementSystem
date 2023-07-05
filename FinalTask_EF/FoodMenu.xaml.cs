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
    /// Interaction logic for FoodMenu.xaml
    /// </summary>
    public partial class FoodMenu : Window
    {
        #region Food Menu Property
        public Frontend front { get; set; }
        public Kitchen Kitchen { get; set; }


        private int lchQntity = 0;
        public int LchQntity { get { return lchQntity; } set { lchQntity = value; } }

        private int bFQntity = 0;

        public int BFQntity { get { return bFQntity; } set { bFQntity = value; } }

        private int dinnerQntity = 0;

        public int DinnerQntity { get { return dinnerQntity; } set { dinnerQntity = value; } }

        private bool clean = false;

        public bool Clean { get { return clean; } set { clean = value; } }

        private bool towel = false;

        public bool Towel { get { return towel; } set { towel = value; } }

        private bool surprize = false;

        public bool Surprize { get { return surprize; } set { surprize = value; } } 
        #endregion


        public FoodMenu(Frontend frontend)
        {
            InitializeComponent();
            front = frontend;

            Breakfast.IsEnabled = false;
            BreakLunch.IsEnabled = false;
            Dinner.IsEnabled = false;
        }
        public FoodMenu(Kitchen kitchen)
        {
            InitializeComponent();

            Kitchen = kitchen;

            Breakfast.IsEnabled = false;
            BreakLunch.IsEnabled = false;
            Dinner.IsEnabled = false;
        }

        private void BF_Checked(object sender, RoutedEventArgs e)
        {
            if (select_breakFast.IsChecked == true)
            {
                Breakfast.IsEnabled = true;
            }
        }

        private void Lch_Check(object sender, RoutedEventArgs e)
        {
            if (select_breakLaunch.IsChecked == true)
            {
                BreakLunch.IsEnabled = true;
            }
        }

       

        private void Dinner_Check(object sender, RoutedEventArgs e)
        {
            if (selectDinner.IsChecked == true)
            {
                Dinner.IsEnabled = true;
            }
        }

        private void Next_Btn(object sender, RoutedEventArgs e)
        {
            if (select_breakFast.IsChecked == true)
            {
                BFQntity = int.Parse(Breakfast.Text);
            }

            if (select_breakLaunch.IsChecked == true)
            {
                LchQntity = int.Parse(BreakLunch.Text);
            }

            if (selectDinner.IsChecked == true)
            {
                DinnerQntity = int.Parse(Dinner.Text);
            }

            if (Cleaning_Chk.IsChecked == true)
            {
                Clean = true;
            }
            if (Towels_Chk.IsChecked == true)
            {
                Towel = true;
            }
            if (Sweetess_Chk.IsChecked == true)
            {
                Surprize = true;
            }
            if(this.front != null)
            {
                //send data to Frontend form 
                front.breakfast = BFQntity;
                front.lunch = LchQntity;
                front.dinner = DinnerQntity;
                front.FoodBill = BFQntity * 5 + LchQntity * 5 + DinnerQntity * 5;
                front.cleaning = Clean;
                front.towel = Towel;
                front.surprise = Surprize;
            }
            if (this.Kitchen != null)
            {

                //send data to kitchen form
                Kitchen.breakfast = BFQntity;
                Kitchen.lunch = LchQntity;
                Kitchen.dinner = DinnerQntity;
                Kitchen.foodBill = BFQntity * 5 + LchQntity * 5 + DinnerQntity * 5;
                Kitchen.cleaning = Clean;
                Kitchen.towel = Towel;
                Kitchen.surprise = Surprize;
                


            }
            this.Close();
        }
    }
}
