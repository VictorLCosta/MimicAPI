using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MimicAPI.Database;
using MimicAPI.Helpers;
using MimicAPI.Models;
using MimicAPI.Repositories.Contracts;

namespace MimicAPI.Repositories
{
    public class WordRepository : IWordRepository
    {
        private readonly MimicContext _context;

        public PaginationList<Word> FindAllWordsAsync(WordUrlQuery query)
        {
            var list = new PaginationList<Word>();
            var item = _context.Words.AsNoTracking().AsQueryable();

            if(query.date.HasValue)
            {
                item = item.Where(i => i.CreationDate >= query.date.Value && i.UpdateDate >= query.date.Value);
            }

            if(query.numPage.HasValue)
            {
                var qtdRegisters = _context.Words.Count();
                item = item.Skip((query.numPage.Value - 1) * query.regPerPage.Value).Take(query.regPerPage.Value);

                var pagination = new Pagination();
                pagination.PageNumber = query.numPage.Value;
                pagination.RegisterPerPage = query.regPerPage.Value;
                pagination.TotalRegisters = qtdRegisters;
                pagination.TotalPages = (int)Math.Ceiling((double)pagination.TotalRegisters / pagination.RegisterPerPage);

                list.Pagination = pagination;
            }

            list.AddRange(item.ToList());
            return list;
        }

        public async Task<Word> FindWordAsync(int id)
        {
            return await _context.Words.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task CreateAsync(Word word)
        {
            await _context.Words.AddAsync(word);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Word word)
        {
            _context.Words.Update(word);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var obj = await _context.Words.FindAsync(id);

            obj.Active = false;
            _context.Words.Update(obj);
            await _context.SaveChangesAsync();
        }

    }
}