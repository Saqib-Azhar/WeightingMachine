using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace LoginForm
{
    public partial class UsersForm : Form
    {
        String cs = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\saqib\Desktop\WeightingMachine\LoginForm\LoginForm\Database1.mdf;Integrated Security=True";

        public UsersForm()
        {
            InitializeComponent();
            loadAllUsers();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void UsersForm_Load(object sender, EventArgs e)
        {

        }

        private void loadAllUsers()
        {
            SqlConnection myConnection = default(SqlConnection);
            myConnection = new SqlConnection(cs);
            myConnection.Open();
            SqlCommand myCommand = default(SqlCommand);

            myCommand = new SqlCommand("SELECT Username as [User Name],Created_By as [Created By],Created_At as [Created At],Is_Deleted [Inactive] FROM Users", myConnection);
            DataTable dt = new DataTable();
            SqlDataAdapter sdr = new SqlDataAdapter(myCommand);
            sdr.Fill(dt);
            dataGridView1.DataSource = dt;

            myConnection.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var UserName = txtUserName.Text;
            var Password = txtPassword.Text;
            var RePassword = textBox1.Text;
            if (string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(UserName))
            {
                MessageBox.Show("Please enter Correct Username & Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUserName.Focus();
                return;
            }
            else if (Password == RePassword)
            {
                try
                {
                    SqlConnection myConnection = default(SqlConnection);
                    myConnection = new SqlConnection(cs);
                    myConnection.Open();
                    SqlCommand myCommand = default(SqlCommand);

                    myCommand = new SqlCommand("INSERT INTO USERS VALUES (@UserName,@Password,@User_Type_Id,@Created_By,@Created_At,@Is_Deleted,@Deleted_Date)", myConnection);
                    myCommand.Parameters.AddWithValue("@UserName", txtUserName.Text);
                    myCommand.Parameters.AddWithValue("@Password", txtPassword.Text);
                    myCommand.Parameters.AddWithValue("@User_Type_Id", 1);
                    myCommand.Parameters.AddWithValue("@Created_By", 1);
                    myCommand.Parameters.AddWithValue("@Created_At", DateTime.Now);
                    myCommand.Parameters.AddWithValue("@Is_Deleted", false);
                    myCommand.Parameters.AddWithValue("@Deleted_Date", DateTime.Now);
                    myCommand.ExecuteNonQuery();
                    MessageBox.Show("User Added Successfully");
                    loadAllUsers();
                    myConnection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Password fields do not match!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Focus();
                return;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainForm uf = new MainForm();
            uf.ShowDialog();
        }
    }
}
