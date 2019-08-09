using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
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
            var hello = new { Text = "Hello", SubType = new { Text = "World" } };

            //String interpolation vs format
            Assert.Equal("Hello World", $"{hello.Text} {hello.SubType.Text}");
            Assert.Equal("Hello World", string.Format("{0} {1}", hello.Text, hello.SubType.Text));

            var helloDynamic = GetHello();

            //String interpolation
            Assert.Equal("Hello World", $"{helloDynamic.Text} {helloDynamic.SubType.Text}");
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

            dynamic CreateObjectFor(Dictionary<string, object> values)
            {
                dynamic personExpando = new ExpandoObject();
                var dictionary = (IDictionary<string, object>)personExpando;

                foreach (var pair in values)
                    dictionary.Add(pair.Key, pair.Value);

                return personExpando;
            }

            var properties = new Dictionary<string, object>()
            {
                ["Name"] = "James",
                ["Age"] = 50
            };

            dynamic person2 = CreateObjectFor(properties);

            // Common case is that you get some data, parse do some stuff and create a new object from it and send it
            person2.Age = 50;
            var json = JsonConvert.SerializeObject(person2);

            // It is also possible to add methods
            person2.SayHello = new Action(() => Trace.WriteLine("Hello"));

            var expDict = (IDictionary<string, object>)person2;
            expDict.Add("SayWorld", new Action(() => { Trace.WriteLine("World"); }));

            person2.SayHello();
            person2.SayWorld();
        }

        [Fact]
        public void DynamicsCompilerOff_SetWrongType()
        {
            dynamic validNumber = GetNumber();
            int other = TransformIntToString(validNumber);
        }

        private int GetNumber()
        {
            return 4;
        }

        private string TransformIntToString(int input)
        {
            return input.ToString();
        }

        [Fact]
        public void DynamicsCompilerOff_ReturnWrongType()
        {
            int a = GetNumber2IfParameterTrue(true);

            int b = GetNumber2IfParameterTrue(false);
        }

        private int GetNumber2IfParameterTrue(dynamic isTrue)
        {
            dynamic GetInner(bool check)
            {
                if (check)
                    return 2;
                else
                    return "1";
            }

            // When using dynamics, compiler will not give errors on compile time
            var value = GetInner(isTrue);
            return value;
        }
    }
}