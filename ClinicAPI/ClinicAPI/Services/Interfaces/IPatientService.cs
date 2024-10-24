using ClinicAPI.Models.Request_Models;
using ClinicAPI.Models.Response_Models;
using System.Collections.Generic;

namespace ClinicAPI.Services.Interfaces
{
    public interface IPatientService
    {
        List<PatientResponse> GetAll();
        PatientResponse GetById(int id);
        int Create(PatientRequest patientRequest);
        void Update(int id , PatientRequest patientRequest);
        void Delete(int id);

    }
}
