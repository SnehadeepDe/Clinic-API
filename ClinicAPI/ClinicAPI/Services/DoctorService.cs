using AutoMapper;
using ClinicAPI.CustomException;
using ClinicAPI.Models.DB_Models;
using ClinicAPI.Models.Request_Models;
using ClinicAPI.Models.Response_Models;
using ClinicAPI.Repository;
using ClinicAPI.Repository.Interfaces;
using ClinicAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace ClinicAPI.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly ISpecializationService _specializationService;
        private readonly IMapper _mapper;
        private readonly IDoctorScheduleRepository _doctorScheduleRepository;
        private readonly IAppointmentRepository _appointmentRepository;

        public DoctorService(IDoctorRepository doctorRepository,ISpecializationService specializationService ,
            IMapper mapper, IDoctorScheduleRepository doctorScheduleRepository, IAppointmentRepository appointmentRepository)
        {
            _doctorRepository = doctorRepository;
            _specializationService = specializationService;
            _mapper = mapper;
            _doctorScheduleRepository = doctorScheduleRepository;
            _appointmentRepository = appointmentRepository;
        }
        public int Create(DoctorRequest doctorRequest)
        {
            var validationError = Validate(doctorRequest);
            if (!string.IsNullOrEmpty(validationError))
            {
                throw new BadRequestException(validationError);
            }

            var doctor = _mapper.Map<Doctor>(doctorRequest);
            return _doctorRepository.Create(doctor);
        }

        public void Delete(int id)
        {
            GetById(id);
            _doctorRepository.Delete(id);
        }

        public List<DoctorResponse> GetAll(int? specializationId)
        {
            var doctors = _doctorRepository.GetAll();
            if(specializationId.HasValue)
                doctors = doctors.Where(d => d.SpecializationId == specializationId.Value).ToList();
            var doctorResponses = _mapper.Map<List<DoctorResponse>>(doctors)
                .Select(doctorResponse =>
                {
                    var doctor = doctors.First(d => d.Id == doctorResponse.Id);

                    doctorResponse.Specialization = _specializationService.GetById(doctor.SpecializationId);

                    return doctorResponse;
                });
            return doctorResponses.ToList();
        }

        public DoctorResponse GetById(int id)
        {
            var doctor = _doctorRepository.GetById(id);
            if (doctor == null)
            {
                throw new NotFoundException("Doctor not found");
            }
            var doctorResponse = _mapper.Map<DoctorResponse>(doctor);

            doctorResponse.Specialization = _specializationService.GetById(doctor.SpecializationId);

            return doctorResponse;
        }

        public void Update(int id, DoctorRequest doctorRequest)
        {
            GetById(id);
            var validationError = Validate(doctorRequest);
            if (!string.IsNullOrEmpty(validationError))
            {
                throw new BadRequestException(validationError);
            }
            var doctor = _mapper.Map<Doctor>(doctorRequest);
            doctor.Id = id;
            _doctorRepository.Update(id, doctor);
        }
        public DoctorResponse GetBestDoctorForSpecialization(int specializationId, string day)
        {
            var doctors = GetAll(specializationId);
            var availableDoctors = new List<DoctorResponse>();
            DoctorResponse bestDoctor = null;
            var daysOfWeek = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            var dayIndex = daysOfWeek.IndexOf(day);

            for (int i = 0; i < 7; i++)
            {
                var currentDayIndex = (dayIndex + i) % 7;

                var currentDay = daysOfWeek[currentDayIndex];

                foreach (var doctor in doctors)
                {
                    var schedules = _doctorScheduleRepository.GetAll().Where(d => d.Id == doctor.Id).ToList();
                    var scheduleForDay = schedules.Any(s => s.DayInWeek.Equals(currentDay, StringComparison.OrdinalIgnoreCase));

                    if (scheduleForDay)
                    {
                        availableDoctors.Add(doctor);
                    }
                }

                bestDoctor = FindBestDoctor(availableDoctors);

                if (bestDoctor!=null)
                {
                    return bestDoctor;
                }
                availableDoctors.Clear();
            }
            return null;
        }


        private DoctorResponse FindBestDoctor(List<DoctorResponse> doctors)
        {
            var experiencedDoctors = doctors.Where(d => d.ExperienceYears > 10).ToList();

            if (experiencedDoctors.Any())
                return experiencedDoctors.OrderByDescending(d => d.ExperienceYears).FirstOrDefault();

            var doctorsWithMostAppointments = new List<DoctorResponse>();
            foreach(var doctor in doctors )
            {
                var appointments = _appointmentRepository.GetAll().Where(a => a.DoctorId == doctor.Id).ToList();
                if (appointments.Count > 20)
                    doctorsWithMostAppointments.Add(doctor);
            }
            

            if (doctorsWithMostAppointments.Any())
            {
                return doctorsWithMostAppointments.OrderByDescending(d => d.ExperienceYears).FirstOrDefault();
            }

            return null;
        }


        private string Validate(DoctorRequest doctorRequest)
        {
            if (string.IsNullOrEmpty(doctorRequest.Name))
                return "Doctor name is required";

            var specialization = _specializationService.GetById(doctorRequest.SpecializationId);
            if (specialization == null)
                return "Valid specialization ID is required";

            if (string.IsNullOrEmpty(doctorRequest.Phone))
                return "Doctor phone number is required";

            if (string.IsNullOrEmpty(doctorRequest.Email))
                return "Doctor email is required";

            if (doctorRequest.ExperienceYears < 0)
                return "Experience years must be a non-negative value";

            return null;
        }

    }
}
