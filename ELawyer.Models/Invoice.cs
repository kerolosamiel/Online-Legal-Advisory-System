using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELawyer.Models
{
    public class Invoice
    {
        public int ID { get; set; }
        public int ServiceOrderID { get; set; }
        public DateTime CreatedAt { get; set; }
         public int paymentID { get; set; }
        public double Amount { get; set; }

        public int clientID { get; set; }
       



    }
}
