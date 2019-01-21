using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace csharp_tutorial
{
    public class ParallelExamples
    {
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
            var idList = Enumerable.Range(1, 5);

            Trace.WriteLine($"Parallel.ForEach with async");
            var sw = Stopwatch.StartNew();

            // No way to know when this is ready, so don't use async with Parallel.ForEach
            var res = Parallel.ForEach(idList, async (id) =>
            {
                var result = await SlowAsyncAction1Sec(id);
                Trace.WriteLine("Result: " + result);
            });

            await Task.Delay(3);

            Trace.WriteLine($"Parallel.ForEach with async: {sw.ElapsedMilliseconds}ms");

            // NEXT

            Trace.WriteLine($"AsParallel().ForAll with async");
            sw = Stopwatch.StartNew();

            // No way to know when this is ready, so don't use async with Parallel.ForEach
            idList.AsParallel().ForAll(async (id) =>
            {
                var result = await SlowAsyncAction1Sec(id);
                Trace.WriteLine("Result: " + result);
            });

            await Task.Delay(3);

            Trace.WriteLine($"AsParallel().ForAll with async: {sw.ElapsedMilliseconds}ms");

            /// NEXT

            Trace.WriteLine($"AsParallel Select sync");
            sw = Stopwatch.StartNew();

            var syncActions = idList.AsParallel().Select(e => SlowAction1SecResult(e)).ToList();

            syncActions.ForEach(e => Trace.WriteLine("Result: " + e));

            Trace.WriteLine($"AsParallel Select sync: {sw.ElapsedMilliseconds}ms");

            /// NEXT

            // This is not much faster, beacause AsParallel already runs these in parallel
            Trace.WriteLine($"AsParallel Select async");
            sw = Stopwatch.StartNew();

            var asyncActions = idList.AsParallel().Select(async e => await SlowAsyncAction1Sec(e)).ToList();

            asyncActions.ForEach(e => Trace.WriteLine("Result: " + e.Result));

            Trace.WriteLine($"AsParallel Select async: {sw.ElapsedMilliseconds}ms");

            /// NEXT

            // This wont be parallel as this is worng: foreach (int i in idList.AsParallel())
            Trace.WriteLine($"Foreach AsParallel no async");
            sw = Stopwatch.StartNew();

            foreach (int i in idList.AsParallel())
            {
                var r = SlowAction1SecResult(i);
                Trace.WriteLine($"id: {i} result:{r}");
            }

            Trace.WriteLine($"Foreach AsParallel no async: {sw.ElapsedMilliseconds}ms");

            /// NEXT

            // This wont be parallel as this is worng: foreach (int i in idList.AsParallel())
            Trace.WriteLine($"Foreach AsParallel with async");
            sw = Stopwatch.StartNew();

            foreach (int i in idList.AsParallel())
            {
                var r = await SlowAsyncAction1Sec(i);
                Trace.WriteLine($"id: {i} result:{r}");
            }

            Trace.WriteLine($"Foreach AsParallel with async: {sw.ElapsedMilliseconds}ms");
        }

        [Fact]
        public async Task AsyncAwaitForEachSelect()
        {
            var idList = Enumerable.Range(1, 5);

            // This will work
            Trace.WriteLine($"Foreach TaskWhenAll with async");
            var sw = Stopwatch.StartNew();

            var results = new Dictionary<int, Task>(5);

            foreach (int i in idList)
            {
                var r = SlowAsyncAction1Sec(i);
                results.Add(i, r);
            }

            await Task.WhenAll(results.Values.ToList());

            Trace.WriteLine($"{sw.ElapsedMilliseconds}ms");

            /// NEXT

            Trace.WriteLine($"Select TaskWhenAll no async");

            var slowTasks = idList.Select(i => SlowAsyncAction1Sec(i));
            // Tasks finish at different times, but returned array has results in the same order as slowTasks
            var slowResuls = await Task.WhenAll(slowTasks);

            /// NEXT

            Trace.WriteLine($"Select TaskWhenAll async");
            sw = Stopwatch.StartNew();

            var asyncActions2 = idList.Select(async e => await SlowAsyncAction1Sec(e)).ToList();

            await Task.WhenAll(asyncActions2);

            asyncActions2.ForEach(e => Trace.WriteLine("Result: " + e.Result));

            Trace.WriteLine($"Select async: {sw.ElapsedMilliseconds}ms");
        }

        private void SlowAction1Sec(int id)
        {
            // This simulates longer process
            Thread.Sleep(1000);

            Trace.WriteLine($"Ready: {id}");
        }

        private int SlowAction1SecResult(int id)
        {
            // This simulates longer process
            Thread.Sleep(1000);

            Trace.WriteLine($"Ready: {id}");

            return id * 2;
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