using csharp_tutorial.Helpers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace csharp_tutorial
{
    public class Generators
    {
        public Generators(ITestOutputHelper outputHelper) => Trace.Listeners.Add(new TestTraceListener(outputHelper));

        public class WebRequester
        {
            public IEnumerable<double> GetValues()
            {
                yield return -1;

                while (true)
                {
                    yield return SensorData.GetDataSync("iddqd");
                    yield return SensorData.GetDataSync("idkfa");
                }
            }
        }

        [Fact]
        public void Generator_Sync()
        {
            var hello = new WebRequester();

            foreach (var result in hello.GetValues())
            {
                Trace.WriteLine(result);
            }
        }

        public class WebRequesterAsync
        {
            public IEnumerable<Task<double>> GetValues()
            {
                while (true)
                {
                    yield return Task.FromResult(-1d);
                    yield return SensorData.GetDataAsync("iddqd");
                    yield return SensorData.GetDataAsync("idkfa");
                }
            }
        }

        [Fact]
        public async Task Generator_Async()
        {
            var hello = new WebRequesterAsync();

            foreach (var result in hello.GetValues())
            {
                Trace.WriteLine(await result);
            }
        }
    }
}