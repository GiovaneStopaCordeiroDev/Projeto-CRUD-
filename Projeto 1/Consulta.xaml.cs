using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Data;

namespace Projeto_1
{
    /// <summary>
    /// Lógica interna para Consulta.xaml
    /// </summary>
    public partial class Consulta : Window
    {
        public Consulta()
        {
            InitializeComponent();
            ListarMedicos();
            ListarPacientes();
            ListarConsultas();
        }
        private void ListarConsultas()
        {
            string connectionstring = @"Data Source=WIN-49KV5UR3A6T;Initial Catalog=master;Integrated Security=True";
            string sql = @"SELECT C.ID_CONSULTA, C.ID_MEDICO, C.ID_PACIENTE, P.NOME_PACIENTE, M.NOME_MEDICO, C.SINTOMAS, C.MOTIVO, C.DATA_CONSULTA
                            FROM DadosConsultas C
                            INNER JOIN DadosPaciente P ON C.ID_PACIENTE = P.ID_PACIENTE
                            INNER JOIN DadosMedico M ON C.ID_MEDICO = M.ID_MEDICO";
            try
            {
                using (SqlConnection con = new SqlConnection(connectionstring))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            con.Open();
                            da.Fill(dt);
                            dgConsultas.ItemsSource = dt.DefaultView;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao listar consultas. Verifique a conexão com o banco de dados." + ex);
            }
        }



