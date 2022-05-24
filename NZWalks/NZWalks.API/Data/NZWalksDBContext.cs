using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data
{
    public class NZWalksDBContext : DbContext
    {
        public NZWalksDBContext(DbContextOptions<NZWalksDBContext> options) : base(options)
        {

        }
        /* Below are 3 properties of DbSet which basically tells
           EF that please create "Regions" table for us if it doesn't exist
           in the DB. So by giving the property like this (like DbSet),we can now use
           the "NZWalksDBContext" so that it can query/persist the data to the region table that
           EF core will create as part of migrations (we will see migrations later)
        */
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
        public DbSet<WalkDifficulty> WalkDifficulty { get; set; }
    }
}
