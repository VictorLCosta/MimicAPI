using System.Collections.Generic;

namespace MimicAPI.V1.Models.DTO
{
    public abstract class DTOBase
    {
        public List<DTOLink> Links { get; set; } = new List<DTOLink>();
    }
}