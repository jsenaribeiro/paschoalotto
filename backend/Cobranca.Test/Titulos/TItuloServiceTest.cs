namespace Cobranca.Test.Titulos;

using Cobranca.Domain;
using Cobranca.Domain.Clientes;
using Cobranca.Domain.Titulos;
using Cobranca.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

public class TituloServiceTest : AbstractTest
{
    private readonly TituloService _service;

    private readonly Expression<Func<Titulo, bool>> qualquer = Arg.Any<Expression<Func<Titulo, bool>>>();

    public TituloServiceTest()
    {
        _service = new TituloService(_provider);
    }

    [Fact]
    public async Task ObterTodosTitulosAsync_DeveRetornarTitulos()
    {
        var titulo1 = new Titulo("1", Cliente.Empty, DateTime.Now, 100, 1);
        var titulo2 = new Titulo("2", Cliente.Empty, DateTime.Now, 100, 2);

        await _unitOfWork.Titulos.CreateAsync(titulo1);
        await _unitOfWork.Titulos.CreateAsync(titulo2);

        var result = await _service.ObterTodosTitulosAsync();

        Assert.Equal(2, result.Length);

        await _unitOfWork.Titulos.DeleteAsync(titulo1);
        await _unitOfWork.Titulos.DeleteAsync(titulo2);
    }

    [Fact]
    public async Task ObterTitulosEmAtrasoAsync_DeveRetornarTitulosEmAtraso()
    {
        var titulo = new Titulo("1", Cliente.Empty, DateTime.Now.AddMonths(-5), 100, 1);

        await _unitOfWork.Titulos.CreateAsync(titulo);

        var result = await _service.ObterTitulosEmAtrasoAsync();

        Assert.Single(result);
        Assert.True(result[0].EmAtraso);

        await _unitOfWork.Titulos.DeleteAsync(titulo);
    }

    [Fact]
    public async Task ObterTodosTitulosAsync_Status_DeveRetornarTitulosComStatus()
    {
        var titulo = new Titulo("1", Cliente.Empty, DateTime.Now, 100, 1);

        await _unitOfWork.Titulos.CreateAsync(titulo);

        var result = await _service.ObterTodosTitulosAsync(StatusTitulo.EmAberto);

        Assert.Single(result);
        Assert.Equal(StatusTitulo.EmAberto, result[0].Status);

        await _unitOfWork.Titulos.DeleteAsync(titulo);
    }
}
