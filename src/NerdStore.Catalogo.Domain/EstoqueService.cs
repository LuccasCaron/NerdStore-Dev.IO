using NerdStore.Catalogo.Domain.Events;
using NerdStore.Catalogo.Domain.Interfaces;
using NerdStore.Core.Bus;

namespace NerdStore.Catalogo.Domain;

internal sealed class EstoqueService : IEstoqueService
{

    #region Properties

    private readonly IProdutoRepository _produtoRepository;
    private readonly IMediatrHandler _bus;

    #endregion

    #region Constructor

    public EstoqueService(IProdutoRepository produtoRepository, IMediatrHandler bus)
    {
        _produtoRepository = produtoRepository;
        _bus = bus;
    }

    #endregion

    public async Task<bool> DebitarEstoque(Guid produtoId, int quantidade)
    {
        var produto = await _produtoRepository.ObterPorId(produtoId);

        if (produto is null) return false;

        if (!produto.PossuiEstoque(quantidade)) return false;

        produto.DebitarEstoque(quantidade);

        if(produto.QuantidadeEstoque < 10)
        {
            await _bus.PublicarEvento(new ProdutoAbaixoEstoqueEvent(produtoId, produto.QuantidadeEstoque));
        }

        _produtoRepository.Atualizar(produto);

        return await _produtoRepository.UnitOfWork.Commit();
    }

    public async Task<bool> ReporEstoque(Guid produtoId, int quantidade)
    {
        var produto = await _produtoRepository.ObterPorId(produtoId);

        if (produto is null) return false;

        produto.ReporEstoque(quantidade);

        _produtoRepository.Atualizar(produto);

        return await _produtoRepository.UnitOfWork.Commit();
    }

    public void Dispose()
    {
        _produtoRepository.Dispose();
    }

}
