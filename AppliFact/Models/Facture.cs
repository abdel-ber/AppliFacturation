using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppliFact.Models
{
    public class Facture
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string RaisonSocile { get; set; }



        public byte[] Logo { get; set; }


        public string Adresse { get; set; }

        public string CodePostal { get; set; }

        public string Ville { get; set; }

        public string Pays { get; set; }

        //[RegularExpression(@"\d{}")]
        //[DisplayFormat(NullDisplayText = "-----")]
        [Display(Name = "Numéro facture")]
        public string NumeroFacture { get; set; }
        [Display(Name = "Date de facturation")]
        
        [DisplayFormat(DataFormatString = "{0:d}")]

        public DateTime DateCreation { get; set; } = DateTime.Now;
        [DisplayFormat(DataFormatString = "{0:d}")]

        public DateTime? DateEdition { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]

        public DateTime DateEcheance { get; set; } = DateTime.Now.AddDays(30);
        public Guid IdClient { get; set; }
        public virtual ICollection<Prestation> Prestations { get; set; }
        public virtual Client Client { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal MontantTtc
        {
            get
            {
                if (this.Prestations == null) return 0m;
                return this.Prestations.Sum(c => c.MontantTtc);
            }
        }

        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal MontantTva
        {
            get
            {
                if (this.Prestations == null) return 0m;
                return this.Prestations.Sum(c => c.MontanTva);
            }
        }
        [DisplayFormat(DataFormatString = "{0:c}")]

        public decimal MontantHorsTax
        {
            get
            {
                if (this.Prestations == null) return 0m;
                return this.Prestations.Sum(c => c.MontantHorsTax);
            }
        }

        public bool CanCreate
        {
            get
            {
                return String.IsNullOrWhiteSpace(NumeroFacture);
            }
        }
        public bool IsValidated
        {
            get
            {
                return DateEdition != null;
            }
            
        }
        public bool CanValidate
        {
            get
            {
                if (IsValidated) return false;
                if (Prestations == null) return false;
                //return this.IsValidated;
                return Prestations.Count() > 0;
            }
        }
    }
}
