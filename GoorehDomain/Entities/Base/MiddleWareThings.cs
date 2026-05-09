using System;
using System.Collections.Generic;
using System.Text;

namespace GoorehDomain.Entities.Base
{
    public class MiddleWareThings
    {
        public int Id { get; set; }
        public string? Guid { get; set; }=System.Guid.NewGuid().ToString();
        public DateTime MiddleWareDate { get; set; }

    }
}
