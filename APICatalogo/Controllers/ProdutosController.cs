using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Filter;
using APICatalogo.Repository;
using AutoMapper;
using APICatalogo.DTO;

//Parâmetros de rota
//[HttpGet("{id}")] Passando parâmetro único e obrigatório na rota
//[HttpGet("{id}/{param2}")] Passando mais de um parâmetro obrigatório na rota
//[HttpGet("{id}/{param2?}")] Passando um parâmetro obrigatório mais um parâmetro opcional na rota único na rota
//[HttpGet("{id}/{param2="option"}")] Passando um valor padrão se o parâmetro não for informado
/* 
 * Mesmo método atendendo mais de um Endpoint
 * [HttpGet("{id}")] 
 * [HttpGet("{/id}")]
*/

//Limitação e filtro em rotas
//[HttpGet("{id:int:min(1)}")] Aceita somente um inteiro maior que 1
//[HttpGet("{id:alpha)}")] Aceita somente alfanumérico em uma rota
//[HttpGet("{id:alpha:lenght(5))}")] Aceita somente alfanumérico com 5 caracteres em uma rota
//[HttpGet("{id:alpha:maxlenght(5))}")] Aceita somente alfanumérico com no máximo 5 caracteres em uma rota

/*
 Valores aceitos
 int
 alplha
 bool
 datetime
 decimal
 double
 float
 guid
 */

/*
 lenght
 maxlength
 minlength
 range
 min
 max
 */

namespace ApiCatalogo.Controllers
{
    [Route("api/[Controller]")]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        public ProdutosController(IUnitOfWork contexto,IMapper mapper)
        {
            _uof = contexto;
            _mapper = mapper;
        }

        [HttpGet("menorpreco")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosPrecos()
        {
            var produtos = _uof.ProdutoRepository.GetProdutosPorPreco().ToList();
            var ProdutoDTO = _mapper.Map<List<ProdutoDTO>>(produtos);
            return ProdutoDTO; 
        }

        // api/produtos
        [HttpGet]
        public ActionResult<IEnumerable<ProdutoDTO>> Get()
        {
            var produtos = _uof.ProdutoRepository.Get().ToList();
            var ProdutoDTO = _mapper.Map<List<ProdutoDTO>>(produtos);
            return ProdutoDTO;
        }

        // api/produtos/1
        [HttpGet("{id}", Name = "ObterProduto")]
        public ActionResult<ProdutoDTO> Get(int id)
        {
            var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
            var ProdutoDTO = _mapper.Map<ProdutoDTO>(produto);

            if (produto == null)
            {
                return NotFound("Não foi encontrado nenhum produto");
            }
            return ProdutoDTO;
        }

        //  api/produtos
        [HttpPost]
        public ActionResult Post([FromBody] ProdutoDTO produtoDTO)
        {
            var produto = _mapper.Map<Produto>(produtoDTO);
            _uof.ProdutoRepository.Add(produto);
            _uof.Commit();
            var ProdutoDTO = _mapper.Map<ProdutoDTO>(produto);

            return new CreatedAtRouteResult("ObterProduto",
                new { id = produto.ProdutoId }, ProdutoDTO);
        }

        // api/produtos/1
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] ProdutoDTO produtoDTO)
        {
            if (id != produtoDTO.ProdutoId)
            {
                return BadRequest("O id informado não corresponde a nenhum id de categoria");
            }

            var produto = _mapper.Map<Produto>(produtoDTO);

            _uof.ProdutoRepository.update(produto);
            _uof.Commit();
            return Ok();
        }

        //  api/produtos/1
        [HttpDelete("{id}")]
        public ActionResult<ProdutoDTO> Delete(int id)
        {
            var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

            if (produto == null)
            {
                return NotFound($"Não existe produto cadastrado com o id {id}");
            }

            _uof.ProdutoRepository.Delete(produto);
            _uof.Commit();
            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);
            return produtoDTO;
        }
    }
}
