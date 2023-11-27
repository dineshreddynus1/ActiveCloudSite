using Microsoft.EntityFrameworkCore;
using UNIVERSITYJOBSPORTAL.Models;

namespace UNIVERSITYJOBSPORTAL.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {


        }
        public DbSet<JobModel> jobs { get; set; }

        public DbSet<DepartmentModel> departments { get; set; }

        public DbSet<ApplicantModel> applicants { get; set; }

        public DbSet<JobApplicantModel> jobApplicants { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DepartmentModelConfiguration());
            modelBuilder.ApplyConfiguration(new JobApplicantModelConfiguration());
            modelBuilder.ApplyConfiguration(new DepartmentModelConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicantModelConfiguration());


        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Server=SURUKK;Database=UniversityJobsPortal;Trusted_Connection=True;TrustServerCertificate=True");
        }


    }
}
