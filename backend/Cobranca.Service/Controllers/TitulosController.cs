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
    private readonly TituloService _tituloService;

    public TitulosController(IServiceProvider provider): base(provider) =>
        _tituloService = provider.GetRequiredService<TituloService>();

    [HttpGet]
    public Task<IActionResult> ObterTodosOsTitulos() => 
        TryAsync(_tituloService.ObterTodosTitulosAsync(), t => new TituloResponse(t));


    [HttpGet("{status}")]
    public Task<IActionResult> ObterTitulosPorStatus(StatusTitulo status) =>
        TryAsync(_tituloService.ObterTodosTitulosAsync(status), t => new TituloResponse(t));

    [HttpGet("em-atraso")]
    public Task<IActionResult> ObterTitulosEmAtraso() =>
        TryAsync(_tituloService.ObterTitulosEmAtrasoAsync(), t => new TituloResponse(t));
}