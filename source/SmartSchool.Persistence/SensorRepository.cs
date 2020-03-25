using SmartSchool.Core.Contracts;
using System.Linq;

namespace SmartSchool.Persistence
{
    public class SensorRepository : ISensorRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SensorRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public (string Name, string Location, double Avg)[] GetSensorsWithAvgValue()
        {
            return _dbContext.Sensors
                 .Select(s => new
                 {
                     Name = s.Name,
                     Location = s.Location,
                     Avg = s.Measurements.Average(m => m.Value)
                 })
                 .OrderBy(s => s.Location)
                 .ThenBy(s => s.Name)
                 .AsEnumerable()
                 .Select(s => (s.Name, s.Location, s.Avg))
                 .ToArray();
        }

       
    }
}