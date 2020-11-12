using Microsoft.AspNetCore.Mvc;

namespace MimicAPI.V2.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class WordsController : ControllerBase
    {
        [HttpGet("", Name = "FindAllWords")]
        public string FindAllWords()
        {
            return "Versão 2.0"; 
        }
    }
}