using ClinicAPI.Models.DB_Models;
using System.Collections.Generic;

namespace ClinicAPI.Repository.Interfaces
{
    public interface IDoctorScheduleRepository
    {
        int Create( DoctorSchedule schedule);
        void Delete( int id);
        List<DoctorSchedule> GetAll();
        DoctorSchedule GetById(int id);
        void Update( int id, DoctorSchedule schedule);
    }
}
