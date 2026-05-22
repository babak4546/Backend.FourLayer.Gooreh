using GoorehDomain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoorehDomain.Entities
{
    public class DbContextLogger:DbContextLogThing
    {
        public string? Title { get; set; }
    }
}
