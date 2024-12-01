using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrabajoFinalVisualComp
{
	public partial class MainMenu : Form
	{
		public MainMenu()
		{
			InitializeComponent();
		}

		private void label1_Click(object sender, EventArgs e)
		{

		}

		private void label4_Click(object sender, EventArgs e)
		{

		}

		private void MainMenu_Load(object sender, EventArgs e)
		{

		}

		private void btnManEmp_Click(object sender, EventArgs e)
		{
			ManageEmployeesForm manageEmployees = new ManageEmployeesForm();
			manageEmployees.Show();
			this.Hide();
		}

		private void btnChkIn_Click(object sender, EventArgs e)
		{
			CheckInForm checkinForm = new CheckInForm();
			checkinForm.Show();
			this.Hide();
		}

		private void btnChkOut_Click(object sender, EventArgs e)
		{
			CheckOutForm checkOutForm = new CheckOutForm();
			checkOutForm.Show();
			this.Hide();
		}

		private void btnChkInDtb_Click(object sender, EventArgs e)
		{
			CheckInDatabase checkInDatabaseForm = new CheckInDatabase();
			checkInDatabaseForm.Show();
			this.Hide();
		}

		private void btnChkOutDtb_Click(object sender, EventArgs e)
		{
			CheckOutDatabase checkOutDatabaseForm = new CheckOutDatabase();
			checkOutDatabaseForm.Show();
			this.Hide();
		}

		private void btnSignOut_Click(object sender, EventArgs e)
		{
			Login loginForm = new Login();
			loginForm.Show();
			this.Hide();
		}

		private void manageEmployeesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ManageEmployeesForm manageEmployees = new ManageEmployeesForm();
			manageEmployees.Show();
			this.Hide();
		}

		private void checkInGuestsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CheckInForm checkinForm = new CheckInForm();
			checkinForm.Show();
			this.Hide();
		}

		private void checkInDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
		{
			
		}

		private void checkOutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CheckOutForm checkOutForm = new CheckOutForm();
			checkOutForm.Show();
			this.Hide();
		}

		private void checkOutDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
		{
			
		}

		private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Login loginForm = new Login();
			loginForm.Show();
			this.Hide();
		}

		private void checkInDatabaseToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			CheckInDatabase checkInDatabaseForm = new CheckInDatabase();
			checkInDatabaseForm.Show();
			this.Hide();
		}

		private void checkOutDatabaseToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			CheckOutDatabase checkOutDatabaseForm = new CheckOutDatabase();
			checkOutDatabaseForm.Show();
			this.Hide();
		}
	}
}