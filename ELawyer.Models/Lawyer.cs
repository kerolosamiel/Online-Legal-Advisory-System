using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELawyer.Models
{
    public class Lawyer
    {
        [Key]
        public int ID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ImageUrl { get; set; }
        public string? About { get; set; }
        public string? Address { get; set; }
       
      public int? NoOfClients { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
     
        public string? FrontCardImage { get; set; }

        public string? BackCardImage { get; set; }

        public string? UserStatus { get; set; }
        public string? LinceseNumber { get; set; }
        public int?   ExperienceYears { get; set; }
        public string? Linkedin { get; set; }
        public int ?  ConsultationFee { get; set; }
        public double? AverageRateing { get; set; }
     
        public ICollection<LawyerSpecialization> specializationnews { get; set; } = new List<LawyerSpecialization>();

       

        public int? ServiceID { get; set; }
        [ForeignKey(nameof(ServiceID))]
        public Service? Service { get; set; }

        public int? LawyerRatingID { get; set; }
        [ForeignKey(nameof(LawyerRatingID))]

        public ICollection<Rating>? Rating { get; set; } = new List<Rating>();

    }
}
