using System;
using Xunit;

namespace csharp_tutorial
{
    public class Tuples
    {
        // Tuples fall a little bit to the same category as dynamic. Usually not needed or recommended in the production code,
        // as you should use classes instead, but using these while e.g. prototyping is faster

        [Fact]
        public void TupleTest()
        {
            Tuple<string, int> tp = Tuple.Create("a1", 3);
            var tp2 = Tuple.Create("a2", 4);

            // Tuples are immutable
            // tp.Item2 = 4;

            Assert.Equal(3, tp.Item2);
            Assert.Equal(4, tp2.Item2);
        }

        [Fact]
        public void ValueTuple()
        {
            var tp = ("a1", 3);
            var tp2 = ("a2", 4);

            // ValueTuples are not immutable
            tp.Item2 = 4;

            Assert.Equal(4, tp.Item2);
            Assert.Equal(4, tp2.Item2);

            var tpAsTuple = tp.ToTuple();

            // Immutable again
            //tpAsTuple.Item2 = 4;
        }

        [Fact]
        public void ValueTuple_Names()
        {
            // Create a ValueTuple with names.
            var result = (name: "Timmy", age: 25);

            Assert.Equal("Timmy", result.name);
            Assert.Equal(25, result.age);

            var i = GetPersonInfo();

            Assert.Equal("Timmy", i.Item1);
            Assert.Equal(25, i.Item2);

            var (name, age) = GetPersonInfo();

            var p = GetPersonInfoNamed();

            Assert.Equal("Timmy", p.name);
            Assert.Equal(25, p.age);
        }

        private static (string, int) GetPersonInfo()
        {
            var myName = "Timmy";
            var myAge = 25;
            return (myName, myAge);
        }

        private static (string name, int age) GetPersonInfoNamed()
        {
            var myName = "Timmy";
            var myAge = 25;
            return (myName, myAge);
        }

        public class Person
        {
            private readonly string _firstName;
            private readonly string _lastName;

            public Person(string firstName, string lastName)
            {
                _firstName = firstName;
                _lastName = lastName;
            }

            public int Age { get; set; }

            public void Deconstruct(out string fullName, out int age)
            {
                fullName = $"{_firstName} {_lastName}";
                age = Age;
            }
        }

        [Fact]
        public void Deconstruct()
        {
            var person = new Person("Timmy", "Tester")
            {
                Age = 30
            };

            var (name, age) = person;

            Assert.Equal("Timmy Tester", name);
            Assert.Equal(30, age);
        }
    }
}