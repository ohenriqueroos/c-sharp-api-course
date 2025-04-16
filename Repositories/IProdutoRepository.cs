using APICataloog.Models;
using APICataloog.Pagination;

namespace APICataloog.Repositories;

public interface IProdutoRepository : IRepository<Produto>
{
    IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParams);
    IEnumerable<Produto> GetProdutosPorCategoria(int id);
}
