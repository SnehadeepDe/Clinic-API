using ClinicAPI.Models.DB_Models;
using ClinicAPI.Repository.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace ClinicAPI.Repository
{
    public class DoctorScheduleRepository : BaseRepository<DoctorSchedule>, IDoctorScheduleRepository
    {
        public DoctorScheduleRepository(IOptions<ConnectionString> connectionString)
            : base(connectionString.Value.CLINICDB)
        {
        }

        public int Create(DoctorSchedule doctorSchedule)
        {
            const string query = @"
                INSERT INTO [Foundation].[DoctorAvailability] (DoctorId, DayInWeek, StartTime, EndTime)
                VALUES (@DoctorId, @DayInWeek, @StartTime, @EndTime);
                SELECT CAST(SCOPE_IDENTITY() as int)";

            return base.Create(query, doctorSchedule);
        }

        public void Delete(int id)
        {
            const string query = @"
                DELETE FROM [Foundation].[DoctorAvailability] WHERE [ID] = @Id;";

            base.Delete(query, id);
        }

        public List<DoctorSchedule> GetAll()
        {
            const string query = @"
                SELECT 
                    ID,
                    DoctorId,
                    DayInWeek,
                    StartTime,
                    EndTime
                FROM Foundation.DoctorAvailability;";

            return base.GetAll(query, null);
        }

        public DoctorSchedule GetById(int id)
        {
            const string query = @"
                SELECT 
                    ID,
                    DoctorId,
                    DayInWeek,
                    StartTime,
                    EndTime
                FROM Foundation.DoctorAvailability
                WHERE ID = @Id;";

            return base.GetById(query, id);
        }

        public void Update(int id, DoctorSchedule doctorSchedule)
        {
            const string query = @"
                UPDATE [Foundation].[DoctorAvailability]
                SET [DoctorId] = @DoctorId,
                    [DayInWeek] = @DayInWeek,
                    [StartTime] = @StartTime,
                    [EndTime] = @EndTime
                WHERE [ID] = @Id";

            var parameters = new
            {
                Id = id,
                doctorSchedule.DoctorId,
                doctorSchedule.DayInWeek,
                doctorSchedule.StartTime,
                doctorSchedule.EndTime
            };

            base.Update(query, parameters);
        }
    }
}
