using NodaTime;
using System;

namespace Grundlov.App.Models
{
    public class Like
    {
        public Guid Id { get; set; }
        public Instant Created { get; set; }

        public Guid OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }

        public Guid ArticleId { get; set; }
        public Article Article { get; set; }
    }
}