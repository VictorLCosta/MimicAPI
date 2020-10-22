using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimicAPI.Database;
using MimicAPI.Models;

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
        public IActionResult FindAllWords(DateTime? date, int? numPage, int? regPerPage) 
        {
            var item = _context.Words.AsQueryable();

            if(date.HasValue)
            {
                item = item.Where(i => i.CreationDate >= date.Value || i.UpdateDate >= date.Value);
            }

            if(numPage.HasValue)
            {
                item = item.Skip((numPage.Value - 1) * regPerPage.Value).Take(regPerPage.Value);
            }
            return Ok(item);
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> FindWord(int id)
        {
            var obj = await _context.Words.FindAsync(id);

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
            await _context.Words.AddAsync(word);
            await _context.SaveChangesAsync();

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
            _context.Words.Update(word);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var obj = await _context.Words.FindAsync(id);

            if (obj == null)
            {
                return NotFound();
            }

            obj.Active = false;
            _context.Words.Update(obj);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
