using System.Diagnostics.Tracing;
using System;
using System.Collections.Generic;
using System.Text;
using Serilog;

namespace Utils
{
	public class MyLogger
	{
		public static SqlCommandsLogObserver SQLCmdLogObs { get; set; }
		public static void InitializeLogger()
		{
			SQLCmdLogObs = new SqlCommandsLogObserver();
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Information()
				.WriteTo.Console()
				.WriteTo.File("Serilog.log")
				.WriteTo.Observers(events => events.Subscribe(SQLCmdLogObs))
				.CreateLogger();

		}
	}

}