using System;
using Xunit;

namespace csharp_tutorial
{
    public class A_General
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
                get { return _myValue; }
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
        }

        [Fact]
        public void Null_Coalescing()
        {
            string value = null;

            // Null coalescing, if left side is null, take right side
            string print = value ?? "default value";
            // Same as normal one line condition
            string print2 = value != null ? value : "default value";
        }

        public class NullExample
        {
            public NullExample Child { get; set; }

            public string Text { get; set; }
        }

        [Fact]
        public void Null_Check()
        {
            var hello = new NullExample
            {
                Child = new NullExample { Text = "Abba" }
            };

            string text = hello.Child.Text;

            try
            {
                // Because of null values NullReferenceException is thrown
                string textException = hello.Child.Child.Child.Text;
            }
            catch (NullReferenceException e)
            {
            }

            string text2 = hello?.Child?.Child?.Child?.Text;
            Assert.Null(text2);

            string text3 = hello?.Child?.Text;
            Assert.Equal(text, text3);
        }

        [Fact]
        public void NullCoalescing_Check()
        {
            var example = new NullExample();

            // If value is not null left side is processed, else right side
            var text = example.Child?.Text ?? "Not set";

            Assert.Equal("Not set", text);
        }

        [Fact]
        public void Nullable_Types()
        {
            // int is a value type and values types can't be null (default value of value type is 0bit, so for int is 0, bool is false etc.)
            // By adding ? to type identifier value types can also be null
            int? myValue = null;
            int valueCheck = myValue ?? 2;

            myValue = 6;

            valueCheck = myValue ?? 2;

            if (myValue.HasValue)
            {
                // Do something
            }
        }
    }
}
