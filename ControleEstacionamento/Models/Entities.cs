namespace ControleEstacionamento.EstacionamentoApp
{
    public class TabelaPreco
    {
        public int Id { get; set; }
        public DateTime DataInicioVigencia { get; set; }
        public DateTime DataFimVigencia { get; set; }
        public decimal ValorHoraInicial { get; set; }
        public decimal ValorHoraAdicional { get; set; }
    }

    public class RegistroEstacionamento
    {
        public int Id { get; set; }
        public string Placa { get; set; } = string.Empty;
        public DateTime HorarioEntrada { get; set; }
        public DateTime? HorarioSaida { get; set; }
        public decimal? ValorPago { get; set; }
    }
}
