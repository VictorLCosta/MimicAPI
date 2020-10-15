﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimicAPI.Database;
using MimicAPI.Models;

namespace MimicAPI.Controllers
{
    public class WordsController : ControllerBase
    {
        private readonly MimicContext _context;

        public WordsController(MimicContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> FindAllWords() 
        {
            return Ok(await _context.Words.ToListAsync());
        }

        public async Task<IActionResult> FindWord(int id)
        {
            return Ok(await _context.Words.FindAsync(id));
        }

        public async Task<IActionResult> Register(Word word)
        {
            await _context.Words.AddAsync(word);
            return Ok();
        }

        public async Task<IActionResult> Update(int id, Word word)
        {
            _context.Words.Update(word);

            return Ok();
        }

        public async Task<IActionResult> Delete(int id)
        {
            _context.Words.Remove(await _context.Words.FindAsync(id));

            return Ok();
        }
    }
}