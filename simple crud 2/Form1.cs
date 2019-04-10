using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace simple_crud_2
{
    public partial class Form1 : Form
    {
        static bdManager manager = new bdManager();

        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            bool result;
            manager.cmdString = "SELECT * FROM usuarios WHERE login = '" + txtLogin.Text + "' and password = '" + txtPassword.Text + "'";

            try
            {
                using (manager.conn = new Npgsql.NpgsqlConnection(manager.connString))
                {
                    manager.conn.Open();

                    using (manager.cmd)
                    {
                        manager.cmd = new Npgsql.NpgsqlCommand(manager.cmdString, manager.conn);
                        using (var reader = manager.cmd.ExecuteReader())
                        {
                            result = reader.HasRows;
                            if (!result)
                            {
                                MessageBox.Show("Login ou senha incorreto");
                            }
                            else
                            {
                                while (reader.Read())
                                {
                                    int acess = Convert.ToInt32(reader.GetValue(3));
                                    this.Hide();
                                    Form2 form2 = new Form2(acess);
                                    form2.Show();
                                }
                            }
                            manager.conn.Close();
                        }
                    }
                }
            }
            catch (ObjectDisposedException ex)
            {
                throw ex;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
