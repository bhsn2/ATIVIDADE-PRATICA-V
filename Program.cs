//Bruno Henrique | Maverson Souza – 6º Período
using System;
using System.Windows.Forms;
using Estacionamento.Data;
using Estacionamento.Forms;

namespace Estacionamento
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!Conexao.Testar())
            {
                MessageBox.Show("Não foi possível conectar ao banco de dados.\nVerifique se o SQL Server está ativo.",
                    "Erro de Conexão", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Application.Run(new FormPrincipal());
        }
    }
}