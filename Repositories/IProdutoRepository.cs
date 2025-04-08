using APICataloog.Models;

namespace APICataloog.Repositories;

public interface IProdutoRepository : IRepository<Produto>
{
    IEnumerable<Produto> GetProdutosPorCategoria(int id);
}
