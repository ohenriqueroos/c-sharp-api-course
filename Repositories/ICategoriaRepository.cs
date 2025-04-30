using APICataloog.Models;
using APICataloog.Pagination;

namespace APICataloog.Repositories;

public interface ICategoriaRepository : IRepository<Categoria>
{
    PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParams);
}