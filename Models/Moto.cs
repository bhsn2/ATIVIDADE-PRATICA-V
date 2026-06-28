using System;
using Estacionamento.Enums;

namespace Estacionamento.Models
{
    // HERANÇA: Moto herda de Veiculo (Slide 13 Aula I)
    public class Moto : Veiculo
    {
        // POLIMORFISMO: cada classe filha define seus próprios valores
        public override TipoVeiculo Tipo => TipoVeiculo.Moto;
        public override decimal ValorHora => 5.00m;
        public override decimal TaxaAdicional => 0.00m;

        public Moto(string placa, string modelo, string cor)
            : base(placa, modelo, cor) { }

        public Moto() : base() { }

        // POLIMORFISMO: implementação específica do cálculo
        public override decimal CalcularValor(int horas)
        {
            return horas * ValorHora;
        }
    }
}