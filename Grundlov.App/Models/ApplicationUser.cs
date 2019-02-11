using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Grundlov.App.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ICollection<Constitution> Constitutions { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<ConstitutionFollower> Following { get; set; }
    }
}
