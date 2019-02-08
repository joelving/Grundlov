using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grundlov.Server.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ICollection<Constitution> Constitutions { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<ConstitutionFollower> Following { get; set; }
    }
}
