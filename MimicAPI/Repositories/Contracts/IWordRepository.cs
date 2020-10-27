using System.Collections.Generic;
using System.Threading.Tasks;
using MimicAPI.Helpers;
using MimicAPI.Models;

namespace MimicAPI.Repositories.Contracts
{
    public interface IWordRepository
    {
         PaginationList<Word> FindAllWordsAsync(WordUrlQuery query);
         Task<Word> FindWordAsync(int id);
         Task CreateAsync(Word word);
         Task UpdateAsync(Word word);
         Task Delete(int id);
    }
}