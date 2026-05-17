using System;
using System.Collections.Generic;
using System.Text;

namespace GoorehApplication.DTOs.UserProductDtos
{
    public class UpdateUserProductsDto
    {
        public string? Title { get; set; }
        public string? Value { get; set; }
        public required string ConcurrencyStamp { get; set; }
    }
}
