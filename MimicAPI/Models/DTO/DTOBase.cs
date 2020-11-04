using System.Collections.Generic;

namespace MimicAPI.Models.DTO
{
    public abstract class DTOBase
    {
        public List<DTOLink> Links { get; set; }
    }
}