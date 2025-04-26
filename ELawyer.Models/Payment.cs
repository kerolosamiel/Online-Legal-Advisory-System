using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELawyer.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public double Amount { get; set; }

         public DateTime? PaidAt { get; set; }

        public DateTime? Recievedat { get; set; }
        public int? lawyerID { get; set; }
        [ForeignKey("lawyerID")]
        public Lawyer? Lawyer { get; set; }

        public int? ClientID { get; set; }
        [ForeignKey("ClientID")]
        public Client? Client { get; set; }
    }
}
