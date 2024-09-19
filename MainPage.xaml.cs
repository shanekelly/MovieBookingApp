using System;
using System.Collections.Generic;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MovieBookingApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            List<string> showtimesMovie1 = new List<string>
            {
                "10:00 AM", "12:00 PM", "2:00 PM"

            };
            List<string> showtimesMovie2 = new List<string>
            {
                 "4:00 PM", "6:00 PM", "8:00 PM"

            };
            List<string> showtimesMovie3 = new List<string>
            {
                 "7:00 PM", "9:00 PM", "11:00 PM"

            };
            InitializeShowtimes(showtimesComboBox1, showtimesMovie1);
            InitializeShowtimes(showtimesComboBox2, showtimesMovie2);
            InitializeShowtimes(showtimesComboBox3, showtimesMovie3);


        }
        private void InitializeShowtimes(ComboBox comboBox, List<string> showtimes)
        {
            // Bind the list of showtimes to the ComboBox
            comboBox.ItemsSource = showtimes;
        }


        private void showtimesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }


        private async void DatePicker_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            // Get the selected date from the DatePicker
            DateTimeOffset selectedDate = datePicker.Date;

            // Get the current date
            DateTimeOffset currentDate = DateTimeOffset.Now;

            // Calculate the maximum allowed date (14 days in the future)
            DateTimeOffset maxAllowedDate = currentDate.AddDays(14);

            // Check if the selected date is in the future and within the allowed range
            if (selectedDate > currentDate && selectedDate <= maxAllowedDate)
            {
                // Show the MovieViewGrid
                MovieViewGrid.Visibility = Visibility.Visible;
                // Hide the DatePickerTextblock
                DatePickerTextblock.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Hide the MovieViewGrid
                MovieViewGrid.Visibility = Visibility.Collapsed;
                // Show the DatePickerTextblock
                DatePickerTextblock.Visibility = Visibility.Visible;

                // Display error message
                var messageDialog = new MessageDialog("No available showtimes for the selected date.", "Invalid Date");
                await messageDialog.ShowAsync();
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainLoginPage));
        }

        private void PriceTextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
        private string CheckShowtime(ComboBox comboBox)
        {
            string showtime = comboBox.SelectedItem?.ToString(); // Check if an item is selected
            if (showtime == null)
            {
                ShowError("Please select a showtime.");
            }
            return showtime;
        }

        private void ReserveButton1_Click(object sender, RoutedEventArgs e)
        {
            string showtime = CheckShowtime(showtimesComboBox1);
            if (showtime != null)
            {
                ReserveButton_Click("Movie 1", showtime, datePicker.Date, slider1.Value, EmailTextbox1.Text);
            }
        }

        private void ReserveButton2_Click(object sender, RoutedEventArgs e)
        {
            string showtime = CheckShowtime(showtimesComboBox2);
            if (showtime != null)
            {
                ReserveButton_Click("Movie 2", showtime, datePicker.Date, slider2.Value, EmailTextbox2.Text);
            }
        }

        private void ReserveButton3_Click(object sender, RoutedEventArgs e)
        {
            string showtime = CheckShowtime(showtimesComboBox3);
            if (showtime != null)
            {
                ReserveButton_Click("Movie 3", showtime, datePicker.Date, slider3.Value, EmailTextbox3.Text);
            }
        }

        private void ReserveButton_Click(string movieTitle, string showtime, DateTimeOffset selectedDate, double numberOfTickets, string email)
        {
            // Calculate price
            double price = numberOfTickets * 10;

            // Log inputted data for debugging
            Console.WriteLine($"Movie Title: {movieTitle}");
            Console.WriteLine($"Showtime: {showtime}");
            Console.WriteLine($"Selected Date: {selectedDate.Day}/{selectedDate.Month}/{selectedDate.Year}");
            Console.WriteLine($"Number of Tickets: {numberOfTickets}");
            Console.WriteLine($"Price: ${price}");
            Console.WriteLine($"Email: {email}");

            // Validate data
            if (string.IsNullOrWhiteSpace(email))
            {
                // Email is required
                ShowError("Please enter your email address.");
                return;
            }

            if (!IsValidEmail(email))
            {
                // Invalid email format
                ShowError("Please enter a valid email address.");
                return;
            }

            if (numberOfTickets < 1 || numberOfTickets > 8)
            {
                // Number of tickets is out of range
                ShowError("Please select between 1 and 8 tickets.");
                return;
            }

            // Data is valid, store it or proceed with booking
            // For demonstration, just show the data in a message box
            string bookingInfo = $"Movie: {movieTitle}\nShowtime: {showtime}\nSelected Date: {selectedDate.Day}/{selectedDate.Month}/{selectedDate.Year}\nNumber of Tickets: {numberOfTickets}\nPrice: ${price}\nEmail: {email}";
            ShowMessage("Booking Confirmed!", bookingInfo);
        }




        // Method to validate email address format
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // Helper method to display error message
        private async void ShowError(string message)
        {
            var dialog = new ContentDialog
            {
                Title = "Error",
                Content = message,
                CloseButtonText = "Ok"
            };

            await dialog.ShowAsync();
        }

        // Helper method to display message box
        private async void ShowMessage(string title, string message)
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "Ok"
            };

            await dialog.ShowAsync();
        }



    }
}
