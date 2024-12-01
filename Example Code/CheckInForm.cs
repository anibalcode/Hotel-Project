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

using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TrabajoFinalVisualComp
{
    public partial class CheckInForm : Form
    {
        //   private string connectionString = "Connection String;";

        public CheckInForm()
        {
            InitializeComponent();
            InitializeRoomNumbers();
        }

        private void InitializeRoomNumbers()
        {
           
            for (int i = 1; i <= 10; i++)
            {
                cmbRoomNo.Items.Add(i);
            }
        }

        private void btnAddCheckIn_Click(object sender, EventArgs e)
        {
            string guestName = txtGName.Text;
            int roomNumber = Convert.ToInt32(cmbRoomNo.SelectedItem);
            DateTime dateFrom = dtpFrom.Value.Date;
            DateTime dateTo = dtpTo.Value.Date;
            decimal cost = Convert.ToDecimal(txtCost.Text);

           
            if (IsRoomBooked(roomNumber, dateFrom, dateTo))
            {
                MessageBox.Show("This room is already booked for the selected dates. Please choose another room or adjust the dates.");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO CheckIn (GuestName, GuestNumber, RoomNumber, DateFrom, DateTo, Cost) " +
                                   "VALUES (@GuestName, @GuestNumber, @RoomNumber, @DateFrom, @DateTo, @Cost)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@GuestName", guestName);
                        command.Parameters.AddWithValue("@GuestNumber", GenerateGuestNumber());
                        command.Parameters.AddWithValue("@RoomNumber", roomNumber);
                        command.Parameters.AddWithValue("@DateFrom", dateFrom);
                        command.Parameters.AddWithValue("@DateTo", dateTo);
                        command.Parameters.AddWithValue("@Cost", cost);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Check-in added successfully!");
                            ClearFields();
                        }
                        else
                        {
                            MessageBox.Show("Failed to add check-in.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private string GenerateGuestNumber()
        {
            
            Random random = new Random();
            return random.Next(10000, 99999).ToString();
        }

        private bool IsRoomBooked(int roomNumber, DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT COUNT(*) FROM CheckIn " +
                                   "WHERE RoomNumber = @RoomNumber " +
                                   "AND ((@DateFrom BETWEEN DateFrom AND DateTo) OR (@DateTo BETWEEN DateFrom AND DateTo))";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RoomNumber", roomNumber);
                        command.Parameters.AddWithValue("@DateFrom", dateFrom);
                        command.Parameters.AddWithValue("@DateTo", dateTo);

                        connection.Open();
                        int count = (int)command.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while checking room availability: " + ex.Message);
                return true; // Assume room is booked in case of an error
            }
        }

        private void ClearFields()
        {
            txtGName.Text = "";
            cmbRoomNo.SelectedIndex = -1;
            dtpFrom.Value = DateTime.Today;
            dtpTo.Value = DateTime.Today;
            txtCost.Text = "";
        }

		private void txtCost_TextChanged(object sender, EventArgs e)
		{

		}

		private void button2_Click(object sender, EventArgs e)
		{
            CheckInDatabase checkInDatabaseForm = new CheckInDatabase();
            checkInDatabaseForm.Show();
            this.Hide();
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
