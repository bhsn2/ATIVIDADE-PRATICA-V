using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Estacionamento.Models;
using Estacionamento.Repositories;

namespace Estacionamento.Forms
{
    public class FormListaVeiculos : Form
    {
        private Label lblTitulo;
        private DataGridView dgvVeiculos;
        private Button btnFechar;

        private VeiculoRepository veiculoRepo = new VeiculoRepository();

        public FormListaVeiculos()
        {
            InicializarComponentes();
            CarregarDados();
        }

        private void InicializarComponentes()
        {
            this.Text = "Veículos Cadastrados";
            this.Size = new Size(600, 400);
            this.StartPosition = FormStartPosition.CenterParent;

            lblTitulo = new Label();
            lblTitulo.Text = "VEÍCULOS CADASTRADOS";
            lblTitulo.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTitulo.Location = new Point(20, 15);
            lblTitulo.AutoSize = true;

            dgvVeiculos = new DataGridView();
            dgvVeiculos.Location = new Point(20, 55);
            dgvVeiculos.Size = new Size(540, 260);
            dgvVeiculos.ReadOnly = true;
            dgvVeiculos.AllowUserToAddRows = false;
            dgvVeiculos.AllowUserToDeleteRows = false;
            dgvVeiculos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVeiculos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvVeiculos.BackgroundColor = Color.White;

            dgvVeiculos.Columns.Add("ID", "ID");
            dgvVeiculos.Columns.Add("Placa", "Placa");
            dgvVeiculos.Columns.Add("Modelo", "Modelo");
            dgvVeiculos.Columns.Add("Cor", "Cor");
            dgvVeiculos.Columns.Add("Tipo", "Tipo");
            dgvVeiculos.Columns.Add("ValorHora", "Valor/Hora");

            btnFechar = new Button();
            btnFechar.Text = "Fechar";
            btnFechar.Size = new Size(110, 35);
            btnFechar.Location = new Point(450, 325);
            btnFechar.Click += (s, e) => this.Close();

            this.Controls.Add(lblTitulo);
            this.Controls.Add(dgvVeiculos);
            this.Controls.Add(btnFechar);
        }

        private void CarregarDados()
        {
            try
            {
                dgvVeiculos.Rows.Clear();

                List<Veiculo> veiculos = veiculoRepo.ListarTodos();

                foreach (Veiculo v in veiculos)
                {
                    dgvVeiculos.Rows.Add(
                        v.Id,
                        v.Placa,
                        v.Modelo,
                        v.Cor,
                        v.Tipo.ToString(),
                        $"R$ {v.ValorHora:F2}"
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}