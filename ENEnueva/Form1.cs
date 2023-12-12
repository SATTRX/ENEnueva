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
using System.Security.Cryptography;

namespace ENEnueva
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = "server=SATTROX\\SQLEXPRESS; database=EneSegundo; integrated security=true";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM USUARIO WHERE nombre = @nombre AND llave = @llave";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nombre", textBox1.Text);
                        command.Parameters.AddWithValue("@llave", Encryp.EncryptString(textBox2.Text));
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                bool isAdmin = reader.GetBoolean(reader.GetOrdinal("administrador"));
                                if (isAdmin)
                                {
                                    Form3 adminForm = new Form3();
                                    adminForm.Show();
                                }
                                else
                                {
                                    Form2 userForm = new Form2();
                                    userForm.Show();
                                }
                                reader.Close();
                            }
                            else
                            {
                                MessageBox.Show("Usuario o contraseña incorrectos");
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + (ex.Message) + "Error");
            }
        
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            pictureBox3.BringToFront();
            textBox2.PasswordChar = '\0';
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            pictureBox2.BringToFront();
            textBox2.PasswordChar = '*';
        }
    }
}
