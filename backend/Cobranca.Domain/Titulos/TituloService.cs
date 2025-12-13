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

    public Task<Titulo[]> ObterTitulosEmAtrasoAsync() =>
        _unitOfWork.Titulos.ObterAtrasadosAsync();

    public Task<Titulo[]> ObterTodosTitulosAsync() =>
        _unitOfWork.Titulos.ObterTodosAsync();

    public Task<Titulo[]> ObterTodosTitulosAsync(TituloStatus statusTitulo) =>
        _unitOfWork.Titulos.ObterPorStatusAsync(statusTitulo);
}