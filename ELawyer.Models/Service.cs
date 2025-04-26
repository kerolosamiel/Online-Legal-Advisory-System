using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELawyer.Models
{
    public class Service
    {
        [Key]
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ServiceType { get; set; }
     
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public TimeOnly Duration { get; set; }

        public int LawyerID { get; set; }
        [ForeignKey(nameof(LawyerID))]
        public Lawyer? Lawyer { get; set; }

    }
}
