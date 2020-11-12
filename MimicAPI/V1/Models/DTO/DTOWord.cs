using System;

namespace MimicAPI.V1.Models.DTO
{
    public class DTOWord : DTOBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public bool Active { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}