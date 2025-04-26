using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELawyer.Models.ViewModels
{
   public class LawyerEditViewModel
    {
        public int ID { get; set; }
        public Lawyer Lawyer { get; set; }
        public List<int> SelectedSpecialization { get; set; } = default!;
        public IEnumerable<SelectListItem> SpecializationList { get; set; } = Enumerable.Empty<SelectListItem>();
        //public List<SelectListItem> SpecializationList { get; set; } = new List<SelectListItem>();
    }
}
