using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppliFact.Models
{
    public class Prestation
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Display(Name = "Montant Hors Tax")]
        public decimal MontantHorsTax { get; set; }
        [Display(Name = "Taux Tva")]
        public decimal TauxTva { get; set; }

        [Display(Name = "Montant Tva")]
        public decimal MontanTva
        {
            get { return this.MontantHorsTax * TauxTva / 100; }
            
        }

        [Display(Name = "Montant Ttc") ]
        public decimal MontantTtc
        {
            get { return this.MontantHorsTax+this.MontanTva; }
            
        }
        [MinLength(2, ErrorMessage = "Le {0} doit avoir au moins {1} charactères")]
        public string Description { get; set; }
        public Guid IdFacture { get; set; }
        public virtual Facture Facture { get; set; }
        public bool IsValidated
        {
            get
            {
                if (this.Facture == null) return true;
                return this.Facture.IsValidated;
            }
        }

    }
}
