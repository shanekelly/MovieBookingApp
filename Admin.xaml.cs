using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
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
using Windows.UI.Popups;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MovieBookingApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Admin : Page
    {
        const string conString = "SERVER=;DATABASE=;USER ID=;PASSWORD=;";
        private ObservableCollection<Reservation> reservations = new ObservableCollection<Reservation>();

        public Admin()
        {
            this.InitializeComponent();
            LoadReservations();
        }

        private void ViewUsers_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AdminViewUser));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AdminViewMovie));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            LoadReservations();
        }

        public class Reservation
        {
            public int ReservationId { get; set; }
            public string MovieName { get; set; }
            public string Time { get; set; }
            public DateTime Date { get; set; }
            public decimal TicketPrice { get; set; }
            public int TicketAmount { get; set; }
            public int UserId { get; set; }
        }
        private async void LoadReservations()
        {
            var reservations = new ObservableCollection<Reservation>();

            try
            {
                using (SqlConnection conn = new SqlConnection(conString))
                {
                    await conn.OpenAsync();
                    string query = "SELECT Reservation_Id, Movie_Name, Time, Date, Ticket_Price, Ticket_Amount, User_Id FROM ReservationInfo";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                reservations.Add(new Reservation
                                {
                                    ReservationId = reader.GetInt32(0),
                                    MovieName = reader.GetString(1),
                                    Time = reader.GetString(2),
                                    Date = reader.GetDateTime(3),
                                    TicketPrice = reader.GetDecimal(4),
                                    TicketAmount = reader.GetInt32(5),
                                    UserId = reader.GetInt32(6)
                                });
                            }
                        }
                    }
                }

                ReservationsListView.ItemsSource = reservations;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.Message);
            }
        }

        private async void DeleteBooking_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ReservationId.Text))
            {
                await new MessageDialog("Please enter a valid Reservation ID.").ShowAsync();
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(conString))
                {
                    await conn.OpenAsync();
                    string query = "DELETE FROM ReservationInfo WHERE Reservation_Id = @ReservationId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ReservationId", Convert.ToInt32(ReservationId.Text));

                        int result = await cmd.ExecuteNonQueryAsync();
                        if (result > 0)
                        {
                            await new MessageDialog("Reservation deleted successfully.").ShowAsync();

                            LoadReservations();
                        }
                        else
                        {
                            await new MessageDialog("No reservation found.").ShowAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.Message);
                await new MessageDialog($"An error occurred: {ex.Message}").ShowAsync();
            }
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));

        }
    }
}