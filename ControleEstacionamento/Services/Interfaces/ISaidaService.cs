using ControleEstacionamento.Models;

namespace ControleEstacionamento.Services.Interfaces;

public interface ISaidaService
{
    ResultadoSaida RegistrarSaida(SaidaRequest request);
}
