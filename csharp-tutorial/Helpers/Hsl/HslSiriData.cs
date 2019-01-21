using Newtonsoft.Json;
using System.Collections.Generic;

namespace csharp_tutorial.Helpers.Hsl
{
    // Generated with: https://app.quicktype.io/#l=cs&r=json2csharp

    public partial class HslSiriData
    {
        [JsonProperty("Siri")]
        public Siri Siri { get; set; }
    }

    public partial class Siri
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("ServiceDelivery")]
        public ServiceDelivery ServiceDelivery { get; set; }
    }

    public partial class ServiceDelivery
    {
        [JsonProperty("ResponseTimestamp")]
        public long ResponseTimestamp { get; set; }

        [JsonProperty("ProducerRef")]
        public Ref ProducerRef { get; set; }

        [JsonProperty("Status")]
        public bool Status { get; set; }

        [JsonProperty("MoreData")]
        public bool MoreData { get; set; }

        [JsonProperty("VehicleMonitoringDelivery")]
        public List<VehicleMonitoringDelivery> VehicleMonitoringDelivery { get; set; }
    }

    public partial class Ref
    {
        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public partial class VehicleMonitoringDelivery
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("ResponseTimestamp")]
        public long ResponseTimestamp { get; set; }

        [JsonProperty("Status")]
        public bool Status { get; set; }

        [JsonProperty("VehicleActivity")]
        public List<VehicleActivity> VehicleActivity { get; set; }
    }

    public partial class VehicleActivity
    {
        [JsonProperty("ValidUntilTime")]
        public long ValidUntilTime { get; set; }

        [JsonProperty("RecordedAtTime")]
        public long RecordedAtTime { get; set; }

        [JsonProperty("MonitoredVehicleJourney")]
        public MonitoredVehicleJourney MonitoredVehicleJourney { get; set; }
    }

    public partial class MonitoredVehicleJourney
    {
        [JsonProperty("LineRef")]
        public Ref LineRef { get; set; }

        [JsonProperty("DirectionRef")]
        public DirectionRef DirectionRef { get; set; }

        [JsonProperty("FramedVehicleJourneyRef")]
        public FramedVehicleJourneyRef FramedVehicleJourneyRef { get; set; }

        [JsonProperty("OperatorRef")]
        public Ref OperatorRef { get; set; }

        [JsonProperty("Monitored")]
        public bool Monitored { get; set; }

        [JsonProperty("VehicleLocation")]
        public VehicleLocation VehicleLocation { get; set; }

        [JsonProperty("VehicleRef")]
        public Ref VehicleRef { get; set; }
    }

    public partial class DirectionRef
    {
    }

    public partial class FramedVehicleJourneyRef
    {
        [JsonProperty("DataFrameRef")]
        public Ref DataFrameRef { get; set; }
    }

    public partial class VehicleLocation
    {
        [JsonProperty("Longitude")]
        public double Longitude { get; set; }

        [JsonProperty("Latitude")]
        public double Latitude { get; set; }
    }
}