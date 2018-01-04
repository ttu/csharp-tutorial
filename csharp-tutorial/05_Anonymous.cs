using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Xunit;

namespace csharp_tutorial
{
    public class AnonymousExamples
    {
        [Fact]
        public void Implicit_Type()
        {
            int myNumeber = 2;
            var otherNumber = 2;

            Assert.Equal(myNumeber, otherNumber);

            string hello = "Hello";
            var hello2 = "Hello";

            Assert.Equal(hello, hello2);
        }

        [Fact]
        public void Anonymous_Types()
        {
            var hello = GetHello();

            //String interpolation vs format
            Assert.Equal("Hello World", $"{hello.Text} {hello.SubType.Text}");
            Assert.Equal("Hello World", string.Format("{0} {1}", hello.Text, hello.SubType.Text));
        }

        // Can return anonymous types with dynamic
        public dynamic GetHello()
        {
            return new { Text = "Hello", SubType = new { Text = "World" } };
        }

        [Fact]
        public void Dynamic_Immutable()
        {
            dynamic immu = new { Name = "Immu" };

            Assert.Equal(immu.Name, "Immu");

            // Dynamic objects are read only
            // immu.Name = "Not possible";

            dynamic person = new ExpandoObject();
            person.Name = "James";
            person.Age = 50;

            Assert.Equal(50, person.Age);

            person.Age = 30;
            Assert.Equal(30, person.Age);

            dynamic CreateObjectFor(params dynamic[] values)
            {
                dynamic personExpando = new ExpandoObject();
                var dictionary = (IDictionary<string, object>)personExpando;

                // We just have to rely that values are tuples, but that is normal with dynamic programming
                foreach (var pair in values)
                    dictionary.Add(pair.Item1, pair.Item2);

                return personExpando;
            }

            dynamic person2 = CreateObjectFor(Tuple.Create("Name", "James"), Tuple.Create("Age", 40));

            // Common case is that you get some data, parse do some stuff and create a new object from it and send it
            person2.Age = 50;
            var json = JsonConvert.SerializeObject(person2);

            // It is also possible to add methods
            var expDict = (IDictionary<string, object>)person2;
            expDict.Add("Say", new Action(() => { Console.WriteLine("Hello"); }));
            person2.Say();
        }
    }
}