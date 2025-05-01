using APICataloog.Context;
using APICataloog.DTOs;
using APICataloog.DTOs.Mappings;
using APICataloog.Filters;
using APICataloog.Models;
using APICataloog.Pagination;
using APICataloog.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using X.PagedList;

namespace APICataloog.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnityOfWork _uof;
        private readonly ILogger _logger;

        public CategoriasController(ILogger<CategoriasController> logger, IUnityOfWork uof)
        {
            _logger = logger;
            _uof = uof;
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get([FromQuery] CategoriasParameters categoriasParameters)
        {
            var categorias = await _uof.CategoriaRepository.GetCategoriasAsync(categoriasParameters);
            return ObterCategorias(categorias);
        }

        private ActionResult<IEnumerable<CategoriaDTO>> ObterCategorias(IPagedList<Categoria> categorias)
        {
            var metadata = new
            {
                categorias.Count,
                categorias.PageSize,
                categorias.PageCount,
                categorias.TotalItemCount,
                categorias.HasNextPage,
                categorias.HasPreviousPage
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var categoriasDto = categorias.ToCategoriaDTOList();

            return Ok(categoriasDto);
        }

        [HttpGet("filter/nome/pagination")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasFiltradasAsync([FromQuery] CategoriasFiltroNome categoriasFiltro)
        {
            var categoriasFiltradas = await _uof.CategoriaRepository.GetCategoriasFiltroNomeAsync(categoriasFiltro);

            return ObterCategorias(categoriasFiltradas);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get()
        {
            var categorias = await _uof.CategoriaRepository.GetAllAsync();

            if (categorias is null)
                return NotFound("Nenhuma categoria encontrada...");

            //var categoriasDto = new List<CategoriaDTO>();

            //foreach(var categoria in categorias)
            //{
            //    var categoriaDto = new CategoriaDTO()
            //    {
            //        CategoriaId = categoria.CategoriaId,
            //        Nome = categoria.Nome,
            //        ImagemUrl = categoria.ImagemUrl
            //    };

            //    categoriasDto.Add(categoriaDto);
            //}

            var categoriasDto = categorias.ToCategoriaDTOList();

            return Ok(categoriasDto);
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
                var categoria = await _uof.CategoriaRepository.GetAsync(c => c.CategoriaId == id);
            
                if (categoria is null)
                {
                    _logger.LogWarning($"Categoria com id = {id} não encontrada...");
                    return NotFound($"Categoria com id = {id} não encontrada...");
                }

                //var categoriaDto = new CategoriaDTO()
                //{
                //    CategoriaId = categoria.CategoriaId,
                //    Nome = categoria.Nome,
                //    ImagemUrl = categoria.ImagemUrl
                //};

                var categoriaDto = categoria.ToCategoriaDTO();

            return Ok(categoriaDto);
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaDTO>> Post(CategoriaDTO categoriaDto)
        {
            if (categoriaDto is null)
            {
                _logger.LogWarning("Dados inválidos...");
                return BadRequest("Dados inválidos...");
            }

            //var categoria = new Categoria()
            //{
            //    CategoriaId = categoriaDto.CategoriaId,
            //    Nome = categoriaDto.Nome,
            //    ImagemUrl = categoriaDto.ImagemUrl,
            //};

            var categoria = categoriaDto.ToCategoria();

            var categoriaCriada = _uof.CategoriaRepository.Create(categoria);
            await _uof.CommitAsync();

            //var novaCategoriaDto = new CategoriaDTO()
            //{
            //    CategoriaId = categoriaCriada.CategoriaId,
            //    Nome = categoriaCriada.Nome,
            //    ImagemUrl = categoriaCriada.ImagemUrl
            //};

            var novaCategoriaDto = categoriaCriada.ToCategoriaDTO();

            return new CreatedAtRouteResult("ObterCategoria", new { id = novaCategoriaDto.CategoriaId }, novaCategoriaDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> Put(int id, CategoriaDTO categoriaDto)
        {
            if (id != categoriaDto.CategoriaId)
            {
                _logger.LogWarning("Dados inválidos...");
                return BadRequest("Dados inválidos...");
            }

            //var categoria = new Categoria()
            //{
            //    CategoriaId = categoriaDto.CategoriaId,
            //    Nome = categoriaDto.Nome,
            //    ImagemUrl = categoriaDto.ImagemUrl
            //};

            var categoria = categoriaDto.ToCategoria();

            var categoriaAtualizada = _uof.CategoriaRepository.Update(categoria);
            await _uof.CommitAsync();

            //var categoriaAtualizadaDto = new CategoriaDTO()
            //{
            //    CategoriaId = categoriaAtualizada.CategoriaId,
            //    Nome = categoriaAtualizada.Nome,
            //    ImagemUrl = categoriaAtualizada.ImagemUrl
            //};

            var categoriaAtualizadaDto = categoriaAtualizada.ToCategoriaDTO();

            return Ok(categoriaAtualizadaDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
            var categoria = await _uof.CategoriaRepository.GetAsync(c => c.CategoriaId == id);

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com id={id} não encontrada...");
                return NotFound($"Categoria com id={id} não encontrada...");
            }

            var categoriaExcluida = _uof.CategoriaRepository.Delete(categoria);
            await _uof.CommitAsync();

            //var categoriaExcluidaDto = new CategoriaDTO()
            //{
            //    CategoriaId = categoriaExcluida.CategoriaId,
            //    Nome = categoriaExcluida.Nome,
            //    ImagemUrl = categoriaExcluida.ImagemUrl
            //};

            var categoriaExcluidaDto = categoriaExcluida.ToCategoriaDTO();

            return Ok(categoriaExcluidaDto);
        }
    }
}
