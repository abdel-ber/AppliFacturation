using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppliFact.Models
{
    public class Paiement
    {
        public Guid Id { get; set; } =Guid.NewGuid();
        public String Moyen { get; set; }

        [DisplayFormat(DataFormatString = "{0:c}")]
        public Decimal Montant { get; set; }
        [Display(Name = "Date de Réception")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime DateReception { get; set; } = DateTime.Now;
        [Display(Name = "Date Banque")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? DateBanque { get; set; }

        public Guid IdClient { get; set; }
        public virtual Client Client { get; set; }
    }
}
