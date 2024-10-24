using ClinicAPI.Models.DB_Models;
using ClinicAPI.Models.Request_Models;
using ClinicAPI.Models.Response_Models;
using System.Collections.Generic;

namespace ClinicAPI.Repository.Interfaces
{
    public interface ISpecializationRepository
    {
        List<Specialization> GetAll();
        Specialization GetById(int id);
        int Create(Specialization specialization);
        void Update(int id, Specialization specialization);
        void Delete(int id);
    }
}
