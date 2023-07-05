using Dapper;
using FinalTask_EF.Context;
using FinalTask_EF.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for Kitchen.xaml
    /// </summary>
    public partial class Kitchen : Window
    {
        public bool taskFinder = false;

        #region Food Menu and another Propeties

        public int ID { get; set; }

        public bool checkin = false;


        public int foodBill { get; set; } = 0;
        public double totalAmount { get; set; } = 0;

        public int breakfast { get; set; } = 0;
        public int lunch { get; set; } = 0;
        public int dinner { get; set; } = 0;

        public bool cleaning { get; set; }
        public bool towel { get; set; }
        public bool surprise { get; set; }
        public bool supply_status { get; set; } = false;

        #endregion
        public Kitchen()
        {
            InitializeComponent();
        }

        ReservationDbContext Context = new ReservationDbContext();
        private void OnKitchenFrmLoaded(object sender, RoutedEventArgs e)
        {


            DisableTextBoxes();

            //loading data from database
            LoadForDataGridView();
            listBoxFromDataBase();
        }
        private void DisableTextBoxes()
        {


            RoomFname.IsEnabled = false;
            RoomLname.IsEnabled = false;
            PhoneNum.IsEnabled = false;
            RoomType.IsEnabled = false;
            RoomFloor.IsEnabled = false;
            RoomNum.IsEnabled = false;
            BearkfastQTY.IsEnabled = false;
            LanuchQTY.IsEnabled = false;
            DinnerQTY.IsEnabled = false;


            cleanChk.IsHitTestVisible = false;
            cleanChk.Focusable = false;
            towelChk.IsHitTestVisible = false;
            towelChk.Focusable = false;
            SweetestSurprise.IsHitTestVisible = false;
            SweetestSurprise.Focusable = false;

        }
        private void LoadForDataGridView()
        {
            SqlConnection sqlCNN = new SqlConnection("Data Source=.;Initial Catalog=Reservation-Db;Integrated Security=True;Encrypt=false;");

            gridOverview.ItemsSource = sqlCNN.Query<Reservation>("select * from Reservations");

        }
        private void listBoxFromDataBase()
        {

            lstKitchen.ItemsSource = null;

            SqlConnection sqlCN = new SqlConnection("Data Source=.;Initial Catalog=Reservation-Db;Integrated Security=True;Encrypt=false;");

            var Result = sqlCN.Query<Reservation>("GetDataForLstKitchen", commandType: System.Data.CommandType.StoredProcedure);

            List<string> selectReservation = new List<string>();

            foreach (var item in Result)
            {
                selectReservation.Add($"{item.Id} | {item.room_type} | {item.first_name} {item.last_name} | {item.phone_number}");
            }

            lstKitchen.ItemsSource = selectReservation;

        }

        private void OnKitchenLstChanged(object sender, SelectionChangedEventArgs e)
        {
            taskFinder = true;

            if (lstKitchen.SelectedItem != null)
            {

                string[] GetServation = lstKitchen.SelectedItem.ToString().Split(" ");
                string id = GetServation[0];
                int resId = int.Parse(id);

                ID = resId;

                Reservation Result = Context.Reservations.Find(resId);


                RoomFname.Text = Result.first_name;
                RoomLname.Text = Result.last_name;
                PhoneNum.Text = Result.phone_number;
                RoomType.Text = Result.room_type;
                RoomFloor.Text = Result.room_floor;
                RoomNum.Text = Result.room_number;
                BearkfastQTY.Text = Result.break_fast.ToString();
                LanuchQTY.Text = Result.lunch.ToString();
                DinnerQTY.Text = Result.dinner.ToString();

                totalAmount = Result.total_bill;//////
                foodBill = Result.food_bill;//////

                totalAmount -= foodBill;

                breakfast = Result.break_fast;
                lunch = Result.lunch;
                dinner = Result.dinner;

                checkin = Result.check_in;



                supply_status = Result.supply_status;
                if (supply_status == true)
                {
                    FoodSupply.IsChecked = true;
                }
                else
                {
                    FoodSupply.IsChecked = false;
                }



                cleaning = Result.cleaning;
                towel = Result.towel;
                surprise = Result.s_surprise;

                if (cleaning == true)
                    cleanChk.IsChecked = true;
                else
                    cleanChk.IsChecked = false;


                if (towel == true)
                    towelChk.IsChecked = true;
                else
                    towelChk.IsChecked = false;


                if (surprise == true)
                    SweetestSurprise.IsChecked = true;
                else
                    SweetestSurprise.IsChecked = false;



            }
        }

        private void FoodChangeBtn(object sender, RoutedEventArgs e)
        {
            FoodMenu menu = new FoodMenu(this);


            if (taskFinder)
            {
                if (breakfast > 0)
                {
                    menu.select_breakFast.IsChecked = true;
                    menu.Breakfast.Text = Convert.ToString(breakfast);
                }
                if (lunch > 0)
                {
                    menu.select_breakLaunch.IsChecked = true;
                    menu.BreakLunch.Text = Convert.ToString(lunch);
                }
                if (dinner > 0)
                {
                    menu.selectDinner.IsChecked = true;
                    menu.Dinner.Text = Convert.ToString(dinner);
                }

                if (cleaning == true)
                {
                    menu.Cleaning_Chk.IsChecked = true;
                }

                if (towel == true)
                {
                    menu.Towels_Chk.IsChecked = true;
                }

                if (surprise == true)
                {
                    menu.Sweetess_Chk.IsChecked = true;
                }
            }
            menu.Show();
        }

        private void UpdateBtn(object sender, RoutedEventArgs e)
        {
            try
            {
                Reservation Result = Context.Reservations.Find(ID);

                Result.break_fast = breakfast;
                Result.lunch = lunch;
                Result.dinner = dinner;
                Result.cleaning = cleaning;
                Result.towel = towel;
                Result.s_surprise = surprise;
                Result.supply_status = supply_status;

                Result.food_bill = foodBill;
                Result.total_bill = foodBill + totalAmount;

                Context.Entry(Result).State = EntityState.Modified;

                Context.SaveChanges();
                MessageBox.Show("updated", "updated", MessageBoxButton.OK);

                listBoxFromDataBase();
                LoadForDataGridView();

                resetAllFields();


            }
            catch (Exception)
            {
                MessageBox.Show("error", "error", MessageBoxButton.OK);
            }
        }

        private void onFoodSupplyCheckedChanged(object sender, RoutedEventArgs e)
        {
            cleanChk.IsChecked = false;
            cleanChk.Content = "Cleaned";
            towelChk.IsChecked = false;
            towelChk.Content = "Toweled";
            SweetestSurprise.IsChecked = false;
            SweetestSurprise.Content = "Surprised";
            supply_status = true;
        }

        private void resetAllFields()
        {
            ClearAllTextBoxes();
            cleanChk.IsChecked = false;
            towelChk.IsChecked = false;
            SweetestSurprise.IsChecked = false;
            FoodSupply.IsChecked = false;
        }

        public void ClearAllTextBoxes()
        {
            RoomFname.Clear();
            RoomLname.Clear();
            PhoneNum.Clear();
            RoomType.Clear();
            RoomFloor.Clear();
            RoomNum.Clear();
            BearkfastQTY.Clear();
            LanuchQTY.Clear();
            DinnerQTY.Clear();
        }
    }
}
