using System.Collections.Generic;
using MimicAPI.Models.DTO;

namespace MimicAPI.Helpers
{
    public class PaginationList<T>
    {
        public List<T> Results { get; set; } = new List<T>();
        public Pagination Pagination { get; set; }
        public List<DTOLink> Links { get; set; } = new List<DTOLink>();
    }
}