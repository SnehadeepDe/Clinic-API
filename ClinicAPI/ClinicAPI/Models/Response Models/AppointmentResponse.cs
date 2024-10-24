using ClinicAPI.Models.DB_Models;
using System;

namespace ClinicAPI.Models.Response_Models
{
    public class AppointmentResponse
    {
        public int Id { get; set; }
        public PatientResponse Patient { get; set; }
        public DoctorResponse Doctor { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
    }
}
