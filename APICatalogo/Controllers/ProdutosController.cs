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
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        public ProdutosController(IUnitOfWork contexto)
        {
            _uof = contexto;
        }

        [HttpGet("menorpreco")]
        public ActionResult<IEnumerable<Produto>> GetProdutosPrecos()
        {
            return _uof.ProdutoRepository.GetProdutosPorPreco().ToList();
        }

        // api/produtos
        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            return _uof.ProdutoRepository.Get().ToList();
        }

        // api/produtos/1
        [HttpGet("{id}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

            if (produto == null)
            {
                return NotFound();
            }
            return produto;
        }

        //  api/produtos
        [HttpPost]
        public ActionResult Post([FromBody] Produto produto)
        {
            _uof.ProdutoRepository.Add(produto);
            _uof.Commit();

            return new CreatedAtRouteResult("ObterProduto",
                new { id = produto.ProdutoId }, produto);
        }

        // api/produtos/1
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                return BadRequest();
            }

            _uof.ProdutoRepository.update(produto);
            _uof.Commit();
            return Ok();
        }

        //  api/produtos/1
        [HttpDelete("{id}")]
        public ActionResult<Produto> Delete(int id)
        {
            var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

            if (produto == null)
            {
                return NotFound();
            }

            _uof.ProdutoRepository.Delete(produto);
            _uof.Commit();
            return produto;
        }
    }
}
