using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace csharp_tutorial
{
    public static class SensorData
    {
        private const string URL = "http://dummy-sensors.azurewebsites.net/api/sensor/";

        public static double GetDataSync(string sensorId = "iddqd")
        {
            var request = HttpWebRequest.Create($"{URL}{sensorId}");
            request.Method = "GET";

            using (var response = request.GetResponse())
            {
                var dataStream = response.GetResponseStream();
                var reader = new StreamReader(dataStream);

                var sensorJson = reader.ReadToEnd();

                reader.Close();
                dataStream.Close();

                dynamic sensor = JsonConvert.DeserializeObject(sensorJson);

                return sensor.data;
            }
        }

        public static SensorDto GetSensorSync(string sensorId = "iddqd")
        {
            var request = HttpWebRequest.Create($"{URL}{sensorId}");
            request.Method = "GET";

            using (var response = request.GetResponse())
            {
                var dataStream = response.GetResponseStream();
                var reader = new StreamReader(dataStream);

                var sensorJson = reader.ReadToEnd();

                reader.Close();
                dataStream.Close();

                return JsonConvert.DeserializeObject<SensorDto>(sensorJson);
            }
        }

        /*
        public static double GetDataSync(string sensrorId = "iddqd")
        {
            using (var client = new HttpClient())
            {
                // HttpClient doesn't have sync methods, so call GetAsync in an own thread
                // If you need sync http methods you can also use WebRequest
                var response = Task.Run(() => client.GetAsync($"{_url}{sensrorId}")).GetAwaiter().GetResult();

                if (!response.IsSuccessStatusCode)
                    return double.MinValue;

                var sensorJson = response.Content.ReadAsStringAsync().Result;
                dynamic sensor = JsonConvert.DeserializeObject(sensorJson);
                return sensor.data;
            }
        }
        */

        public static async Task<double> GetDataAsync(string sensrorId = "iddqd")
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{URL}{sensrorId}");

                if (!response.IsSuccessStatusCode)
                    return double.MinValue;

                var sensorJson = await response.Content.ReadAsStringAsync();
                dynamic sensor = JsonConvert.DeserializeObject(sensorJson);
                return sensor.data;
            }
        }

        public static async Task<SensorDto> GetSensorAsync(string sensrorId = "iddqd")
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{URL}{sensrorId}");

                if (!response.IsSuccessStatusCode)
                    return null;

                var sensorJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<SensorDto>(sensorJson);
            }
        }
    }

    public class SensorDto
    {
        public string Id { get; set; }
        public double Data { get; set; }
    }
}