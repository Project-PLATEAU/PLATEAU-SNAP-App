using System;
using System.IO;
using UnityEngine;
using VContainer.Unity;
using Synesthesias.Snap.Sample;

namespace Synesthesias.Snap.Sample
{
	public sealed class DevLogger : IStartable, IDisposable
	{
		private readonly IEnvironmentModel environment;
		private StreamWriter writer;
		private readonly object lockObject = new object();
		private readonly TimeZoneInfo TokyoTz = TimeZoneInfo.FindSystemTimeZoneById("Asia/Tokyo");
		
		public DevLogger(IEnvironmentModel environment)
		{
			this.environment = environment;
		}

		public void Start()
		{
			if (environment.EnvironmentType != EnvironmentType.Development) return;

			lock (lockObject)
			{
				if (writer != null)
				{
					Debug.LogWarning("DevLogger already started.");
					return; 
				}

				var dir = Path.Combine(Application.persistentDataPath, "Logs");
				var path = Path.Combine(dir, $"log_{DateTime.Now:yyyyMMdd_HHmmss}.txt");

				Directory.CreateDirectory(dir);
				writer = new StreamWriter(path, true) { AutoFlush = true };
				Application.logMessageReceivedThreaded += OnLog;
				Debug.Log($"DevLog path: {path}");
			}
		}
		
		public void Dispose()
		{
			Application.logMessageReceivedThreaded -= OnLog;
			lock (lockObject)
			{
				writer?.Dispose();
				writer = null;
			}
		}


		private void OnLog(string content, string stackTrace, LogType type)
		{
			lock (lockObject)
			{
	            var timeStamp = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, TokyoTz).ToString("yyyy-MM-dd'T'HH:mm:ss.fff 'JST'"); 
                writer?.WriteLine($"[{timeStamp}] [{type}] {content}");
				if (type == LogType.Error || type == LogType.Exception || type == LogType.Assert)
					writer?.WriteLine(stackTrace);
			}
		}
    }
}	