using Xunit;

namespace csharp_tutorial
{
    public class B4_RefVal
    {
        // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/passing-parameters
        // Types are passed to methods by value
        // Passing a value-type variable to a method by value means passing a copy of the variable to the method
        // A variable of a reference type does not contain its data directly; it contains a reference to its data

        // http://net-informations.com/faq/general/valuetype-referencetype.htm

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
    }
}