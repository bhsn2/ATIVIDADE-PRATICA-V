using System;
using System.Drawing;
using System.Windows.Forms;
using Estacionamento.Models;
using Estacionamento.Repositories;

namespace Estacionamento.Forms
{
    public class FormCadastroVeiculo : Form
    {
        private Label lblTitulo;
        private Label lblPlaca;
        private Label lblModelo;
        private Label lblCor;
        private Label lblTipo;
        private TextBox txtPlaca;
        private TextBox txtModelo;
        private TextBox txtCor;
        private ComboBox cmbTipo;
        private Button btnCadastrar;
        private Button btnCancelar;

        private VeiculoRepository veiculoRepo = new VeiculoRepository();

        public FormCadastroVeiculo()
        {
            InicializarComponentes();
        }

        private void InicializarComponentes()
        {
            this.Text = "Cadastrar Veículo";
            this.Size = new Size(400, 350);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            lblTitulo = new Label();
            lblTitulo.Text = "CADASTRAR VEÍCULO";
            lblTitulo.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTitulo.Location = new Point(20, 15);
            lblTitulo.AutoSize = true;

            int inicioY = 60;
            int espaco = 40;

            lblPlaca = new Label();
            lblPlaca.Text = "Placa:";
            lblPlaca.Location = new Point(20, inicioY);
            lblPlaca.AutoSize = true;

            txtPlaca = new TextBox();
            txtPlaca.Location = new Point(120, inicioY - 3);
            txtPlaca.Size = new Size(230, 25);
            txtPlaca.CharacterCasing = CharacterCasing.Upper;

            lblModelo = new Label();
            lblModelo.Text = "Modelo:";
            lblModelo.Location = new Point(20, inicioY + espaco);
            lblModelo.AutoSize = true;

            txtModelo = new TextBox();
            txtModelo.Location = new Point(120, inicioY + espaco - 3);
            txtModelo.Size = new Size(230, 25);

            lblCor = new Label();
            lblCor.Text = "Cor:";
            lblCor.Location = new Point(20, inicioY + espaco * 2);
            lblCor.AutoSize = true;

            txtCor = new TextBox();
            txtCor.Location = new Point(120, inicioY + espaco * 2 - 3);
            txtCor.Size = new Size(230, 25);

            lblTipo = new Label();
            lblTipo.Text = "Tipo:";
            lblTipo.Location = new Point(20, inicioY + espaco * 3);
            lblTipo.AutoSize = true;

            cmbTipo = new ComboBox();
            cmbTipo.Location = new Point(120, inicioY + espaco * 3 - 3);
            cmbTipo.Size = new Size(230, 25);
            cmbTipo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTipo.Items.Add("Moto (R$ 5,00/h)");
            cmbTipo.Items.Add("Carro (R$ 10,00/h)");
            cmbTipo.Items.Add("Caminhão (R$ 18,00/h + taxa)");
            cmbTipo.SelectedIndex = 0;

            btnCadastrar = new Button();
            btnCadastrar.Text = "Cadastrar";
            btnCadastrar.Size = new Size(110, 40);
            btnCadastrar.Location = new Point(120, inicioY + espaco * 4 + 10);
            btnCadastrar.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnCadastrar.BackColor = Color.MediumSeaGreen;
            btnCadastrar.ForeColor = Color.White;
            btnCadastrar.FlatStyle = FlatStyle.Flat;
            btnCadastrar.Click += BtnCadastrar_Click;

            btnCancelar = new Button();
            btnCancelar.Text = "Cancelar";
            btnCancelar.Size = new Size(110, 40);
            btnCancelar.Location = new Point(240, inicioY + espaco * 4 + 10);
            btnCancelar.Font = new Font("Segoe UI", 10);
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Click += (s, e) => this.Close();

            this.Controls.Add(lblTitulo);
            this.Controls.Add(lblPlaca);
            this.Controls.Add(txtPlaca);
            this.Controls.Add(lblModelo);
            this.Controls.Add(txtModelo);
            this.Controls.Add(lblCor);
            this.Controls.Add(txtCor);
            this.Controls.Add(lblTipo);
            this.Controls.Add(cmbTipo);
            this.Controls.Add(btnCadastrar);
            this.Controls.Add(btnCancelar);
        }

        private void BtnCadastrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtPlaca.Text))
                {
                    MessageBox.Show("Informe a placa do veículo.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPlaca.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtModelo.Text))
                {
                    MessageBox.Show("Informe o modelo do veículo.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtModelo.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtCor.Text))
                {
                    MessageBox.Show("Informe a cor do veículo.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCor.Focus();
                    return;
                }

                string placa = txtPlaca.Text.Trim();

                Veiculo existente = veiculoRepo.BuscarPorPlaca(placa);
                if (existente != null)
                {
                    MessageBox.Show($"Veículo já cadastrado:\n{existente}", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string modelo = txtModelo.Text.Trim();
                string cor = txtCor.Text.Trim();

                Veiculo veiculo;
                switch (cmbTipo.SelectedIndex)
                {
                    case 0: veiculo = new Moto(placa, modelo, cor); break;
                    case 1: veiculo = new Carro(placa, modelo, cor); break;
                    case 2: veiculo = new Caminhao(placa, modelo, cor); break;
                    default: throw new Exception("Tipo inválido.");
                }

                int id = veiculoRepo.Inserir(veiculo);

                MessageBox.Show($"Veículo cadastrado com sucesso!\n{veiculo}",
                    "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtPlaca.Clear();
                txtModelo.Clear();
                txtCor.Clear();
                cmbTipo.SelectedIndex = 0;
                txtPlaca.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}