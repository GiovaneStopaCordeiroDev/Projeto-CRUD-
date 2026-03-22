using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projeto_1
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnCadastroPaciente_Click(object sender, RoutedEventArgs e)
        {
            CadastroPacientes cadastroPacientes = new CadastroPacientes();
            cadastroPacientes.ShowDialog();
        }

        private void btnCadastroMedico_Click(object sender, RoutedEventArgs e)
        {
            CadastroMedico cadastroMedico = new CadastroMedico();
            cadastroMedico.ShowDialog();
        }

        private void btnCadastroConsulta_Click(object sender, RoutedEventArgs e)
        {
            Consulta consulta = new Consulta();
            consulta.ShowDialog();
        }

      
    }
}
