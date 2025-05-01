using APICataloog.Models;
using APICataloog.Pagination;
using X.PagedList;

namespace APICataloog.Repositories;

public interface IProdutoRepository : IRepository<Produto>
{
    //IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParams);
    Task<IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParams);
    Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroParams);
    Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id);
}
