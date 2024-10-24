using ClinicAPI.Models.Request_Models;
using ClinicAPI.Models.Response_Models;
using System.Collections.Generic;

namespace ClinicAPI.Services.Interfaces
{
    public interface IAppointmentService
    {
        List<AppointmentResponse> GetAll(int? patientId,int? doctorId);
        AppointmentResponse GetById(int id);
        int Create(AppointmentRequest appointmentRequest);
        void Update(int id,  AppointmentRequest appointmentRequest);
        void Delete(int id);
    }
}
