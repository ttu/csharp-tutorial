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

            var personTuple = GetPersonInfoValueTuple();

            Assert.Equal("Timmy", personTuple.Item1);
            Assert.Equal(25, personTuple.Item2);

            var (name, age) = GetPersonInfoValueTuple();

            Assert.Equal("Timmy", name);
            Assert.Equal(25, age);

            var person = GetPersonInfoNamedTuple();

            Assert.Equal("Timmy", person.name);
            Assert.Equal(25, person.age);
        }

        private Tuple<string, int> GetPersonInfoTuple()
        {
            var myName = "Timmy";
            var myAge = 25;
            return Tuple.Create(myName, myAge);
        }

        private (string, int) GetPersonInfoValueTuple()
        {
            var myName = "Timmy";
            var myAge = 25;
            return (myName, myAge);
        }

        private (string name, int age) GetPersonInfoNamedTuple()
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