using Microsoft.EntityFrameworkCore;
using SocialNetworkAnalyser.Entitites;

namespace SocialNetworkAnalyser.DAL
{
    public class SocialNetworkAnalyserContext(DbContextOptions<SocialNetworkAnalyserContext> options) : DbContext(options)
    {
        public DbSet<SocialNetworkAnalysis> SocialNetworkAnalysis  { get; set; }

        public DbSet<AnalyzedUser> AnalyzedUser { get; set; }

        public DbSet<AnalyzedUserToAnalyzedUser> AnalyzedUserToAnalyzedUser { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SocialNetworkAnalysis>().HasKey(ds => ds.Id);
            modelBuilder.Entity<AnalyzedUser>().HasKey(user => user.Id);
            modelBuilder.Entity<AnalyzedUserToAnalyzedUser>().HasKey(user => user.Id);
        }
    }
}