        private void ListarPacientes()
        {

            string connectionstring = @"Data Source=WIN-49KV5UR3A6T;Initial Catalog=master;Integrated Security=True";
            string sql = @"SELECT ID_PACIENTE, NOME_PACIENTE FROM DadosPaciente";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionstring))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            con.Open();
                            da.Fill(dt);
                            cbPacienteConsulta.ItemsSource = dt.DefaultView;
                            cbPacienteConsulta.DisplayMemberPath = "NOME_PACIENTE";
                            cbPacienteConsulta.SelectedValuePath = "ID_PACIENTE";
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao listar pacientes. Verifique a conexão com o banco de dados." + ex);
            }
        }

        private void ListarMedicos()
        {
            string connectionstring = @"Data Source=WIN-49KV5UR3A6T;Initial Catalog=master;Integrated Security=True";
            string sql = @"SELECT ID_MEDICO, NOME_MEDICO FROM DadosMedico";
            try
            {
                using (SqlConnection con = new SqlConnection(connectionstring))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            con.Open();
                            da.Fill(dt);
                            cbMedicoConsulta.ItemsSource = dt.DefaultView;
                            cbMedicoConsulta.DisplayMemberPath = "NOME_MEDICO";
                            cbMedicoConsulta.SelectedValuePath = "ID_MEDICO";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao listar médicos. Verifique a conexão com o banco de dados." + ex);
            }
        }

        private void btnAdicionarConsultas_Click(object sender, RoutedEventArgs e)
        {

            if (cbPacienteConsulta.SelectedValue == null || cbMedicoConsulta.SelectedValue == null)
            {
                MessageBox.Show("Selecione um paciente e um médico para cadastrar a consulta.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtSintomas.Text))
            {
                MessageBox.Show("Preencha os sintomas do paciente");
                return;
            }

            if (string.IsNullOrEmpty(txtMotivo.Text))
            {
                MessageBox.Show("Preencha o motivo da consulta");
                return;
            }

            if (dpDataConsulta.SelectedDate == null)
            {
                MessageBox.Show("Selecione a data da consulta");
                return;
            }

            int idPaciente = Convert.ToInt32(cbPacienteConsulta.SelectedValue);
            int idMedico = Convert.ToInt32(cbMedicoConsulta.SelectedValue);
            string consultaSintomas = txtSintomas.Text;
            string consultaMotivo = txtMotivo.Text;
            DateTime dataConsulta = dpDataConsulta.SelectedDate.Value;

            string connectionstring = @"Data Source=WIN-49KV5UR3A6T;Initial Catalog=master;Integrated Security=True";
            string sql = @"INSERT INTO DadosConsultas (ID_PACIENTE, ID_MEDICO, SINTOMAS, MOTIVO, DATA_CONSULTA) VALUES (@ID_PACIENTE, @ID_MEDICO, @SINTOMAS, @MOTIVO, @DATA_CONSULTA)";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionstring))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@ID_PACIENTE", idPaciente);
                        cmd.Parameters.AddWithValue("@ID_MEDICO", idMedico);
                        cmd.Parameters.AddWithValue("@SINTOMAS", consultaSintomas);
                        cmd.Parameters.AddWithValue("@MOTIVO", consultaMotivo);
                        cmd.Parameters.AddWithValue("@DATA_CONSULTA", dataConsulta);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Consulta cadastrada com sucesso!");
                        ListarConsultas();
                        LimparCampos();
                    }
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show("Erro encontado " + ex.Message);
            }
        }

        private int idConsultaSelecionada = 0;
        private void dgConsultas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgConsultas.SelectedItem != null) //Seleciona o item que não estiver nulo
            {


                DataRowView linha = (DataRowView)dgConsultas.SelectedItem; // Transforma o Item selecionado genérico em uma linha da tabela(DataRowView)
                idConsultaSelecionada = Convert.ToInt32(linha["ID_CONSULTA"]);
                cbMedicoConsulta.SelectedValue = Convert.ToInt32(linha["ID_MEDICO"]);
                cbPacienteConsulta.SelectedValue = Convert.ToInt32(linha["ID_PACIENTE"]);
                txtSintomas.Text = linha["SINTOMAS"].ToString();
                txtMotivo.Text = linha["MOTIVO"].ToString();
                dpDataConsulta.SelectedDate = Convert.ToDateTime(linha["DATA_CONSULTA"]);
            }
        }

        private void btnAlterarConsultas_Click(object sender, RoutedEventArgs e)
        {
            if (cbPacienteConsulta.SelectedValue == null || cbMedicoConsulta.SelectedValue == null)
            {
                MessageBox.Show("Selecione um paciente e um médico para alterar a consulta.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtSintomas.Text))
            {
                MessageBox.Show("Preencha os sintomas do paciente");
                return;
            }

            if (string.IsNullOrEmpty(txtMotivo.Text))
            {
                MessageBox.Show("Preencha o motivo da consulta");
                return;
            }

            if (dpDataConsulta.SelectedDate == null)
            {
                MessageBox.Show("Selecione a data da consulta");
                return;
            }
            string connectionstring = @"Data Source=WIN-49KV5UR3A6T;Initial Catalog=master;Integrated Security=True";
            string sql = @"UPDATE DadosConsultas
                            SET ID_PACIENTE = @ID_PACIENTE,
                                ID_MEDICO = @ID_MEDICO,
                                SINTOMAS = @SINTOMAS,
                                DATA_CONSULTA = @DATA_CONSULTA,
                                MOTIVO = @MOTIVOS
                                WHERE ID_CONSULTA = @ID_CONSULTA";
            try
            {
                using (SqlConnection con = new SqlConnection(connectionstring))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        con.Open();
                        cmd.Parameters.AddWithValue("@ID_CONSULTA", idConsultaSelecionada);
                        cmd.Parameters.AddWithValue("@ID_PACIENTE", Convert.ToInt32(cbPacienteConsulta.SelectedValue));
                        cmd.Parameters.AddWithValue("@ID_MEDICO", Convert.ToInt32(cbMedicoConsulta.SelectedValue));
                        cmd.Parameters.AddWithValue("@SINTOMAS", txtSintomas.Text);
                        cmd.Parameters.AddWithValue("@MOTIVOS", txtMotivo.Text);
                        cmd.Parameters.AddWithValue("@DATA_CONSULTA", dpDataConsulta.SelectedDate.Value);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Consulta alterada com sucesso!");
                        ListarConsultas();
                        LimparCampos();
                    }
                }
                ;
            }
            catch (Exception ex)
            {
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
        private void LimparCampos()
        {
            cbPacienteConsulta.SelectedValue = -1;
            cbMedicoConsulta.SelectedValue = -1;
            txtSintomas.Clear();
            txtMotivo.Clear();
            dpDataConsulta.SelectedDate = null;
        }

        private void btnRemoverConsultas_Click(object sender, RoutedEventArgs e)
        {
            if (cbPacienteConsulta.SelectedValue == null || cbMedicoConsulta.SelectedValue == null)
            {
                MessageBox.Show("Selecione um paciente e um médico para remover a consulta.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtSintomas.Text))
            {
                MessageBox.Show("Preencha os sintomas do paciente");
                return;
            }

            if (string.IsNullOrEmpty(txtMotivo.Text))
            {
                MessageBox.Show("Preencha o motivo da consulta");
                return;
            }

            if (dpDataConsulta.SelectedDate == null)
            {
                MessageBox.Show("Selecione a data da consulta");
                return;
            }

            string connectionstring = @"Data Source=WIN-49KV5UR3A6T;Initial Catalog=master;Integrated Security=True";
            string sql = @"DELETE FROM DadosConsultas 
                           WHERE ID_CONSULTA = @ID_CONSULTA";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionstring))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        con.Open();
                        cmd.Parameters.AddWithValue("@ID_CONSULTA", idConsultaSelecionada);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Consulta removida com sucesso!");
                        ListarConsultas();
                        LimparCampos();
                    }
                }
            }
            catch (Exception ex)
            {
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void PesquisarConsulta()
        {
            if (dgConsultas == null)
            {
                return;
            }

            string consultaPesquisa = txtPesquisaConsulta.Text;
            string connectionstring = @"Data Source=WIN-49KV5UR3A6T;Initial Catalog=master;Integrated Security=True";
            string sql = @"SELECT C.ID_CONSULTA, C.ID_MEDICO, C.ID_PACIENTE, P.NOME_PACIENTE, M.NOME_MEDICO, C.SINTOMAS, C.MOTIVO, C.DATA_CONSULTA
                            FROM DadosConsultas C
                            INNER JOIN DadosPaciente P ON C.ID_PACIENTE = P.ID_PACIENTE
                            INNER JOIN DadosMedico M ON C.ID_MEDICO = M.ID_MEDICO
                            WHERE P.NOME_PACIENTE LIKE @pesquisa OR M.NOME_MEDICO LIKE @pesquisa";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionstring))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            cmd.Parameters.AddWithValue("@pesquisa", "%" + consultaPesquisa + "%");
                            da.Fill(dt);
                            dgConsultas.ItemsSource = dt.DefaultView;
                        }

                    }
                }
            }

            catch (Exception ex)
            {
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
        private void txtPesquisaConsulta_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (!IsLoaded)
            {
                return;
            }

            PesquisarConsulta();
        }
    }
}
