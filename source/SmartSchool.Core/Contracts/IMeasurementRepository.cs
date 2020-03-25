using SmartSchool.Core.Entities;

namespace SmartSchool.Core.Contracts
{
    public interface IMeasurementRepository
    {
        void AddRange(Measurement[] measurements);
        int GetCountLivingRoom();
        Measurement[] GetGreatestMeasurements();
        double GetCo2AvgFromOffice();
    }
}
