using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.V1.Models
{
    public class Word
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Score { get; set; }
        public bool Active { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
