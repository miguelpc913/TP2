
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System;

namespace Persistencia.Models
{
    [Index(nameof(Username), IsUnique = true)]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [StringLength(20)]
        [Required]
        public string Username{get;set;}

        [Required]
        public string Password{get;set;}
        public string Token { get; set; }
        public virtual List<Categoria> Categorias { get; set; }
        public virtual List<Gasto> Gastos { get; set; }
    }
}