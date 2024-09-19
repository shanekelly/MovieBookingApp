using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using static MovieBookingApp.AdminViewUser;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MovieBookingApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainLoginPage : Page
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public static int UserId { get; private set; }

        public MainLoginPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        private void Register_SelectionChanged(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Register));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Admin)); //this button is just to knowif the admin is working..
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage)); //The welcome page just change the "Register" whatever name of the page.
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var userInfo = ValidateUser(Email, Password);
            if (userInfo != null)
            {
                UserId = userInfo.UserId;
                Frame.Navigate(typeof(MainPage));
            }
            else
            {
                ShowLoginError();
            }
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
        private UserInfo ValidateUser(string email, string password)
        {
            const string connString = "SERVER=;DATABASE=;USER ID=;PASSWORD=;";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string sql = "SELECT Id, Email FROM UsersInfo WHERE Email = @Email AND Password = @Password";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new UserInfo
                        {
                            UserId = reader.GetInt32(0),
                            Email = reader.GetString(1)
                        };
                    }
                }
            }
            return null;
        }

        private void ShowLoginError()
        {
            var dialog = new MessageDialog("Invalid email or password.");
            dialog.ShowAsync();
        }
    }

}