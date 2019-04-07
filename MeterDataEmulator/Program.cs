using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;

namespace MeterDataEmulator
{
	class Program
	{
		private static IReadOnlyCollection<uint> METER_UNIQUE_IDS = new List<uint>
		{
			0//, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10
		};
		
		static void Main(string[] args)
		{
			Console.WriteLine("Hello MeterDataEmulator!");
			
			var random = new Random();

			using (var httpClient = new HttpClient())
			{
				httpClient.BaseAddress = new Uri(args[0]);
				
				foreach (var meterUniqueId in METER_UNIQUE_IDS)
				{
					int value = 0;
					var totalHours = (int)TimeSpan.FromDays(100).TotalHours;

					for (int i = 0; i < totalHours; i += 4)
					{
						Console.WriteLine(DateTime.Now.AddHours(i));
						
						httpClient.GetAsync(
							$"?uniqueIdentifier={meterUniqueId}" +
							$"&value={value}" +
							$"&measurementType={0}" +
							$"&dateTime={DateTime.Now.AddHours(i).ToString("dd.MM.yyyyTHH:mm:ss", CultureInfo.InvariantCulture)}")
							.GetAwaiter()
							.GetResult();
						value += random.Next(1, 30);
					}
				}
			}
		}
	}
}