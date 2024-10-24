using ClinicAPI.Models.DB_Models;
using ClinicAPI.Models.Request_Models;
using ClinicAPI.Models.Response_Models;
using System.Collections.Generic;

namespace ClinicAPI.Services.Interfaces
{
    public interface IDoctorScheduleService
    {
        int Create(int doctorId, DoctorScheduleRequest scheduleRequest);
        void Delete(int doctorId, int id);
        List<DoctorScheduleResponse> GetAll(int doctorId);
        DoctorScheduleResponse GetById(int doctorId, int id);
        void Update(int doctorId, int id, DoctorScheduleRequest scheduleRequest);
    }
}
