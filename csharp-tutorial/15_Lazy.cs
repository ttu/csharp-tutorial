using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace csharp_tutorial
{
    public class Lazy
    {
        /*
         * Do not do expensive operations if not needed
         *  - DB reads, API fetches
         * Do not consumer memory if not needed
         *  - Create new temporary collections
         *  - Fetch unnecessary data from database
         * ...
         *
         */

        public class WebRequesterAsync
        {
            public IEnumerable<Task<SensorDto>> OpenStream(IEnumerable<string> ids)
            {
                while (true)
                {
                    foreach (var id in ids)
                    {
                        yield return SensorData.GetSensorAsync(id);
                    }
                }
            }
        }

        [Fact]
        public async Task Yield_Lazy()
        {
            var sensors = new WebRequesterAsync();

            foreach (var result in sensors.OpenStream(new[] { "iddqd", "idkfa", "abba5", "acdc1" }))
            {
                var data = await result;

                if (data.Data > 23)
                    break;

                Trace.WriteLine(JsonConvert.SerializeObject(data));
            }
        }

        private class Person
        {
            public string Name { get; set; }
            public int Location { get; set; }
            public int Salary { get; set; }
        }

        [Fact]
        public void Linq_Lazy()
        {
            var employees = new List<Person>(10)
            {
                new Person { Name = "Andy", Location = 1, Salary = 1 },
                new Person { Name = "Thomas", Location = 2, Salary = 1 },
                new Person { Name = "Jefferson", Location = 2, Salary = 2 },
                new Person { Name = "Jim", Location = 2, Salary = 2 },
                new Person { Name = "Ben", Location = 1, Salary = 2 }
            };

            // Where is lazy so no need to create location2 array if not needed
            var location2 = employees.Where(e => e.Location == 2);

            // Add next where later
            var loc2sal2 = location2.Where(e => e.Salary == 2);

            // Will stop execution on First
            var first = loc2sal2.First();

            // Will go through again the all employees
            var last = loc2sal2.Last();
        }

        [Fact]
        public async Task Linq_Select_Lazy()
        {
            // 07_Linq has and example of LINQ Select. This example shows more complicated case with API requests

            // Find first sensor with data value under 23

            var sensorIds = new[] { "iddqd", "idkfa", "abba5", "acdc1" };

            // Have to use own extension method as Where/First etc. do not support async
            var item = await sensorIds.Select(async id =>
                                    {
                                        var data = await SensorData.GetSensorAsync(id);
                                        Trace.WriteLine(JsonConvert.SerializeObject(data));
                                        return data;
                                    })
                                    .FirstOrDefaultAsync(async s => (await s).Data < 23);

            Trace.WriteLine($"First item: {JsonConvert.SerializeObject(item)}");
        }

        [Fact]
        public async Task Lazy_Class()
        {
            var sensor = new Lazy<Task<SensorDto>>(async () =>
            {
                Trace.WriteLine("Fetching data");
                return await SensorData.GetSensorAsync("abba5");
            });

            Assert.False(sensor.IsValueCreated);

            var data = await sensor.Value;

            var dataOther = await sensor.Value;
        }
    }

    public static class LinqExtensions
    {
        public static async Task<T> FirstOrDefaultAsync<T>(this IEnumerable<Task<T>> items, Func<Task<T>, Task<bool>> predicate)
        {
            foreach (var item in items)
            {
                if (await predicate(item))
                {
                    return await item;
                }
            }

            return default;
        }
    }
}