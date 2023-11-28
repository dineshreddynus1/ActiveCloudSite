using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace UNIVERSITYJOBSPORTAL.Models
{
    public class JobModel
    {
        public int Id { get; set; }


        public string title { get; set; }

        public string description { get; set; }


        public string type { get; set; }

        public string location { get; set; }

        public string salary { get; set; }

        public int positions { get; set; }

        public int departmentId { get; set; }

        public virtual List<JobApplicantModel> JobApplicants { get; set; }

        public DepartmentModel department { get; set; }

    }


    public class JobModelConfiguration : IEntityTypeConfiguration<JobModel>
    {
        public void Configure(EntityTypeBuilder<JobModel> builder)
        {
            builder.HasKey(x => new { x.Id });

            builder.HasOne(x => x.department).WithMany(x => x.jobs).HasForeignKey(x => x.departmentId).OnDelete(DeleteBehavior.ClientSetNull);
         }

    }
}
//This model helps in retrieving applicant job data