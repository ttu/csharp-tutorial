using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace csharp_tutorial
{
    public static class SensorData
    {
        private static string _url = "http://dummy-sensors.azurewebsites.net/api/sensor/";

        public static double GetDataSync(string sensrorId = "iddqd")
        {
            using (var client = new HttpClient())
            {
                // HttpClient doesn't have sync methods, so run GetAsync in own thread
                var response = Task.Run(() => client.GetAsync($"{_url}{sensrorId}")).GetAwaiter().GetResult();

                if (!response.IsSuccessStatusCode)
                    return double.MinValue;

                var sensorJson = response.Content.ReadAsStringAsync().Result;
                dynamic sensor = JsonConvert.DeserializeObject(sensorJson);
                return sensor.data;
            }
        }

        public static async Task<double> GetDataAsync(string sensrorId = "iddqd")
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{_url}{sensrorId}");

                if (!response.IsSuccessStatusCode)
                    return double.MinValue;

                var sensorJson = await response.Content.ReadAsStringAsync();
                dynamic sensor = JsonConvert.DeserializeObject(sensorJson);
                return sensor.data;
            }
        }
    }
}