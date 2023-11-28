using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace UNIVERSITYJOBSPORTAL.Models
{
    public class ApplicantModel
    {
        public int applicantId { get; set; }

        public string name { get; set; }

        public string phone { get; set; }

        public string email { get; set; }

        public string coverLetter { get; set; }

        public string univID { get; set; }

        public virtual List<JobApplicantModel> JobApplicants { get; set; }
    }


    public class ApplicantModelConfiguration : IEntityTypeConfiguration<ApplicantModel>
    {
        public void Configure(EntityTypeBuilder<ApplicantModel> builder)
        {
            builder.HasKey(x => new { x.applicantId });


        }
    }
}
//This model helps in retrieving applicant data