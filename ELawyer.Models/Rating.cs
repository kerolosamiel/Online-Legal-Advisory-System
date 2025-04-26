using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELawyer.Models
{
    public class Rating
    {
        [Key]
        public int ID { get; set; }

       public string? Comment { get; set; }
       public int? Rate { get; set; }

        public DateTime? CreatedAt { get; set; }
        public int? lawyerID { get; set; }
        [ForeignKey("lawyerID")]
        public Lawyer? Lawyer { get; set; }

        public int? ClientID { get; set; }
        [ForeignKey("ClientID")]
        public Client? Client { get; set; }
    }
}
