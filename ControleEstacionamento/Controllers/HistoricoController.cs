using ControleEstacionamento.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ControleEstacionamento.Controllers;

public class HistoricoController : Controller
{
    private readonly IConsultaEstacionamentoService _consultaService;

    public HistoricoController(IConsultaEstacionamentoService consultaService)
    {
        _consultaService = consultaService;
    }

    public IActionResult Index()
    {
        return View(_consultaService.ObterHistorico());
    }
}
