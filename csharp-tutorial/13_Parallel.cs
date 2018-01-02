using csharp_tutorial.Helpers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace csharp_tutorial
{
    public class ParallelExamples
    {
        public ParallelExamples(ITestOutputHelper outputHelper) => Trace.Listeners.Add(new TestTraceListener(outputHelper));

        [Fact]
        public void Parallel_ForEach()
        {
            // Easy parallel with Parallel.ForEach
            // Can't use async with Parallel

            var idList = new List<int> { 1, 2, 3, 4 };

            var sw = Stopwatch.StartNew();

            Parallel.ForEach(idList, SlowAction1Sec);

            // Previous statement is same as these

            //Parallel.ForEach(idList, (id) => { SlowAction(id); });

            //foreach (var id in idList.AsParallel())
            //{
            //    SlowAction1Sec(id);
            //}

            Trace.WriteLine($"{sw.ElapsedMilliseconds}ms");

            sw = Stopwatch.StartNew();

            idList.ForEach(SlowAction1Sec);

            // Previous statement is same as this
            //foreach (var id in idList)
            //{
            //    SlowAction1Sec(id);
            //}

            Trace.WriteLine($"{sw.ElapsedMilliseconds}ms");
        }

        [Fact]
        public async Task AsyncAwaitParallel()
        {
            var sw = Stopwatch.StartNew();

            Parallel.ForEach(Enumerable.Range(1, 5), async (id) => { await SlowAsyncAction1Sec(id); });

            Trace.WriteLine($"{sw.ElapsedMilliseconds}ms");

            sw = Stopwatch.StartNew();

            // This wont be parallel
            foreach (int i in Enumerable.Range(1, 5).AsParallel())
            {
                var r = await SlowAsyncAction1Sec(i);
                Trace.WriteLine($"id: {i} result:{r}");
            }

            Trace.WriteLine($"{sw.ElapsedMilliseconds}ms");

            sw = Stopwatch.StartNew();

            // This will work
            var results = new Dictionary<int, Task>(5);

            foreach (int i in Enumerable.Range(1, 5))
            {
                var r = SlowAsyncAction1Sec(i);
                results.Add(i, r);
            }

            await Task.WhenAll(results.Values.ToList());

            Trace.WriteLine($"{sw.ElapsedMilliseconds}ms");

            var slowTasks = Enumerable.Range(1, 5).Select(i => SlowAsyncAction1Sec(i));
            // Tasks finish at different times, but returned array has results in the same order as slowTasks
            var slowResuls = await Task.WhenAll(slowTasks);
        }

        private void SlowAction1Sec(int id)
        {
            // This simulates longer process
            Thread.Sleep(1000);

            Trace.WriteLine($"Ready: {id}");
        }

        private Task<int> SlowAsyncAction1Sec(int id)
        {
            return Task.Run(() =>
            {
                Thread.Sleep(1000);
                Trace.WriteLine($"Ready: {id}");
                return id * 2;
            });
        }

        [Fact]
        public void Tasks()
        {
            // var longProcess = Task.Factory.StartNew(() => { Thread.Sleep(1000); });
            // Task.Run is a shorthand for Task.Factory.StartNew with some default argumnents
            var longProcess = Task.Run(() => { Thread.Sleep(1000); });

            var results = ParallelPartitionerPi(20000000);
        }

        // http://stackoverflow.com/a/4283808/1292530
        private static decimal ParallelPartitionerPi(int steps)
        {
            decimal sum = 0.0M;
            decimal step = 1.0M / (decimal)steps;
            object lockObj = new object();

            Parallel.ForEach(
                Partitioner.Create(0, steps),
                () => 0.0M,
                (range, state, partial) =>
                {
                    for (int i = range.Item1; i < range.Item2; i++)
                    {
                        decimal x = (i - 0.5M) * step;
                        partial += 4.0M / (1.0M + x * x);
                    }

                    return partial;
                },
                partial => { lock (lockObj) sum += partial; });

            return step * sum;
        }
    }
}