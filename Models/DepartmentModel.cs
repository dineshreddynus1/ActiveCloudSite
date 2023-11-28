using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace UNIVERSITYJOBSPORTAL.Models
{
    public class DepartmentModel
    {
        public int Id { get; set; } 

        public string Name { get; set; }

        public string email { get; set; }

        public virtual List<JobModel> jobs { get; set; }


    }

    public class DepartmentModelConfiguration : IEntityTypeConfiguration<DepartmentModel>
    {
        public void Configure(EntityTypeBuilder<DepartmentModel> builder)
        {
            builder.HasKey(x => new { x.Id });
            

        }
    }
}
//This model helps in retrieving department data