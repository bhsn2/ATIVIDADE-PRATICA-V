using System;
using Estacionamento.Enums;

namespace Estacionamento.Models
{
    // CLASSE ABSTRATA: não pode ser instanciada diretamente
    // Define o contrato que todo veículo deve seguir (Abstração - Slide 12 Aula I)
    public abstract class Veiculo
    {
        // PROPRIEDADES com encapsulamento (Slide 11 Aula I)
        public int Id { get; set; }

        private string _placa;
        public string Placa
        {
            get => _placa;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Placa não pode ser vazia.");
                _placa = value.ToUpper().Trim();
            }
        }

        private string _modelo;
        public string Modelo
        {
            get => _modelo;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Modelo não pode ser vazio.");
                _modelo = value.Trim();
            }
        }

        private string _cor;
        public string Cor
        {
            get => _cor;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Cor não pode ser vazia.");
                _cor = value.Trim();
            }
        }

        // Propriedade abstrata: cada tipo define seu valor (Polimorfismo - Slide 13 Aula I)
        public abstract TipoVeiculo Tipo { get; }
        public abstract decimal ValorHora { get; }
        public abstract decimal TaxaAdicional { get; }

        // Método abstrato: cada tipo calcula de forma diferente (Polimorfismo)
        public abstract decimal CalcularValor(int horas);

        // Construtor protegido (só filhos podem usar)
        protected Veiculo(string placa, string modelo, string cor)
        {
            Placa = placa;
            Modelo = modelo;
            Cor = cor;
        }

        // Construtor vazio para o Repository
        protected Veiculo() { }

        public override string ToString()
        {
            return $"[{Tipo}] {Placa} - {Modelo} ({Cor})";
        }
    }
}