using System;
using System.ComponentModel.DataAnnotations;

namespace MVCUser.Models
{
    public class Users
    {
        public int Id {get; set;}

        [StringLength(20, MinimumLength=3)]
        [Required]
        public string username {get; set;}
        [StringLength(20, MinimumLength=5)]
        [Required]
        public string password {get; set;}
    }
}