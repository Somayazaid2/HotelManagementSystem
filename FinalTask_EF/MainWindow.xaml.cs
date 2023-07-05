using FinalTask_EF.Context;
using FinalTask_EF.Entities;
using Intersoft.Crosslight;
using MahApps.Metro.Controls;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FinalTask_EF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void License_Click(object sender, RoutedEventArgs e)
        {
            License license = new License();
            license.ShowDialog();
        }
        
        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            ReservationDbContext db = new ReservationDbContext();
            //if (db.frontends.) { }
            //var username = textBoxUserName.Text;
            //var password = passwordBox1.Password;

            var LoggedReservation = db.frontends.FirstOrDefault(r=>r.user_name == textBoxUserName.Text && r.pass_word == passwordBox1.Password);
            var LoggedKitchen = db.kitchens.FirstOrDefault(k => k.user_name == textBoxUserName.Text && k.pass_word == passwordBox1.Password);

            //Debug.WriteLine("hello"+db.frontends.FirstOrDefault(r=>r.user_name == username));
            if (textBoxUserName.Text == "" || passwordBox1.Password == "")
            {
                MessageBox.Show("Username or Password Incorrect!");
            } 
            if (LoggedReservation != null)
            {
                Frontend frontend = new Frontend();
                frontend.Show();
                this.Close();
                
            }
            else if(LoggedKitchen != null)
            {
                Kitchen kitchen = new Kitchen();
                kitchen.Show();
                this.Close();


            }
            else
            {
                MessageBox.Show("invalid username or password");
                
            }
            

            //if(db.frontends.Any(x=>x.user_name == "admin") && db.frontends.Any(x=>x.pass_word == "admin") &&(textBoxUserName.Text=="admin") && (passwordBox1.Password == "admin")) { 
            //    Frontend frontend = new Frontend();
            //    frontend.ShowDialog();
            //}
            ////else if (db.frontends.Any(x => x.user_name == "kitchen") && db.frontends.Any(x => x.pass_word == "kitchen") && (textBoxUserName.Text == "kitchen") && (passwordBox1.Password == "kitchen"))
            //else if (textBoxUserName.Text == db.frontends.Select(x => x.user_name))
            //{
            //    Kitchen kitchen = new Kitchen();
            //    kitchen.ShowDialog();
            //}
            //else
            //{
                
            //}
        }

        
    }
}
