using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MySql.Data.MySqlClient;

namespace WFDotNetCoreGravarDadosMySQL
{
    public partial class Form1 : Form
    {
        private MySqlConnection Conexao;
        private string data_source = "datasource=localhost;username=root;password=;database=db_agenda";

        public Form1()
        {
            InitializeComponent();

            lst_contatos.View = View.Details;
            lst_contatos.LabelEdit = true;
            lst_contatos.AllowColumnReorder = true;
            lst_contatos.FullRowSelect = true;
            lst_contatos.GridLines = true;

            lst_contatos.Columns.Add("ID", 30, HorizontalAlignment.Left);
            lst_contatos.Columns.Add("Nome", 150, HorizontalAlignment.Left);
            lst_contatos.Columns.Add("E-mail", 150, HorizontalAlignment.Left);
            lst_contatos.Columns.Add("Telefone", 150, HorizontalAlignment.Left);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Conexao = new MySqlConnection(data_source);
                Conexao.Open();


                MySqlCommand cmd = new MySqlCommand();

                cmd.Connection = Conexao;
               
                cmd.CommandText = "INSERT INTO contato (nome, email, telefone) " +
                                  "VALUES " +
                                  "(@nome, @email, @telefone) ";
                cmd.Prepare();
                
                cmd.Parameters.AddWithValue("@nome", txtNome.Text);
                cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@telefone", txtTelefone.Text);
                
                cmd.ExecuteNonQuery();

                MessageBox.Show("Contato Inserido com Sucesso!",
                                "Sucesso!", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);


            } catch (MySqlException ex)
            {
                MessageBox.Show("Erro " + ex.Number + " ocorreu: " + ex.Message,
                                "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu: " + ex.Message,
                                "Erro", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            } finally
            {
                Conexao.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                string q = "'%" + txt_buscar.Text + "%'";

                // Criar conexão com MySQL
                Conexao = new MySqlConnection(data_source);

                string sql = "SELECT * " +
                            "FROM contato " +
                            "WHERE nome LIKE " + q + "OR email LIKE " + q;

                Conexao.Open();

                // Executar Comando Insert
                MySqlCommand comando = new MySqlCommand(sql, Conexao);

                MySqlDataReader reader = comando.ExecuteReader();

                lst_contatos.Items.Clear();

                while (reader.Read())
                {
                    string[] row =
                    {
                        reader.GetString(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                    };

                    var linha_listview = new ListViewItem(row);

                    lst_contatos.Items.Add(linha_listview);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally
            {
                Conexao.Close();
            }
        }
    }
}
