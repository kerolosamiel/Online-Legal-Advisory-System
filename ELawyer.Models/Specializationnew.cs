using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELawyer.Models
{
   public  class Specializationnew
    {
        [Key]
        
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        public virtual ICollection<SubSpecialization> SubSpecializations { get; set; }

        public  ICollection<Lawyer> Lawyers { get; set; }
    }
}
