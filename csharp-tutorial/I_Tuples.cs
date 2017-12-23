using System;
using Xunit;

namespace csharp_tutorial
{
    public class I_Tuples
    {
        // Tuples fall little bit to the same category as dynamic. Usually not needed or recommended in production code,
        // as you should use classes instead, but makes prototyping faster

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
    }
}