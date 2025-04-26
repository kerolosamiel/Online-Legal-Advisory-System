using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELawyer.Models
{
   
    public class LawyerSpecialization
    {
        
        public int LawyerId { get; set; }
        public Lawyer Lawyer { get; set; }

        public int SpecializationId { get; set; }
        public Specializationnew Specialization { get; set; }

    }
}
