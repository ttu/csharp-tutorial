using System.Collections.Generic;
using Xunit;

namespace csharp_tutorial
{
    public class RefVal
    {
        /*
         * The Types in .NET Framework are either treated by Value Type or by Reference Type.
         * A Value Type holds the data within its own memory allocation and
         * a Reference Type contains a pointer to another memory location that holds the real data.
         * Reference Type variables are stored in the heap while Value Type variables are stored in the stack.
        */

        // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/passing-parameters
        // Types are passed to methods by value
        // Passing a value-type variable to a method by value means passing a copy of the variable to the method
        // A variable of a reference type does not contain its data directly; it contains a reference to its data

        // http://net-informations.com/faq/general/valuetype-referencetype.htm

        // Coffee cup example
        // https://www.mathwarehouse.com/programming/passing-by-value-vs-by-reference-visual-explanation.php

        [Fact]
        public void Update_Value()
        {
            int original = 2;

            DoSomething(original);
            Assert.Equal(2, original);

            DoSomething(ref original);
            Assert.Equal(5, original);
        }

        private void DoSomething(int myValue)
        {
            myValue = 5;
        }

        private void DoSomething(ref int myValue)
        {
            myValue = 5;
        }

        public class Person
        {
            public string Name { get; set; }
        }

        [Fact]
        public void Update_Class()
        {
            var person = new Person { Name = "Harry" };

            UpdatePerson(person);
            Assert.Equal("Harry", person.Name);

            UpdatePersonName(person);
            Assert.Equal("Timmy", person.Name);

            UpdatePerson(ref person);
            Assert.Equal("Sammy", person.Name);
        }

        private void UpdatePerson(Person person)
        {
            person = new Person { Name = "Sammy" };
        }

        private void UpdatePersonName(Person person)
        {
            person.Name = "Timmy";
        }

        private void UpdatePerson(ref Person person)
        {
            person = new Person { Name = "Sammy" };
        }

        [Fact]
        public void Out()
        {
            void GetSome(out int value)
            {
                value = 5;
            }

            GetSome(out var myValue);

            Assert.Equal(5, myValue);
        }

        [Fact]
        public void Out_Collection()
        {
            var dict = new Dictionary<string, Person>
            {
                ["111"] = new Person { Name = "Roger" }
            };

            if (dict.TryGetValue("111", out var person))
            {
                person.Name = "Steve";
            }
        }

        private class User
        {
            public string Ssn { get; set; }

            public string Name { get; set; }

            public override bool Equals(object obj) => obj is User us ? Ssn == us.Ssn : false;

            // Some collections group items to buckets by hashcode
            public override int GetHashCode() => Ssn.GetHashCode();
        }

        // For predefined value types, the equality operator (==) returns true if the values of its operands are equal, false otherwise.
        // For reference types other than string, == returns true if its two operands refer to the same object.

        [Fact]
        public void EqualsExample()
        {
            var person1 = new Person { Name = "James" };
            var person2 = new Person { Name = "James" };

            Assert.False(person1.Equals(person2));
            Assert.False(person1 == person2);
            Assert.True(person1 == person1);

            var user1 = new User { Ssn = "12345" };
            var user2 = new User { Ssn = "12345" };

            Assert.True(user1.Equals(user2));
            Assert.False(user1 == user2);
        }
    }
}