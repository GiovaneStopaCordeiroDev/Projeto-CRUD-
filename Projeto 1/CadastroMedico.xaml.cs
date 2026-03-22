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
    /// Lógica interna para CadastroMedico.xaml
    /// </summary>
    public partial class CadastroMedico : Window
    {
        public CadastroMedico()
        {
            InitializeComponent();
            CarregarEspecialidades();
            ListarMedicos();
        }

        private void BtnAdicionarMedico_Click(object sender, RoutedEventArgs e)
        {
            string nomeMedico = txtNomeMedico.Text;
            string cpfMedico = txtCPFMedico.Text;
            string telefoneMedico = txtTelefoneMedico.Text;
            string especialidadeMedico = cbEspecialidadeMedico.Text;
            string crmMedico = txtCRMMedico.Text;
            string sexoMedico = "";

            if (string.IsNullOrWhiteSpace(nomeMedico) || string.IsNullOrWhiteSpace(cpfMedico) || string.IsNullOrWhiteSpace(telefoneMedico) || string.IsNullOrWhiteSpace(especialidadeMedico) || string.IsNullOrWhiteSpace(crmMedico))
            {
                MessageBox.Show("Preencha todos os campos para cadastrar o médico.");
                return;
            }
            if (cbMasculino.IsChecked == true)
            {
                sexoMedico = "Masculino";
            }
            else if (cbFeminino.IsChecked == true)
            {
                sexoMedico = "Feminino";
            }
            else
            {
                MessageBox.Show("Selecione o sexo do médico.");
                return;
            }

            if (cbEspecialidadeMedico.SelectedItem == null)
            {
                MessageBox.Show("Selecione uma especialidade para o médico.");
            }

            string connectionString = @"Data Source=WIN-49KV5UR3A6T; Initial Catalog=master; Integrated Security=True";
            string sql = @"INSERT INTO DadosMedico(NOME_MEDICO, CPF_MEDICO, TELEFONE_MEDICO, ESPECIALIDADE_MEDICO, CRM_MEDICO, SEXO_MEDICO) VALUES (@nomeMedico, @cpfMedico, @telefoneMedico, @especialidadeMedico, @crmMedico, @sexoMedico)";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@nomeMedico", nomeMedico);
                        cmd.Parameters.AddWithValue("@cpfMedico", cpfMedico);
                        cmd.Parameters.AddWithValue("@telefoneMedico", telefoneMedico);
                        cmd.Parameters.AddWithValue("@especialidadeMedico", especialidadeMedico);
                        cmd.Parameters.AddWithValue("@crmMedico", crmMedico);
                        cmd.Parameters.AddWithValue("@sexoMedico", sexoMedico);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Médico cadastrado com sucesso!");
                    LimparTextMedico();
                    ListarMedicos();
                }



            }

            catch (Exception ex)
            {
                MessageBox.Show("Erro" + ex);
            }
        }

        private void btnAlterarPaciente_Copiar_Click(object sender, RoutedEventArgs e)
        {
            ListarMedicos();
        }

        private int idMedicoSelecionado = 0;
        private void dgMedicos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgMedicos.SelectedItem != null)
            {
                DataRowView row = (DataRowView)dgMedicos.SelectedItem;
                idMedicoSelecionado = Convert.ToInt32(row["ID_MEDICO"]);
                txtNomeMedico.Text = row["NOME_MEDICO"].ToString();
                txtTelefoneMedico.Text = row["TELEFONE_MEDICO"].ToString();
                cbEspecialidadeMedico.SelectedValue = row["ESPECIALIDADE_MEDICO"].ToString();
                txtCPFMedico.Text = row["CPF_MEDICO"].ToString();
                txtCRMMedico.Text = row["CRM_MEDICO"].ToString();
                cbFeminino.IsChecked = row["SEXO_MEDICO"].ToString() == "Feminino";
                cbMasculino.IsChecked = row["SEXO_MEDICO"].ToString() == "Masculino";

            }
        }

        private void CarregarEspecialidades()
        {
            cbEspecialidadeMedico.Items.Clear();
            cbEspecialidadeMedico.Items.Add("Cardiologista");
            cbEspecialidadeMedico.Items.Add("Dermatologista");
            cbEspecialidadeMedico.Items.Add("Gastroenterologista");
            cbEspecialidadeMedico.Items.Add("Neurologista");
            cbEspecialidadeMedico.Items.Add("Pediatra");
            cbEspecialidadeMedico.Items.Add("Psiquiatra");
            cbEspecialidadeMedico.Items.Add("Ortopedista");
        }

        private void btnAlterarMedico_Click(object sender, RoutedEventArgs e)
        {

            string nomeMedico = txtNomeMedico.Text;
            string cpfMedico = txtCPFMedico.Text;
            string telefoneMedico = txtTelefoneMedico.Text;
            string especialidadeMedico = cbEspecialidadeMedico.Text;
            string crmMedico = txtCRMMedico.Text;
            string sexoMedico = "";

            if (string.IsNullOrWhiteSpace(nomeMedico) || string.IsNullOrWhiteSpace(cpfMedico) || string.IsNullOrWhiteSpace(telefoneMedico) || string.IsNullOrWhiteSpace(especialidadeMedico) || string.IsNullOrWhiteSpace(crmMedico))
            {
                MessageBox.Show("Preencha todos os campos para alterar o médico.");
                return;
            }
            if (cbMasculino.IsChecked == true)
            {
                sexoMedico = "Masculino";
            }
            else if (cbFeminino.IsChecked == true)
            {
                sexoMedico = "Feminino";
            }
            else
            {
                MessageBox.Show("Selecione o sexo do médico.");
                return;
            }

            if (cbEspecialidadeMedico.SelectedItem == null)
            {
                MessageBox.Show("Selecione uma especialidade para o médico.");
            }


            string connectionstring = @"Data Source=WIN-49KV5UR3A6T; Initial Catalog=master; Integrated Security=True";
            string sql = @"UPDATE DadosMedico
                            SET NOME_MEDICO = @nomeMedico,
                                CPF_MEDICO = @cpfMedico,
                                TELEFONE_MEDICO = @telefoneMedico,
                                ESPECIALIDADE_MEDICO = @especialidadeMedico,
                                CRM_MEDICO = @crmMedico,
                                SEXO_MEDICO = @sexoMedico
                                WHERE ID_MEDICO = @idMedico";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionstring))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        con.Open();
                        cmd.Parameters.AddWithValue("@idMedico", idMedicoSelecionado);
                        cmd.Parameters.AddWithValue("@nomeMedico", txtNomeMedico.Text);
                        cmd.Parameters.AddWithValue("@cpfMedico", txtCPFMedico.Text);
                        cmd.Parameters.AddWithValue("@telefoneMedico", txtTelefoneMedico.Text);
                        cmd.Parameters.AddWithValue("@especialidadeMedico", cbEspecialidadeMedico.Text);
                        cmd.Parameters.AddWithValue("@crmMedico", txtCRMMedico.Text);
                        cmd.Parameters.AddWithValue("@sexoMedico", cbMasculino.IsChecked == true ? "Masculino" : "Feminino");
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Médico alterado com sucesso!");
                    ListarMedicos();
                    LimparTextMedico();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnRemoverMedico_Click(object sender, RoutedEventArgs e)
        {
            string nomeMedico = txtNomeMedico.Text;
            string cpfMedico = txtCPFMedico.Text;
            string telefoneMedico = txtTelefoneMedico.Text;
            string especialidadeMedico = cbEspecialidadeMedico.Text;
            string crmMedico = txtCRMMedico.Text;
            string sexoMedico = "";

            if (string.IsNullOrWhiteSpace(nomeMedico) || string.IsNullOrWhiteSpace(cpfMedico) || string.IsNullOrWhiteSpace(telefoneMedico) || string.IsNullOrWhiteSpace(especialidadeMedico) || string.IsNullOrWhiteSpace(crmMedico))
            {
                MessageBox.Show("Preencha todos os campos para remover o médico.");
                return;
            }
            if (cbMasculino.IsChecked == true)
            {
                sexoMedico = "Masculino";
            }
            else if (cbFeminino.IsChecked == true)
            {
                sexoMedico = "Feminino";
            }
            else
            {
                MessageBox.Show("Selecione o sexo do médico.");
                return;
            }

            if (cbEspecialidadeMedico.SelectedItem == null)
            {
                MessageBox.Show("Selecione uma especialidade para o médico.");
            }

            string connectionstring = @"Data Source=WIN-49KV5UR3A6T; Initial Catalog=master; Integrated Security=True";
            string sql = @"DELETE FROM DadosMedico
                           WHERE ID_MEDICO = @idMedico";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionstring))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        con.Open();
                        cmd.Parameters.AddWithValue("@idMedico", idMedicoSelecionado);
                        cmd.ExecuteNonQuery();
                    }

                }
                MessageBox.Show("Médico removido com sucesso!");
                ListarMedicos();
                LimparTextMedico();
            }
            catch (Exception ec)
            {
                MessageBox.Show(ec.ToString());
            }
        }

        private void LimparTextMedico()
        {
            txtNomeMedico.Clear();
            txtCPFMedico.Clear();
            txtTelefoneMedico.Clear();
            cbEspecialidadeMedico.SelectedIndex = -1;
            txtCRMMedico.Clear();
            cbMasculino.IsChecked = false;
            cbFeminino.IsChecked = false;
        }

        private void PesquisarMedico()
        {
            if (dgMedicos == null || txtPesquisarMedico == null)
                return;

            string nomePesquisa = txtPesquisarMedico.Text;

            if (string.IsNullOrWhiteSpace(nomePesquisa))
            {
                ListarMedicos();
                return;
            }

            string conexaostring = @"Data Source=WIN-49KV5UR3A6T;Initial Catalog=master;Integrated Security=True";
            string sql = @"SELECT ID_MEDICO, NOME_MEDICO, CPF_MEDICO, TELEFONE_MEDICO, ESPECIALIDADE_MEDICO, CRM_MEDICO, SEXO_MEDICO FROM DadosMedico WHERE NOME_MEDICO LIKE @nomeMedico";

            try
            {
                using (SqlConnection con = new SqlConnection(conexaostring))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@nomeMedico", "%" + nomePesquisa + "%");
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            con.Open();
                            da.Fill(dt);
                            dgMedicos.ItemsSource = dt.DefaultView;
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Erro ao pesquisar médicos. Verifique a conexão com o banco de dados.");
            }

        }

        private void txtPesquisarMedico_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!IsLoaded)
            {
                return;
            }
            PesquisarMedico();
        }
        private void ListarMedicos()
        {

            if (dgMedicos == null)
            {
                return;
            }

            string conexaostring = @"Data Source=WIN-49KV5UR3A6T;Initial Catalog=master;Integrated Security=True";
            string sql = @"SELECT ID_MEDICO, NOME_MEDICO, CPF_MEDICO, TELEFONE_MEDICO, ESPECIALIDADE_MEDICO, CRM_MEDICO, SEXO_MEDICO FROM DadosMedico";

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

                            dgMedicos.ItemsSource = dt.DefaultView;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao listar médicos. Verifique a conexão com o banco de dados." + ex);
            }
        }

        private void txtCPFMedico_TextChanged(object sender, TextChangedEventArgs e)
        {

            string texto = txtCPFMedico.Text.Replace(".", "").Replace("-", "");
            if (txtCPFMedico.Text.Length >= 3)
            {
                texto = texto.Insert(3, ".");
            }
            if (txtCPFMedico.Text.Length >= 7)
            {
                texto = texto.Insert(7, ".");
            }
            if (txtCPFMedico.Text.Length >= 11)
            {
                texto = texto.Insert(11, "-");
            }

            if (txtCPFMedico.Text.Length > 14)
            {
                texto = texto.Substring(0, 14);
            }

            txtCPFMedico.Text = texto;  
            txtCPFMedico.SelectionStart = txtCPFMedico.Text.Length;

        }

        private void txtTelefoneMedico_TextChanged(object sender, TextChangedEventArgs e)
        {
            string telefone = txtTelefoneMedico.Text.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
            if (txtTelefoneMedico.Text.Length >= 0)
            {
                telefone = telefone.Insert(0, "(");
            }
            if (txtTelefoneMedico.Text.Length >= 3)
            {
                telefone = telefone.Insert(3, ") ");
            }
            if (txtTelefoneMedico.Text.Length >= 4)
            {
                telefone = telefone.Insert(4, " ");
            }
            if (txtTelefoneMedico.Text.Length >= 11)
            {
                telefone = telefone.Insert(11, "-");
            }
            if (txtTelefoneMedico.Text.Length >= 16)
            {
                telefone = telefone.Substring(0, 16);
            }
            txtTelefoneMedico.Text = telefone;
            txtTelefoneMedico.SelectionStart = txtTelefoneMedico.Text.Length;

        }
    }
}
