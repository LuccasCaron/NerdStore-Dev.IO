using AutoMapper;
using NerdStore.Catalago.Application.ViewModels;
using NerdStore.Catalogo.Domain;

namespace NerdStore.Catalago.Application.AutoMapper;

public class DomainToViewModelMappingProfile : Profile
{
    public DomainToViewModelMappingProfile()
    {
        CreateMap<Produto, ProdutoViewModel>()
            .ForMember(d => d.Largura, o => o.MapFrom(s => s.Dimensoes.Largura))
            .ForMember(d => d.Altura, o => o.MapFrom(s => s.Dimensoes.Altura))
            .ForMember(d => d.Profundidade, o => o.MapFrom(s => s.Dimensoes.Profundidade))
            .ReverseMap() 
            .ConstructUsing(p =>
                new Produto(p.Nome, p.Descricao, p.Ativo,
                    p.Valor, p.CategoriaId, p.DataCadastro,
                    p.Imagem, new Dimensoes(p.Altura, p.Largura, p.Profundidade)));

        CreateMap<Categoria, CategoriaViewModel>()
            .ReverseMap()
            .ConstructUsing(c => new Categoria(c.Nome, c.Codigo));
    }
}
