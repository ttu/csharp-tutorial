using System;
using Xunit;

namespace csharp_tutorial
{
    public class B3_Composition
    {
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
        public void Handler_Ingeritence()
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

            Handler h2 = handlerFactory(1);
            var response2 = h2.Handle("HeLlo");
        }

        public class HandlerComposition
        {
            private readonly Func<string, string> _handleFunc;

            public HandlerComposition(Func<string, string> handleFunc) => _handleFunc = handleFunc;

            public string Handle(string input) => _handleFunc(input);
        }

        [Fact]
        public void Anonymous_Composition()
        {
            var h1 = new HandlerComposition((i) => i.ToLower());
            var response = h1.Handle("HeLlo");

            var h2 = new HandlerComposition((i) => i.ToUpper());
            var response2 = h2.Handle("HeLlo");
        }
    }
}