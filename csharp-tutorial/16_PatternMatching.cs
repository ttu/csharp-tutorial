using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace csharp_tutorial
{
    public class PatternMatching
    {
        [Fact]
        public void Switch()
        {
            var items = new List<object> { "Hello", 1, 5, new List<int> { 1, 2, 3 }, new List<int> { 1, -2 } };

            foreach (var item in items)
            {
                switch (item)
                {
                    case string e:
                        break;

                    case int e when e > 4:
                        break;

                    case int e:
                        break;

                    case IEnumerable<int> e when e.All(i => i > 0):
                        break;

                    case IEnumerable<int> e:
                        break;

                    default:
                        break;
                }
            }
        }

        [Fact]
        public void If()
        {
            var items = new List<object> { 1, "Hello" };

            foreach (var item in items)
            {
                if (item is int e)
                { }
                else if (item is string st)
                { }
            }
        }
    }
}