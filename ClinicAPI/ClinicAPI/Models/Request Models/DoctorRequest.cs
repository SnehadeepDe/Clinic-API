using System.Collections.Generic;

namespace ClinicAPI.Models.Request_Models
{
    public class DoctorRequest
    {
        public string Name { get; set; }
        public int SpecializationId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int ExperienceYears { get; set; }
    }

}
