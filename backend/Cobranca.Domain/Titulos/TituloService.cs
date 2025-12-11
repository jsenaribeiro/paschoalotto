using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cobranca.Domain.Titulos;

public class TituloService
{
    private IUnitOfWork _unitOfWork;

    public TituloService(IServiceProvider provider) => 
        _unitOfWork = (provider.GetService(typeof(IUnitOfWork)) as IUnitOfWork)!;

    public Task<Titulo[]> ObterTitulosEmAtrasoAsync()
    {
        return _unitOfWork.Titulos.ObterAtrasadosAsync();
    }

    public Task<Titulo[]> ObterTodosTitulosAsync() =>
        _unitOfWork.Titulos.ListAsync(true);

    public Task<Titulo[]> ObterTodosTitulosAsync(StatusTitulo statusTitulo) => 
        _unitOfWork.Titulos.FilterBy(t => t.Status == statusTitulo).ListAsync();
}