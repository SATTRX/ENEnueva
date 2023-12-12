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
    public partial class Form2 : Form
    {
        private SqlConnection connection;
        
        public Form2()
        {
            InitializeComponent();
            comboBox1.Items.Add("CUPRUM");
            comboBox1.Items.Add("MODELO");
            comboBox1.Items.Add("CAPITAL");
            comboBox1.Items.Add("PROVIDA");

            comboBox2.Items.Add("FONASA");
            comboBox2.Items.Add("CONSALUD");
            comboBox2.Items.Add("MASVIDA");
            comboBox2.Items.Add("BANMEDICA");
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            int horasTrabajadas;
            int horasExtras;
            int sueldoBase;
            int sueldoExtra;
            int sueldoBruto;
            double descuentoAFP = 0;
            double descuentoSalud = 0;
            int sueldoLiquido;

            if (int.TryParse(textBox1.Text, out horasTrabajadas) && int.TryParse(textBox2.Text, out horasExtras))
            {
                sueldoBase = horasTrabajadas * 5000;
                sueldoExtra = horasExtras * 7000;
                sueldoBruto = sueldoBase + sueldoExtra;

                if (comboBox1.SelectedItem != null)
                {
                    if (comboBox1.SelectedItem.ToString() == "CUPRUM")
                    {
                        descuentoAFP = sueldoBruto * 0.07;
                    }
                    else if (comboBox1.SelectedItem.ToString() == "MODELO")
                    {
                        descuentoAFP = sueldoBruto * 0.09;
                    }
                    else if (comboBox1.SelectedItem.ToString() == "CAPITAL")
                    {
                        descuentoAFP = sueldoBruto * 0.12;
                    }
                    else if (comboBox1.SelectedItem.ToString() == "PROVIDA")
                    {
                        descuentoAFP = sueldoBruto * 0.13;
                    }
                }

                if (comboBox2.SelectedItem != null)
                {
                    if (comboBox2.SelectedItem.ToString() == "FONASA")
                    {
                        descuentoSalud = sueldoBruto * 0.12;
                    }
                    else if (comboBox2.SelectedItem.ToString() == "CONSALUD")
                    {
                        descuentoSalud = sueldoBruto * 0.13;
                    }
                    else if (comboBox2.SelectedItem.ToString() == "MASVIDA")
                    {
                        descuentoSalud = sueldoBruto * 0.14;
                    }
                    else if (comboBox2.SelectedItem.ToString() == "BANMEDICA")
                    {
                        descuentoSalud = sueldoBruto * 0.15;
                    }
                }

                sueldoLiquido = (int)(sueldoBruto - descuentoAFP - descuentoSalud);

                textBox3.Text = sueldoBruto.ToString();
                textBox4.Text = sueldoLiquido.ToString();
            }
            else
            {
                MessageBox.Show("Por favor, ingrese valores numéricos válidos en las horas trabajadas y horas extras.");
            }
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = "server=SATTROX\\SQLEXPRESS; database=EneSegundo; integrated security=true";

                using (connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string rutBuscar = textBox5.Text;

                    string query = "SELECT rut FROM USUARIO WHERE rut = @rutBuscar";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@rutBuscar", rutBuscar);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                reader.Close();
                                string sueldosQuery = "INSERT INTO SUELDO (hrs_trabajadas, hrs_extras, sueldo_liquido) VALUES (@hrs_trabajadas, @hrs_extras, @sueldo_liquido)";
                                using (SqlCommand sueldosCommand = new SqlCommand(sueldosQuery, connection))
                                {
                                    sueldosCommand.Parameters.AddWithValue("@hrs_trabajadas", Convert.ToInt32(textBox1.Text));
                                    sueldosCommand.Parameters.AddWithValue("@hrs_extras", Convert.ToInt32(textBox2.Text));
                                    sueldosCommand.Parameters.AddWithValue("@sueldo_liquido", Convert.ToInt32(textBox4.Text));

                                    sueldosCommand.ExecuteNonQuery();
                                }

                                string afpQuery = "INSERT INTO AFP (nombre, descuentos) VALUES (@nombre, @descuentos)";
                                using (SqlCommand afpCommand = new SqlCommand(afpQuery, connection))
                                {
                                    afpCommand.Parameters.AddWithValue("@nombre", comboBox1.SelectedItem.ToString());
                                    afpCommand.Parameters.AddWithValue("@descuentos", ObtenerDescuentoAFP());

                                    afpCommand.ExecuteNonQuery();
                                }

                                string isapreQuery = "INSERT INTO ISAPRES (nombre, descuentos) VALUES (@nombre, @descuentos)";
                                using (SqlCommand isapresCommand = new SqlCommand(isapreQuery, connection))
                                {
                                    isapresCommand.Parameters.AddWithValue("@nombre", comboBox2.SelectedItem.ToString());
                                    isapresCommand.Parameters.AddWithValue("@descuentos", ObtenerDescuentoIsapre());

                                    isapresCommand.ExecuteNonQuery();
                                }

                                MessageBox.Show("Los datos se han guardado correctamente.");
                                reader.Close();
                            }
                            else
                            {
                                MessageBox.Show("No se ha encontrado un usuario con ese rut.");
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Ha ocurrido un error al ejecutar la consulta SQL: " + ex.Message);
            }
        }
       
        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form4 listarForm = new Form4();
            listarForm.Show();
        }
        private string ObtenerDescuentoAFP()
        {
            if (comboBox1.SelectedIndex == 0)
            {
                return "7%";
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                return "9%";
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                return "12%";
            }
            else if (comboBox1.SelectedIndex == 3)
            {
                return "13%";
            }
            else
            {
                return string.Empty;
            }
        }

        private string ObtenerDescuentoIsapre()
        {
            if (comboBox2.SelectedIndex == 0)
            {
                return "12%";
            }
            else if (comboBox2.SelectedIndex == 1)
            {
                return "13%";
            }
            else if (comboBox2.SelectedIndex == 2)
            {
                return "14%";
            }
            else if (comboBox2.SelectedIndex == 3)
            {
                return "15%";
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
