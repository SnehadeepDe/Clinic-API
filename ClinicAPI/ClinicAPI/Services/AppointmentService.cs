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
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorService _doctorService;
        private readonly IDoctorScheduleService _doctorScheduleService;
        private readonly IMapper _mapper;

        public AppointmentService(IAppointmentRepository appointmentRepository, IPatientRepository patientRepository,
                                  IDoctorService doctorService,IDoctorScheduleService doctorScheduleService ,IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
            _doctorService = doctorService;
            _doctorScheduleService = doctorScheduleService;
            _mapper = mapper;
        }

        public int Create(AppointmentRequest appointmentRequest)
        {
            var validationError = Validate(appointmentRequest);
            if (!string.IsNullOrEmpty(validationError))
            {
                throw new BadRequestException(validationError);
            }

            var existingPatient = _patientRepository.GetAll().FirstOrDefault(p =>
                p.Name.Equals(appointmentRequest.Name, StringComparison.OrdinalIgnoreCase) &&
                p.Phone.Equals(appointmentRequest.Phone, StringComparison.OrdinalIgnoreCase) &&
                p.Email.Equals(appointmentRequest.Email, StringComparison.OrdinalIgnoreCase));

            int patientId;

            if (existingPatient == null)
            {
                var newPatient = new Patient
                {
                    Name = appointmentRequest.Name,
                    Phone = appointmentRequest.Phone,
                    Email = appointmentRequest.Email,
                };

                var mappedPatient = _mapper.Map<Patient>(newPatient);
                patientId = _patientRepository.Create(mappedPatient);
            }
            else
            {
                patientId = existingPatient.Id;
            }
            var appointment = _mapper.Map<Appointment>(appointmentRequest);
            appointment.PatientId = patientId;
            appointment.AppointmentDate = DateTime.Parse(appointmentRequest.AppointmentDate);
            appointment.AppointmentTime = TimeSpan.Parse(appointmentRequest.AppointmentTime);

            return _appointmentRepository.Create(appointment);
        }



        public void Delete(int id)
        {
            GetById(id);
            _appointmentRepository.Delete(id);
        }

        public List<AppointmentResponse> GetAll(int? patientId, int? doctorId)
        {
            var appointments = _appointmentRepository.GetAll();

            if (patientId.HasValue)
            {
                appointments = appointments.Where(a => a.PatientId == patientId.Value).ToList();
            }

            if (doctorId.HasValue)
            {
                appointments = appointments.Where(a => a.DoctorId == doctorId.Value).ToList();
            }

            var appointmentResponses = _mapper.Map<List<AppointmentResponse>>(appointments)
                .Select(appointmentResponse =>
                {
                    var appointment = appointments.First(a => a.Id == appointmentResponse.Id);

                    var patient = _patientRepository.GetById(appointment.PatientId);
                    appointmentResponse.Patient = _mapper.Map<PatientResponse>(patient);

                    var doctor = _doctorService.GetById(appointment.DoctorId);
                    appointmentResponse.Doctor = _mapper.Map<DoctorResponse>(doctor);

                    appointmentResponse.AppointmentTime = appointment.AppointmentTime.ToString(@"hh\:mm");

                    return appointmentResponse;
                }).ToList();

            return appointmentResponses;
        }

        public AppointmentResponse GetById(int id)
        {
            var appointment = _appointmentRepository.GetById(id);
            if (appointment == null)
            {
                throw new NotFoundException("Appointment not found");
            }

            var appointmentResponse = _mapper.Map<AppointmentResponse>(appointment);
            var patient = _patientRepository.GetById(appointment.PatientId);
            appointmentResponse.Patient = _mapper.Map<PatientResponse>(patient);
            appointmentResponse.Doctor = _doctorService.GetById(appointment.DoctorId);
            appointmentResponse.AppointmentTime = appointment.AppointmentTime.ToString(@"hh\:mm");

            return appointmentResponse;
        }

        public void Update(int id, AppointmentRequest appointmentRequest)
        {
            var appointmentDetails = GetById( id);
            var patient = _patientRepository.GetById(appointmentDetails.Patient.Id);
            var validationError = Validate(appointmentRequest);
            if (!string.IsNullOrEmpty(validationError))
            {
                throw new BadRequestException(validationError);
            }
            var appointment = _mapper.Map<Appointment>(appointmentRequest);

            
            if (patient == null)
            {
                throw new NotFoundException($"Patient with Id {appointment.PatientId} not found.");
            }
            appointment.Id = id;
            appointment.PatientId = patient.Id;
            appointment.AppointmentDate = DateTime.Parse(appointmentRequest.AppointmentDate);
            appointment.AppointmentTime = TimeSpan.Parse(appointmentRequest.AppointmentTime);

            _appointmentRepository.Update(id,appointment);
        }
        private string Validate(AppointmentRequest appointmentRequest)
        {
            if (string.IsNullOrEmpty(appointmentRequest.Name))
                return "Patient name is required";

            if (string.IsNullOrEmpty(appointmentRequest.Phone))
                return "Patient phone number is required";

            if (string.IsNullOrEmpty(appointmentRequest.Email))
                return "Patient email is required";

            if (_doctorService.GetById(appointmentRequest.DoctorId) == null)
                return "No Doctor Found";

            if (!DateTime.TryParse(appointmentRequest.AppointmentDate, out DateTime appointmentDate))
                throw new BadRequestException("Invalid AppointmentDate format.");
            
            if (!TimeSpan.TryParse(appointmentRequest.AppointmentTime, out TimeSpan appointmentTime))
                throw new BadRequestException("Invalid AppointmentTime format.");

            var doctorSchedules = _doctorScheduleService.GetAll(appointmentRequest.DoctorId);
            if (doctorSchedules == null || !doctorSchedules.Any())
                return "Doctor schedule not found";

            var matchingSchedule = doctorSchedules.FirstOrDefault(ds =>
                ds.DayInWeek.Equals(appointmentDate.DayOfWeek.ToString(), StringComparison.OrdinalIgnoreCase) &&
                appointmentTime >= TimeSpan.Parse(ds.StartTime) &&
                appointmentTime < TimeSpan.Parse(ds.EndTime));

            if (matchingSchedule == null)
                return "Appointment not possible on this day or time.";

            return null;
        }
    }
}
