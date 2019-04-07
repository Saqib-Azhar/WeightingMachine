using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoginForm
{
    public partial class ProductsForm : Form
    {
        String cs = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\saqib\Desktop\WeightingMachine\LoginForm\LoginForm\Database1.mdf;Integrated Security=True";
        public ProductsForm()
        {
            InitializeComponent();
            loadAllProducts();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            this.Hide();
            MainForm uf = new MainForm();
            uf.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var ProductName = txtProductName.Text;
            var MinWeight = MinimumNumeric.Value;
            var MaxWeight = MaximumNumeric.Value;
            if (string.IsNullOrEmpty(ProductName))
            {
                MessageBox.Show("Please enter a Product Name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtProductName.Focus();
                return;
            }
            else if (!(MinWeight > MaxWeight))
            {
                try
                {
                    SqlConnection myConnection = default(SqlConnection);
                    myConnection = new SqlConnection(cs);
                    myConnection.Open();
                    SqlCommand myCommand = default(SqlCommand);

                    myCommand = new SqlCommand("INSERT INTO PRODUCTS (Product_Name,Created_At,Created_By,Is_Deleted,Deleted_Date,Min_Weight,Max_Weight) VALUES (@Product_Name,@Created_At,@Created_By,@Is_Deleted,@Deleted_Date,@Min_Weight,@Max_Weight)", myConnection);
                    myCommand.Parameters.AddWithValue("@Product_Name", txtProductName.Text);
                    myCommand.Parameters.AddWithValue("@Created_At", DateTime.Now);
                    myCommand.Parameters.AddWithValue("@Created_By", 1);
                    myCommand.Parameters.AddWithValue("@Is_Deleted", false);
                    myCommand.Parameters.AddWithValue("@Deleted_Date", DateTime.Now);
                    myCommand.Parameters.AddWithValue("@Min_Weight", MinWeight);
                    myCommand.Parameters.AddWithValue("@Max_Weight", MaxWeight);
                    myCommand.ExecuteNonQuery();
                    MessageBox.Show("User Added Successfully");
                    loadAllProducts();
                    myConnection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Minimum weight can not be greater than Maximum Weight!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MinimumNumeric.Focus();
                return;
            }
        }

        private void loadAllProducts()
        {
            SqlConnection myConnection = default(SqlConnection);
            myConnection = new SqlConnection(cs);
            myConnection.Open();
            SqlCommand myCommand = default(SqlCommand);

            myCommand = new SqlCommand("SELECT Product_Name as [Product Name], Min_Weight as [Min Weight], Max_Weight as [Max Weight],Created_By as [Created By],Created_At as [Created At],Is_Deleted [Inactive] FROM Products", myConnection);
            DataTable dt = new DataTable();
            SqlDataAdapter sdr = new SqlDataAdapter(myCommand);
            sdr.Fill(dt);
            dataGridView1.DataSource = dt;

            myConnection.Close();
        }
    }
}
