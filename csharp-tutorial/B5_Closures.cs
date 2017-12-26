using System;
using System.Threading.Tasks;
using Xunit;

namespace csharp_tutorial
{
    public class B5_Closures
    {
        private void DoCalcluation(Func<int, int> adder, int expected)
        {
            var result = adder(5);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Simple()
        {
            int myValue = 4;
            var adder = new Func<int, int>(i => myValue + i);

            DoCalcluation(adder, 9);

            // myValue will change inside closure
            myValue = 6;

            DoCalcluation(adder, 11);
        }

        [Fact]
        public void SensorId()
        {
            var selectedSensor = "iddqd";

            var getData = new Func<Task<SensorDto>>(async () =>
            {
                await Task.Delay(1000);
                return await SensorData.GetSensorAsync(selectedSensor);
            });

            var sensorTask = getData();

            selectedSensor = "idkfa";

            var result = sensorTask.Result;

            // Fix
            // var getData = new Func<string Task<SensorDto>>(async sensorId =>
        }

        public class SensorRequest
        {
            public string Id { get; set; }
        }

        [Fact]
        public void ReferenceType()
        {
            var sensorRequest = new SensorRequest { Id = "iddqd" };

            var getData = new Func<SensorRequest, Task<SensorDto>>(async request =>
            {
                await Task.Delay(1000);
                return await SensorData.GetSensorAsync(request.Id);
            });

            var sensorTask = getData(sensorRequest);

            sensorRequest.Id = "idkfa";

            var result = sensorTask.Result;
        }

        [Fact]
        public void ReferenceTypeFixed()
        {
            var sensorRequest = new SensorRequest { Id = "iddqd" };

            var getData = new Func<SensorRequest, Task<SensorDto>>(async request =>
            {
                var req = new SensorRequest { Id = request.Id };
                await Task.Delay(1000);
                return await SensorData.GetSensorAsync(req.Id);
            });

            var sensorTask = getData(sensorRequest);

            sensorRequest.Id = "idkfa";

            var result = sensorTask.Result;
        }
    }
}