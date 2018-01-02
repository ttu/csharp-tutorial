using System;
using System.Threading.Tasks;
using Xunit;

namespace csharp_tutorial
{
    public class Closures
    {
        [Fact]
        public void Simple()
        {
            int myValue = 4;

            var adder = new Func<int, int>(i =>
            {
                return myValue + i;
            });

            var result = adder(5);
            Assert.Equal(9, result);

            //DoCalcluation(adder, 9);
            //Assert.Equal(9, result);

            // myValue will change inside closure
            myValue = 6;

            var result2 = adder(5);
            Assert.Equal(11, result2);

            //DoCalcluation(adder, 11);
            //Assert.Equal(11, result2);
        }

        private void DoCalcluation(Func<int, int> adder, int expected)
        {
            var result = adder(5);
            Assert.Equal(expected, result);
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

        public class InputHandlerOptions
        {
            public int MaxValue = 0;
            public bool MustBeEven = false;
            public Action OnSuccess;
            public Action OnFail;
        }

        public class InputHandler
        {
            private readonly InputHandlerOptions _opts = new InputHandlerOptions();

            public void AddOptions(Action<InputHandlerOptions> add) => add(_opts);

            public void Handle(int input)
            {
                if (input > _opts.MaxValue || (_opts.MustBeEven && input % 2 != 0))
                    _opts.OnFail();

                _opts.OnSuccess();
            }
        }

        [Fact]
        public void InputHandlerExample()
        {
            var myMax = 10;
            var mustBeEven = false;
            var isSuccess = false;

            var handler = new InputHandler();

            handler.AddOptions((opts) =>
            {
                opts.MaxValue = myMax;
                opts.MustBeEven = mustBeEven;
                opts.OnSuccess = new Action(() =>
                {
                    isSuccess = true;
                });
                opts.OnFail = new Action(() =>
                {
                    isSuccess = false;
                });
            });

            handler.Handle(9);
            Assert.True(isSuccess);

            handler.Handle(11);
            Assert.False(isSuccess);
        }
    }
}