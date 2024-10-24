using ClinicAPI.Models.Request_Models;
using ClinicAPI.Models.Response_Models;
using System.Collections.Generic;

namespace ClinicAPI.Services.Interfaces
{
    public interface IDoctorService
    {
        List<DoctorResponse> GetAll(int? specializationId);
        DoctorResponse GetById(int id);
        int Create(DoctorRequest doctorRequest);
        void Update(int id, DoctorRequest doctorRequest);
        void Delete(int id);
        DoctorResponse GetBestDoctorForSpecialization(int specializationId, string day);
    }
}
