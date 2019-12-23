using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace csharp_tutorial
{
    public class Null
    {
        [Fact]
        public void Null_Coalescing()
        {
            string value = null;

            // Null coalescing, if left side is null, take right side
            string print = value ?? "default value";

            Assert.Equal("default value", print);

            // Same as normal one line condition
            string print2 = value != null ? value : "default value";

            Assert.Equal("default value", print2);

            // Set something to value
            value = "This has something";

            string print3 = value ?? "default value";

            Assert.Equal("This has something", print3);
        }

        [Fact]
        public void Null_Coalescing_CompoundAssignment()
        {
            string value = string.Empty;

            //value = value ?? "default value";
            value ??= "default value";
        }

        public class NullExample
        {
            public NullExample Child { get; set; }

            public string Text { get; set; }
        }

        [Fact]
        public void Null_Check()
        {
            var hello = new NullExample
            {
                Child = new NullExample { Text = "Abba" }
            };

            string text = hello.Child.Text;

            try
            {
                // Because of null values NullReferenceException is thrown
                string textException = hello.Child.Child.Child.Text;
            }
            catch (NullReferenceException)
            {
            }

            string text2 = hello?.Child?.Child?.Child?.Text;
            Assert.Null(text2);

            string text3 = hello?.Child?.Text;
            Assert.Equal(text, text3);

            // Null coalescing check
            var text4 = hello?.Child?.Child?.Text ?? "Not set";

            Assert.Equal("Not set", text4);
        }

        [Fact]
        public void Nullable_Types()
        {
            // int is a value type and values types can't be null (default value of value type is 0bit, so for int is 0, bool is false etc.)
            // By adding ? to type identifier value types can also be null
            int? myValue = null;
            int valueCheck = myValue ?? 2;

            Assert.Equal(2, valueCheck);

            myValue = 6;

            valueCheck = myValue ?? 2;

            Assert.Equal(6, valueCheck);

            if (myValue.HasValue)
            {
                // Do something
            }
        }

#nullable enable
// Normally would just add <Nullable>enable</Nullable> to csproj-file to enable nullable for the whole project

        [Fact]
        public void Nullable_ReferenceTypes()
        {
            var sensor = GetSensor(0);
            var data = GetData(sensor);
            Assert.Equal(3, data);
        }

        public SensorDto GetSensor(int id)
        {
            if (id == 0)
            {
                return new SensorDto { Data = 3 };
            }
            else
            {
                throw new Exception("Sensor not found");
                // if return value would be nullable (SensorDto?), then null would be possible 
                //return null;
            }
        }

        public double GetData(SensorDto sensor)
        {
            // No need for null check as sensor can't be null
            return sensor.Data;
        }

        [Fact]
        public void Nullable_CanBeNull()
        {
            var sensor = GetSensorCanBeNull(0);

            if (sensor == null)
                return;

            var data = GetData(sensor);
            Assert.Equal(3, data);
        }

        public SensorDto? GetSensorCanBeNull(int id)
        {
            if (id == 0)
            {
                return new SensorDto();
            }
            else
            {
                return null;
            }
        }

        public double GetDataCanBeNull(SensorDto? sensor)
        {
            if (sensor == null)
                return double.MinValue;

            return sensor.Data;
        }

        // Get GetSensorAsync with nullable reference type
        public static async Task<SensorDto?> GetSensorAsync(string sensrorId = "iddqd")
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"http://dummy-sensors.azurewebsites.net/api/sensor/{sensrorId}");

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                    //throw new Exception("Sensor not found");
                }

                var sensorJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<SensorDto>(sensorJson);
            }
        }

#nullable disable
    }
}