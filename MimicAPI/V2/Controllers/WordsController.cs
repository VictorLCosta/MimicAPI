using Microsoft.AspNetCore.Mvc;

namespace MimicAPI.V2.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class WordsController : ControllerBase
    {
        ///<summary>
        ///Operação que seleciona do banco de dados todas as palavras existentes
        ///</summary>
        ///<param name="query">Filtros de pesquisa</param>
        ///<returns>Listagem de palavras</returns>
        [HttpGet("", Name = "FindAllWords")]
        public string FindAllWords()
        {
            return "Versão 2.0"; 
        }
    }
}