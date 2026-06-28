using System;
using System.Drawing;
using System.Windows.Forms;
using Estacionamento.Models;
using Estacionamento.Repositories;

namespace Estacionamento.Forms
{
    public class FormConsultaTicket : Form
    {
        private Label lblTitulo;
        private Label lblPlaca;
        private TextBox txtPlaca;
        private Button btnBuscar;
        private GroupBox grpResultado;
        private Label lblResultado;
        private Button btnFechar;

        private TicketRepository ticketRepo = new TicketRepository();

        public FormConsultaTicket()
        {
            InicializarComponentes();
        }

        private void InicializarComponentes()
        {
            this.Text = "Consultar Ticket";
            this.Size = new Size(450, 350);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            lblTitulo = new Label();
            lblTitulo.Text = "CONSULTAR TICKET";
            lblTitulo.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTitulo.Location = new Point(20, 15);
            lblTitulo.AutoSize = true;

            lblPlaca = new Label();
            lblPlaca.Text = "Placa:";
            lblPlaca.Location = new Point(20, 60);
            lblPlaca.AutoSize = true;

            txtPlaca = new TextBox();
            txtPlaca.Location = new Point(100, 57);
            txtPlaca.Size = new Size(200, 25);
            txtPlaca.CharacterCasing = CharacterCasing.Upper;

            btnBuscar = new Button();
            btnBuscar.Text = "Buscar";
            btnBuscar.Location = new Point(310, 55);
            btnBuscar.Size = new Size(100, 28);
            btnBuscar.Click += BtnBuscar_Click;

            grpResultado = new GroupBox();
            grpResultado.Text = "Resultado";
            grpResultado.Location = new Point(20, 100);
            grpResultado.Size = new Size(390, 160);
            grpResultado.Visible = false;

            lblResultado = new Label();
            lblResultado.Location = new Point(15, 25);
            lblResultado.Size = new Size(360, 120);
            lblResultado.Font = new Font("Segoe UI", 10);
            grpResultado.Controls.Add(lblResultado);

            btnFechar = new Button();
            btnFechar.Text = "Fechar";
            btnFechar.Size = new Size(100, 35);
            btnFechar.Location = new Point(310, 270);
            btnFechar.Click += (s, e) => this.Close();

            this.Controls.Add(lblTitulo);
            this.Controls.Add(lblPlaca);
            this.Controls.Add(txtPlaca);
            this.Controls.Add(btnBuscar);
            this.Controls.Add(grpResultado);
            this.Controls.Add(btnFechar);
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtPlaca.Text))
                {
                    MessageBox.Show("Informe a placa.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Ticket ticket = ticketRepo.BuscarTicketAbertoPorPlaca(txtPlaca.Text.Trim());

                if (ticket == null)
                {
                    lblResultado.Text = "Nenhum ticket aberto para esta placa.";
                    lblResultado.ForeColor = Color.Red;
                    grpResultado.Visible = true;
                    return;
                }

                string resultado = $"Ticket: #{ticket.Id}\n" +
                                   $"Veículo: {ticket.Veiculo}\n" +
                                   $"Vaga: {ticket.Vaga}\n" +
                                   $"Entrada: {ticket.HoraEntrada:dd/MM/yyyy HH:mm}\n" +
                                   $"Tempo atual: {ticket.TempoFormatado()}\n" +
                                   $"Valor/hora: R$ {ticket.Veiculo.ValorHora:F2}";

                if (ticket.Veiculo.TaxaAdicional > 0)
                    resultado += $"\nTaxa adicional: R$ {ticket.Veiculo.TaxaAdicional:F2}";

                lblResultado.Text = resultado;
                lblResultado.ForeColor = Color.Black;
                grpResultado.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}