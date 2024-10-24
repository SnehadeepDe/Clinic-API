using AutoMapper;
using ClinicAPI.Models.DB_Models;
using ClinicAPI.Models.Request_Models;
using ClinicAPI.Models.Response_Models;

namespace ClinicAPI.Helper
{
    public class ApplicationMapper:Profile
    {
        public ApplicationMapper()
        {
            CreateMap<DoctorRequest, Doctor>();
            CreateMap<Doctor, DoctorResponse>();

            CreateMap<PatientRequest, Patient>();
            CreateMap<Patient, PatientResponse>();

            CreateMap<SpecializationRequest, Specialization>();
            CreateMap<Specialization, SpecializationResponse>();

            CreateMap<AppointmentRequest, Appointment>();
            CreateMap<Appointment, AppointmentResponse>();

            CreateMap<DoctorScheduleRequest, DoctorSchedule>();
            CreateMap<DoctorSchedule, DoctorScheduleResponse>();
        }
    }
}
