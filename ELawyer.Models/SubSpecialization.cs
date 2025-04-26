using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELawyer.Models
{
    public class SubSpecialization
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public int SpecializationID { get; set; }
        [ForeignKey(nameof(SpecializationID))]

        public virtual Specializationnew Specialization { get; set; }
    } 
}
