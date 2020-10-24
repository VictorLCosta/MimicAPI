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
            var item = _context.Words.AsQueryable();

            if(query.date.HasValue)
            {
                item = item.Where(i => i.CreationDate >= query.date.Value || i.UpdateDate >= query.date.Value);
            }

            if(query.numPage.HasValue)
            {
                var qtdRegister = item.Count();
                item = item.Skip((query.numPage.Value - 1) * query.regPerPage.Value).Take(query.regPerPage.Value);

                var pagination = new Pagination();
                pagination.PageNumber = query.numPage.Value;
                pagination.RegisterPerPage = query.regPerPage.Value;
                pagination.TotalRegisters = qtdRegister;
                pagination.TotalPages = (int)Math.Ceiling(((double)pagination.TotalRegisters / pagination.RegisterPerPage));

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(pagination));

                if(query.numPage > pagination.TotalPages)
                {
                    return NotFound();
                }
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
