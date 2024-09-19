using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using System.Collections.ObjectModel;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MovieBookingApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Register : Page
    {
        const string conString = "SERVER=;DATABASE=;USER ID=;PASSWORD=;";

        public Register()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainLoginPage));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainLoginPage)); //go to main page and make sure if you really register
        }

        private void tbPass_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string fullName = tbName.Text;
            string email = tbEmail.Text;
            string password = tbPass.Text;


            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                var messageDialog = new MessageDialog("Make sure all fields are valid.");
                await messageDialog.ShowAsync();
            }
            else
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(conString))
                    {
                        await conn.OpenAsync();
                        string query = "INSERT INTO UserInfo (Full_Name, Email, Password) VALUES (@FullName, @Email, @Password)";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@FullName", fullName);
                            cmd.Parameters.AddWithValue("@Email", email);
                            cmd.Parameters.AddWithValue("@Password", password);

                            int result = await cmd.ExecuteNonQueryAsync();
                            if (result > 0)
                            {
                                await new MessageDialog("User registered successfully.").ShowAsync();
                            }
                            else
                            {
                                await new MessageDialog("Registration failed.").ShowAsync();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    await new MessageDialog($"An error occurred: {ex.Message}").ShowAsync();
                }
            }
        }
    }
}