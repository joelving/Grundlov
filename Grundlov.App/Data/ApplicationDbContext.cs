using Grundlov.App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using System;

namespace Grundlov.App.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var instantConversion = new ValueConverter<Instant, long>(
                v => v.ToUnixTimeMilliseconds(),
                v => Instant.FromUnixTimeMilliseconds(v)
            );

            builder.Entity<Article>()
                .HasKey(a => new { a.Id, a.Revision });
            builder.Entity<Article>()
                .Property(e => e.Created).HasConversion(instantConversion);

            builder.Entity<ArticleComment>()
                .HasKey(a => new { a.Id, a.Revision });
            builder.Entity<ArticleComment>()
                .Property(e => e.Created).HasConversion(instantConversion);

            builder.Entity<Like>()
                .Property(e => e.Created).HasConversion(instantConversion);

            builder.Entity<ConstitutionFollower>()
                .HasKey(a => new { a.ConstitutionId, a.UserId });
        }
    }
}
