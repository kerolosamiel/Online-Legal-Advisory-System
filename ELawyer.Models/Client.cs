using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELawyer.Models
{
   public class Client
    {
        [Key]
        public int ID { get; set; }
        
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ImageUrl { get; set; }
        public string? Address { get; set; }
     
        public int? NoOfLawyers { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
   

        public string? ClientType { get; set; }
        public string? FrontCardImage { get; set; }
        public string? BackCardImage { get; set; }
        public string? UserStatus { get; set; }
     
       

        public int? ClientRatingID { get; set; }
        [ForeignKey(nameof(ClientRatingID))]

        public ICollection<Rating>? Rating { get; set; } = new List<Rating>();


    }
}
