using csharp_tutorial.Helpers.Hsl;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace csharp_tutorial
{
    public class JsonExamples
    {
        [Fact]
        public async Task Get_Hsl_Data()
        {
            var results = await HslService.GetLocation(HslService.SearchType.VehicleRef, "3606");
        }
    }

    public static class HslService
    {
        public enum SearchType
        {
            LineRef,
            VehicleRef
        }

        public static async Task<dynamic> GetLocation(SearchType searchType, string reference)
        {
            using (var client = new HttpClient())
            {
                // NOTE: HSL endpoint is not working anymore. Use hard coded data.
                //var jsonData = await client.GetStringAsync("http://dev.hsl.fi/siriaccess/vm/json?ProducerRef=HSL");
                var jsonData = HslJsonSample.Json;

                await Task.Delay(0);

                // Json example can use either SelectToken or indexers
                var locations = JObject.Parse(jsonData)
                       .SelectToken("Siri.ServiceDelivery.VehicleMonitoringDelivery")
                       .SelectMany(s => s["VehicleActivity"])
                       .Where(s => s.SelectToken($"MonitoredVehicleJourney.{searchType.ToString()}.value").ToString() == reference)
                       .Select(s => s["MonitoredVehicleJourney"])
                       .Select(s => new
                       {
                           Lon = s["VehicleLocation"]["Longitude"],
                           Lat = s["VehicleLocation"]["Latitude"]
                       })
                       .FirstOrDefault();

                // Example with generated model

                var data = JsonConvert.DeserializeObject<HslSiriData>(jsonData);

                var locationsFromModel = data.Siri.ServiceDelivery.VehicleMonitoringDelivery
                   .SelectMany(e => e.VehicleActivity)
                   .Where(e => searchType == SearchType.VehicleRef 
                                ? e.MonitoredVehicleJourney.VehicleRef.Value == reference
                                : e.MonitoredVehicleJourney.LineRef.Value == reference)
                   .Select(e => e.MonitoredVehicleJourney)
                   .Select(e => new
                   {
                       Lon = e.VehicleLocation.Longitude,
                       Lat = e.VehicleLocation.Latitude
                   })
                   .FirstOrDefault();

                return locations;

                // TODO: Exception handling

                // var description = infoJson["company"]?.FirstOrDefault()?["procurationAbstractDescription"]?.FirstOrDefault(e => e["language"]?.Value<string>() == "Finnish")?["description"]?.Value<string>();
            }
        }
    }
}