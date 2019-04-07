using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoginForm
{
    public partial class WeightEntries : Form
    {
        private SerialPort _serialPort;         //<-- declares a SerialPort Variable to be used throughout the form
        private const int BaudRate = 9600;      //<-- BaudRate Constant. 9600 seems to be the scale-units default value

        String cs = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\saqib\Desktop\WeightingMachine\LoginForm\LoginForm\Database1.mdf;Integrated Security=True";
        public WeightEntries()
        {
            InitializeComponent();
            fillProductsComboBox();
            loadAllWeightEntries();
            loadAllPorts();
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

        private void loadAllPorts()
        {

            string[] portNames = System.IO.Ports.SerialPort.GetPortNames();     //<-- Reads all available comPorts
            foreach (var portName in portNames)
            {
                comboBox2.Items.Add(portName);                  //<-- Adds Ports to combobox
            }

            comboBox2.SelectedIndex = 0;
            
        }


        private void SerialPortOnDataReceived(object sender, SerialDataReceivedEventArgs serialDataReceivedEventArgs)
        {
            while (_serialPort.BytesToRead > 0)
            {
                textBox1.Text += string.Format("{0:X2} ", _serialPort.ReadByte());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var selectedProduct = comboBox1.SelectedText;
            if (string.IsNullOrEmpty(selectedProduct))
            {
                MessageBox.Show("Please Select a Product!");
                comboBox1.Focus();
            }
            else
            {
                try
                {

                    SqlConnection myConnection = default(SqlConnection);
                    myConnection = new SqlConnection(cs);

                    SqlCommand myCommand = default(SqlCommand);

                    myCommand = new SqlCommand("SELECT * FROM Products WHERE Product_Name = @Product_Name", myConnection);

                    SqlParameter pName = new SqlParameter("@Product_Name", SqlDbType.VarChar);

                    pName.Value = selectedProduct;

                    myCommand.Parameters.Add(pName);

                    myCommand.Connection.Open();

                    SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

                    if (myReader.Read() == true)
                    {
                        if (_serialPort != null && _serialPort.IsOpen)
                            _serialPort.Close();
                        if (_serialPort != null)
                            _serialPort.Dispose();
                        //<-- End of Block

                        _serialPort = new SerialPort(comboBox1.Text, BaudRate, Parity.None, 8, StopBits.One);       //<-- Creates new SerialPort using the name selected in the combobox
                        _serialPort.DataReceived += SerialPortOnDataReceived;       //<-- this event happens everytime when new data is received by the ComPort
                        _serialPort.Open();     //<-- make the comport listen
                        textBox1.Text = "Listening on " + _serialPort.PortName + "...\r\n";
                    }
                    else
                    {
                        MessageBox.Show("Something went wrong please try later!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }
    }
}
