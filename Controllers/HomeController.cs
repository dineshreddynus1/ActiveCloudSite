using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Net.Http.Json;
using System.Text;
using UNIVERSITYJOBSPORTAL.Context;
using UNIVERSITYJOBSPORTAL.Dtos;
using UNIVERSITYJOBSPORTAL.Models;


namespace UNIVERSITYJOBSPORTAL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public DataContext dbContext;

        static string url = "https://api.adzuna.com/v1/api/jobs/gb/search/1?app_id=cde8ee0f&app_key=7727cfe477c798fcd754fb4d094c45ff&results_per_page=1000";
        static string api_key = "d3d998a782mshd5354a54cd09284p1b522ejsn43b7af1e5eee";
        static string host = "motorcycles-by-api-ninjas.p.rapidapi.com/";
        HttpClient httpClient;

        public HomeController(ILogger<HomeController> logger, DataContext context)
        {
            _logger = logger;
            dbContext=context;
        }

        public async Task<IActionResult> Index()
        {
            if (dbContext!=null  )
            {
                if (dbContext.departments.Count() == 0)
                {

                    List<DepartmentModel> department = new List<DepartmentModel>() { new DepartmentModel() { email = "collegeofEngineering@univ.edu" ,Name="CollegeOfEngineering"},
                new DepartmentModel() { email = "collegeofBusiness@univ.edu" ,Name="CollegeOfBusiness"},
                new DepartmentModel() { email = "Arts@univ.edu" ,Name="Arts college"} };
                    dbContext.departments.AddRange(department);
                }
                if (dbContext.jobs.Count() == 0)
                {
                    List<JobModel> jobsList = new List<JobModel>();

                   

                  //  string API_PATH = url;
                    string itemsData = "";


                    for (int k = 1; k < 11; k++)
                    {
                        string API_PATH = "https://api.adzuna.com/v1/api/jobs/gb/search/" + k.ToString() + "?app_id=cde8ee0f&app_key=7727cfe477c798fcd754fb4d094c45ff&results_per_page=1000";
                        httpClient = new HttpClient();
                        httpClient.BaseAddress = new Uri(API_PATH);
                       
                        httpClient.DefaultRequestHeaders.Accept.Clear();

                        httpClient.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                        httpClient.DefaultRequestHeaders.AcceptCharset.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("utf-8"));


                        using (HttpClient httpClient = new HttpClient())
                        {


                            HttpResponseMessage response = httpClient.GetAsync(API_PATH)
                                                                    .GetAwaiter().GetResult();



                            string responseContent = "";
                            if (response.IsSuccessStatusCode)
                            {
                                var contentType = response.Content.Headers.ContentType;
                                byte[] byteArray = await response.Content.ReadAsByteArrayAsync();
                                responseContent = Encoding.UTF8.GetString(byteArray);

                                // itemsData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                                // HttpContent content = new StringContent(response.Content.ReadAsStringAsync().GetAwaiter().GetResult(), Encoding.UTF8, "application/json");

                            }

                            if (!responseContent.Equals(""))
                            {
                                List<JobModel> job = new List<JobModel>();
                                JObject jsonObject = JObject.Parse(responseContent);
                                var res = jsonObject["results"];
                                dynamic List = JsonConvert.DeserializeObject<List<Object>>(res.ToString());
                                if (List != null)
                                {
                                    for (int i = 0; i < List.Count; i++)
                                    {
                                        job.Add(new JobModel { title = List[i].title, type = List[i].contract_type==null?"fulltime": List[i].contract_type, description = List[i].description, salary = List[i].salary_max, departmentId = (i % 2 == 0 ? 1 : 2), location = List[i].location.display_name, positions = 1 });

                                    }
                                }
                                jobsList.AddRange(job);
                            }




                        }
                    }

                    
                    dbContext.jobs.AddRange(jobsList);

                   await dbContext.SaveChangesAsync();

                }



            }
           



            dbContext.SaveChanges();
            return View("~/Views/Home/Home.cshtml");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult AboutUs()
        {

            return View("~/Views/Home/AboutUs.cshtml");
        }

        [HttpGet]
        public IActionResult ManageJobs()
        {
            List<JobsDTO> dTOs = new List<JobsDTO>();

            List<JobModel> models = dbContext.jobs.Count()==0? new List<JobModel>() : dbContext.jobs.ToList();
            List<DepartmentModel> departmentModels = dbContext.departments.ToList();
            Dictionary<int,string> dict = new Dictionary<int, string>();
            foreach (var di in departmentModels)
            {
                dict[di.Id] = di.Name;
            }

            foreach (var mi in models)
            {
                dTOs.Add(new JobsDTO() { department = dict[mi.departmentId], jobName = mi.title, jobID = mi.Id, Location = mi.location });    
            
            }
            return View("~/Views/Home/JobPosting.cshtml",dTOs);
        }

       

        [HttpGet]
        public IActionResult UpdateJob()
        {

            return View("~/Views/Home/UpdateJob.cshtml");
        }

        [HttpGet]
        public IActionResult GetApplicants()
        {

            return View("~/Views/Home/Applicants.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> createApplicant(JobApplicationDTO dto)
        {
            int appId = -1;
             if (dbContext.applicants.Where(x => x.univID == dto.ApplicantID).ToList().Count <= 0)
            {
                ApplicantModel model = new ApplicantModel();
                model.univID = dto.ApplicantID;
                model.phone = dto.phone;
                model.coverLetter = dto.coverLetter;
                model.email = dto.email;
                model.name = dto.Name;
                dbContext.applicants.Add(model);
               await dbContext.SaveChangesAsync();
            } 
            appId = dbContext.applicants.Where(x => x.univID == dto.ApplicantID).ToList().FirstOrDefault().applicantId;

          

            JobApplicantModel jobApplicantModel = new JobApplicantModel();
            jobApplicantModel.ApplicantID = appId;
            jobApplicantModel.JobID = dto.job.Id;
            dbContext.jobApplicants.Add(jobApplicantModel);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("ManageJobs", "Home");
        }

        [HttpGet]
        public IActionResult GetApplicantsByJobId(int id)
        {
            List<JobApplicantModel> modelList = dbContext.jobApplicants.Where(x => x.JobID == id).ToList();

            List<int> appIds = modelList.Select(x => x.ApplicantID).ToList();

            List<ApplicantModel> applicants = dbContext.applicants.Where(x=>appIds.Contains(x.applicantId)).ToList();


            return View("~/Views/Home/Applicants.cshtml",applicants );

        }

        [HttpGet]
        public IActionResult UpdateJobDetailsbyId(int id)
        {
            JobModel model = dbContext.jobs.Where(x => x.Id == id).ToList().FirstOrDefault();
            return View("~/Views/Home/UpdateJob.cshtml", model);

        }

        [HttpPost]
        public async Task<IActionResult> UpdateJobDetailsbyId(JobModel model)
        {
            dbContext.jobs.Update(model);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("ManageJobs", "Home");

        }

        [HttpGet]
        public async Task<IActionResult> DeleteJobDetailsbyId(int id)
        {
            JobModel model = dbContext.jobs.Where(x => x.Id == id).ToList().FirstOrDefault();
            List<JobApplicantModel> models = dbContext.jobApplicants.Where(x => x.JobID == id).ToList();
            dbContext.jobApplicants.RemoveRange(models);
            dbContext.jobs.Remove(model);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("ManageJobs", "Home");

        }

        [HttpGet]
        public IActionResult createJobPosting()
        {
            ViewBag.departmentList = GetDepartmentsList();
            return View("~/Views/Home/CreateJob.cshtml");
        }

        [HttpGet]
        public IActionResult GetJobDetailsbyId(int id)
        {
            JobApplicationDTO dto = new JobApplicationDTO();
            dto.job = new JobModel();
           JobModel model = dbContext.jobs.Where(x => x.Id == id).ToList().FirstOrDefault();
            dto.job.title = model.title;
            dto.job.salary = model.salary;
            dto.job.description = model.description;
            dto.job.positions   = model.positions;
            dto.job.location = model.location;
            dto.job.Id = model.Id;



            return View("~/Views/Home/CreateApplication.cshtml",dto);
        }


        

        [HttpPost]
        public async Task<IActionResult> createJobPosting(JobModel model)
        {
            model.departmentId = Int32.Parse(model.type);
            dbContext.jobs.Add(model);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("ManageJobs", "Home");
        }

        [HttpGet]

        public IActionResult getJobsList()
        {
            List<JobModel> model = dbContext.jobs.ToList();
            return View("~/Views/Home/JobList.cshtml", model);
        }

        private IEnumerable<SelectListItem> GetDepartmentsList()
        {

            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in dbContext.departments.ToList())
            {

                list.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Name });

            }
            return list;

        }

        public IActionResult GetData()


        {

            var data = dbContext.jobs.ToList();
            int total = data.Count;

            int permanent = data.Count(item => item.type == "permanent");
            int full = data.Count(item => item.type == "fulltime");
            int con = data.Count(item => item.type == "contract");

          


            return Json(new { permanent, full,con });
        }

    }
}