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
        public ActionResult<IEnumerable<CategoriaDTO>> Get([FromQuery] CategoriasParameters categoriasParameters)
        {
            var categorias = _uof.CategoriaRepository.GetCategorias(categoriasParameters);
            return ObterCategorias(categorias);
        }

        private ActionResult<IEnumerable<CategoriaDTO>> ObterCategorias(PagedList<Categoria> categorias)
        {
            var metadata = new
            {
                categorias.TotalCount,
                categorias.PageSize,
                categorias.CurrentPage,
                categorias.TotalPages,
                categorias.HasNext,
                categorias.HasPrevious
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var categoriasDto = categorias.ToCategoriaDTOList();

            return Ok(categoriasDto);
        }

        [HttpGet("filter/nome/pagination")]
        public ActionResult<IEnumerable<CategoriaDTO>> GetCategoriasFiltradas([FromQuery] CategoriasFiltroNome categoriasFiltro)
        {
            var categoriasFiltradas = _uof.CategoriaRepository.GetCategoriasFiltroNome(categoriasFiltro);

            return ObterCategorias(categoriasFiltradas);
        }

        [HttpGet]
        public ActionResult<IEnumerable<CategoriaDTO>> Get()
        {
            var categorias = _uof.CategoriaRepository.GetAll();

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
        public ActionResult<CategoriaDTO> Get(int id)
        {
                var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);
            
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
        public ActionResult<CategoriaDTO> Post(CategoriaDTO categoriaDto)
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
            _uof.Commit();

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
        public ActionResult<CategoriaDTO> Put(int id, CategoriaDTO categoriaDto)
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
            _uof.Commit();

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
        public ActionResult<CategoriaDTO> Delete(int id)
        {
            var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com id={id} não encontrada...");
                return NotFound($"Categoria com id={id} não encontrada...");
            }

            var categoriaExcluida = _uof.CategoriaRepository.Delete(categoria);
            _uof.Commit();

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
