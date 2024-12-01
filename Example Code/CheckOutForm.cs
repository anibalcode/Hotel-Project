using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TrabajoFinalVisualComp
{
    public partial class CheckOutForm : Form
    {
        //   private string connectionString = "Connection String;";

        public CheckOutForm()
        {
            InitializeComponent();
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            string guestID = txtIDNo.Text;

            if (string.IsNullOrEmpty(guestID))
            {
                MessageBox.Show("Please enter guest ID.");
                return;
            }

            try
            {
                DataTable checkInData = GetCheckInData(guestID);
                if (checkInData.Rows.Count == 0)
                {
                    MessageBox.Show("Guest not found in check-in records.");
                    return;
                }

                string guestName = checkInData.Rows[0]["GuestName"].ToString();
                string guestNumber = checkInData.Rows[0]["GuestNumber"].ToString();
                int roomNumber = Convert.ToInt32(checkInData.Rows[0]["RoomNumber"]);
                decimal amountPaid = Convert.ToDecimal(txtPaidAmount.Text);
                DateTime dateCheckedOut = dtpCO.Value.Date;

                InsertCheckOutData(guestName, guestNumber, dateCheckedOut, roomNumber, amountPaid);

                DeleteCheckInData(guestNumber);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private DataTable GetCheckInData(string guestID)
        {
            DataTable checkInData = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM CheckIn WHERE GuestNumber = @GuestNumber";
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                {
                    adapter.SelectCommand.Parameters.AddWithValue("@GuestNumber", guestID);
                    adapter.Fill(checkInData);
                }
            }
            return checkInData;
        }

        private void InsertCheckOutData(string guestName, string guestNumber, DateTime dateCheckedOut, int roomNumber, decimal amountPaid)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO CheckOut (GuestName, GuestNumber, DateCheckedOut, RoomNumber, AmountPaid) " +
                               "VALUES (@GuestName, @GuestNumber, @DateCheckedOut, @RoomNumber, @AmountPaid)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@GuestName", guestName);
                    command.Parameters.AddWithValue("@GuestNumber", guestNumber);
                    command.Parameters.AddWithValue("@DateCheckedOut", dateCheckedOut);
                    command.Parameters.AddWithValue("@RoomNumber", roomNumber);
                    command.Parameters.AddWithValue("@AmountPaid", amountPaid);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        private void DeleteCheckInData(string guestNumber)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM CheckIn WHERE GuestNumber = @GuestNumber";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@GuestNumber", guestNumber);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string guestName = txtSearch.Text;

            try
            {
                DataTable searchData = SearchGuest(guestName);
                if (searchData.Rows.Count == 0)
                {
                    MessageBox.Show("No guests found with the provided name.");
                    return;
                }

                dtgSearch.DataSource = searchData;
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
                string query = "SELECT GuestNumber, GuestName, Cost FROM CheckIn WHERE GuestName LIKE @GuestName";
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                {
                    adapter.SelectCommand.Parameters.AddWithValue("@GuestName", "%" + guestName + "%");
                    adapter.Fill(searchData);
                }
            }
            return searchData;
        }

		private void button2_Click(object sender, EventArgs e)
		{
            CheckOutDatabase checkOutDatabaseForm = new CheckOutDatabase();
            checkOutDatabaseForm.Show();
            this.Hide();
        }

        private void backToMainMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void signOffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

		private void backToMainMenuToolStripMenuItem_Click_1(object sender, EventArgs e)
		{
            MainMenu mainMenuForm = new MainMenu();
            mainMenuForm.Show();
            this.Hide();
        }

		private void signOffToolStripMenuItem_Click_1(object sender, EventArgs e)
		{
            Login loginForm = new Login();
            loginForm.Show();
            this.Hide();
        }
	}

}


