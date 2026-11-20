using ControleEstacionamento.Models;

namespace ControleEstacionamento.Services.Interfaces;

public interface IEntradaService
{
    void RegistrarEntrada(EntradaRequest request);
}
