﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Models
{
    public class Word
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public bool Active { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
