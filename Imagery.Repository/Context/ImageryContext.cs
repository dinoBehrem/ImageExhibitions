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

        public DbSet<User> Users { get; set; }
        public DbSet<Exhibition> Exhibitions { get; set; }
        public DbSet<ExponentItem> ExponentItems { get; set; }
    }
}
