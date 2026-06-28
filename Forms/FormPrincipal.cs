using System;
using System.Drawing;
using System.Windows.Forms;
using Estacionamento.Data;

namespace Estacionamento.Forms
{
    public class FormPrincipal : Form
    {
        private Button btnCadastrarVeiculo;
        private Button btnRegistrarEntrada;
        private Button btnRegistrarSaida;
        private Button btnListarEstacionados;
        private Button btnConsultarTicket;
        private Button btnListarVeiculos;
        private Button btnSair;
        private Label lblTitulo;
        private Label lblStatus;

        public FormPrincipal()
        {
            InicializarComponentes();
        }

        private void InicializarComponentes()
        {
            this.Text = "Sistema de Estacionamento";
            this.Size = new Size(500, 520);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            lblTitulo = new Label();
            lblTitulo.Text = "SISTEMA DE ESTACIONAMENTO";
            lblTitulo.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Height = 60;
            lblTitulo.ForeColor = Color.DarkBlue;

            int larguraBotao = 320;
            int alturaBotao = 45;
            int espacamento = 10;
            int inicioY = 80;

            btnCadastrarVeiculo = CriarBotao("1 - Cadastrar Veículo", inicioY, larguraBotao, alturaBotao);
            btnCadastrarVeiculo.Click += BtnCadastrarVeiculo_Click;

            btnRegistrarEntrada = CriarBotao("2 - Registrar Entrada", inicioY + (alturaBotao + espacamento), larguraBotao, alturaBotao);
            btnRegistrarEntrada.Click += BtnRegistrarEntrada_Click;

            btnRegistrarSaida = CriarBotao("3 - Registrar Saída + Pagamento", inicioY + 2 * (alturaBotao + espacamento), larguraBotao, alturaBotao);
            btnRegistrarSaida.Click += BtnRegistrarSaida_Click;

            btnListarEstacionados = CriarBotao("4 - Veículos Estacionados", inicioY + 3 * (alturaBotao + espacamento), larguraBotao, alturaBotao);
            btnListarEstacionados.Click += BtnListarEstacionados_Click;

            btnConsultarTicket = CriarBotao("5 - Consultar Ticket por Placa", inicioY + 4 * (alturaBotao + espacamento), larguraBotao, alturaBotao);
            btnConsultarTicket.Click += BtnConsultarTicket_Click;

            btnListarVeiculos = CriarBotao("6 - Veículos Cadastrados", inicioY + 5 * (alturaBotao + espacamento), larguraBotao, alturaBotao);
            btnListarVeiculos.Click += BtnListarVeiculos_Click;

            btnSair = CriarBotao("0 - Sair", inicioY + 6 * (alturaBotao + espacamento), larguraBotao, alturaBotao);
            btnSair.BackColor = Color.IndianRed;
            btnSair.ForeColor = Color.White;
            btnSair.Click += BtnSair_Click;

            lblStatus = new Label();
            lblStatus.Text = "Conectado ao banco de dados ✓";
            lblStatus.ForeColor = Color.Green;
            lblStatus.Dock = DockStyle.Bottom;
            lblStatus.Height = 25;
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;

            this.Controls.Add(lblTitulo);
            this.Controls.Add(btnCadastrarVeiculo);
            this.Controls.Add(btnRegistrarEntrada);
            this.Controls.Add(btnRegistrarSaida);
            this.Controls.Add(btnListarEstacionados);
            this.Controls.Add(btnConsultarTicket);
            this.Controls.Add(btnListarVeiculos);
            this.Controls.Add(btnSair);
            this.Controls.Add(lblStatus);
        }

        private Button CriarBotao(string texto, int y, int largura, int altura)
        {
            Button btn = new Button();
            btn.Text = texto;
            btn.Size = new Size(largura, altura);
            btn.Location = new Point((this.ClientSize.Width - largura) / 2, y);
            btn.Font = new Font("Segoe UI", 11);
            btn.FlatStyle = FlatStyle.Flat;
            btn.Cursor = Cursors.Hand;
            return btn;
        }

        private void BtnCadastrarVeiculo_Click(object sender, EventArgs e)
        {
            FormCadastroVeiculo form = new FormCadastroVeiculo();
            form.ShowDialog();
        }

        private void BtnRegistrarEntrada_Click(object sender, EventArgs e)
        {
            FormEntrada form = new FormEntrada();
            form.ShowDialog();
        }

        private void BtnRegistrarSaida_Click(object sender, EventArgs e)
        {
            FormSaida form = new FormSaida();
            form.ShowDialog();
        }

        private void BtnListarEstacionados_Click(object sender, EventArgs e)
        {
            FormListaEstacionados form = new FormListaEstacionados();
            form.ShowDialog();
        }

        private void BtnConsultarTicket_Click(object sender, EventArgs e)
        {
            FormConsultaTicket form = new FormConsultaTicket();
            form.ShowDialog();
        }

        private void BtnListarVeiculos_Click(object sender, EventArgs e)
        {
            FormListaVeiculos form = new FormListaVeiculos();
            form.ShowDialog();
        }

        private void BtnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}