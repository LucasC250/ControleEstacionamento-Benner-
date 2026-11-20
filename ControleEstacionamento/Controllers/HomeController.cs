
using ControleEstacionamento.Models;
using ControleEstacionamento.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ControleEstacionamento.Controllers;

public class HomeController : Controller
{
    private readonly IEntradaService _entradaService;
    private readonly ISaidaService _saidaService;
    private readonly IConsultaEstacionamentoService _consultaService;

    public HomeController(
        IEntradaService entradaService,
        ISaidaService saidaService,
        IConsultaEstacionamentoService consultaService)
    {
        _entradaService = entradaService;
        _saidaService = saidaService;
        _consultaService = consultaService;
    }

    public IActionResult Error() => View();

    public IActionResult Index()
    {
        CarregarDashboard();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Entrada(EntradaRequest request)
    {
        try
        {
            _entradaService.RegistrarEntrada(request);
            TempData["Mensagem"] = $"Entrada do veículo {request.PlacaEntrada!.Trim().ToUpperInvariant()} registrada com sucesso.";
            TempData["TipoMensagem"] = "success";
        }
        catch (Exception ex)
        {
            TempData["Mensagem"] = ex.Message;
            TempData["TipoMensagem"] = "danger";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Saida(SaidaRequest request)
    {
        try
        {
            var resultado = _saidaService.RegistrarSaida(request);
            TempData["Mensagem"] = $"Saída registrada: {request.PlacaSaida!.Trim().ToUpperInvariant()} · Permanência: {FormatarDuracao(resultado.Duracao)} · Total: {resultado.Valor:C}";
            TempData["TipoMensagem"] = "success";
        }
        catch (Exception ex)
        {
            TempData["Mensagem"] = ex.Message;
            TempData["TipoMensagem"] = "danger";
        }

        return RedirectToAction(nameof(Index));
    }

    private void CarregarDashboard()
    {
        ViewBag.Veiculos = _consultaService.ObterVeiculosEstacionados();

        var hoje = DateTime.Today;
        var historicoHoje = _consultaService.ObterHistorico(1000)
            .Where(r => r.HorarioEntrada.Date == hoje || r.HorarioSaida?.Date == hoje)
            .ToList();

        ViewBag.TotalEntradasHoje = historicoHoje.Count(r => r.HorarioEntrada.Date == hoje);
        ViewBag.TotalSaidasHoje = historicoHoje.Count(r => r.HorarioSaida?.Date == hoje);
        ViewBag.FaturamentoHoje = historicoHoje
            .Where(r => r.HorarioSaida?.Date == hoje && r.ValorPago.HasValue)
            .Sum(r => r.ValorPago!.Value);
    }

    public static string FormatarDuracao(TimeSpan duracao)
    {
        var totalHoras = (int)duracao.TotalHours;
        return $"{totalHoras:D2}h {duracao.Minutes:D2}min {duracao.Seconds:D2}s";
    }
}
