using System;
using Xunit;

namespace csharp_tutorial
{
    public class I_Tuples
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

            var (name, age) = PersonInfo();

            var p = PersonInfo2();

            Assert.Equal("Timmy", p.name);
            Assert.Equal(25, p.age);
        }

        private static (string, int) PersonInfo()
        {
            return ("Timmy", 25);
        }

        private static (string name, int age) PersonInfo2()
        {
            return ("Timmy", 25);
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
            var person = new Person("Timmy", "Tester");
            person.Age = 30;

            var (name, age) = person;

            Assert.Equal("Timmy Tester", name);
            Assert.Equal(30, age);
        }
    }
}