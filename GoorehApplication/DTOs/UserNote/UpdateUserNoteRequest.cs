using System;
using System.Collections.Generic;
using System.Text;

namespace GoorehApplication.DTOs.UserNote
{
    public class UpdateUserNoteRequest
    {
        public string? Title { get; set; }
        public string? Text { get; set; }
    }
}
