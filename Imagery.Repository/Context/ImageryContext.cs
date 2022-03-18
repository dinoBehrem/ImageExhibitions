using Imagery.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagery.Repository.Context
{
    public class ImageryContext : IdentityDbContext
    {
        public ImageryContext(DbContextOptions<ImageryContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ExhibitionTopics>().HasKey(et => new { et.ExhibitionId, et.TopicId });
            builder.Entity<UserSubscription>().HasKey(us => new { us.SubscriberId, us.CreatorId });
            builder.Entity<ExhibitionSubscription>().HasKey(es => new { es.ExhibitionId, es.UserId });
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Exhibition> Exhibitions { get; set; }
        public DbSet<ExponentItem> ExponentItems { get; set; }
        public DbSet<Dimensions> Dimensions { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<ExhibitionTopics> ExhibitionTopics { get; set; }
        public DbSet<CollectionItem> CollectionItems { get; set; }
        public DbSet<UserSubscription> UserSubscriptions { get; set; }
        public DbSet<ExhibitionSubscription> ExhibitionSubscriptions { get; set; }
    }
}
