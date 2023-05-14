namespace JobApplicationTrackerAPI.Models
{
    public class JobApplication
    {
        public int Id { get; set; }
        public string Company { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string ApplicationStatus { get; set; } = string.Empty;
    }
}
