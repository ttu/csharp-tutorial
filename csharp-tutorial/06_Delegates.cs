using System;
using System.Diagnostics;
using Xunit;

namespace csharp_tutorial
{
    public class DelegateExamples
    {
        [Fact]
        public void Anonymous_Functions()
        {
            // These are all same thing

            // Lambda syntax
            Func<string, string> func = (s) => $"Hello {s}";
            Func<string, string> func2 = (s) => { return $"Hello {s}"; };

            // Old-school delegate syntax (do not use)
            Func<string, string> func3 = delegate (string s) { return $"Hello {s}"; };
            // In same case had to use delefate where amount of parameters was different
            // public delegate T Method(params object[] parameters);
            //var funcParams = delegate (params object[] s) { return $"Hello {s}"; };

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

        [Fact]
        public void MulticastDelegates()
        {
            Action<int> crunchNumber = (i) => { Trace.WriteLine($"Do some fancy stuff with this integer {i}"); };

            crunchNumber(1);

            // Later we decide that we need to do some writing to log when action is executed
            crunchNumber += (i) => Trace.WriteLine($"Writing to log {i}");

            crunchNumber(2);

            // Later also add POST
            crunchNumber += (i) =>
            {
                //TODO: POST with http to some external service
                Trace.WriteLine($"POST {i}");
            };

            crunchNumber(3);
        }

        [Fact]
        public void Retry_Example()
        {
            int Add2(int i) => i + 2;

            T MaybeException<T>(Func<T> action)
            {
                if (DateTime.Now.Ticks % 2 == 0)
                    throw new Exception("Bad luck");
                return action();
            }

            Assert.Equal(5, Add2(3));

            var result = RetryHelper(() => MaybeException(() => Add2(4)));
            Assert.Equal(6, result);
        }

        public static T RetryHelper<T>(Func<T> action, int tryCount = 2)
        {
            while (true)
            {
                try
                {
                    return action();
                }
                catch (Exception)
                {
                    Trace.WriteLine("Got exception");

                    if (--tryCount > 0)
                        continue;

                    throw;
                }
            }
        }
    }
}