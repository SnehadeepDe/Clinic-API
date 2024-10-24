using AutoMapper;
using ClinicAPI.CustomException;
using ClinicAPI.Models.DB_Models;
using ClinicAPI.Models.Request_Models;
using ClinicAPI.Models.Response_Models;
using ClinicAPI.Repository.Interfaces;
using ClinicAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClinicAPI.Services
{
    public class SpecializationService : ISpecializationService
    {
        private readonly ISpecializationRepository _specializationRepository;
        private readonly IMapper _mapper;

        public SpecializationService(ISpecializationRepository specializationRepository, IMapper mapper)
        {
            _specializationRepository = specializationRepository;
            _mapper = mapper;
        }

        public int Create(SpecializationRequest specializationRequest)
        {
            var validationError = Validate(specializationRequest);
            if (!string.IsNullOrEmpty(validationError))
            {
                throw new BadRequestException(validationError);
            }

            var specialization = _mapper.Map<Specialization>(specializationRequest);
            return _specializationRepository.Create(specialization);
        }

        public void Delete(int id)
        {
            GetById(id);
            _specializationRepository.Delete(id);
        }

        public List<SpecializationResponse> GetAll()
        {
            var specializations = _specializationRepository.GetAll();
            return _mapper.Map<List<SpecializationResponse>>(specializations);
        }

        public SpecializationResponse GetById(int id)
        {
            var specialization = _specializationRepository.GetById(id);
            if (specialization == null)
            {
                throw new NotFoundException("Specialization not found");
            }
            return _mapper.Map<SpecializationResponse>(specialization);
        }

        public void Update(int id, SpecializationRequest specializationRequest)
        {
            GetById(id);
            var validationError = Validate(specializationRequest);
            if (!string.IsNullOrEmpty(validationError))
            {
                throw new BadRequestException(validationError);
            }
            var specialization = _mapper.Map<Specialization>(specializationRequest);
            specialization.Id = id;
            _specializationRepository.Update(id, specialization);
        }

        private string Validate(SpecializationRequest specializationRequest)
        {
            if (string.IsNullOrEmpty(specializationRequest.Name))
                return "Specialization name is required";

            return null;
        }
    }
}
