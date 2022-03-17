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
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Exhibition> Exhibitions { get; set; }
        public DbSet<ExponentItem> ExponentItems { get; set; }
        public DbSet<Dimensions> Dimensions { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<ExhibitionTopics> ExhibitionTopics { get; set; }
        public DbSet<CollectionItem> CollectionItems { get; set; }
    }
}
