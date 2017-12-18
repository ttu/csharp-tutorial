using System;
using System.Diagnostics;
using Xunit;

namespace csharp_tutorial
{
    public class B_Anonymous
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
        public void Anonymous_Functions()
        {
            // These are all same thing

            // Lambda syntax
            Func<string, string> func = (s) => $"Hello {s}";
            Func<string, string> func2 = (s) => { return $"Hello {s}"; };
            // Old-school delegate syntax
            Func<string, string> func3 = delegate (string s) { return $"Hello {s}"; };
            // Or can just take reference to method
            Func<string, string> func4 = GetHelloText;

            // Hopefully some day compiler will get smart enough and we can just do this
            //var func = (s) => { return $"Hello {s}"; };

            Trace.WriteLine(func("World"));
            Trace.WriteLine(func2("World"));
            Trace.WriteLine(func3("World"));
            Trace.WriteLine(func4("World"));

            var sayer = new HelloSayer(func);
            string text = sayer.Say("World");

            var sayer2 = new HelloSayer((s) => $"Not {s}");
            string text2 = sayer2.Say("Nice");

            Trace.WriteLine(text);
            Trace.WriteLine(text2);
        }

        public class HelloSayer
        {
            private readonly Func<string, string> _say;

            public HelloSayer(Func<string, string> say)
            {
                _say = say;
            }

            public string Say(string text)
            {
                return _say(text);
            }
        }

        private string GetHelloText(string text)
        {
            return $"Hello {text}";
        }
    }
}