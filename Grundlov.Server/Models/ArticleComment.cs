using NodaTime;
using System;

namespace Grundlov.Server.Models
{
    public class ArticleComment
    {
        public Guid Id { get; set; }
        public int Revision { get; set; }

        public string Text { get; set; }
        public Instant Created { get; set; }
    }
}