using APICataloog.Context;
using APICataloog.Models;
using APICataloog.Pagination;
using Microsoft.EntityFrameworkCore;

namespace APICataloog.Repositories;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(AppDbContext context) : base(context)
    {
    }

    public PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParams)
    {
        var categorias = GetAll().OrderBy(p => p.CategoriaId).AsQueryable();

        var categoriasOrdenadas = PagedList<Categoria>.ToPagedList(categorias, categoriasParams.PageNumber, categoriasParams.PageSize);

        return categoriasOrdenadas;
    }

    public PagedList<Categoria> GetCategoriasFiltroNome(CategoriasFiltroNome categoriasParams)
    {
        var categorias = GetAll().AsQueryable();

        if (!string.IsNullOrEmpty(categoriasParams.Nome))
        {
            categorias = categorias.Where(c => c.Nome.Contains(categoriasParams.Nome));
        }

        var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categorias, categoriasParams.PageNumber, categoriasParams.PageSize);

        return categoriasFiltradas;
    }
}