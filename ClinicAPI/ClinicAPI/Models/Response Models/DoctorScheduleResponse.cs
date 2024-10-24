using ClinicAPI.Models.DB_Models;
using System;

namespace ClinicAPI.Models.Response_Models
{
    public class DoctorScheduleResponse
    {
        public int Id { get; set; }
        public DoctorResponse Doctor { get; set; }
        public string DayInWeek { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
