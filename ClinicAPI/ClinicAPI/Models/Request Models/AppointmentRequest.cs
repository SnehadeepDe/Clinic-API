using System;

namespace ClinicAPI.Models.Request_Models
{
    public class AppointmentRequest
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int DoctorId { get; set; }
        public string AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
    }
}
