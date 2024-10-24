using ClinicAPI.Models.DB_Models;
using ClinicAPI.Models.Request_Models;
using ClinicAPI.Models.Response_Models;
using System.Collections.Generic;

namespace ClinicAPI.Repository.Interfaces
{
    public interface IPatientRepository
    {
        List<Patient> GetAll();
        Patient GetById(int id);
        int Create(Patient patient);
        void Update(int id, Patient patient);
        void Delete(int id);
    }
}
