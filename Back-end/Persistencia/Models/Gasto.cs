using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Persistencia.Models
{
    public class Gasto{

        [Key]
        public int Id{get;set;}

        [Required]
        public string Description{get;set;}

        [Required]
        [Column(TypeName="Date")]
        public DateTime Date { get; set; }

        [Required]
        public int Price{get;set;}
        
        [Required]
        public bool Pagado{get;set;}
        
        [Required]
        public bool Compartido{get;set;}
        
        [Required]
        public int CompartidoEntre{get;set;}
        
        [Required]
        public virtual Categoria categoria{get;set;}
    }
}
