using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TrabajoFinalVisualComp
{
    public partial class Login : Form
    {
        //   private string connectionString = "Connection String;";

        public Login()
        {
            InitializeComponent();

           
            this.AcceptButton = btnSignIn;
        }

        private void btnSignIn_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter username and password.");
                return;
            }

           
            if (AuthenticateUser(username, password))
            {
                MainMenu mainMenuForm = new MainMenu();
                mainMenuForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }

        private bool AuthenticateUser(string username, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Employee WHERE Username = @Username AND Password = @Password";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

		private void label2_Click(object sender, EventArgs e)
		{

		}

		private void txtPassword_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
		{
        
		}
	}
}
