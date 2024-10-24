using ClinicAPI.Models.DB_Models;
using ClinicAPI.Models.Request_Models;
using ClinicAPI.Models.Response_Models;
using System.Collections.Generic;

namespace ClinicAPI.Repository.Interfaces
{
    public interface IAppointmentRepository
    {
        List<Appointment> GetAll();
        Appointment GetById(int id);
        int Create(Appointment appointment);
        void Update(int id, Appointment appointment);
       // void UpdateTime(int id, UpdateAppointmentTimeRequest updateTimeRequest);
        void Delete(int id);
    }
}
