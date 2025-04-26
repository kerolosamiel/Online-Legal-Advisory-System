using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELawyer.Models
{
    public class ServiceOrders
    {


        public int ID { get; set; }
        public int ServiceID { get; set; }
        public double PaymentID { get; set; }
        public int ClientID { get; set; }
        public int LawyerID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ScheduledAt { get; set; }
        public double Amount { get; set; }


    }
}
