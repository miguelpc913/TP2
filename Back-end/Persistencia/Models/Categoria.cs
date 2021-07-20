using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System;

namespace Persistencia.Models
{
    public class Categoria{

        [Key]
        public int Id{get;set;}

        [StringLength(50)]
        public string Tipo{get;set;}

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
    }
}
