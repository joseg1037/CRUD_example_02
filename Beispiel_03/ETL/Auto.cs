using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Beispiel_03.ETL
{
    public class Auto
    {
        [Display(Name = "Code")]
        [Required(ErrorMessage = "Code eingeben")]
        public string Code { get; set; }
        [Display(Name = "Marke")]
        public string Marke { get; set; }
        [Display(Name = "Kilometerstand")]        
        public decimal Kilometerstand { get; set; }        
        public DateTime Herstellungsdatum { get; set; }
    }
}