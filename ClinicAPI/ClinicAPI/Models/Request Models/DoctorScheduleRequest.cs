using System;

namespace ClinicAPI.Models.Request_Models
{
    public class DoctorScheduleRequest
    {
        public int DoctorId { get; set; }
        public string DayInWeek { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
