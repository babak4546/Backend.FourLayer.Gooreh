using GoorehDomain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoorehDomain.Entities
{
    public class MiddlewareLog:MiddleWareThings
    {
        public string? Title { get; set; } = "";
        public string? Method { get; set; }
        public string? Path { get; set; }
        public string? ContextUserGuid { get; set; }
        public string? ContextUserIp { get; set; }
    }

}
