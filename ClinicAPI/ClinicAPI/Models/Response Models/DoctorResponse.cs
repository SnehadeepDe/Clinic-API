using ClinicAPI.Models.DB_Models;

namespace ClinicAPI.Models.Response_Models
{
    public class DoctorResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public SpecializationResponse Specialization { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int ExperienceYears { get; set; }
    }
}
