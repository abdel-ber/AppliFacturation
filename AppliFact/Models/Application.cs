﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppliFact.Models
{
    
        public class ApplicationRole : IdentityRole<String>
    {
            public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
            public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; }
        }

        public class ApplicationUserRole : IdentityUserRole<string>
        {
            public virtual ApplicationUser User { get; set; }
            public virtual ApplicationRole Role { get; set; }
        }

        public class ApplicationUserClaim : IdentityUserClaim<string>
        {
            public virtual ApplicationUser User { get; set; }
        }

        public class ApplicationUserLogin : IdentityUserLogin<string>
        {
            public virtual ApplicationUser User { get; set; }
        }

        public class ApplicationRoleClaim : IdentityRoleClaim<string>
        {
            public virtual ApplicationRole Role { get; set; }
        }

        public class ApplicationUserToken : IdentityUserToken<string>
        {
            public virtual ApplicationUser User { get; set; }
        }
    }

