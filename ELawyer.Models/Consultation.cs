using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELawyer.Models
{
    public class Consultation
    {
        public int ID { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }


        public int? lawyerID { get; set; }
        [ForeignKey("lawyerID")]
        public Lawyer? Lawyer { get; set; }

        public int? ClientID { get; set; }
        [ForeignKey("ClientID")]
        public Client? Client { get; set; }


        public string? Attachments { get; set; }

        public int?     PaymentId { get; set; }
    }
}
