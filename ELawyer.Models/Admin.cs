using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELawyer.Models
{
   public class Admin
   {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
        public string? Address { get; set; }



      
    }
}
