
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELawyer.Models
{
    public class ApplicationUser :IdentityUser
    {

        public string Role { get; set; }
        public int? ClientID { get; set; }
        [ForeignKey(nameof(ClientID))]

        public  Client? Client { get; set; } 

        public int? LawyerID { get; set; }
        [ForeignKey(nameof(LawyerID))]

        public  Lawyer? Lawyer { get; set; }

        public int? AdminID { get; set; }
        [ForeignKey(nameof(AdminID))]

        public Admin? Admin { get; set; }


    }
}
