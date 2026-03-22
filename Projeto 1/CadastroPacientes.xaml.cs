using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace Projeto_1
{
    /// <summary>
    /// Lógica interna para CadastroPacientes.xaml
    /// </summary>
    public partial class CadastroPacientes : Window
    {
        public CadastroPacientes()
        {
            InitializeComponent();
            ListarPacientes();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void LimparCampos()
        {
            txtNomePaciente.Clear();
            txtCPFPaciente.Clear();
            txtTelefonePaciente.Clear();
            txtEmailPaciente.Clear();
            dpDataNascimentoPaciente.SelectedDate = null;
            cbMasculino.IsChecked = false;
            cbFeminino.IsChecked = false;
        }

        public void ListarPacientes()
        {
            string conexaostring = @"Data Source=WIN-49KV5UR3A6T;Initial Catalog=master;Integrated Security=True";

            string sql = @"SELECT ID_PACIENTE, NOME_PACIENTE, CPF_PACIENTE, TELEFONE_PACIENTE, EMAIL_PACIENTE, DATANASCIMENTO_PACIENTE, SEXO_PACIENTE FROM DadosPaciente";
            try
            {
                using (SqlConnection con = new SqlConnection(conexaostring))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            con.Open();
                            da.Fill(dt);

                            dgPacientes.ItemsSource = dt.DefaultView;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao listar pacientes. Verifique a conexão com o banco de dados." + ex);
            }
        }

        private void btnAdicionarPaciente_Click(object sender, RoutedEventArgs e)
        {
            string Nome = txtNomePaciente.Text;
            string CPF = txtCPFPaciente.Text;
            string Telefone = txtTelefonePaciente.Text;
            string Email = txtEmailPaciente.Text;
            string Sexo = "";
            DateTime dataNascimento = dpDataNascimentoPaciente.SelectedDate.Value;

            if (cbMasculino.IsChecked == true && cbFeminino.IsChecked == true)
            {
                MessageBox.Show("Selecione apenas um gênero.");
                return;
            }
            else
            {
                Sexo = cbMasculino.IsChecked == true ? "Masculino" : (cbFeminino.IsChecked == true ? "Feminino" : "");
            }

            if (Nome == "" || CPF == "" || Telefone == "" || Email == "")
            {
                MessageBox.Show("Preencha todos os campos corretamente.");
                return;
            }

            if (dpDataNascimentoPaciente.SelectedDate == null)
            {
                MessageBox.Show("Selecione uma data de nascimento válida.");
                return;
            }

            string conexaoString = @"Data Source=WIN-49KV5UR3A6T;Initial Catalog=master;Integrated Security=True";
            string sql = @"INSERT INTO DadosPaciente (NOME_PACIENTE, CPF_PACIENTE, TELEFONE_PACIENTE, EMAIL_PACIENTE, DATANASCIMENTO_PACIENTE, SEXO_PACIENTE) VALUES (@Nome, @CPF, @Telefone, @Email, @dataNascimento, @Sexo)";

            try
            {
                using (SqlConnection con = new SqlConnection(conexaoString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Nome", Nome);
                        cmd.Parameters.AddWithValue("@CPF", CPF);
                        cmd.Parameters.AddWithValue("@Telefone", Telefone);
                        cmd.Parameters.AddWithValue("@Email", Email);
                        cmd.Parameters.AddWithValue("@Sexo", Sexo);
                        cmd.Parameters.AddWithValue("@DataNascimento", dataNascimento);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Paciente adicionado com sucesso!");
                    }
                    LimparCampos();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao adicionar paciente. Verifique os dados e tente novamente." + ex);
            }
        }

        private int idPacienteSelecionado = 0;
        private void dgPacientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgPacientes.SelectedItem != null)
            {
                DataRowView row = (DataRowView)dgPacientes.SelectedItem;
                idPacienteSelecionado = Convert.ToInt32(row["ID_PACIENTE"]);
                txtNomePaciente.Text = row["NOME_PACIENTE"].ToString();
                txtCPFPaciente.Text = row["CPF_PACIENTE"].ToString();
                txtEmailPaciente.Text = row["EMAIL_PACIENTE"].ToString();
                txtTelefonePaciente.Text = row["TELEFONE_PACIENTE"].ToString();
                cbFeminino.IsChecked = row["SEXO_PACIENTE"].ToString() == "Feminino";
                cbMasculino.IsChecked = row["SEXO_PACIENTE"].ToString() == "Masculino";
                dpDataNascimentoPaciente.SelectedDate = Convert.ToDateTime(row["DATANASCIMENTO_PACIENTE"]);
            }
        }

        private void btnAlterarPaciente_Click(object sender, RoutedEventArgs e)
        {
            string Nome = txtNomePaciente.Text;
            string CPF = txtCPFPaciente.Text;
            string Telefone = txtTelefonePaciente.Text;
            string Email = txtEmailPaciente.Text;
            string Sexo = "";
            DateTime dataNascimento = dpDataNascimentoPaciente.SelectedDate.Value;

            if (cbMasculino.IsChecked == true && cbFeminino.IsChecked == true)
            {
                MessageBox.Show("Selecione apenas um gênero.");
                return;
            }
            else
            {
                Sexo = cbMasculino.IsChecked == true ? "Masculino" : (cbFeminino.IsChecked == true ? "Feminino" : "");
            }

            if (Nome == "" || CPF == "" || Telefone == "" || Email == "")
            {
                MessageBox.Show("Preencha todos os campos corretamente.");
                return;
            }

            if (dpDataNascimentoPaciente.SelectedDate == null)
            {
                MessageBox.Show("Selecione uma data de nascimento válida.");
                return;
            }

            string conectionString = @"Data Source=WIN-49KV5UR3A6T;Initial Catalog=master;Integrated Security=True";
            string sql = @"UPDATE DadosPaciente
                            SET NOME_PACIENTE = @Nome,  
                            CPF_PACIENTE = @CPF,
                            TELEFONE_PACIENTE = @Telefone,
                            EMAIL_PACIENTE = @Email,
                            DATANASCIMENTO_PACIENTE = @dataNascimento,
                            SEXO_PACIENTE = @Sexo
                            WHERE ID_PACIENTE = @ID_PACIENTE";
            try
            {
                using (SqlConnection con = new SqlConnection(conectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@ID_PACIENTE", idPacienteSelecionado);
                        cmd.Parameters.AddWithValue("@Nome", txtNomePaciente.Text);
                        cmd.Parameters.AddWithValue("@CPF", txtCPFPaciente.Text);
                        cmd.Parameters.AddWithValue("@Telefone", txtTelefonePaciente.Text);
                        cmd.Parameters.AddWithValue("@Email", txtEmailPaciente.Text);
                        cmd.Parameters.AddWithValue("@dataNascimento", dpDataNascimentoPaciente.SelectedDate.Value);
                        cmd.Parameters.AddWithValue("@Sexo", cbMasculino.IsChecked == true ? "Masculino" : (cbFeminino.IsChecked == true ? "Feminino" : ""));
                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Paciente alterado com sucesso!");
                        ListarPacientes();
                    }

                }
            }
            catch (Exception ex)
            {
                {
                    MessageBox.Show("Erro ao alterar paciente. Verifique os dados e tente novamente." + ex);
                }

            }


        }

        private void btnRemoverPaciente_Click(object sender, RoutedEventArgs e)
        {

            string Nome = txtNomePaciente.Text;
            string CPF = txtCPFPaciente.Text;
            string Telefone = txtTelefonePaciente.Text;
            string Email = txtEmailPaciente.Text;
            string Sexo = "";
            DateTime dataNascimento = dpDataNascimentoPaciente.SelectedDate.Value;
            if (cbMasculino.IsChecked == true && cbFeminino.IsChecked == true)
            {
                MessageBox.Show("Selecione apenas um gênero.");
                return;
            }
            else
            {
                Sexo = cbMasculino.IsChecked == true ? "Masculino" : (cbFeminino.IsChecked == true ? "Feminino" : "");
            }

            if (Nome == "" || CPF == "" || Telefone == "" || Email == "")
            {
                MessageBox.Show("Preencha todos os campos corretamente.");
                return;
            }

            if (dpDataNascimentoPaciente.SelectedDate == null)
            {
                MessageBox.Show("Selecione uma data de nascimento válida.");
                return;
            }

            string conectionString = @"Data Source=WIN-49KV5UR3A6T;Initial Catalog=master;Integrated Security=True";
            string sql = @"DELETE FROM DadosPaciente WHERE ID_PACIENTE = @ID_PACIENTE";

            try
            {
                using (SqlConnection con = new SqlConnection(conectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@ID_PACIENTE", idPacienteSelecionado);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Paciente removido com sucesso!");
                        ListarPacientes();
                    }
                }
            }
            catch (Exception ex)
            {
                {
                    MessageBox.Show("Erro ao remover paciente. Verifique os dados e tente novamente." + ex);
                }
            }
        }

        private void txtPesquisarPaciente_TextChanged(object sender, TextChangedEventArgs e)
        {


            string connectionstring = @"Data Source=WIN-49KV5UR3A6T;Initial Catalog=master;Integrated Security=True";

            string sql = @"SELECT ID_PACIENTE, NOME_PACIENTE, CPF_PACIENTE, TELEFONE_PACIENTE, EMAIL_PACIENTE, DATANASCIMENTO_PACIENTE, SEXO_PACIENTE FROM DadosPaciente WHERE NOME_PACIENTE LIKE @Nome";
            
            try
            {
                using (SqlConnection con = new SqlConnection(connectionstring))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd)) 
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            cmd.Parameters.AddWithValue("@Nome", "%" + txtPesquisarPaciente.Text + "%");
                            dgPacientes.ItemsSource = dt.DefaultView;
                        }
                    }
                }
            }
            catch
            {

            }

        }

        private void txtCPFPaciente_TextChanged(object sender, TextChangedEventArgs e)
        {
            string texto = txtCPFPaciente.Text.Replace(".", "").Replace("-", "");
            if (txtCPFPaciente.Text.Length >= 3)
            {
                texto = texto.Insert(3, ".");
            }
            if (txtCPFPaciente.Text.Length >= 7)
            {
                texto = texto.Insert(7, ".");
            }
            if (txtCPFPaciente.Text.Length >= 11)
            {
                texto = texto.Insert(11, "-");
            }

            if (txtCPFPaciente.Text.Length > 14)
            {
                texto = texto.Substring(0, 14);
            }

            txtCPFPaciente.Text = texto;
            txtCPFPaciente.SelectionStart = txtCPFPaciente.Text.Length;
        }

        private void txtTelefonePaciente_TextChanged(object sender, TextChangedEventArgs e)
        {
            string telefone = txtTelefonePaciente.Text.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
            if (txtTelefonePaciente.Text.Length >= 0)
            {
                telefone = telefone.Insert(0, "(");
            }
            if (txtTelefonePaciente.Text.Length >= 3)
            {
                telefone = telefone.Insert(3, ") ");
            }
            if (txtTelefonePaciente.Text.Length >= 4)
            {
                telefone = telefone.Insert(4, " ");
            }
            if (txtTelefonePaciente.Text.Length >= 11)
            {
                telefone = telefone.Insert(11, "-");
            }
            if (txtTelefonePaciente.Text.Length >= 16)
            {
                telefone = telefone.Substring(0, 16);
            }
            txtTelefonePaciente.Text = telefone;
            txtTelefonePaciente.SelectionStart = txtTelefonePaciente.Text.Length;
        }
    }
}

