using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TrabajoFinalVisualComp
{
    public partial class ManageEmployeesForm : Form
    {
      
     //   private string connectionString = "Connection String;";

        public ManageEmployeesForm()
        {
            InitializeComponent();
            cbxIsAdmin.Items.AddRange(new string[] { "Y", "N" });
            RefreshEmployeeData();
        }

        private void RefreshEmployeeData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM Employee";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        DataTable employeeTable = new DataTable();
                        adapter.Fill(employeeTable);
                        dtgEmp.DataSource = employeeTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while refreshing employee data: " + ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshEmployeeData();
        }

       

        private void btnConfirm_Click(object sender, EventArgs e)
        {
           
           
        }




        private void ClearTextBoxes()
        {
            txtEmpNo.Text = "";
            txtEmpName.Text = "";
            txtEmpUserN.Text = "";
            txtEmpPW.Text = "";
            cbxIsAdmin.SelectedIndex = -1; 
        }

		private void btnAdd_Click(object sender, EventArgs e)
		{
            {
                string empName = txtEmpName.Text;
                string empUserN = txtEmpUserN.Text;
                string empPW = txtEmpPW.Text;
                string isAdmin = cbxIsAdmin.SelectedItem?.ToString(); 
                string empNumber = txtEmpNo.Text; 

                if (string.IsNullOrEmpty(empName) || string.IsNullOrEmpty(empUserN) || string.IsNullOrEmpty(empPW) || string.IsNullOrEmpty(isAdmin) || string.IsNullOrEmpty(empNumber))
                {
                    MessageBox.Show("Please fill in all fields.");
                    return;
                }

                string query = "INSERT INTO Employee (FullName, EmployeeNumber, Username, Password, IsAdmin) " +
                               "VALUES (@FullName, @EmployeeNumber, @Username, @Password, @IsAdmin)";

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@FullName", empName);
                            command.Parameters.AddWithValue("@EmployeeNumber", empNumber);
                            command.Parameters.AddWithValue("@Username", empUserN);
                            command.Parameters.AddWithValue("@Password", empPW);
                            command.Parameters.AddWithValue("@IsAdmin", isAdmin);

                            connection.Open();
                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Employee added successfully!");
                                ClearTextBoxes();
                                RefreshEmployeeData(); 
                            }
                            else
                            {
                                MessageBox.Show("Failed to add employee.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }


            }
        }
       
        private void btnDelete_Click(object sender, EventArgs e)
        {
            string empNumber = txtEmpNo.Text;

           
            if (string.IsNullOrEmpty(empNumber))
            {
                MessageBox.Show("Please enter an employee number.");
                return;
            }

           
            DialogResult result = MessageBox.Show("Are you sure you want to delete the employee?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                
                DeleteEmployee(empNumber);
            }
        }

        private void DeleteEmployee(string empNumber)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Employee WHERE EmployeeNumber = @EmployeeNumber";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeNumber", empNumber);
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Employee deleted successfully!");
                            ClearTextBoxes();
                            RefreshEmployeeData(); 
                        }
                        else
                        {
                            MessageBox.Show("Employee with the provided number does not exist.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while deleting employee: " + ex.Message);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            string empNumber = txtEmpNo.Text;

          
            if (string.IsNullOrEmpty(empNumber))
            {
                MessageBox.Show("Please enter an employee number.");
                return;
            }

         
            if (!EmployeeExists(empNumber))
            {
                MessageBox.Show("Employee with the provided number does not exist.");
                return;
            }

           
            DialogResult result = MessageBox.Show("Are you sure you want to edit the employee?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
            
                EditEmployee(empNumber);
            }
        }


        private bool EmployeeExists(string empNumber)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT COUNT(*) FROM Employee WHERE EmployeeNumber = @EmployeeNumber";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeNumber", empNumber);
                        connection.Open();
                        int count = (int)command.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while checking employee existence: " + ex.Message);
                return false;
            }
        }

        private void EditEmployee(string empNumber)
        {
            string empName = txtEmpName.Text;
            string empUserN = txtEmpUserN.Text;
            string empPW = txtEmpPW.Text;
            string isAdmin = cbxIsAdmin.SelectedItem?.ToString(); 

            string query = "UPDATE Employee SET FullName = @FullName, Username = @Username, Password = @Password, IsAdmin = @IsAdmin WHERE EmployeeNumber = @EmployeeNumber";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FullName", empName);
                        command.Parameters.AddWithValue("@Username", empUserN);
                        command.Parameters.AddWithValue("@Password", empPW);
                        command.Parameters.AddWithValue("@IsAdmin", isAdmin);
                        command.Parameters.AddWithValue("@EmployeeNumber", empNumber);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Employee updated successfully!");
                            RefreshEmployeeData(); 
                        }
                        else
                        {
                            MessageBox.Show("Failed to update employee.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while editing employee: " + ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string empName = txtSrchName.Text.Trim();

            if (string.IsNullOrEmpty(empName))
            {
                MessageBox.Show("Please enter a name to search.");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM Employee WHERE FullName LIKE @FullName";
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@FullName", "%" + empName + "%");
                        DataTable employeeTable = new DataTable();
                        adapter.Fill(employeeTable);
                        dtgEmp.DataSource = employeeTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while searching for employees: " + ex.Message);
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

