using System;
using Xunit;

namespace csharp_tutorial
{
    public class CompositionExamples
    {
        // In real life inheritance/composition cases are never this simple

        public abstract class Handler
        {
            public abstract string Handle(string input);
        }

        public class UpperHandler : Handler
        {
            public override string Handle(string input)
            {
                return input.ToUpper();
            }
        }

        public class LowerHandler : Handler
        {
            public override string Handle(string input)
            {
                return input.ToLower();
            }
        }

        [Fact]
        public void Inheritance()
        {
            Handler handlerFactory(int type)
            {
                if (type == 0)
                    return new LowerHandler();
                else
                    return new UpperHandler();
            }

            Handler h1 = handlerFactory(0);
            var response = h1.Handle("HeLlo");
            Assert.Equal("hello", response);

            Handler h2 = handlerFactory(1);
            var response2 = h2.Handle("HeLlo");
            Assert.Equal("HELLO", response2);
        }

        public class HandlerComposition
        {
            private readonly Func<string, string> _handleFunc;

            public HandlerComposition(Func<string, string> handleFunc) => _handleFunc = handleFunc;

            public string Handle(string input) => _handleFunc(input);
        }

        [Fact]
        public void Composition()
        {
            HandlerComposition Builder(int type)
            {
                if (type == 0)
                    return new HandlerComposition((i) => i.ToLower());
                else
                    return new HandlerComposition((i) => i.ToUpper());
            }

            HandlerComposition h1 = Builder(0);
            var response = h1.Handle("HeLlo");
            Assert.Equal("hello", response);

            HandlerComposition h2 = Builder(1);
            var response2 = h2.Handle("HeLlo");
            Assert.Equal("HELLO", response2);
        }
    }
}