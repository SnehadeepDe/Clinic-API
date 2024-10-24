using ClinicAPI.Models.DB_Models;
using ClinicAPI.Repository.Interfaces;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace ClinicAPI.Repository
{
    public class SpecializationRepository : BaseRepository<Specialization>, ISpecializationRepository
    {
        public SpecializationRepository(IOptions<ConnectionString> connectionString)
            : base(connectionString.Value.CLINICDB)
        {
        }

        public int Create(Specialization specialization)
        {
            const string query = @"
                INSERT INTO [Foundation].[Specializations] (Name)
                VALUES (@Name);
                SELECT CAST(SCOPE_IDENTITY() as int)";

            return base.Create(query, specialization);
        }

        public void Delete(int id)
        {
            const string query = @"
                DELETE FROM [Foundation].[Specializations] WHERE [ID] = @Id;";

            base.Delete(query, id);
        }

        public List<Specialization> GetAll()
        {
            const string query = @"
                SELECT 
                    ID,
                    Name
                FROM Foundation.Specializations;";

            return base.GetAll(query, null);
        }

        public Specialization GetById(int id)
        {
            const string query = @"
                SELECT 
                    ID,
                    Name
                FROM Foundation.Specializations
                WHERE ID = @Id;";

            return base.GetById(query, id);
        }

        public void Update(int id, Specialization specialization)
        {
            const string query = @"
                UPDATE [Foundation].[Specializations]
                SET [Name] = @Name
                WHERE [ID] = @Id";

            var parameters = new
            {
                Id = id,
                specialization.Name
            };

            base.Update(query, parameters);
        }
    }
}
