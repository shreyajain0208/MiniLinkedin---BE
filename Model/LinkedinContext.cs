using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentDemo.Model
{
    public class LinkedinContext : DbContext
    {
        public LinkedinContext(DbContextOptions<LinkedinContext> options) : base(options)
        {

        }

        public DbSet<LinkedInPost> LinkedIn { get; set; }
        public DbSet<LinkedInComments> LinkedInComments { get; set; }
        public DbSet<LinkedInLikes> LinkedInLikes { get; set; }
       // public DbSet<LinkedIn> LinkedIns { get; set; }

    }
}
