using ControleEstacionamento.Models;
using ControleEstacionamento.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ControleEstacionamento.Controllers;

public class PrecosController : Controller
{
    private readonly ITabelaPrecoService _tabelaPrecoService;

    public PrecosController(ITabelaPrecoService tabelaPrecoService)
    {
        _tabelaPrecoService = tabelaPrecoService;
    }

    public IActionResult Index()
    {
        ViewBag.Tabelas = _tabelaPrecoService.ObterTabelasPreco();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Salvar(TabelaPrecoRequest request)
    {
        try
        {
            _tabelaPrecoService.SalvarTabelaPreco(request);
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
