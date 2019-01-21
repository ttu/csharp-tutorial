using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace csharp_tutorial
{
    public class TaskExamples
    {
        [Fact]
        public void RunThread()
        {
            void BackgroundExecution()
            {
                while (true)
                {
                    // Do something important
                    var data = SensorData.GetSensorSync();
                    Thread.Sleep(1000);
                    Trace.WriteLine($"In the background: {data.Data}");
                }
            }

            var t = new Thread(new ThreadStart(BackgroundExecution));
            t.Start();

            while (t.IsAlive)
                Thread.Sleep(1000);
        }

        [Fact]
        public void RunTask()
        {
            void BackgroundExecution()
            {
                while (true)
                {
                    var data = SensorData.GetSensorSync();
                    Thread.Sleep(1000);
                    Trace.WriteLine($"In the background: {data.Data}");
                }
            }

            var task = Task.Run(() => BackgroundExecution());

            while (task.IsCompleted == false)
                Thread.Sleep(1000);
        }

        [Fact]
        public void RunTaskReturn()
        {
            // Often tasks return something rather than just run "forever" on the background

            double BackgroundExecution()
            {
                var data = SensorData.GetSensorSync();
                Thread.Sleep(1000);
                return data.Data;
            }

            var task = Task.Run(() => BackgroundExecution());

            while (task.IsCompleted == false)
            {
                Thread.Sleep(1000);
            }

            var result = task.Result;

            Assert.Equal(2, result);
        }

        [Fact]
        public async Task RunAtBackground()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            var collection = new BlockingCollection<string>();

            // Explain why task has token and why it is checked in while loop
            var backgroundTask = Task.Run(() =>
            {
                while (token.IsCancellationRequested == false)
                {
                    var sensorToFetch = collection.Take(token);
                    var sensorData = SensorData.GetSensorSync(sensorToFetch);

                    // Do something nice with the result
                    Trace.WriteLine(JsonConvert.SerializeObject(sensorData));
                }
            }, token);

            // Application keeps getting requests from somewhere
            while (true)
            {
                await Task.Delay(5000);
                collection.Add(DateTime.Now.Ticks % 2 == 0 ? "abba5" : "iddqd");
            }
        }

        private Task<double> GetSensorDataAsync(string id)
        {
            return Task.Run(() =>
            {
                return SensorData.GetDataSync(id);
            });
        }
    }
}