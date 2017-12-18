using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;

namespace csharp_tutorial
{
    public class Generators
    {
        public class WebRequester
        {
            public IEnumerable<double> GetValues()
            {
                while (true)
                {
                    yield return -1;
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