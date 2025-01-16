using AutoMapper;
using NerdStore.Catalago.Application.Services.Interfaces;
using NerdStore.Catalago.Application.ViewModels;
using NerdStore.Catalogo.Domain;
using NerdStore.Catalogo.Domain.Interfaces;
using NerdStore.Core.DomainObjects;

namespace NerdStore.Catalago.Application.Services;

internal sealed class ProdutoAppService : IProdutoAppService
{

    #region Properties

    private readonly IProdutoRepository _produtoRepository;
    private readonly IEstoqueService _estoqueService;
    private readonly IMapper _mapper;

    #endregion

    #region Constructor

    public ProdutoAppService(IProdutoRepository produtoRepository, IEstoqueService estoqueService, IMapper mapper)
    {
        _produtoRepository = produtoRepository;
        _estoqueService = estoqueService;
        _mapper = mapper;
    }

    #endregion

    public async Task<IEnumerable<ProdutoViewModel>> ObterPorCategoria(int codigo)
    {
        return _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterPorCategoria(codigo));
    }

    public async Task<ProdutoViewModel> ObterPorId(Guid id)
    {
        return _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterPorId(id));
    }

    public async Task<IEnumerable<ProdutoViewModel>> ObterTodos()
    {
        return _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterTodos());
    }

    public async Task<IEnumerable<CategoriaViewModel>> ObterCategorias()
    {
        return _mapper.Map<IEnumerable<CategoriaViewModel>>(await _produtoRepository.ObterCategorias());
    }

    public async Task AdicionarProduto(ProdutoViewModel produtoViewModel)
    {
        var produto = _mapper.Map<Produto>(produtoViewModel);
        _produtoRepository.Adicionar(produto);

        await _produtoRepository.UnitOfWork.Commit();
    }

    public async Task AtualizarProduto(ProdutoViewModel produtoViewModel)
    {
        var produto = _mapper.Map<Produto>(produtoViewModel);
        _produtoRepository.Atualizar(produto);

        await _produtoRepository.UnitOfWork.Commit();
    }

    public async Task<ProdutoViewModel> DebitarEstoque(Guid id, int quantidade)
    {
        if (!_estoqueService.DebitarEstoque(id, quantidade).Result)
        {
            throw new DomainException("Falha ao debitar estoque");
        }

        return _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterPorId(id));
    }

    public async Task<ProdutoViewModel> ReporEstoque(Guid id, int quantidade)
    {
        if (!_estoqueService.ReporEstoque(id, quantidade).Result)
        {
            throw new DomainException("Falha ao repor estoque");
        }

        return _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterPorId(id));
    }

    public void Dispose()
    {
        _produtoRepository?.Dispose();
        _estoqueService?.Dispose();
    }
}
