﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELawyer.Models.ViewModels
{
    public class ConsultationPaymentViewModel
    {
        public int ID { get; set; }
        public Payment Payment { get; set; }
        public Consultation Consultation { get; set; }
    }
}
