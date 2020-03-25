using System;
using System.Collections.Generic;
using SmartSchool.Core.Entities;
using Utils;
using System.IO;



namespace SmartSchool.TestConsole
{
    public class ImportController
    {
        const string Filename = "measurements.csv";

        /// <summary>
        /// Liefert die Messwerte mit den dazugehörigen Sensoren
        /// </summary>
        public static IEnumerable<Measurement> ReadFromCsv()
        {
             List<Measurement> measurements = new List<Measurement>();
             List<Sensor> sensors = new List<Sensor>();

            string filePath = MyFile.GetFullNameInApplicationTree(Filename);
            string[] lines = File.ReadAllLines(filePath);
 
            
            for (int i = 0;  i < lines.Length; i++)
            {
                string[] line = lines[i].Split(";");

                if (line[0] != "Date")
                {
                    string[] locationAndName = line[2].Split("_");
                    DateTime time = new DateTime();
                    DateTime.TryParse(line[0] + " " + line[1], out time);
                    Sensor current = new Sensor
                    {
                        Name = locationAndName[1],
                        Location = locationAndName[0]
                    };

                    if (!sensors.Contains(current))
                    {
                        sensors.Add(current);
                    }

                    measurements.Add(new Measurement
                    {
                        Sensor = sensors.Find(searched => searched.Location.Equals(current.Location) && searched.Name.Equals(current.Name)),
                        Time = time,
                        Value = double.Parse(line[3], System.Globalization.CultureInfo.InvariantCulture)

                    }); 
                }
            }

            return measurements;
        }

        public static void AddMeasurement(Sensor current)
        {

        }


    }
}
