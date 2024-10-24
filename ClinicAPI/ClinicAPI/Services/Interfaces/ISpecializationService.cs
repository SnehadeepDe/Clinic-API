using ClinicAPI.Models.Request_Models;
using ClinicAPI.Models.Response_Models;
using System.Collections.Generic;

namespace ClinicAPI.Services.Interfaces
{
    public interface ISpecializationService
    {
        List<SpecializationResponse> GetAll();
        SpecializationResponse GetById(int id);
        int Create(SpecializationRequest specializationRequest);
        void Update(int id, SpecializationRequest specializationRequest);
        void Delete(int id);
    }
}
