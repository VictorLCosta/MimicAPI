using System.Collections.Generic;
using System.Threading.Tasks;
using MimicAPI.Helpers;
using MimicAPI.V1.Models;

namespace MimicAPI.V1.Repositories.Contracts
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