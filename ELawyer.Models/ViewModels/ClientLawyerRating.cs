using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELawyer.Models.ViewModels
{
    public class ClientLawyerRating
    {
        public int ID { get; set; }
        public Lawyer Lawyer { get; set; }

        public Payment Payment { get; set; }
        public IEnumerable<Rating> Rating { get; set; }
        
}
}
