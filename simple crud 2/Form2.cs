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
    public partial class Form2 : Form
    {

        static bdManager manager = new bdManager();
        static int acessLevel;

        public Form2(int lvl)
        {
            InitializeComponent();
            acessLevel = lvl;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (txtName.Text != "" && txtAge.Text != "" && txtEmail.Text != "")
            {
                AddNewFunc(txtName.Text, Convert.ToInt32(txtAge.Text), txtEmail.Text);
            } else
            {
                MessageBox.Show("Preencha todos os campos para efetuar esta ação");
            }
        }

        private void BtnVisualizar_Click(object sender, EventArgs e)
        {
            dataGrid.DataSource = AllRegisters();
        }

        public void AddNewFunc(string name, int age, string email)
        {
            manager.cmdString = String.Format("INSERT INTO funcionarios(name, idade, email) VALUES ('{0}',{1},'{2}')",
                                                name, age, email);

            using (manager.conn = new Npgsql.NpgsqlConnection(manager.connString))
            {
                manager.conn.Open();

                using (manager.cmd = new Npgsql.NpgsqlCommand(manager.cmdString, manager.conn))
                {
                    manager.cmd.ExecuteNonQuery();
                    MessageBox.Show("Inserção realizada");
                }
            }
        }

        public DataTable AllRegisters()
        {
            DataTable dt = new DataTable();

            manager.cmdString = "SELECT * FROM funcionarios";

            using (manager.conn = new Npgsql.NpgsqlConnection(manager.connString))
            {
                manager.conn.Open();

                using (Npgsql.NpgsqlDataAdapter data = new Npgsql.NpgsqlDataAdapter(manager.cmdString, manager.conn))
                {
                    data.Fill(dt);
                }
            }
            return dt;
        }

        private void BtnDeletar_Click(object sender, EventArgs e)
        {
            if (acessLevel < 2)
            {
                MessageBox.Show("Você não possui nível de acesso para executar esta ação");
            } else {
                if (dataGrid.SelectedRows.Count > 0)
                {
                    DeleteRegister();
                } else
                {
                    MessageBox.Show("Selecione uma Row para deletar");
                }
            }
        }

        private void DeleteRegister()
        {
            int id = Convert.ToInt32(dataGrid.SelectedRows[0].Cells["id"].Value.ToString());
            manager.cmdString = String.Format("DELETE FROM funcionarios WHERE id = '{0}'", id);

            using (manager.conn = new Npgsql.NpgsqlConnection(manager.connString))
            {
                manager.conn.Open();

                using (manager.cmd = new Npgsql.NpgsqlCommand(manager.cmdString, manager.conn))
                {
                    int deletedRows = manager.cmd.ExecuteNonQuery();
                    if (deletedRows > 0)
                    {
                        MessageBox.Show("Row Deletada");
                    }
                    else
                    {
                        MessageBox.Show("Row não selecionada ou não encontrada");
                    }
                }
            }
        }

        private void UpdateRegister(int id, string nome, int idade, string email)
        {
            manager.cmdString = String.Format("UPDATE funcionarios SET (name, idade, email) = ('{0}', {1}, '{2}') WHERE id = {3}",
                                                nome, idade, email, id);

            using (manager.conn = new Npgsql.NpgsqlConnection(manager.connString))
            {
                manager.conn.Open();

                using (manager.cmd = new Npgsql.NpgsqlCommand(manager.cmdString, manager.conn))
                {
                    manager.cmd.ExecuteNonQuery();
                    MessageBox.Show("Alteração executada");
                }
            }

        }

        private void BtnAtualizar_Click(object sender, EventArgs e)
        {
            if (dataGrid.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dataGrid.SelectedRows[0].Cells["id"].Value.ToString());
                string name = txtName.Text;
                string email = txtEmail.Text;
                int idade = Convert.ToInt32(txtAge.Text);
                UpdateRegister(id, name, idade, email);
            } else
            {
                MessageBox.Show("Selecione uma linha para fazer a alteração");
            }
        }
    }
}
