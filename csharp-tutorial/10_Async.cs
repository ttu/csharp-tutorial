using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace csharp_tutorial
{
    public class AsyncExamples
    {
        [Fact]
        public async Task TaskAndResult()
        {
            var task = SlowAsyncAction1Sec(1, 5000);

            var firstResult = task.Result;

            var result = await SlowAsyncAction1Sec(1, 5000);

            Assert.Equal(task.Result, result);
        }

        [Fact]
        public async Task AsyncAwait()
        {
            var sw = Stopwatch.StartNew();

            var r1 = await SlowAsyncAction1Sec(1);
            var r2 = await SlowAsyncAction1Sec(2);
            var r3 = await SlowAsyncAction1Sec(3);
            var r4 = await SlowAsyncAction1Sec(4);

            Trace.WriteLine($"{sw.ElapsedMilliseconds}ms");

            sw = Stopwatch.StartNew();

            var t1 = SlowAsyncAction1Sec(1);
            var t2 = SlowAsyncAction1Sec(2);
            var t3 = SlowAsyncAction1Sec(3);
            var t4 = SlowAsyncAction1Sec(4);

            await Task.WhenAll(t1, t2, t3, t4);

            Trace.WriteLine($"{sw.ElapsedMilliseconds}ms");

            // To get results need to use .Result
            var result1 = t1.Result;
        }

        private Task<int> SlowAsyncAction1Sec(int id, int sleepTime = 1000)
        {
            return Task.Run(() =>
            {
                Thread.Sleep(sleepTime);
                Trace.WriteLine($"Ready: {id}");
                return id * 2;
            });
        }

        [Fact]
        public async Task AsyncAwait_Comparison()
        {
            // Warmup
            SensorData.GetDataSync("iddqd");
            await SensorData.GetDataAsync("iddqd");

            var sw = Stopwatch.StartNew();

            var syncResult1 = SensorData.GetDataSync("iddqd");
            var syncResult2 = SensorData.GetDataSync("idkfa");
            var syncResult3 = SensorData.GetDataSync("abba5");
            var syncResult4 = SensorData.GetDataSync("acdc1");

            var syncTime = sw.ElapsedMilliseconds;
            sw = Stopwatch.StartNew();

            var r1 = await SensorData.GetDataAsync("iddqd");
            var r2 = await SensorData.GetDataAsync("idkfa");
            var r3 = await SensorData.GetDataAsync("abba5");
            var r4 = await SensorData.GetDataAsync("acdc1");

            var asyncTime = sw.ElapsedMilliseconds;
            sw = Stopwatch.StartNew();

            var t1 = SensorData.GetDataAsync("iddqd");
            var t2 = SensorData.GetDataAsync("idkfa");
            var t3 = SensorData.GetDataAsync("abba5");
            var t4 = SensorData.GetDataAsync("acdc1");

            var results = await Task.WhenAll(t1, t2, t3, t4);

            var awaitTime = sw.ElapsedMilliseconds;
        }

        [Fact]
        public async Task Tasks_FromSync()
        {
            var ids = new string[] { "iddqd", "idkfq", "abba5", "acdc1" };

            var tasks = new List<Task<SensorDto>>();

            foreach (var id in ids)
            {
                var task = Task.Run(() =>
                {
                    var value = SensorData.GetSensorSync(id);
                    return value;
                });

                // var task = Task.Run(() => SensorData.GetSensorSync(id));

                tasks.Add(task);
            }

            var results = await Task.WhenAll(tasks);
        }

        [Fact]
        public async Task Tasks_FromSync_Linq()
        {
            var ids = new string[] { "iddqd", "idkfq", "abba5", "acdc1" };

            var tasks = ids.Select(i => Task.Run(() => SensorData.GetSensorSync(i))).ToList();

            var results = await Task.WhenAll(tasks);
        }

        [Fact]
        public async Task Tasks_FromAsync()
        {
            var ids = new string[] { "iddqd", "idkfq", "abba5", "acdc1" };

            var tasks = new List<Task<double>>();

            foreach (var id in ids)
            {
                var dataTask = SensorData.GetDataAsync(id);
                tasks.Add(dataTask);
            }

            var results = await Task.WhenAll(tasks);
        }

        [Fact]
        public async Task Tasks_FromAsync_Linq()
        {
            var ids = new string[] { "iddqd", "idkfq", "abba5", "acdc1" };

            var tasksLinq = ids.Select(e => SensorData.GetSensorAsync(e)).ToList();

            var results = await Task.WhenAll(tasksLinq);

            // Select is lazy, but now tasksLinq is a List
            // Try what happes without .ToList() and with it
            var over10Sensors = tasksLinq.Where(e => e.Result.Data > 10).ToList();
        }

        // Sometimes is is really hard to understand how Framework behaves
        // Examples have blocking Sleep before and after await Delay
        [Fact]
        public async Task Threading_Is_Hard()
        {
            var partiallyBlocking = LongRunningWithSync_Blocking();

            await partiallyBlocking;

            var returnImmediately = LongRunningWithSync_Non_Blocking();

            await returnImmediately;

            var returnImmediately2 = LongRunningWithSync_Extra_Thread();

            await returnImmediately2;
        }

        private async Task LongRunningWithSync_Blocking()
        {
            var delayedTasks = Enumerable.Range(0, 5).Select(async e =>
            {
                Thread.Sleep(e * 1000);
                await Task.Delay(e * 1000);
                return e;
            });

            await Task.WhenAll(delayedTasks);
        }

        private async Task LongRunningWithSync_Non_Blocking()
        {
            var delayedTasks = Enumerable.Range(0, 5).Select(async e =>
            {
                await Task.Delay(e * 1000);
                Thread.Sleep(e * 1000);
                return e;
            });

            await Task.WhenAll(delayedTasks);
        }

        private async Task LongRunningWithSync_Extra_Thread()
        {
            var delayedTasks = Enumerable.Range(0, 5).Select(async e =>
            {
                return await Task.Run(async () =>
                {
                    Thread.Sleep(e * 1000);
                    await Task.Delay(e * 1000);
                    return e;
                });
            });

            await Task.WhenAll(delayedTasks);
        }
    }
}