using APICataloog.Context;
using APICataloog.Models;
using Microsoft.EntityFrameworkCore;

namespace APICataloog.Repositories;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(AppDbContext context) : base(context)
    {
    }

    
}