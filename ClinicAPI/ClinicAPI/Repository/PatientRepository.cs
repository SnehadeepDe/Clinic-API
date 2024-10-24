using ClinicAPI.Models.DB_Models;
using ClinicAPI.Repository.Interfaces;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace ClinicAPI.Repository
{
    public class PatientRepository : BaseRepository<Patient>, IPatientRepository
    {
        public PatientRepository(IOptions<ConnectionString> connectionString)
            : base(connectionString.Value.CLINICDB)
        {
        }

        public int Create(Patient patient)
        {
            const string query = @"
                INSERT INTO [Foundation].[Patients] (Name, Phone, Email)
                VALUES (@Name, @Phone, @Email);
                SELECT CAST(SCOPE_IDENTITY() as int)";

            return base.Create(query, patient);
        }

        public void Delete(int id)
        {
            const string query = @"
                DELETE FROM [Foundation].[Patients] WHERE [Id] = @Id;
                DELETE FROM [Foundation].[Appointments] WHERE [PatientId] = @Id;";

            base.Delete(query, id);
        }

        public List<Patient> GetAll()
        {
            const string query = @"
                SELECT 
                    Id,
                    Name,
                    Phone,
                    Email
                FROM Foundation.Patients;";

            return base.GetAll(query, null);
        }

        public Patient GetById(int id)
        {
            const string query = @"
                SELECT 
                    Id,
                    Name,
                    Phone,
                    Email
                FROM Foundation.Patients
                WHERE Id = @Id;";

            return base.GetById(query, id);
        }

        public void Update(int id, Patient patient)
        {
            const string query = @"
                UPDATE [Foundation].[Patients]
                SET [Name] = @Name,
                    [Phone] = @Phone,
                    [Email] = @Email
                WHERE [Id] = @Id";

            var parameters = new
            {
                Id = id,
                patient.Name,
                patient.Email,
                patient.Phone
            };

            base.Update(query, parameters);
        }
    }
}
