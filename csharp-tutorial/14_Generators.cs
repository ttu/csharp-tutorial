using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;

namespace csharp_tutorial
{
    public class Generators
    {
        private IEnumerable<int> GetNumbers()
        {
            yield return 1;
            yield return 2;
            yield return 3;
            yield return 4;
            yield return 5;

            foreach (var number in _numbers)
                yield return number;
        }

        private IEnumerable<int> _numbers = new[] { 6, 7, 8, 9, 10 };

        [Fact]
        public void Simple_Yield()
        {
            foreach (var result in GetNumbers())
            {
                Trace.WriteLine(result);
            }
        }

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

                if (result > 0)
                    break;
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