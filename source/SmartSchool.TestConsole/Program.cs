using System;
using System.Linq;
using SmartSchool.Core.Entities;
using SmartSchool.Persistence;

namespace SmartSchool.TestConsole
{
	class Program
	{
		static void Main()
		{
			Console.WriteLine("Import der Measurements und Sensors in die Datenbank");
			using (UnitOfWork unitOfWorkImport = new UnitOfWork())
			{
				Console.WriteLine("Datenbank löschen");
				unitOfWorkImport.DeleteDatabase();
				Console.WriteLine("Datenbank migrieren");
				unitOfWorkImport.MigrateDatabase();
				Console.WriteLine("Messwerte werden von measurements.csv eingelesen");
				var measurements = ImportController.ReadFromCsv().ToArray();
				if (measurements.Length == 0)
				{
					Console.WriteLine("!!! Es wurden keine Messwerte eingelesen");
					return;
				}

				Console.WriteLine(
						$"  Es wurden {measurements.Count()} Messwerte eingelesen, werden in Datenbank gespeichert ...");
				unitOfWorkImport.MeasurementRepository.AddRange(measurements);
				int countSensors = measurements.GroupBy(m => m.Sensor).Count();
				int savedRows = unitOfWorkImport.SaveChanges();
				Console.WriteLine(
						$"{countSensors} Sensoren und {savedRows - countSensors} Messwerte wurden in Datenbank gespeichert!");
				Console.WriteLine();
			}

			using (UnitOfWork unitOfWork = new UnitOfWork())
			{
				Console.WriteLine("Import beendet, Test der gespeicherten Daten");
				Console.WriteLine("--------------------------------------------");
				Console.WriteLine();

				var count = 0;
				count = unitOfWork.MeasurementRepository.GetCountLivingRoom();
				Console.WriteLine($"Anzahl Messwerte für Sensor temperature in location livingroom: {count}");
				Console.WriteLine();

				var greatestmeasurements = new Measurement[0];
				greatestmeasurements = unitOfWork.MeasurementRepository.GetGreatestMeasurements();
				Console.WriteLine("Letzte 3 höchste Temperaturmesswerte im Wohnzimmer");
				WriteMeasurements(greatestmeasurements);
				Console.WriteLine();

				double average = -1;
				average = unitOfWork.MeasurementRepository.GetCo2AvgFromOffice();
				Console.WriteLine($"Durchschnitt der gültigen Co2-Werte (>300, <5000) im office: {average}");
				Console.WriteLine();
				Console.WriteLine("Alle Sensoren mit dem Durchschnitt der Messwerte");
				GetSensorsWithAvgValueListed(unitOfWork);
			}

			Console.Write("Beenden mit Eingabetaste ...");
			Console.ReadLine();
		}

		private static void WriteMeasurements(Measurement[] measurements)
		{
			Console.WriteLine("Date       Time     Value");
			for (int i = 0; i < measurements.Length; i++)
			{
				Console.WriteLine($"{measurements[i].Time} {measurements[i].Value}°");
			}
		}

		public static void GetSensorsWithAvgValueListed(UnitOfWork uof)
		{
			var sensors = uof.SensorRepository.GetSensorsWithAvgValue();
			Console.WriteLine($"{"Location",-15}{"Name",-15}   {"Value"}");
			foreach (var current in sensors)
			{
				Console.WriteLine($"{ current.Location,-15} { current.Name,-15}  { current.Avg.ToString("F")}");
			}

		}


	}
}
