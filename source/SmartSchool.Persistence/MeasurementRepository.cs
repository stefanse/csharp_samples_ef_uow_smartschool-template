using Microsoft.EntityFrameworkCore;
using SmartSchool.Core.Contracts;
using SmartSchool.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SmartSchool.Persistence
{
    public class MeasurementRepository : IMeasurementRepository
    {
        private ApplicationDbContext _dbContext;

        public MeasurementRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddRange(Measurement[] measurements)
        {
            _dbContext.Measurements.AddRange(measurements);
        }

        public IEnumerable<Measurement> GetAll() =>      
             _dbContext.Measurements
             .Include(m => m.Sensor)
             .ToArray();
        


        public double GetCo2AvgFromOffice() =>
             _dbContext.Measurements
                .Include(m => m.Sensor)
                .Where(m => m.Sensor.Location.Equals("office")
                    && m.Sensor.Name.Equals("co2")
                    && m.Value > 300
                    && m.Value < 5000)
                .Average(m => m.Value);
        

        public int GetCountLivingRoom() =>
        
             _dbContext.Measurements
                .Include(m => m.Sensor)
                .Where(m => m.Sensor.Location == "livingroom" && m.Sensor.Name == "temperature")
                .Count();
       

        public Measurement[] GetGreatestMeasurements() =>
             _dbContext.Measurements
                .Include(m => m.Sensor)
                .Where(m => m.Sensor.Location == "livingroom" && m.Sensor.Name == "temperature")
                .OrderByDescending(m => m.Value)
                .ThenByDescending(m => m.Time)
                .Take(3)
                .ToArray();
        
    }
}