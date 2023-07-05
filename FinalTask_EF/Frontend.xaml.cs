using ControlzEx.Standard;
using FinalTask_EF.Context;
using FinalTask_EF.Entities;
using MahApps.Metro.Controls;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
using Dapper;

namespace FinalTask_EF
{
    /// <summary>
    /// Interaction logic for Frontend.xaml
    /// </summary>
    public partial class Frontend : Window
    {


        public Frontend()
        {
            InitializeComponent();
            //Day.ItemsSource = new List<string> { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31" };
            //Month.ItemsSource = new List<string> { "January", "February", "March", "April", " May", " June", "July", "August", "September", "October", " November", "December" };
            FillCmbBox();

            getChecked();


            ReservedRoom();
            GetOccupiedRoom();

            HideSomeContents();


        }
        ReservationDbContext Context = new ReservationDbContext();

        public int FoodPrice { get; set; }

        public bool taskFinder = false;

        public bool checkin = false;
        public int ID { get; set; }

        public string birthday = "";

        #region FoodMenu Form 

        public int breakfast { get; set; } = 0;
        public int lunch { get; set; } = 0;
        public int dinner { get; set; } = 0;

        public bool cleaning { get; set; }
        public bool towel { get; set; }
        public bool surprise { get; set; }

        public bool foodStatus = false;
        public int FoodBill { get; set; }
        public int TotalBill { get; set; }
        public bool editClicked = false;
        #endregion


        #region Payment Form

        public string FPayment, FCnumber, FcardExpOne, FcardExpTwo, FCardCVC;
        public double TotalPayment { get; set; }

        public string PaymentType { get; set; }
        public string CardNumber { get; set; }
        public string CardDate { get; set; }
        public string CVC_Card { get; set; }
        public string CardType { get; set; }
        #endregion
       
        
        private void FillCmbBox()
        {
            //days ComboBox 
            List<int> days = new List<int>();
            for (int i = 0; i < 31; i++)
                days.Add(i + 1);

            Day.ItemsSource = days;

            //Months ComboBox
            List<int> months = new List<int>();
            for (int i = 0; i < 12; i++)
                months.Add(i + 1);
            
            Month.ItemsSource = months;
            //city comboBox
            State.ItemsSource = new List<string> { "Minia", "Giza", "October", "Cairo", "Aswan", "Mansoura" };
            //Gender comboBox
            Gender.ItemsSource = new List<string> { "Male", "Female" };

            //no. of Guests comboBox
            guest.ItemsSource = new List<int> { 1, 2, 3, 4, 5, 6 };

            //type of Room comboBox
            Room.ItemsSource = new List<string> { "Single", "Double", "Twin", "Duplex", "Suite" };
            //no. of floor which room is.
            Floor.ItemsSource = new List<int> { 1, 2, 3, 4, 5 };

            Selectreservation();

            LoadDataIntoReservationAdvView();
        }

        // function to fill Reservation Data comboBox [selectDataCombo] 
        private void Selectreservation()
        {
            SqlConnection sqlCN = new SqlConnection("Data Source=.;Initial Catalog=Reservation-Db;Integrated Security=True;Encrypt=false;");

            var Result = sqlCN.Query<Reservation>("select * from Reservations");

            List<string> selectReservation = new List<string>();

            foreach (var item in Result)
            {
                selectReservation.Add($"{item.Id}|{item.first_name}|{item.last_name}|{item.phone_number}");
            }

            selectDataCombo.ItemsSource = selectReservation;

        }

        //fuction to load reservation data from database and display it in datagrid named [Reservation Adv.View TabItem]
        private void LoadDataIntoReservationAdvView()
        {
            SqlConnection sqlCNN = new SqlConnection("Data Source=.;Initial Catalog=Reservation-Db;Integrated Security=True;Encrypt=false;");

            gridView.ItemsSource = sqlCNN.Query<Reservation>("select * from Reservations");

        }


        private void HideSomeContents()
        {
            selectDataCombo.Visibility = Visibility.Hidden;
            deleteBtn.Visibility = Visibility.Hidden;
            updateBtn.Visibility = Visibility.Hidden;
        }

        //if you wanna to updata guest data or remove/checkout guest from Hotel, you can click on this button to see update/delete buttons
        private void EditExist_Click(object sender, RoutedEventArgs e)
        {
            //visilble_hidden_Stack.Visibility  = System.Windows.Visibility.Visible;
            //Context.Reservations.Load();
            //selectDataCombo.ItemsSource = Context.Reservations.Local.ToBindingList();
            //selectDataCombo.SelectedValuePath = "Id";
            //selectDataCombo.DisplayMemberPath = "first_name";
            ////selectDataCombo.DisplayMemberPath = "last_name";
            //selectDataCombo.SetBinding(ComboBox.SelectedValueProperty, new Binding("Id"));

            editClicked = true;

            selectDataCombo.Visibility = Visibility.Visible;
            deleteBtn.Visibility = Visibility.Visible;
            updateBtn.Visibility = Visibility.Visible;
            submitBtn.Visibility = Visibility.Visible;


            Selectreservation();
            LoadDataIntoReservationAdvView();

            reset_frontend();

        }

