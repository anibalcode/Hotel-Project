using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TrabajoFinalVisualComp
{
    public partial class CheckInDatabase : Form
    {
        //   private string connectionString = "Connection String;";

        public CheckInDatabase()
        {
            InitializeComponent();
            RefreshCheckInData();
        }

        private void RefreshCheckInData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM CheckIn";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        DataTable checkInData = new DataTable();
                        adapter.Fill(checkInData);
                        dtgCI.DataSource = checkInData;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while refreshing check-in data: " + ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string guestName = txtSrch.Text;

            try
            {
                DataTable searchData = SearchGuest(guestName);
                if (searchData.Rows.Count == 0)
                {
                    MessageBox.Show("No guests found with the provided name.");
                    return;
                }

                dtgCI.DataSource = searchData;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private DataTable SearchGuest(string guestName)
        {
            DataTable searchData = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT GuestNumber, GuestName FROM CheckIn WHERE GuestName LIKE @GuestName";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@GuestName", "%" + guestName + "%");
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    searchData.Load(reader);
                }
            }
            return searchData;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string guestNumber = txtIDNo.Text;

           
            if (string.IsNullOrEmpty(guestNumber))
            {
                MessageBox.Show("Please enter a guest number.");
                return;
            }

           
            DialogResult result = MessageBox.Show("Are you sure you want to delete the guest?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
               
                DeleteGuest(guestNumber);
            }
        }

        private void DeleteGuest(string guestNumber)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM CheckIn WHERE GuestNumber = @GuestNumber";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@GuestNumber", guestNumber);
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Guest deleted successfully!");
                            RefreshCheckInData(); 
                        }
                        else
                        {
                            MessageBox.Show("Guest with the provided number does not exist.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while deleting guest: " + ex.Message);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            string guestNumber = txtIDNo.Text;
            string guestName = txtGName.Text;
            string roomNumber = txtRNo.Text;
            decimal cost = Convert.ToDecimal(txtCost.Text);
            DateTime dateFrom = dtpFrom.Value;
            DateTime dateTo = dtpTo.Value;

          
            if (string.IsNullOrEmpty(guestNumber))
            {
                MessageBox.Show("Please enter a guest number.");
                return;
            }

          
            if (!GuestExists(guestNumber))
            {
                MessageBox.Show("Guest with the provided number does not exist.");
                return;
            }

           
            EditGuest(guestNumber, guestName, roomNumber, cost, dateFrom, dateTo);
        }

        private bool GuestExists(string guestNumber)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM CheckIn WHERE GuestNumber = @GuestNumber";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@GuestNumber", guestNumber);
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        private void EditGuest(string guestNumber, string guestName, string roomNumber, decimal cost, DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "UPDATE CheckIn SET GuestName = @GuestName, RoomNumber = @RoomNumber, Cost = @Cost, DateFrom = @DateFrom, DateTo = @DateTo WHERE GuestNumber = @GuestNumber";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@GuestName", guestName);
                        command.Parameters.AddWithValue("@RoomNumber", roomNumber);
                        command.Parameters.AddWithValue("@Cost", cost);
                        command.Parameters.AddWithValue("@DateFrom", dateFrom);
                        command.Parameters.AddWithValue("@DateTo", dateTo);
                        command.Parameters.AddWithValue("@GuestNumber", guestNumber);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Guest updated successfully!");
                            RefreshCheckInData(); 
                        }
                        else
                        {
                            MessageBox.Show("Failed to update guest.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updating guest: " + ex.Message);
            }
        }

        private void backToMainMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainMenu mainMenuForm = new MainMenu();
            mainMenuForm.Show();
            this.Hide();
        }

        private void signOffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Login loginForm = new Login();
            loginForm.Show();
            this.Hide();
        }
    }
}

