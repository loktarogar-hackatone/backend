using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;

namespace MeterDataEmulator
{
	class Program
	{
		private static IReadOnlyDictionary<uint, byte> METERS = new Dictionary<uint, byte>
		{
			{ 0, 0 },
			{ 1, 0 },
			{ 2, 1 },
			{ 3, 1 },
			{ 4, 2 },
			{ 5, 2 },
			{ 6, 0 },
			{ 7, 3 },
			{ 8, 3 },
			{ 9, 2 },
		};
		
		static void Main(string[] args)
		{
			Console.WriteLine("Hello MeterDataEmulator!");
			
			var random = new Random();

			using (var httpClient = new HttpClient())
			{
				httpClient.BaseAddress = new Uri(args[0]);
				
				foreach (var meter in METERS)
				{
					int value = random.Next(0, 30);
					var totalHours = (int)TimeSpan.FromDays(100).TotalHours;
					var minValue = random.Next(0, 10);
					var maxValue = random.Next(20, 70);git

					for (int i = 0; i < totalHours; i += 4)
					{
						Console.WriteLine(DateTime.Now.AddHours(i));
						
						httpClient.GetAsync(
							$"?uniqueIdentifier={meter.Key}" +
							$"&value={value}" +
							$"&measurementType={meter.Value}" +
							$"&dateTime={DateTime.Now.AddHours(i).ToString("dd.MM.yyyyTHH:mm:ss", CultureInfo.InvariantCulture)}")
							.GetAwaiter()
							.GetResult();
						value += random.Next(minValue, maxValue);
					}
				}
			}
		}
	}
}