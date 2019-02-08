using System;

namespace Grundlov.Server.Models
{
    public class ConstitutionFollower
    {
        public Guid ConstitutionId { get; set; }
        public Constitution Constitution { get; set; }

        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}