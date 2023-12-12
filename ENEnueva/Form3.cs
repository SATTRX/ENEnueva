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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string rut = textBox2.Text;
            string nombre = textBox1.Text;
            string llave = textBox6.Text;
            string email = textBox3.Text;
            string direccion = textBox4.Text;
            string telefono = textBox5.Text;
            bool administrador = checkBox1.Checked;
            string confirmarLlave = textBox7.Text;


            if (llave == confirmarLlave) 
            {
                string connectionString = "server=SATTROX\\SQLEXPRESS; database=EneSegundo; integrated security=true";
                string query = "INSERT INTO USUARIO (rut, nombre, llave, email, direccion, telefono, administrador) VALUES (@rut, @nombre, @llave,@email, @direccion, @telefono,@administrador)";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    using (SqlCommand com = new SqlCommand(query, con))
                    {
                        com.Parameters.AddWithValue("@rut", rut);
                        com.Parameters.AddWithValue("@nombre", nombre);
                        com.Parameters.AddWithValue("@llave", Encryp.EncryptString(llave));
                        com.Parameters.AddWithValue("@email", email);
                        com.Parameters.AddWithValue("@direccion", direccion);
                        com.Parameters.AddWithValue("@telefono", telefono);
                        com.Parameters.AddWithValue("@administrador", administrador);

                        com.ExecuteNonQuery();
                    }

                    con.Close();
                }

                MessageBox.Show("Registro exitoso");
            }
            else
            {
                MessageBox.Show("La confirmación de la llave es incorrecta");
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            pictureBox3.BringToFront();
            textBox6.PasswordChar = '\0';
        }

        private void pictureBox3_Click_1(object sender, EventArgs e)
        {
            pictureBox2.BringToFront();
            textBox6.PasswordChar = '*';
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            pictureBox5.BringToFront();
            textBox7.PasswordChar = '\0';

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            pictureBox4.BringToFront();
            textBox7.PasswordChar = '*';
        }
    }
}
