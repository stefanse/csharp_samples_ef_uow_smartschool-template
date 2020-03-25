using System;
using Serilog.Events;
using System.Linq;

namespace Utils
{
    public class SqlCommandsLogObserver : IObserver<LogEvent>
    {
        public string LogText { get;  set; }

        public void OnCompleted()
        {

        }
        
        public void OnError(Exception exception)
        {

        }

        public void OnNext(LogEvent logEvent)
        {
            foreach(var property in logEvent.Properties.Where(_=>_.Key=="commandText"))
            {
                LogText += $"{property.Value}\n";
            }
        }

        public void ClearLogText()
        {
            LogText = string.Empty;
        }
    }


}