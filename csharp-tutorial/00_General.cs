using Xunit;

namespace csharp_tutorial
{
    public class General
    {
        public class Hello
        {
            // This can be only set in constructor
            // Use this as often as possible, as it is generally not recommended to use mutable class variables
            private readonly int _myBestValue;

            public Hello(int value)
            {
                GettableText = "Set me free";
                _myBestValue = value;
            }

            // Getters and setters
            public string Text { get; set; }

            // Or can be only set in constructor (immutable)
            public string GettableText { get; }

            // Can have private setters
            public string PrivateText { get; private set; }

            // Backing filed for a preperty is a common case to use private variables
            private int _myValue;

            // Properties can have functionality
            public int CurrentValue
            {
                get => _myValue;
                set
                {
                    if (_myValue != value)
                    {
                        PrivateText = $"My current value is {value}";
                        _myValue = value;
                        // Usually here can be some on changeg event notifications etc.
                    }
                }
            }

            // Read only properties
            public string HelloText => $"Hello! My current value is {_myValue}";

            public string HelloTextOnce { get; } = $"Hello! My current value is 000";

            // There is something different how these are handled, so latter one can only have reference to static fields

            // Simple functions
            public string GetText(string ending) => $"Hello {ending}";

            public string GetText2(string ending)
            {
                return $"Hello {ending}";
            }

            // Local functions
            public string GetName(string name)
            {
                string Capitalize(string toChange)
                {
                    return toChange.Substring(0, 1).ToUpper() + toChange.Substring(1);
                }

                return Capitalize(name);
            }

            // When to use field vs property
            // When to use property vs method
        }

        [Fact]
        public void Hello_Samples()
        {
            var myValue = 1;

            var hello = new Hello(myValue);

            var helloText = hello.HelloText;

            var valueText = hello.PrivateText;

            var valueImmutable = hello.GettableText;

            var helloTimmy = hello.GetText("Timmy");
            var helloJames = hello.GetText2("James");

            var capitalized = hello.GetName("ramon");

            Assert.Equal("Ramon", capitalized);
        }

        public class NormalClass
        {
            private int Hidden { get; set; }

            protected int VisibleToParent { get; set; }

            public int Visible { get; set; }
        }

        internal class VisibleOnlyInsideAssembly
        { }

        public class VisibleToAll
        { }
    }
}