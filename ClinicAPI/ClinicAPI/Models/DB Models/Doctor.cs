namespace ClinicAPI.Models.DB_Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SpecializationId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int ExperienceYears { get; set; }
    }
}
