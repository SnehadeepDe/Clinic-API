using ClinicAPI.Models.DB_Models;
using ClinicAPI.Repository.Interfaces;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace ClinicAPI.Repository
{
    public class DoctorRepository : BaseRepository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(IOptions<ConnectionString> connectionString)
            : base(connectionString.Value.CLINICDB)
        {
        }
        public int Create(Doctor doctor)
        {
            const string query = @"
INSERT INTO [Foundation].[Doctors] (Name, SpecializationId, Phone, Email, ExperienceYears)
VALUES (@Name, @SpecializationId, @Phone, @Email, @ExperienceYears)
SELECT CAST(SCOPE_IDENTITY() as int)";
            return base.Create(query, doctor);
        }

        public void Delete(int id)
        {
            const string query = @"
DELETE FROM [Foundation].[Doctors] WHERE [Id] = @Id;
DELETE FROM [Foundation].[DoctorAvailability] WHERE [DoctorId] = @Id;
DELETE FROM [Foundation].[Appointments] WHERE [DoctorId] = @Id;";
            base.Delete(query, id);
        }

        public List<Doctor> GetAll()
        {
            const string query = @"
                SELECT 
                    ID,
                    Name,
                    SpecializationID,
                    Phone,
                    Email,
                    ExperienceYears
                FROM Foundation.Doctors;";

            return base.GetAll(query,null);
        }

        public Doctor GetById(int id)
        {
            const string query = @"
                SELECT 
                    ID,
                    Name,
                    SpecializationID,
                    Phone,
                    Email,
                    ExperienceYears
                FROM Foundation.Doctors
                 WHERE Id = @Id;";
            return base.GetById(query, id);
        }

        public void Update(int id, Doctor doctor)
        {
            const string query = @"
                UPDATE [Foundation].[Doctors]
                SET [Name] = @Name, 
                [SpecializationID] = @SpecializationID, 
                [Phone] = @Phone, 
                [Email] = @Email,
                [ExperienceYears] = @ExperienceYears
                WHERE [Id] = @Id";

            var parameters = new
            {
                Id = id,
                doctor.Name,
                doctor.SpecializationId,
                doctor.Email,
                doctor.Phone,
                doctor.ExperienceYears
            };
            base.Update(query, parameters);
        }
    }
}
