using ClinicAPI.Models.DB_Models;
using ClinicAPI.Repository.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace ClinicAPI.Repository
{
    public class AppointmentRepository : BaseRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(IOptions<ConnectionString> connectionString)
            : base(connectionString.Value.CLINICDB)
        {
        }

        public int Create(Appointment appointment)
        {
            const string query = @"
                INSERT INTO [Foundation].[Appointments] (PatientId, DoctorId, AppointmentDate, AppointmentTime)
                VALUES (@PatientId, @DoctorId, @AppointmentDate, @AppointmentTime);
                SELECT CAST(SCOPE_IDENTITY() as int)";

            return base.Create(query, appointment);
        }

        public void Delete(int id)
        {
            const string query = @"
                DELETE FROM [Foundation].[Appointments] WHERE [ID] = @Id;";

            base.Delete(query, id);
        }

        public List<Appointment> GetAll()
        {
            const string query = @"
                SELECT 
                    ID,
                    PatientId,
                    DoctorId,
                    AppointmentDate,
                    AppointmentTime
                FROM Foundation.Appointments;";

            return base.GetAll(query, null);
        }

        public Appointment GetById(int id)
        {
            const string query = @"
                SELECT 
                    ID,
                    PatientId,
                    DoctorId,
                    AppointmentDate,
                    AppointmentTime
                FROM Foundation.Appointments
                WHERE ID = @Id;";

            return base.GetById(query, id);
        }

        public void Update(int id, Appointment appointment)
        {
            const string query = @"
                UPDATE [Foundation].[Appointments]
                SET [PatientId] = @PatientId,
                    [DoctorId] = @DoctorId,
                    [AppointmentDate] = @AppointmentDate,
                    [AppointmentTime] = @AppointmentTime
                WHERE [ID] = @Id";

            var parameters = new
            {
                Id = id,
                appointment.PatientId,
                appointment.DoctorId,
                appointment.AppointmentDate,
                appointment.AppointmentTime
            };

            base.Update(query, parameters);
        }
    }
}
