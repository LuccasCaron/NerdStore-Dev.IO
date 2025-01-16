using MediatR;
using NerdStore.Core.Messages;
using NerdStore.Vendas.Domain;
using NerdStore.Vendas.Domain.Interfaces;

namespace NerdStore.Vendas.Application.Commands.Handlers;

public class PedidoCommandHandler : IRequestHandler<AdicionarItemPedidoCommand, bool>
{

    #region Properties

    private readonly IPedidoRepository _pedidoRepository;

    #endregion

    #region Constructor

    public PedidoCommandHandler(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    #endregion

    public async Task<bool> Handle(AdicionarItemPedidoCommand request, CancellationToken cancellationToken)
    {
        if (ValidarComando(request)) return false;

        var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(request.ClienteId);

        var pedidoItem = new PedidoItem(request.ProdutoId, request.Nome, request.Quantidade, request.ValorUnitario);

        if(pedido is null)
        {
            pedido = Pedido.PedidoFactory.NovoPedidoRascunho(request.ClienteId);
            pedido.AdicionarItem(pedidoItem);

            _pedidoRepository.Adicionar(pedido);
        }
        else
        {
            var pedidoItemExistente = pedido.PedidoItemExistente(pedidoItem);
            pedido.AdicionarItem(pedidoItem);

            if (pedidoItemExistente)
            {
                _pedidoRepository.AtualizarItem(pedido.PedidoItems.FirstOrDefault(p => p.ProdutoId == pedidoItem.ProdutoId)!);
            }
            else
            {
                _pedidoRepository.AdicionarItem(pedidoItem);
            }
        }

        return await _pedidoRepository.UnitOfWork.Commit();
    }

    private bool ValidarComando(Command request)
    {
        if(request.EhValido()) return true;

        foreach(var error in request.ValidationResult.Errors)
        {
            // lançar evento de erro
        }

        return false;   
    }

}
