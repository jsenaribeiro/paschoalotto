using Cobranca.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Cobranca.Service.Controllers;


public class AbstractController<TController> : ControllerBase where TController : class
{
    protected readonly ILogger<TController> _logger;
    protected readonly IUnitOfWork _uow;

    protected AbstractController(IServiceProvider provider) 
    { 
        _logger = provider.GetRequiredService<ILogger<TController>>();
        _uow = provider.GetRequiredService<IUnitOfWork>();
    }

    public Task<IActionResult> TryAsync<T>(Func<Task<T>> request) => TryAsync(request());

    public async Task<IActionResult> TryAsync<T>(Task<T> request)
    {
        try
        {
            return request is not null
                ? Ok(await request)
                : NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    public async Task<IActionResult> TryAsync<T, U>(Task<T[]> request, Func<T, U> transform) 
    {
        var result = await request;
        var modded = result.Select(x => transform(x));

        return await TryAsync(Task.FromResult(modded));
    }
}