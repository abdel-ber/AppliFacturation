using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppliFact.Models
{
    public class Client
    {
        public String OwnerID { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        [Display(Name="Raison sociale")]
        public String RaisonSociale { get; set; }
        [Required]
        public string Adresse { get; set; }
        [Required]
        public string CodePostal { get; set; }
        [Required]
        public string Ville { get; set; }
        public string Pays { get; set; }
        //public Guid UserId { get; set; }
        //public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Facture> Factures { get; set; }
        public virtual ICollection<Paiement> Paiements { get; set; }
       // public virtual ApplicationUser ApplicationUser { get; set; }
        [Display(Name = "Paiements")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal TotalPaiements
        {
            get
            {
                if (Paiements == null || Paiements.Count() == 0) return 0;
                return Paiements.Sum(c => c.Montant);
            }
        }
        [Display(Name = "Total échéances")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal TotalEcheances
        {
            get
            {
                if (Factures == null || Factures.Count() == 0) return 0;
                return Factures.Where(c => c.DateEcheance != null && c.DateEcheance < DateTime.Now).Sum(c => c.MontantTtc);
            }
        }
        [Display(Name = "Total du")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal TotalDu
        {
            get
            {
                if (Factures == null || Factures.Count() == 0) return 0;
                return Factures.Sum(c => c.MontantTtc);
            }
        }
        [Display(Name = "Retard")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal Retard
        {
            get
            {
                return TotalEcheances - TotalPaiements;
            }
        }
        [Display(Name = "Solde")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal Solde
        {
            get
            {
                return TotalPaiements - TotalDu;
            }
        }


    }
}
