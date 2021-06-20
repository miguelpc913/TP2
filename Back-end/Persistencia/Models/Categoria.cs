using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System;

namespace Persistencia.Models
{
    [Index(nameof(Tipo), IsUnique = true)]
    public class Categoria{

        [Key]
        public int Id{get;set;}

        [StringLength(450)]
        
        public string Tipo{get;set;}
    }
}
