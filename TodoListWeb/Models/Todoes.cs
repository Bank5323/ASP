using System;
using System.ComponentModel.DataAnnotations;

namespace MvcTodoes.Models
{
    public class Todoes
    {
        public int Id { get; set; }

        [StringLength(60)]
        [Required]
        public string Title { get; set; }

        public bool Check { get; set; }
        public string IdUser {get; set;}
    }
}