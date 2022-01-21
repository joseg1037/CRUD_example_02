using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Beispiel_03.ETL
{
    public class Besitzer
    {
        [Display(Name = "Ausweis")]
        [Required(ErrorMessage = "Ausweis eingeben")]
        public string Ausweis { get; set; }
        public string Name { get; set; }
    }
}