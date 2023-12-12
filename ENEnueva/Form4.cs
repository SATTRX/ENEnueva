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

namespace ENEnueva
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
         

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "server=SATTROX\\SQLEXPRESS; database=EneSegundo; integrated security=true";
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {

                connection.Open();
                string commandText = "SELECT * FROM USUARIO WHERE RUT = @RUT";
                SqlCommand command = new SqlCommand(commandText, connection);

                command.Parameters.AddWithValue("@RUT", textBox1.Text);

                SqlDataReader reader = command.ExecuteReader();

                DataTable table = new DataTable();
                table.Load(reader);
                dataGridView1.DataSource = table;

                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
