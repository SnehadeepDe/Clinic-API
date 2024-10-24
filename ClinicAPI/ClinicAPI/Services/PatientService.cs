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
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;

        public PatientService(IPatientRepository patientRepository, IMapper mapper)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
        }

        public int Create(PatientRequest patientRequest)
        {
            var validationError = Validate(patientRequest);
            if (!string.IsNullOrEmpty(validationError))
            {
                throw new BadRequestException(validationError);
            }

            var patient = _mapper.Map<Patient>(patientRequest);
            return _patientRepository.Create(patient);
        }

        public void Delete(int id)
        {
            GetById(id);
            _patientRepository.Delete(id);
        }

        public List<PatientResponse> GetAll()
        {
            var patients = _patientRepository.GetAll();
            return _mapper.Map<List<PatientResponse>>(patients);
        }

        public PatientResponse GetById(int id)
        {
            var patient = _patientRepository.GetById(id);
            if (patient == null)
            {
                throw new NotFoundException("Patient not found");
            }
            return _mapper.Map<PatientResponse>(patient);
        }

        public void Update(int id, PatientRequest patientRequest)
        {
            GetById(id);
            var validationError = Validate(patientRequest);
            if (!string.IsNullOrEmpty(validationError))
            {
                throw new BadRequestException(validationError);
            }
            var patient = _mapper.Map<Patient>(patientRequest);
            patient.Id = id;
            _patientRepository.Update(id, patient);
        }

        private string Validate(PatientRequest patientRequest)
        {
            if (string.IsNullOrEmpty(patientRequest.Name))
                return "Patient name is required";

            if (string.IsNullOrEmpty(patientRequest.Phone))
                return "Patient phone number is required";

            if (string.IsNullOrEmpty(patientRequest.Email))
                return "Patient email is required";

            return null;
        }
    }
}
