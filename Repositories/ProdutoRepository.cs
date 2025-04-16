using APICataloog.Context;
using APICataloog.Models;
using APICataloog.Pagination;

namespace APICataloog.Repositories;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(AppDbContext context) : base(context)
    {
    }

    public IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParams)
    {
        return GetAll().OrderBy(p => p.Nome).Skip((produtosParams.PageNumber - 1) * produtosParams.PageSize).Take(produtosParams.PageSize).ToList();
    }

    public IEnumerable<Produto> GetProdutosPorCategoria(int id)
    {
        return GetAll().Where(c => c.CategoriaId == id);
    }
}
