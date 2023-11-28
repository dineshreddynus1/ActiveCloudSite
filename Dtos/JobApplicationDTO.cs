using UNIVERSITYJOBSPORTAL.Models;

namespace UNIVERSITYJOBSPORTAL.Dtos
{
    public class JobApplicationDTO
    {
        public string ApplicantID { get; set; }
        public int jobID { get; set; }

        public string Name { get; set; }


        public string email { get; set; }

        public string phone { get; set; }

        public string gender { get; set; }

        public string coverLetter{ get; set; }

        public JobModel job { get; set; }
    }
}
/* This file contains jobapplicationDTO for this website*/