using MediatR;
using NerdStore.Catalogo.Domain.Interfaces;

namespace NerdStore.Catalogo.Domain.Events;

public class ProdutoEventHandler : INotificationHandler<ProdutoAbaixoEstoqueEvent>
{

    #region Properties

    private readonly IProdutoRepository _produtoRepository;

    #endregion

    #region Constructor

    public ProdutoEventHandler(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    #endregion

    public async Task Handle(ProdutoAbaixoEstoqueEvent mensagem, CancellationToken cancellationToken)
    {
        var produto = await _produtoRepository.ObterPorId(mensagem.AggregateId);

        // enviar email para aquisicao de mais produtos.
    }
}
