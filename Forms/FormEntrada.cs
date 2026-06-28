using System;
using System.Drawing;
using System.Windows.Forms;
using Estacionamento.Models;
using Estacionamento.Repositories;

namespace Estacionamento.Forms
{
    public class FormEntrada : Form
    {
        private Label lblTitulo;
        private Label lblPlaca;
        private Label lblVaga;
        private Label lblInfo;
        private TextBox txtPlaca;
        private TextBox txtVaga;
        private Button btnBuscar;
        private Button btnRegistrar;
        private Button btnCancelar;

        private VeiculoRepository veiculoRepo = new VeiculoRepository();
        private TicketRepository ticketRepo = new TicketRepository();
        private Veiculo veiculoEncontrado = null;

        public FormEntrada()
        {
            InicializarComponentes();
        }

        private void InicializarComponentes()
        {
            this.Text = "Registrar Entrada";
            this.Size = new Size(450, 330);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            lblTitulo = new Label();
            lblTitulo.Text = "REGISTRAR ENTRADA";
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

            lblInfo = new Label();
            lblInfo.Text = "Informe a placa e clique em Buscar.";
            lblInfo.Location = new Point(20, 100);
            lblInfo.Size = new Size(390, 60);
            lblInfo.Font = new Font("Segoe UI", 10);
            lblInfo.ForeColor = Color.Gray;

            lblVaga = new Label();
            lblVaga.Text = "Vaga:";
            lblVaga.Location = new Point(20, 170);
            lblVaga.AutoSize = true;

            txtVaga = new TextBox();
            txtVaga.Location = new Point(100, 167);
            txtVaga.Size = new Size(200, 25);
            txtVaga.CharacterCasing = CharacterCasing.Upper;
            txtVaga.Enabled = false;

            btnRegistrar = new Button();
            btnRegistrar.Text = "Registrar Entrada";
            btnRegistrar.Size = new Size(150, 40);
            btnRegistrar.Location = new Point(100, 220);
            btnRegistrar.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnRegistrar.BackColor = Color.MediumSeaGreen;
            btnRegistrar.ForeColor = Color.White;
            btnRegistrar.FlatStyle = FlatStyle.Flat;
            btnRegistrar.Enabled = false;
            btnRegistrar.Click += BtnRegistrar_Click;

            btnCancelar = new Button();
            btnCancelar.Text = "Cancelar";
            btnCancelar.Size = new Size(110, 40);
            btnCancelar.Location = new Point(260, 220);
            btnCancelar.Font = new Font("Segoe UI", 10);
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Click += (s, e) => this.Close();

            this.Controls.Add(lblTitulo);
            this.Controls.Add(lblPlaca);
            this.Controls.Add(txtPlaca);
            this.Controls.Add(btnBuscar);
            this.Controls.Add(lblInfo);
            this.Controls.Add(lblVaga);
            this.Controls.Add(txtVaga);
            this.Controls.Add(btnRegistrar);
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

                veiculoEncontrado = veiculoRepo.BuscarPorPlaca(txtPlaca.Text.Trim());

                if (veiculoEncontrado == null)
                {
                    DialogResult resp = MessageBox.Show("Veículo não cadastrado.\nDeseja cadastrar agora?",
                        "Veículo não encontrado", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (resp == DialogResult.Yes)
                    {
                        FormCadastroVeiculo formCad = new FormCadastroVeiculo();
                        formCad.ShowDialog();

                        veiculoEncontrado = veiculoRepo.BuscarPorPlaca(txtPlaca.Text.Trim());
                        if (veiculoEncontrado == null)
                        {
                            lblInfo.Text = "Veículo ainda não cadastrado.";
                            lblInfo.ForeColor = Color.Red;
                            return;
                        }
                    }
                    else
                    {
                        lblInfo.Text = "Veículo não cadastrado.";
                        lblInfo.ForeColor = Color.Red;
                        return;
                    }
                }

                if (ticketRepo.TemTicketAberto(veiculoEncontrado.Id))
                {
                    lblInfo.Text = "Este veículo já está no estacionamento!";
                    lblInfo.ForeColor = Color.Red;
                    veiculoEncontrado = null;
                    return;
                }

                lblInfo.Text = $"Veículo encontrado:\n{veiculoEncontrado}\nValor/hora: R$ {veiculoEncontrado.ValorHora:F2}";
                lblInfo.ForeColor = Color.DarkGreen;

                txtVaga.Enabled = true;
                btnRegistrar.Enabled = true;
                txtVaga.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (veiculoEncontrado == null)
                {
                    MessageBox.Show("Busque o veículo primeiro.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtVaga.Text))
                {
                    MessageBox.Show("Informe a vaga.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtVaga.Focus();
                    return;
                }

                string vaga = txtVaga.Text.Trim();

                if (ticketRepo.VagaOcupada(vaga))
                {
                    MessageBox.Show($"A vaga '{vaga}' já está ocupada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Ticket ticket = new Ticket(veiculoEncontrado, vaga);
                int ticketId = ticketRepo.RegistrarEntrada(ticket);

                MessageBox.Show(
                    $"ENTRADA REGISTRADA!\n\n" +
                    $"Ticket: #{ticketId}\n" +
                    $"Veículo: {veiculoEncontrado}\n" +
                    $"Vaga: {vaga}\n" +
                    $"Entrada: {ticket.HoraEntrada:dd/MM/yyyy HH:mm:ss}",
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