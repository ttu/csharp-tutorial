using System;
using Xunit;

namespace csharp_tutorial
{
    public class Null
    {
        [Fact]
        public void Null_Coalescing()
        {
            string value = null;

            // Null coalescing, if left side is null, take right side
            string print = value ?? "default value";

            Assert.Equal("default value", print);

            // Same as normal one line condition
            string print2 = value != null ? value : "default value";

            Assert.Equal("default value", print2);

            // Set something to value
            value = "This has something";

            string print3 = value ?? "default value";

            Assert.Equal("This has something", print3);
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
            catch (NullReferenceException)
            {
            }

            string text2 = hello?.Child?.Child?.Child?.Text;
            Assert.Null(text2);

            string text3 = hello?.Child?.Text;
            Assert.Equal(text, text3);

            // Null coalescing check
            var text4 = hello?.Child?.Child?.Text ?? "Not set";

            Assert.Equal("Not set", text4);
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