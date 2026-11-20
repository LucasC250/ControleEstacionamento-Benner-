using ControleEstacionamento.EstacionamentoApp;

namespace ControleEstacionamento.Services.Interfaces;

public interface IConsultaEstacionamentoService
{
    List<RegistroEstacionamento> ObterVeiculosEstacionados();
    List<RegistroEstacionamento> ObterHistorico(int limite = 200);
}
