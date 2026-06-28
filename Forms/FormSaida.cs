using System;
using System.Drawing;
using System.Windows.Forms;
using Estacionamento.Enums;
using Estacionamento.Models;
using Estacionamento.Repositories;

namespace Estacionamento.Forms
{
    public class FormSaida : Form
    {
        private Label lblTitulo;
        private Label lblPlaca;
        private TextBox txtPlaca;
        private Button btnBuscar;
        private GroupBox grpResumo;
        private Label lblResumo;
        private Label lblFormaPgto;
        private ComboBox cmbFormaPgto;
        private Label lblValorPago;
        private TextBox txtValorPago;
        private Label lblTroco;
        private Button btnPagar;
        private Button btnCancelar;

        private TicketRepository ticketRepo = new TicketRepository();
        private Ticket ticketAberto = null;
        private decimal valorTotal = 0;

        public FormSaida()
        {
            InicializarComponentes();
        }

        private void InicializarComponentes()
        {
            this.Text = "Registrar Saída + Pagamento";
            this.Size = new Size(500, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            lblTitulo = new Label();
            lblTitulo.Text = "REGISTRAR SAÍDA + PAGAMENTO";
            lblTitulo.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTitulo.Location = new Point(20, 15);
            lblTitulo.AutoSize = true;

            lblPlaca = new Label();
            lblPlaca.Text = "Placa:";
            lblPlaca.Location = new Point(20, 60);
            lblPlaca.AutoSize = true;

            txtPlaca = new TextBox();
            txtPlaca.Location = new Point(100, 57);
            txtPlaca.Size = new Size(220, 25);
            txtPlaca.CharacterCasing = CharacterCasing.Upper;

            btnBuscar = new Button();
            btnBuscar.Text = "Buscar Ticket";
            btnBuscar.Location = new Point(330, 55);
            btnBuscar.Size = new Size(120, 28);
            btnBuscar.Click += BtnBuscar_Click;

            grpResumo = new GroupBox();
            grpResumo.Text = "Resumo da Saída";
            grpResumo.Location = new Point(20, 100);
            grpResumo.Size = new Size(440, 150);
            grpResumo.Visible = false;

            lblResumo = new Label();
            lblResumo.Location = new Point(15, 25);
            lblResumo.Size = new Size(410, 110);
            lblResumo.Font = new Font("Segoe UI", 10);
            grpResumo.Controls.Add(lblResumo);

            lblFormaPgto = new Label();
            lblFormaPgto.Text = "Pagamento:";
            lblFormaPgto.Location = new Point(20, 265);
            lblFormaPgto.AutoSize = true;
            lblFormaPgto.Visible = false;

            cmbFormaPgto = new ComboBox();
            cmbFormaPgto.Location = new Point(120, 262);
            cmbFormaPgto.Size = new Size(200, 25);
            cmbFormaPgto.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFormaPgto.Items.Add("Dinheiro");
            cmbFormaPgto.Items.Add("Cartão");
            cmbFormaPgto.SelectedIndex = 0;
            cmbFormaPgto.Visible = false;
            cmbFormaPgto.SelectedIndexChanged += CmbFormaPgto_Changed;

            lblValorPago = new Label();
            lblValorPago.Text = "Valor pago:";
            lblValorPago.Location = new Point(20, 305);
            lblValorPago.AutoSize = true;
            lblValorPago.Visible = false;

            txtValorPago = new TextBox();
            txtValorPago.Location = new Point(120, 302);
            txtValorPago.Size = new Size(200, 25);
            txtValorPago.Visible = false;

            lblTroco = new Label();
            lblTroco.Text = "";
            lblTroco.Location = new Point(120, 335);
            lblTroco.AutoSize = true;
            lblTroco.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblTroco.ForeColor = Color.DarkGreen;

            btnPagar = new Button();
            btnPagar.Text = "Confirmar Pagamento";
            btnPagar.Size = new Size(180, 40);
            btnPagar.Location = new Point(100, 380);
            btnPagar.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnPagar.BackColor = Color.MediumSeaGreen;
            btnPagar.ForeColor = Color.White;
            btnPagar.FlatStyle = FlatStyle.Flat;
            btnPagar.Visible = false;
            btnPagar.Click += BtnPagar_Click;

            btnCancelar = new Button();
            btnCancelar.Text = "Cancelar";
            btnCancelar.Size = new Size(110, 40);
            btnCancelar.Location = new Point(290, 380);
            btnCancelar.Font = new Font("Segoe UI", 10);
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Click += (s, e) => this.Close();

            this.Controls.Add(lblTitulo);
            this.Controls.Add(lblPlaca);
            this.Controls.Add(txtPlaca);
            this.Controls.Add(btnBuscar);
            this.Controls.Add(grpResumo);
            this.Controls.Add(lblFormaPgto);
            this.Controls.Add(cmbFormaPgto);
            this.Controls.Add(lblValorPago);
            this.Controls.Add(txtValorPago);
            this.Controls.Add(lblTroco);
            this.Controls.Add(btnPagar);
            this.Controls.Add(btnCancelar);
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

                ticketAberto = ticketRepo.BuscarTicketAbertoPorPlaca(txtPlaca.Text.Trim());

                if (ticketAberto == null)
                {
                    MessageBox.Show("Nenhum ticket aberto para esta placa.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                valorTotal = ticketAberto.CalcularValorSaida();

                string resumo = $"Ticket: #{ticketAberto.Id}\n" +
                                $"Veículo: {ticketAberto.Veiculo}\n" +
                                $"Vaga: {ticketAberto.Vaga}\n" +
                                $"Entrada: {ticketAberto.HoraEntrada:dd/MM/yyyy HH:mm}\n" +
                                $"Saída: {ticketAberto.HoraSaida:dd/MM/yyyy HH:mm}\n" +
                                $"Permanência: {ticketAberto.TempoFormatado()}\n";

                if (ticketAberto.Veiculo.TaxaAdicional > 0)
                    resumo += $"Taxa adicional ({ticketAberto.Veiculo.Tipo}): R$ {ticketAberto.Veiculo.TaxaAdicional:F2}\n";

                if (valorTotal == 0)
                {
                    resumo += "\nTOLERÂNCIA - Sem cobrança!";
                    lblResumo.Text = resumo;
                    grpResumo.Visible = true;

                    ticketRepo.RegistrarSaida(ticketAberto.Id, 0);

                    MessageBox.Show("Saída registrada!\nTempo dentro da tolerância (15min). Sem cobrança.",
                        "Tolerância", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    return;
                }

                resumo += $"\nVALOR TOTAL: R$ {valorTotal:F2}";
                lblResumo.Text = resumo;

                grpResumo.Visible = true;
                lblFormaPgto.Visible = true;
                cmbFormaPgto.Visible = true;
                btnPagar.Visible = true;

                CmbFormaPgto_Changed(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CmbFormaPgto_Changed(object sender, EventArgs e)
        {
            bool isDinheiro = cmbFormaPgto.SelectedIndex == 0;
            lblValorPago.Visible = isDinheiro;
            txtValorPago.Visible = isDinheiro;
            lblTroco.Text = "";
        }

        private void BtnPagar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ticketAberto == null) return;

                TipoPagamento tipoPgto = cmbFormaPgto.SelectedIndex == 0
                    ? TipoPagamento.Dinheiro
                    : TipoPagamento.Cartao;

                if (tipoPgto == TipoPagamento.Dinheiro)
                {
                    if (!decimal.TryParse(txtValorPago.Text, out decimal valorPago) || valorPago < valorTotal)
                    {
                        MessageBox.Show($"Valor mínimo: R$ {valorTotal:F2}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtValorPago.Focus();
                        return;
                    }

                    decimal troco = valorPago - valorTotal;
                    if (troco > 0)
                        lblTroco.Text = $"Troco: R$ {troco:F2}";
                }

                Pagamento pagamento = new Pagamento(ticketAberto.Id, valorTotal, tipoPgto);
                ticketRepo.RegistrarPagamento(pagamento);
                ticketRepo.RegistrarSaida(ticketAberto.Id, valorTotal);

                MessageBox.Show(
                    $"Pagamento registrado!\n\n" +
                    $"Ticket #{ticketAberto.Id} fechado.\n" +
                    $"Valor: R$ {valorTotal:F2}\n" +
                    $"Forma: {tipoPgto}",
                    "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}