using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Estacionamento.Models;
using Estacionamento.Repositories;

namespace Estacionamento.Forms
{
    public class FormListaEstacionados : Form
    {
        private Label lblTitulo;
        private Label lblTotal;
        private DataGridView dgvTickets;
        private Button btnAtualizar;
        private Button btnFechar;

        private TicketRepository ticketRepo = new TicketRepository();

        public FormListaEstacionados()
        {
            InicializarComponentes();
            CarregarDados();
        }

        private void InicializarComponentes()
        {
            this.Text = "Veículos Estacionados";
            this.Size = new Size(700, 450);
            this.StartPosition = FormStartPosition.CenterParent;

            lblTitulo = new Label();
            lblTitulo.Text = "VEÍCULOS ESTACIONADOS";
            lblTitulo.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTitulo.Location = new Point(20, 15);
            lblTitulo.AutoSize = true;

            lblTotal = new Label();
            lblTotal.Text = "Total: 0";
            lblTotal.Location = new Point(20, 50);
            lblTotal.AutoSize = true;
            lblTotal.Font = new Font("Segoe UI", 10);

            dgvTickets = new DataGridView();
            dgvTickets.Location = new Point(20, 75);
            dgvTickets.Size = new Size(640, 270);
            dgvTickets.ReadOnly = true;
            dgvTickets.AllowUserToAddRows = false;
            dgvTickets.AllowUserToDeleteRows = false;
            dgvTickets.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTickets.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTickets.BackgroundColor = Color.White;

            dgvTickets.Columns.Add("Ticket", "Ticket");
            dgvTickets.Columns.Add("Placa", "Placa");
            dgvTickets.Columns.Add("Veiculo", "Veículo");
            dgvTickets.Columns.Add("Tipo", "Tipo");
            dgvTickets.Columns.Add("Vaga", "Vaga");
            dgvTickets.Columns.Add("Entrada", "Entrada");
            dgvTickets.Columns.Add("Tempo", "Tempo");

            btnAtualizar = new Button();
            btnAtualizar.Text = "Atualizar";
            btnAtualizar.Size = new Size(110, 35);
            btnAtualizar.Location = new Point(20, 360);
            btnAtualizar.Click += (s, e) => CarregarDados();

            btnFechar = new Button();
            btnFechar.Text = "Fechar";
            btnFechar.Size = new Size(110, 35);
            btnFechar.Location = new Point(550, 360);
            btnFechar.Click += (s, e) => this.Close();

            this.Controls.Add(lblTitulo);
            this.Controls.Add(lblTotal);
            this.Controls.Add(dgvTickets);
            this.Controls.Add(btnAtualizar);
            this.Controls.Add(btnFechar);
        }

        private void CarregarDados()
        {
            try
            {
                dgvTickets.Rows.Clear();

                List<Ticket> tickets = ticketRepo.ListarTicketsAbertos();
                lblTotal.Text = $"Total: {tickets.Count} veículo(s)";

                foreach (Ticket t in tickets)
                {
                    dgvTickets.Rows.Add(
                        $"#{t.Id}",
                        t.Veiculo?.Placa ?? "N/A",
                        $"{t.Veiculo?.Modelo ?? "N/A"} ({t.Veiculo?.Cor ?? "N/A"})",
                        t.Veiculo?.Tipo.ToString() ?? "N/A",
                        t.Vaga,
                        t.HoraEntrada.ToString("dd/MM/yyyy HH:mm"),
                        t.TempoFormatado()
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