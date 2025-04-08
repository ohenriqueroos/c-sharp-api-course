using APICataloog.Models;
using APICataloog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace APICataloog.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly IProdutoRepository _produtoRepository;
    //private readonly IRepository<Produto> _repository;

    public ProdutosController(IProdutoRepository produtoRepository)
    {
        //_repository = repository;
        _produtoRepository = produtoRepository;
    }

    //[HttpGet("primeiro")]
    //public ActionResult<Produto> GetPrimeiro()
    //{
    //    var produto = _context.Produtos.FirstOrDefault();
    //    if (produto is null)
    //    {
    //        return NotFound("Produto não encontrado...");
    //    }
    //    return produto;
    //}

    //[HttpGet]
    //public async Task<ActionResult<IEnumerable<Produto>>> Get2()
    //{
    //    return await _context.Produtos.AsNoTracking().ToListAsync();
    //}

    [HttpGet("produtos/{id}")]
    public ActionResult<IEnumerable<Produto>>GetProdutosCategoria(int id)
    {
        var produtos = _produtoRepository.GetProdutosPorCategoria(id);

        if (produtos is null)
            return NotFound();

        return Ok(produtos);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Produto>> Get()
    {
        var produtos = _produtoRepository.GetAll();

        if (produtos is null)
            return NotFound();

        return Ok(produtos);
    }

    [HttpGet("{id:int}", Name = "ObterProduto")]
    public ActionResult<Produto> Get(int id, string param2)
    {
        var produto = _produtoRepository.Get(p => p.ProdutoId == id);
        if (produto is null)
        {
            return NotFound("Produto não encontrado...");
        }

        return Ok(produto);
    }

    [HttpPost]
    public ActionResult Post(Produto produto)
    {
        if (produto is null)
            return BadRequest();
        
        var novoProduto = _produtoRepository.Create(produto);

        return new CreatedAtRouteResult("ObterProduto", new { id = novoProduto.ProdutoId }, novoProduto);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Produto produto)
    {
        if (id != produto.ProdutoId)
        {
            return BadRequest();
        }

        
        var produtoAtualizado = _produtoRepository.Update(produto);

        return Ok(produtoAtualizado);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var produto = _produtoRepository.Get(p => p.ProdutoId == id);

        if (produto is null)
            return NotFound("Produto não encontrado...");

        var produtoDeletado = _produtoRepository.Delete(produto);
        return Ok(produtoDeletado);
    }
}
