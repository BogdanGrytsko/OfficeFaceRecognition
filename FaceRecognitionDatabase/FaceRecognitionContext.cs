using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace FaceRecognitionDatabase
{
    public class FaceRecognitionContext : DbContext
    {
        public DbSet<LogCommand> LogCommands { get; set; }
        public DbSet<ImageLabel> ImageLabels { get; set; }
        public DbSet<LogRecognition> LogRecognitions { get; set; }
        public DbSet<UserLabel> UserLabels { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
             => options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=FaceRecognition;Trusted_Connection=True");
        //  => options.UseSqlServer(ConfigurationManager.ConnectionStrings["FaceRecognitionDatabase"].ConnectionString);
    }
}
