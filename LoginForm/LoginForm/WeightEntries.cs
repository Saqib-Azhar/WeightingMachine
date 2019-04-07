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
    public partial class WeightEntries : Form
    {
        String cs = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\saqib\Desktop\LoginForm\LoginForm\Database1.mdf;Integrated Security=True";
        public WeightEntries()
        {
            InitializeComponent();
            fillProductsComboBox();
            loadAllWeightEntries();
        }

        private void WeightEntries_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            loadAllWeightEntries();
        }
        private void loadAllWeightEntries()
        {
            SqlConnection myConnection = default(SqlConnection);
            myConnection = new SqlConnection(cs);
            myConnection.Open();
            SqlCommand myCommand = default(SqlCommand);

            myCommand = new SqlCommand("SELECT Product_Name as [Product Name], Weight, Created_By as [Created By],Created_At as [Created At], Loose_Weight [Is Loose Weight] FROM WeightEntries", myConnection);
            DataTable dt = new DataTable();
            SqlDataAdapter sdr = new SqlDataAdapter(myCommand);
            sdr.Fill(dt);
            dataGridView1.DataSource = dt;

            myConnection.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainForm uf = new MainForm();
            uf.ShowDialog();
        }

        private void fillProductsComboBox()
        {
            SqlConnection myConnection = default(SqlConnection);
            myConnection = new SqlConnection(cs);
            SqlCommand myCommand = default(SqlCommand);

            myCommand = new SqlCommand("SELECT * FROM Products", myConnection);

            SqlDataReader myReader;
            try
            {
                myConnection.Open();
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    string prodName = myReader.GetString(1);
                    comboBox1.Items.Add(prodName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            myConnection.Close();
        }
    }
}
