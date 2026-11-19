using ControleEstacionamento.Models;
using ControleEstacionamento.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControleEstacionamento.Controllers;

public class PrecosController : Controller
{
    private readonly EstacionamentoService _service;

    public PrecosController(EstacionamentoService service)
    {
        _service = service;
    }

    public IActionResult Index()
    {
        ViewBag.Tabelas = _service.ObterTabelasPreco();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Salvar(TabelaPrecoRequest request)
    {
        try
        {
            _service.SalvarTabelaPreco(request);
            TempData["Mensagem"] = "Tabela de preços cadastrada com sucesso.";
            TempData["TipoMensagem"] = "success";
        }
        catch (Exception ex)
        {
            TempData["Mensagem"] = ex.Message;
            TempData["TipoMensagem"] = "danger";
        }

        return RedirectToAction(nameof(Index));
    }
}
