using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace csharp_tutorial
{
    public class G_Json
    {
        [Fact]
        public async Task Get_Hsl_Data()
        {
            var results = await HslService.GetLocation(HslService.SearchType.LineRef, "17");
        }
    }

    public static class HslService
    {
        public enum SearchType
        {
            LineRef,
            VehicleRef
        }

        public static async Task<dynamic> GetLocation(SearchType type, string reference)
        {
            using (var client = new HttpClient())
            {
                var jsonData = await client.GetStringAsync("http://dev.hsl.fi/siriaccess/vm/json?ProducerRef=HSL");

                var locations = JObject.Parse(jsonData)["Siri"]["ServiceDelivery"]["VehicleMonitoringDelivery"]
                        .SelectMany(s => s["VehicleActivity"])
                        .Where(s => s["MonitoredVehicleJourney"][(type.ToString())]["value"].ToString() == reference)
                        .Select(s => s["MonitoredVehicleJourney"])
                        .Select(s => new
                        {
                            Lon = s["VehicleLocation"]["Longitude"],
                            Lat = s["VehicleLocation"]["Latitude"]
                        });

                return locations;

                // TODO: Exception handling

                // var description = infoJson["company"]?.FirstOrDefault()?["procurationAbstractDescription"]?.FirstOrDefault(e => e["language"]?.Value<string>() == "Finnish")?["description"]?.Value<string>();
            }
        }
    }
}