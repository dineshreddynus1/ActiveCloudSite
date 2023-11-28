using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace UNIVERSITYJOBSPORTAL.Models
{
    public class JobApplicantModel
    {
        public int jobApplicantID { get; set; }

        public int? JobID { get; set; }


        public int ApplicantID { get; set; }


        public JobModel jobModel { get; set; }

        public ApplicantModel applicantModel { get; set; }  



    }


    public class JobApplicantModelConfiguration : IEntityTypeConfiguration<JobApplicantModel>
    {
        public void Configure(EntityTypeBuilder<JobApplicantModel> builder)
        {
            builder.HasKey(x => new { x.jobApplicantID });

            builder.HasOne(x => x.jobModel).WithMany(x => x.JobApplicants).HasForeignKey(x => x.JobID).OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(x => x.applicantModel).WithMany(x => x.JobApplicants).HasForeignKey(x => x.ApplicantID).OnDelete(DeleteBehavior.ClientSetNull);
      
        }

    }
}
//This model helps in retrieving job applicant data