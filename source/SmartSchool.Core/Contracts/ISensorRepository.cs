using SmartSchool.Core.Entities;

namespace SmartSchool.Core.Contracts
{
    public interface ISensorRepository
    {

        (string Name, string Location, double Avg)[] GetSensorsWithAvgValue();
    }
}
