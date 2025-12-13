using Cobranca.Domain;
using Cobranca.Domain.Titulos;
using Cobranca.Service.Responses;
using Microsoft.AspNetCore.Mvc;
using NLog.Web.LayoutRenderers;

namespace Cobranca.Service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TitulosController : AbstractController<TitulosController>
{
    const int TIMEOUT_CACHE = 1;

    private readonly TituloService _tituloService;

    public TitulosController(IServiceProvider provider) : base(provider) =>
        _tituloService = provider.GetRequiredService<TituloService>();

    [HttpGet]
    [ResponseCache(Duration = TIMEOUT_CACHE)]
    public Task<IActionResult> ObterTodosOsTitulos() =>
        TryAsync(_tituloService.ObterTodosTitulosAsync(), t => new TituloResponse(t));


    [HttpGet("{status}")]
    [ResponseCache(Duration = TIMEOUT_CACHE, VaryByQueryKeys = new[] { "status" })]
    public Task<IActionResult> ObterTitulosPorStatus([FromQuery] TituloStatus status) =>
        TryAsync(_tituloService.ObterTodosTitulosAsync(status), t => new TituloResponse(t));

    [HttpGet("em-atraso")]
    [ResponseCache(Duration = TIMEOUT_CACHE)]
    public Task<IActionResult> ObterTitulosEmAtraso() =>
        TryAsync(_tituloService.ObterTitulosEmAtrasoAsync(), t => new TituloResponse(t));
}