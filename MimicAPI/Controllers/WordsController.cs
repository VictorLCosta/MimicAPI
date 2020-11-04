using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimicAPI.Models;
using MimicAPI.Helpers;
using MimicAPI.Repositories.Contracts;
using Newtonsoft.Json;
using AutoMapper;
using MimicAPI.Models.DTO;

namespace MimicAPI.Controllers
{
    [Route("api/words")]
    public class WordsController : ControllerBase
    {
        private readonly IWordRepository _repository;
        private readonly IMapper _mapper;

        public WordsController(IWordRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [Route("")]
        [HttpGet]
        public IActionResult FindAllWords([FromQuery]WordUrlQuery query) 
        {
            var item = _repository.FindAllWordsAsync(query);

            if(item.Count == 0)
            {
                return NotFound();
            }
            if(item.Pagination != null)
            {
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(item.Pagination));
            }

            var list = _mapper.Map<PaginationList<Word>, PaginationList<DTOWord>>(item);
            foreach(var word in list)
            {
                word.Links = new List<DTOLink>();
                word.Links.Add(new DTOLink("self", Url.Link("FindWord", new {id = word.Id}), "GET"));
            }

            return Ok(list);
        }

        [HttpGet("{id}", Name = "FindWord")]
        public async Task<IActionResult> FindWord(int id)
        {
            var obj = await _repository.FindWordAsync(id);

            if (obj == null)
            {
                return NotFound();
            }
            
            DTOWord wordDTO = _mapper.Map<Word, DTOWord>(obj);
            wordDTO.Links = new List<DTOLink>();

            wordDTO.Links.Add(new DTOLink("self", Url.Link("FindWord", new { id = wordDTO.Id }), "GET"));
            wordDTO.Links.Add(new DTOLink("update", $"https://localhost:5001/api/words/{wordDTO.Id}", "PUT"));
            wordDTO.Links.Add(new DTOLink("delete", $"https://localhost:5001/api/words/{wordDTO.Id}", "DELETE"));

            return Ok(wordDTO);
        }

        [Route("")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody]Word word)
        {
            await _repository.CreateAsync(word);

            return Created($"api/words/{word.Id}", word);
        }

        [HttpPut("{id}", Name = "UpdateWord")]
        public async Task<IActionResult> Update([FromBody]Word word, int id)
        {
            var obj = await _repository.FindWordAsync(id);

            if (obj == null)
            {
                return NotFound();
            }

            word.Id = id;
            await _repository.UpdateAsync(obj);

            return NoContent();
        }

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
    }
}
