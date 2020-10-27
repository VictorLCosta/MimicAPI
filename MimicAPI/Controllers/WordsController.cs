using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimicAPI.Database;
using MimicAPI.Models;
using MimicAPI.Helpers;
using Newtonsoft.Json;

namespace MimicAPI.Controllers
{
    [Route("api/words")]
    public class WordsController : ControllerBase
    {
        private readonly MimicContext _context;

        public WordsController(MimicContext context)
        {
            _context = context;
        }

        [Route("")]
        [HttpGet]
        public IActionResult FindAllWords([FromQuery]WordUrlQuery query) 
        {
            if(query.numPage > pagination.TotalPages)
            {
                    return NotFound();
            }
            return Ok(item);
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> FindWord(int id)
        {
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
            return Created($"api/words/{word.Id}", word);
        }

        [Route("{id}")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody]Word word, int id)
        {
            var obj = await _context.Words.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);

            if (obj == null)
            {
                return NotFound();
            }

            word.Id = id;
            

            return NoContent();
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            

            return NoContent();
        }
    }
}
