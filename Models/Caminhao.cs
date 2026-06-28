using System;
using Estacionamento.Enums;

namespace Estacionamento.Models
{
    public class Caminhao : Veiculo
    {
        public override TipoVeiculo Tipo => TipoVeiculo.Caminhao;
        public override decimal ValorHora => 18.00m;
        public override decimal TaxaAdicional => 5.00m; // Taxa de carga

        public Caminhao(string placa, string modelo, string cor)
            : base(placa, modelo, cor) { }

        public Caminhao() : base() { }

        public override decimal CalcularValor(int horas)
        {
            // Valor por hora + taxa de carga fixa
            return (horas * ValorHora) + TaxaAdicional;
        }
    }
}