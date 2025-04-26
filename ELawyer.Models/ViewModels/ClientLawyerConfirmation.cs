using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELawyer.Models.ViewModels
{
    public class ClientLawyerConfirmation
    {
        public int ID { get; set; }
        public Client? Client { get; set; }
        public Lawyer? Lawyer { get; set; }
    }
}
