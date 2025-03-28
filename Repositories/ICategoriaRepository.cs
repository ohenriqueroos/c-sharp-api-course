using APICataloog.Models;

namespace APICataloog.Repositories;

public interface ICategoriaRepository
{
    IEnumerable<Categoria> GetCategorias();
    Categoria GetCategoria(int id);
    Categoria Create(Categoria categoria);
    Categoria Update(Categoria categoria);
    Categoria Delete(int id);
}