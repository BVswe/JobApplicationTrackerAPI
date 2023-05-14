using Dapper;
using JobApplicationTrackerAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace JobApplicationTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobApplicationController : ControllerBase
    {
        private readonly IConfiguration _config;

        public JobApplicationController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<ActionResult<List<JobApplication>>> GetAllJobApplications()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            IEnumerable<JobApplication> applications = await SelectAllJobApplcations(connection);
            return Ok(applications);
        }

        [HttpGet("{companyName}")]
        public async Task<ActionResult<List<JobApplication>>> GetCompanyJobApplications(string companyName)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            IEnumerable<JobApplication> applications = await connection.QueryAsync<JobApplication>("SELECT * FROM JobApplications WHERE Company=@cName",
                new {cName=companyName});
            return Ok(applications);
        }

        [HttpPost]
        public async Task<ActionResult<JobApplication>> CreateJobApplication(JobApplication app)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

            await connection.ExecuteAsync(
                "INSERT INTO JobApplications(Company, Position, ApplicationStatus) VALUES (@Company, @Position, @ApplicationStatus)",
                app);
            return Ok(await SelectAllJobApplcations(connection));
        }

        [HttpPut]
        public async Task<ActionResult<List<JobApplication>>> UpdateJobApplication(JobApplication app)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync(
                "UPDATE JobApplications SET Company=@Company, Position=@Position, ApplicationStatus=@ApplicationStatus WHERE ID=@Id",
                app);
            return Ok(await SelectAllJobApplcations(connection));
        }

        [HttpDelete("{applicationId}")]
        public async Task<ActionResult<List<JobApplication>>> DeleteJobApplication(int applicationId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync(
                "DELETE FROM JobApplications WHERE ID=@Id",
                new {Id = applicationId});
            return Ok(await SelectAllJobApplcations(connection));
        }

        private static async Task<IEnumerable<JobApplication>> SelectAllJobApplcations(SqlConnection connection)
        {
            return await connection.QueryAsync<JobApplication>("SELECT * FROM JobApplications");
        }
    }
}
