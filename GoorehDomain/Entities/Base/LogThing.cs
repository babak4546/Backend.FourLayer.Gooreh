using System;
using System.Collections.Generic;
using System.Text;

namespace GoorehDomain.Entities.Base
{
    public class LogThing
    {
        public int Id { get; set; }
        public string LogGuid { get; set; } = System.Guid.NewGuid().ToString();
        public DateTime? LogDate { get; set; } = DateTime.Now;
    }
}
