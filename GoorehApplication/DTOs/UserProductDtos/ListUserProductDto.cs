using System;
using System.Collections.Generic;
using System.Text;

namespace GoorehApplication.DTOs.UserProductDtos
{
    public class ListUserProductDto
    {
        public string? Title { get; set; }
        public string? Value { get; set; }
        public string? ConcurrencyStamp { get; set; }
        public string? Guid { get; set; }
        public DateTime CreatedIn { get; set; }
        public DateTime EditedIn { get; set; }
    }
}
