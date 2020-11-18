using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimicAPI.V1.Models;
using MimicAPI.Helpers;
using MimicAPI.V1.Repositories.Contracts;
using Newtonsoft.Json;
using AutoMapper;
using MimicAPI.V1.Models.DTO;

namespace MimicAPI.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class WordsController : ControllerBase
    {
        private readonly IWordRepository _repository;
        private readonly IMapper _mapper;

        public WordsController(IWordRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        ///<summary>
        ///Operação que seleciona do banco de dados todas as palavras existentes
        ///</summary>
        ///<param name="query">Filtros de pesquisa</param>
        ///<returns>Listagem de palavras</returns>
        [HttpGet("", Name = "FindAllWords")]
        [MapToApiVersion("1.1")]
        public IActionResult FindAllWords([FromQuery]WordUrlQuery query)
        {
            var item = _repository.FindAllWordsAsync(query);

            if (item.Results.Count == 0)
            {
                return NotFound();
            }

            PaginationList<DTOWord> list = CreateLinkListDTOWord(query, item);

            return Ok(list);
        }

        ///<summary>
        ///Operação que obtém uma única palavra da base de dados
        ///</summary>
        ///<param name="id">Código identificador da palavra</param>
        ///<returns>Retorna um objeto do tipo Word</returns>
        [MapToApiVersion("1.1")]
        [HttpGet("{id}", Name = "FindWord")]
        public async Task<IActionResult> FindWord(int id)
        {
            var obj = await _repository.FindWordAsync(id);

            if (obj == null)
            {
                return NotFound();
            }
            
            DTOWord wordDTO = _mapper.Map<Word, DTOWord>(obj);

            wordDTO.Links.Add(new DTOLink("self", Url.Link("FindWord", new { id = wordDTO.Id }), "GET"));
            wordDTO.Links.Add(new DTOLink("update", Url.Link("UpdateWord", new {id = wordDTO.Id}), "PUT"));
            wordDTO.Links.Add(new DTOLink("delete", Url.Link("DeleteWord", new {id = wordDTO.Id}), "DELETE"));

            return Ok(wordDTO);
        }

        ///<summary>
        ///Operação que realiza o cadastro de uma nova palavra
        ///</summary>
        ///<param name="word">Um objeto palavra</param>
        ///<returns>Um objeto palavra com seu Id</returns>
        [Route("")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody]Word word)
        {
            if(word == null)
            {
                return BadRequest();
            }
            if(!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
            
            word.Active = true;
            word.CreationDate = DateTime.Now;
            await _repository.CreateAsync(word);

            var dtoword = _mapper.Map<Word, DTOWord>(word);
            dtoword.Links.Add(new DTOLink("self", Url.Link("FindWord", new {id = dtoword.Id}), "GET"));

            return Created($"api/words/{word.Id}", dtoword);
        }
        
        ///<summary>
        ///Operação que realiza a substituição de dados de uma palavra específica
        ///</summary>
        ///<param name="word">Objeto palavra com dados para alteração</param>
        ///<param name="id">Código identificador da palavra a ser alterada</param>
        ///<returns></returns>
        [HttpPut("{id}", Name = "UpdateWord")]
        public async Task<IActionResult> Update([FromBody]Word word, int id)
        {
            var obj = await _repository.FindWordAsync(id);

            if (obj == null)
            {
                return NotFound();
            }

            if(word == null)
            {
                return BadRequest();
            }

            if(!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            word.Id = id;
            word.Active = obj.Active;
            word.CreationDate = obj.CreationDate;
            word.UpdateDate = obj.UpdateDate;
            await _repository.UpdateAsync(obj);

            var dtoword = _mapper.Map<Word, DTOWord>(word);
            dtoword.Links.Add(new DTOLink("self", Url.Link("FindWord", new {id = dtoword.Id}), "GET"));

            return NoContent();
        }

        ///<summary>
        ///Operação que desativa uma palavra do sistema
        ///</summary>
        ///<param name="id">Código identificador da palavra a ser desativada</param>
        ///<returns></returns>
        [HttpDelete("{id}", Name = "DeleteWord")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _repository.FindWordAsync(id);
            if(item == null)
            {
                return NotFound();
            }

            await _repository.Delete(id);
            return NoContent();
        }

        //MÉTODOS PRIVADOS
        private PaginationList<DTOWord> CreateLinkListDTOWord(WordUrlQuery query, PaginationList<Word> item)
        {
            var list = _mapper.Map<PaginationList<Word>, PaginationList<DTOWord>>(item);
            foreach (var word in list.Results)
            {
                word.Links = new List<DTOLink>();
                word.Links.Add(new DTOLink("self", Url.Link("FindWord", new { id = word.Id }), "GET"));
            }

            list.Links.Add(new DTOLink("self", Url.Link("FindAllWords", query), "GET"));
            if (item.Pagination != null)
            {
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(item.Pagination));

                if (query.numPage.Value + 1 <= item.Pagination.TotalPages)
                {
                    var queryString = new WordUrlQuery() { numPage = query.numPage + 1, regPerPage = query.regPerPage, date = query.date };
                    list.Links.Add(new DTOLink("next", Url.Link("FindAllWords", queryString), "GET"));
                }
                if (query.numPage.Value - 1 > 0)
                {
                    var queryString = new WordUrlQuery() { numPage = query.numPage - 1, regPerPage = query.regPerPage, date = query.date };
                    list.Links.Add(new DTOLink("prev", Url.Link("FindAllWords", queryString), "GET"));
                }
            }

            return list;
        }
    }
}
