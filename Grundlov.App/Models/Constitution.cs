using System;
using System.Collections.Generic;

namespace Grundlov.App.Models
{
    public class Constitution
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public Guid OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }

        public ICollection<Article> Articles { get; set; }

        public ICollection<ConstitutionFollower> Followers { get; set; }
    }
}