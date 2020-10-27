using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimicAPI.Database;
using MimicAPI.Models;
using MimicAPI.Helpers;
using MimicAPI.Repositories.Contracts;
using Newtonsoft.Json;

namespace MimicAPI.Controllers
{
    [Route("api/words")]
    public class WordsController : ControllerBase
    {
        private readonly IWordRepository _repository;

        public WordsController(MimicContext context, IWordRepository repository)
        {
            _repository = repository;
        }

        [Route("")]
        [HttpGet]
        public IActionResult FindAllWords([FromQuery]WordUrlQuery query) 
        {
            var item = _repository.FindAllWordsAsync(query);

            if(item == null)
            {
                return NotFound();
            }
            if(query.numPage > item.Pagination.PageNumber)
            {
                return NotFound();
            }

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(item.Pagination));
            return Ok(item);
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> FindWord(int id)
        {
            var obj = await _repository.FindWordAsync(id);

            if (obj == null)
            {
                return NotFound();
            }

            return Ok(obj);
        }

        [Route("")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody]Word word)
        {
            await _repository.CreateAsync(word);

            return Created($"api/words/{word.Id}", word);
        }

        [Route("{id}")]
        [HttpPut]
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

        [Route("{id}")]
        [HttpDelete]
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
