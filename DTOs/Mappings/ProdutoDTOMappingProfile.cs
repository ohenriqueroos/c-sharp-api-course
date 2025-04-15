using APICataloog.Models;
using AutoMapper;

namespace APICataloog.DTOs.Mappings;

public class ProdutoDTOMappingProfile : Profile
{
    public ProdutoDTOMappingProfile()
    {
        CreateMap<Produto, ProdutoDTO>().ReverseMap();
        CreateMap<Categoria, CategoriaDTO>().ReverseMap();
        CreateMap<Produto, ProdutoDTOUpdateRequest>().ReverseMap();
        CreateMap<Produto, ProdutoDTOUpdateResponse>().ReverseMap();
    }
}
