using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;

using System.Data.SqlClient;
using System.Diagnostics;
using System.ServiceModel.Channels;
using Windows.UI.Popups;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MovieBookingApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AdminViewUser : Page
    {
        const string conString = "SERVER=;DATABASE=;USER ID=;PASSWORD=;";
        private ObservableCollection<UserInfo> users = new ObservableCollection<UserInfo>();

        public AdminViewUser()
        {
            this.InitializeComponent();
            LoadUsers();
        }

        private void ViewUsers_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AdminViewMovie));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Admin));
        }

        private async void AddUser_Click(object sender, RoutedEventArgs e)
        {
            string fullName = Name.Text;
            string email = Email.Text;
            string password = Password.Text;


            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                await ShowMessageDialog("Enter valid input for all fields.");
            }
            else
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(conString))
                    {
                        await conn.OpenAsync();
                        string query = "INSERT INTO userInfo (Full_Name, Email, Password) VALUES (@FullName, @Email, @Password)";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@FullName", fullName);
                            cmd.Parameters.AddWithValue("@Email", email);
                            cmd.Parameters.AddWithValue("@Password", password);

                            int result = await cmd.ExecuteNonQueryAsync();

                            if (result > 0)
                            {
                                Name.Text = "";
                                Email.Text = "";
                                Password.Text = "";
                                
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception: " + ex.Message);
                }
            }
        }

        private async void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            string email = Email.Text;

            // Validate input
            if (string.IsNullOrWhiteSpace(email))
            {
                await ShowMessageDialog("Enter a valid email address.");
            }
            else
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(conString))
                    {
                        await conn.OpenAsync();
                        string query = "DELETE FROM UserInfo WHERE Email = @Email";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Email", email);

                            int result = await cmd.ExecuteNonQueryAsync();

                            if (result > 0)
                            {
                                await ShowMessageDialog("User deleted successfully.");
                                Email.Text = string.Empty;
                                LoadUsers();
                            }
                            else
                            {
                                await ShowMessageDialog("No user found with the given email.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception: " + ex.Message);
                }
            }
        }

        private async void UpdateUser_Click(object sender, RoutedEventArgs e)
        {
            string fullName = Name.Text;
            string email = Email.Text;
            string password = Password.Text;

            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                await ShowMessageDialog("Please fill in all fields.");
            }
            else
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(conString))
                    {
                        await conn.OpenAsync();
                        string query = "UPDATE userInfo SET Full_Name = @FullName, Password = @Password WHERE Email = @Email";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@FullName", fullName);
                            cmd.Parameters.AddWithValue("@Password", password);
                            cmd.Parameters.AddWithValue("@Email", email);

                            int result = await cmd.ExecuteNonQueryAsync();

                            if (result > 0)
                            {
                                await ShowMessageDialog("User information updated successfully.");
                            }
                            else
                            {
                                await ShowMessageDialog("No user found.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception: " + ex.Message);
                }
            }
        }
        // User info class
        public class UserInfo
        {
            public int UserId { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; } // Passwords are unsecure
        }
        // Method to load users
        private async void LoadUsers()
        {
            try
            {
                users.Clear();
                using (SqlConnection conn = new SqlConnection(conString))
                {
                    await conn.OpenAsync();

                    string query = "SELECT User_Id, Full_Name, Email, Password FROM UserInfo";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                users.Add(new UserInfo
                                {
                                    UserId = reader.GetInt32(0),
                                    FullName = reader.GetString(1),
                                    Email = reader.GetString(2),
                                    Password = reader.GetString(3)
                                });
                            }
                        }
                    }
                }

                UserListView.ItemsSource = users;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.Message);
            }
        }

        // Display MessageDialogue
        private async Task ShowMessageDialog(string message)
        {
            MessageDialog dialog = new MessageDialog(message);
            await dialog.ShowAsync();
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void RefreshUsers_Click(object sender, RoutedEventArgs e)
        {
            LoadUsers();
        }

        private void UserListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