        private void RoomTypeChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Room.SelectedItem != null)
            {
                if (Room.SelectedItem.ToString() == "Single")
                {
                    TotalBill = 149;
                    Floor.Text = "1";
                }
                else if (Room.SelectedItem.ToString() == "Double")
                {
                    TotalBill = 299;
                    Floor.Text = "2";
                }
                else if (Room.SelectedItem.ToString() == "Twin")
                {
                    TotalBill = 349;
                    Floor.Text = "3";
                }
                else if (Room.SelectedItem.ToString() == "Duplex")
                {
                    TotalBill = 399;
                    Floor.Text = "4";
                }
                else if (Room.SelectedItem.ToString() == "Suite")
                {
                    TotalBill = 499;
                    Floor.Text = "5";
                }



            }


            int selectedTemp = 0;
            string selected;
            bool temp = int.TryParse(guest.Text.ToString(), out selectedTemp);
            if (!temp)
            {
                MessageBox.Show(this, "Enter num of guests first", "Error parsing", MessageBoxButton.OK);
            }
            else
            {

                selected = guest.Text.ToString();
                selectedTemp = Convert.ToInt32(selected);
                if (selectedTemp >= 3)
                {
                    TotalBill += (selectedTemp * 5);
                }
            }
        }
         
        //Add New Guest Data To Database
        private void Submit(object sender, RoutedEventArgs e)
        {
            try
            {


                Reservation R = new Reservation();
                R.first_name = First.Text;
                R.last_name = Last.Text;
                R.birth_day = Day.Text + "/" + Month.Text + "/" + Year.Text;
                R.gender = Gender.Text;
                R.phone_number = Phone.Text;
                R.city = City.Text;
                R.street_address = Address.Text;
                R.email_address = email.Text;
                R.apt_suite = APt.Text;
                R.city = City.Text;
                R.state = State.Text;
                R.zip_code = Zcode.Text;
                R.number_guest = int.Parse(guest.Text);
                R.room_type = Room.Text;
                R.room_number = RoomNum.Text;
                R.room_floor = Floor.Text;
                R.arrival_time = EDate.SelectedDate.Value.Date;
                R.leaving_time = DDate.SelectedDate.Value.Date;


                R.break_fast = breakfast;
                R.lunch = lunch;
                R.dinner = dinner;
                R.cleaning = cleaning;
                R.towel = towel;
                R.s_surprise = surprise;
                R.check_in = checkin;

                R.payment_type = PaymentType;
                R.card_type = CardType;
                R.card_number = CardNumber;
                R.card_exp = CardDate;
                R.card_cvc = CVC_Card;
                R.food_bill = FoodBill;
                R.total_bill = TotalPayment;
                R.supply_status = foodStatus;


                Context.Reservations.Add(R);
                Context.SaveChanges();
                MessageBox.Show("Added Successfully", "Add", MessageBoxButton.OK);

            }
            catch (Exception)
            {

                MessageBox.Show("Not Added", "Error", MessageBoxButton.OK);
            }
            //FinalPayment bill = new FinalPayment();


           


            reset_frontend();

            getChecked();

            ReservedRoom();
            GetOccupiedRoom();

            Selectreservation();

            LoadDataIntoReservationAdvView();

        }
    
    private void new_Reservation(object sender, RoutedEventArgs e)
        {
            //visilble_hidden_Stack.Visibility = System.Windows.Visibility.Hidden;
            selectDataCombo.Visibility = Visibility.Hidden;
            deleteBtn.Visibility = Visibility.Hidden;
            updateBtn.Visibility = Visibility.Hidden;
            submitBtn.Visibility = Visibility.Hidden;

            reset_frontend();
        }

        private void ReservationTabLoad(object sender, RoutedEventArgs e)
        {
            Status.IsEnabled = false;
        }

        //Send Food Menu Data to Frontend
        private void viewMenu(object sender, RoutedEventArgs e)
        {
            FoodMenu menu = new FoodMenu(this);
            if (taskFinder)             //make it true when select item from combobox to edit
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


        //Send Final Payment Data to Frontend
        private void Bill_Click(object sender, RoutedEventArgs e)
        {
           FinalPayment  bill = new FinalPayment(this);
            if (breakfast == 0 && lunch == 0 && dinner == 0 && cleaning == false && towel == false && surprise == false)
            {
                Status.IsChecked = true;
            }
            updateBtn.IsEnabled = true;



            bill.totalAmountFromFrontend = TotalBill;
            bill.foodBillF = FoodBill;

            if (taskFinder)
            {
                bill.Payment.Text = FPayment.Replace(" ", string.Empty);
                bill.CardNum.Text = FCnumber;
                bill.comboMonth.Text = FcardExpOne;
                bill.comboYear.Text = FcardExpTwo;
                bill.txtCVC.Text = FCardCVC;
            }

            bill.Show();

            if (!editClicked)
            {
                submitBtn.Visibility = Visibility.Visible;
            }
        }

        //remove guest from hotel
        private void DeleteData_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Reservation Result = Context.Reservations.Find(ID);

                Context.Reservations.Remove(Result);
                Context.SaveChanges();

                MessageBox.Show("Deleted", "Deleted", MessageBoxButton.OK);

            }
            catch (Exception)
            {

                MessageBox.Show("error", "error", MessageBoxButton.OK);
            }




            Selectreservation();

            LoadDataIntoReservationAdvView();

            reset_frontend();
            getChecked();

            ReservedRoom();
            GetOccupiedRoom();

        }

        //Edit Data For Particular Guest
        private void UpdateData_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Reservation Result = Context.Reservations.Find(ID);

                //Reservation newReservation = new Reservation();
                birthday = (Month.SelectedItem) + "-" + (Day.SelectedIndex + 1) + "-" + Year.Text;

                Result.first_name = First.Text;
                Result.last_name = Last.Text;
                Result.gender = Gender.Text;
                Result.email_address = email.Text;
                Result.street_address = Address.Text;
                Result.apt_suite = APt.Text;
                Result.city = City.Text;
                Result.state = State.Text;
                Result.zip_code = Zcode.Text;
                Result.number_guest = guest.SelectedIndex + 1;
                Result.room_type = Room.Text;
                Result.birth_day = birthday;
                Result.room_floor = Floor.Text;
                Result.phone_number = Phone.Text;
                Result.room_number = RoomNum.Text;
                Result.arrival_time = EDate.SelectedDate.Value.Date;
                Result.leaving_time = DDate.SelectedDate.Value.Date;

                Result.break_fast = breakfast;
                Result.lunch = lunch;
                Result.dinner = dinner;
                Result.cleaning = cleaning;
                Result.towel = towel;
                Result.s_surprise = surprise;

                Result.check_in = checkin;

                //newReservation.Food_bill = FoodPrice;
                Result.food_bill = FoodBill;
                Result.card_type = CardType;
                Result.card_number = CardNumber;
                Result.card_exp = CardDate;
                Result.card_cvc = CVC_Card;
                Result.total_bill = TotalPayment;
                Result.payment_type = PaymentType;
                Result.supply_status = foodStatus;

                Context.Entry(Result).State = EntityState.Modified;

                Context.SaveChanges();


                MessageBox.Show("updated", "updated", MessageBoxButton.OK);




                reset_frontend();
                getChecked();

                ReservedRoom();
                GetOccupiedRoom();

                Selectreservation();

                LoadDataIntoReservationAdvView();



            }
            catch (Exception)
            {

                MessageBox.Show("error", "error", MessageBoxButton.OK);
            }
        }

        private void getChecked()
        {
            List<string> Rooms = new List<string>() { "200", "201", "202", "203", "204", "205", "206", "207", "208", "209", "210", "211", "212" };

            SqlConnection sqlCN = new SqlConnection("Data Source=.;Initial Catalog=Reservation-Db;Integrated Security=True;Encrypt=false;");

            var TakenRoomList = sqlCN.Query<Reservation>("GetTakenRooms", commandType: System.Data.CommandType.StoredProcedure).ToList();


            List<string> NewRooms = new List<string>();

            for (int i = 0; i < TakenRoomList.Count; i++)
            {
                if (Rooms.Contains(TakenRoomList[i].room_number))
                {
                    Rooms.Remove(TakenRoomList[i].room_number);
                }

            }


            RoomNum.ItemsSource = Rooms;

        }

        //search by first name about guest
        private void Search(object sender, RoutedEventArgs e)
        {
            var Res = Context.Reservations.Where(r => r.first_name.Contains(txtSearch.Text)).ToList();
            grdSearch.ItemsSource = Res;
        }

        private void onFrontEndClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void onReservSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            taskFinder = true;


            if (selectDataCombo.SelectedItem != null)
            {


                string[] GetServation = selectDataCombo.SelectedItem.ToString().Split("|");
                string id = GetServation[0];

                int resId = int.Parse(id);

                ID = resId;

                Reservation Result = Context.Reservations.Find(resId);

                First.Text = Result.first_name;
                Last.Text = Result.last_name;


                birthday = Result.birth_day;
                string[] dateOfBirth = Result.birth_day.Split("-");

                //Day.Text = dateOfBirth[0];
                //Month.Text = dateOfBirth[1];
                //Year.Text = dateOfBirth[2];


                Gender.Text = Result.gender;
                email.Text = Result.email_address;
                Address.Text = Result.street_address;
                APt.Text = Result.apt_suite;
                City.Text = Result.city;
                State.Text = Result.state;
                Zcode.Text = Result.zip_code;
                guest.Text = Result.number_guest.ToString();
                Room.Text = Result.room_type;
                Floor.Text = Result.room_floor;
                RoomNum.Text = Result.room_number;

                EDate.Text = Result.arrival_time.ToString();
                DDate.Text = Result.leaving_time.ToString();

                breakfast = Result.break_fast;
                lunch = Result.lunch;
                dinner = Result.dinner;

                cleaning = Result.cleaning;
                towel = Result.towel;
                surprise = Result.s_surprise;

                checkin = Result.check_in;

                FoodBill = Result.food_bill;

                CardType = Result.card_type;
                CardNumber = Result.card_number;
                CardDate = Result.card_exp;
                CVC_Card = Result.card_cvc;
                TotalPayment = Result.total_bill;
                PaymentType = Result.payment_type;
                Phone.Text = Result.phone_number;
                foodStatus = Result.supply_status;


                FPayment = Result.payment_type;
                FCnumber = Result.card_number;

                FCardCVC = Result.card_cvc;

                string[] s = Result.card_exp.Split("/");
                FcardExpOne = s[0];
                //FcardExpTwo = s[1];
            }

        }

        private void chkCheckIn_Checked(object sender, RoutedEventArgs e)
        {
            if (Checkin.IsChecked == true)
            {
                Checkin.Content = "Checked in";
                checkin = true;

            }
            else
            {
                checkin = false;

                Checkin.Content = "Check in ?";
            }
        }

        private void chkFoodSupply_Checked(object sender, RoutedEventArgs e)
        {
            if (Status.IsChecked == true)
            {
                Status.Content = "Food/Supply: Complete";
                foodStatus = true;

            }
            else
            {

                foodStatus = false;
                Status.Content = "Food/Supply status?";
            }
        }
        private void reset_frontend()
        {
            //selectDataCombo.Items.Clear();
            selectDataCombo.ItemsSource = null;
            Checkin.IsChecked = false;
            Status.IsChecked = false;

            ClearAllComboBox(this);
            ClearAllTextBoxes(this);

            Selectreservation();
        }
        public void ClearAllComboBox(Control controls)
        {

            Day.SelectedIndex = -1;
            Floor.SelectedIndex = -1;
            //cmbGender.SelectedIndex = -1;
            Gender.SelectedIndex = -1;
            guest.SelectedIndex = -1;
            RoomNum.SelectedIndex = -1;
            Month.SelectedIndex = -1;
            Room.SelectedIndex = -1;
            selectDataCombo.SelectedIndex = -1;
            State.SelectedIndex = -1;

        }
        public void ClearAllTextBoxes(Control controls)
        {

            APt.Clear();
            City.Clear();
            email.Clear();
            First.Clear();
            Last.Clear();
            Phone.Clear();
            Address.Clear();
            txtSearch.Clear();
            Year.Clear();
            Zcode.Clear();

        }

        private void ReservedRoom()
        {

            //AllDataNonGetTakenRooms
            lstRoomAvailab_Res.ItemsSource = null;

            SqlConnection sqlCN = new SqlConnection("Data Source=.;Initial Catalog=Reservation-Db;Integrated Security=True;Encrypt=false;");

            var Result = sqlCN.Query<Reservation>("AllDataNonGetTakenRooms", commandType: System.Data.CommandType.StoredProcedure);

            List<string> selectReservation = new List<string>();

            foreach (var item in Result)
            {
                selectReservation.Add($"{item.room_number}     |    {item.room_type}    | {item.Id}     |   {item.first_name} {item.last_name} | {item.phone_number} | {item.arrival_time.ToString("MM-dd-yyyy")} |    {item.leaving_time.ToString("MM-dd-yyyy")}");
            }

            lstRoomAvailab_Res.ItemsSource = selectReservation;


        }
        private void GetOccupiedRoom()
        {


            lstRoomAvailab_Occ.ItemsSource = null;

            SqlConnection sqlCN = new SqlConnection("Data Source=.;Initial Catalog=Reservation-Db;Integrated Security=True;Encrypt=false;");

            var Result = sqlCN.Query<Reservation>("AllDataOfTakenRooms", commandType: System.Data.CommandType.StoredProcedure);


            List<string> selectReservation = new List<string>();

            foreach (var item in Result)
            {
                selectReservation.Add($"{item.room_number}     |    {item.room_type}    |   {item.Id}   |   {item.first_name}   |   {item.last_name}    |   {item.phone_number} ");
            }

            lstRoomAvailab_Occ.ItemsSource = selectReservation;


        }


    }
}
