﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MyApp.DTOs
{
    public class DefinitionDto
    {
        private string _book;
        [Required(ErrorMessage = "Book is required")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Book must contain only alphabetic characters")]
        [DefaultValue("book")]
        public string Book
        {
            get => _book;
            set => _book = value?.Trim().ToLower();
        }
        [Required(ErrorMessage = "Valid is required")]
        [DefaultValue(false)]
        public bool Valid { get; set; }
        [Required(ErrorMessage = "Definition is required")]
        [DefaultValue("definition")]
        public string Definition {  get; set; }
    }
}
