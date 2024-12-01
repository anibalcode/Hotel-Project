using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TrabajoFinalVisualComp
{
    public partial class CheckOutDatabase : Form
    {
        //   private string connectionString = "Connection String;";

        public CheckOutDatabase()
        {
            InitializeComponent();
            RefreshCheckOutData();
        }

        private void RefreshCheckOutData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM CheckOut";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        DataTable checkOutData = new DataTable();
                        adapter.Fill(checkOutData);
                        dtgCO.DataSource = checkOutData;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while refreshing check-out data: " + ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string guestName = txtSearchName.Text;

            try
            {
                DataTable searchData = SearchGuest(guestName);
                if (searchData.Rows.Count == 0)
                {
                    MessageBox.Show("No guests found with the provided name.");
                    return;
                }

                dtgCO.DataSource = searchData;
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
                string query = "SELECT GuestNumber, GuestName FROM CheckOut WHERE GuestName LIKE @GuestName";
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
                    string query = "DELETE FROM CheckOut WHERE GuestNumber = @GuestNumber";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@GuestNumber", guestNumber);
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Guest deleted successfully!");
                            RefreshCheckOutData(); 
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
           
        }

        private bool GuestExists(string guestNumber)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM CheckOut WHERE GuestNumber = @GuestNumber";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@GuestNumber", guestNumber);
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        private void EditGuest(string guestNumber, string guestName, string roomNumber, decimal amountPaid, DateTime dateCheckedOut)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "UPDATE CheckOut SET GuestName = @GuestName, RoomNumber = @RoomNumber, AmountPaid = @AmountPaid, DateCheckedOut = @DateCheckedOut WHERE GuestNumber = @GuestNumber";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@GuestName", guestName);
                        command.Parameters.AddWithValue("@RoomNumber", roomNumber);
                        command.Parameters.AddWithValue("@AmountPaid", amountPaid);
                        command.Parameters.AddWithValue("@DateCheckedOut", dateCheckedOut);
                        command.Parameters.AddWithValue("@GuestNumber", guestNumber);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Guest updated successfully!");
                            RefreshCheckOutData(); 
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

		private void btnEdit_Click_1(object sender, EventArgs e)
		{
            string guestNumber = txtIDNo.Text;
            string guestName = txtGName.Text;
            string roomNumber = txtRNo.Text;
            decimal amountPaid = Convert.ToDecimal(txtPaid.Text);
            DateTime dateCheckedOut = dtpCO.Value;


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


            EditGuest(guestNumber, guestName, roomNumber, amountPaid, dateCheckedOut);
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

