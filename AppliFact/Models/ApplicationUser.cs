using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppliFact.Models
{
    public class ApplicationUser : IdentityUser<string>
    {
        public string CustomTag { get; set; }
        public virtual ICollection<ApplicationUserClaim> Claims { get; set; }
    public virtual ICollection<ApplicationUserLogin> Logins { get; set; }
    public virtual ICollection<ApplicationUserToken> Tokens { get; set; }
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        public virtual  ICollection<Facture> Factures { get; set; }
        [PersonalData]
        [Required]
        public string RaisonSocile { get; set; }

        [PersonalData]
        
        public byte[] Logo { get; set; }
        [PersonalData]
        [Required]
        public string Adresse { get; set; }
        [PersonalData]
        [Required]
        public string CodePostal { get; set; }
        [PersonalData]
        [Required]
        public string Ville { get; set; }
        [PersonalData]
        public string Pays { get; set; }
        //public virtual ICollection<Client> Clients { get; set; }
    }
}
