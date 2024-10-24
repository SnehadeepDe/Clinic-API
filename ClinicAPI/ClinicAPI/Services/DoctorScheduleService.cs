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
using System.Numerics;

namespace ClinicAPI.Services
{
    public class DoctorScheduleService : IDoctorScheduleService
    {
        private readonly IDoctorScheduleRepository _doctorScheduleRepository;
        private readonly IDoctorService _doctorService;
        private readonly IMapper _mapper;

        public DoctorScheduleService(IDoctorScheduleRepository doctorScheduleRepository, IDoctorService doctorService, IMapper mapper)
        {
            _doctorScheduleRepository = doctorScheduleRepository;
            _doctorService = doctorService;
            _mapper = mapper;
        }

        public int Create(int  doctorId, DoctorScheduleRequest doctorScheduleRequest)
        {
            var validationError = Validate(doctorId,doctorScheduleRequest);
            if (!string.IsNullOrEmpty(validationError))
            {
                throw new BadRequestException(validationError);
            }

            var doctorSchedule = _mapper.Map<DoctorSchedule>(doctorScheduleRequest);
            doctorSchedule.DoctorId = doctorId;
            doctorSchedule.StartTime = TimeSpan.Parse(doctorScheduleRequest.StartTime);
            doctorSchedule.EndTime = TimeSpan.Parse(doctorScheduleRequest.EndTime);
            return _doctorScheduleRepository.Create(doctorSchedule);
        }

        public void Delete(int doctorId, int id)
        {
            GetById(doctorId,id);
            _doctorScheduleRepository.Delete(id);
        }

        public List<DoctorScheduleResponse> GetAll(int doctorId)
        {
            _doctorService.GetById(doctorId); 

            var doctorSchedules = _doctorScheduleRepository.GetAll().Where(d => d.DoctorId == doctorId).ToList();
            var doctorScheduuleResponses = _mapper.Map<List<DoctorScheduleResponse>>(doctorSchedules)
               .Select(doctorScheduleResponse =>
               {
                   var doctorSchedule = doctorSchedules.First(d => d.Id == doctorScheduleResponse.Id);

                   doctorScheduleResponse.Doctor = _doctorService.GetById(doctorSchedule.DoctorId);
                   doctorScheduleResponse.StartTime = doctorSchedule.StartTime.ToString(@"hh\:mm");
                   doctorScheduleResponse.EndTime = doctorSchedule.EndTime.ToString(@"hh\:mm");

                   return doctorScheduleResponse;
               });

            return doctorScheduuleResponses.ToList();
        }

        public DoctorScheduleResponse GetById(int doctorId, int id)
        {
            var doctorSchedule = _doctorScheduleRepository.GetById(id);
            var doctor = _doctorService.GetById(doctorId);
            if (doctorSchedule == null)
            {
                throw new NotFoundException("Doctor schedule not found");
            }
            if (doctor == null)
            {
                throw new NotFoundException("Doctor not found");
            }
            if (doctorSchedule.DoctorId != doctorId)
            {
                throw new NotFoundException($"No schedule with schedule Id = {id} under Doctor Id = {doctorId}");
            }
            var doctorScheduleResponse = _mapper.Map<DoctorScheduleResponse>(doctorSchedule);

            doctorScheduleResponse.Doctor = doctor;
            doctorScheduleResponse.StartTime = doctorSchedule.StartTime.ToString(@"hh\:mm");
            doctorScheduleResponse.EndTime = doctorSchedule.EndTime.ToString(@"hh\:mm");
            return doctorScheduleResponse;
        }

        public void Update(int doctorId,int id, DoctorScheduleRequest doctorScheduleRequest)
        {
            GetById(doctorId, id);
            var validationError = Validate(doctorId,doctorScheduleRequest);
            if (!string.IsNullOrEmpty(validationError))
            {
                throw new BadRequestException(validationError);
            }
            var doctorSchedule = _mapper.Map<DoctorSchedule>(doctorScheduleRequest);
            doctorSchedule.Id = id;
            doctorSchedule.DoctorId = doctorId;
            doctorSchedule.StartTime = TimeSpan.Parse(doctorScheduleRequest.StartTime);
            doctorSchedule.EndTime = TimeSpan.Parse(doctorScheduleRequest.EndTime);
            _doctorScheduleRepository.Update(id, doctorSchedule);
        }

        private string Validate(int doctorId , DoctorScheduleRequest doctorScheduleRequest)
        {

            if (string.IsNullOrEmpty(doctorScheduleRequest.DayInWeek))
                return "Day of week is required";

            if (_doctorService.GetById(doctorId) == null)
                return "No doctor found with given Id.";

            if (!Enum.TryParse(typeof(DayOfWeek), doctorScheduleRequest.DayInWeek, true, out object dayOfWeekObj))
            {
                throw new BadRequestException("Invalid day of week");
            }

            if (!TimeSpan.TryParse(doctorScheduleRequest.StartTime, out TimeSpan startTime) ||!TimeSpan.TryParse(doctorScheduleRequest.EndTime, out TimeSpan endTime))
            {
                throw new BadRequestException("Invalid start or end time");
            }

            return null;
        }
    }
}
