﻿using Services.Models.Search;
using System.ComponentModel.DataAnnotations;

namespace Services.Models.Book
{
    public class BookReq
    {
        public string OptionCode { get; set; }
        [Required]
        public SearchReq SearchReq { get; set; }
    }
}
