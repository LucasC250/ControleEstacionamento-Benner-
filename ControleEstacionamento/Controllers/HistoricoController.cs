using ControleEstacionamento.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControleEstacionamento.Controllers;

public class HistoricoController : Controller
{
    private readonly EstacionamentoService _service;

    public HistoricoController(EstacionamentoService service)
    {
        _service = service;
    }

    public IActionResult Index()
    {
        return View(_service.ObterHistorico());
    }
}
