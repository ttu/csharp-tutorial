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
            var items = new List<object> {
                "Hello",
                1,
                5,
                new List<int> { 1, 2, 3 },
                new List<int> { 1, -2 },
                new Square { Side = 8 }
            };

            foreach (var item in items)
            {
                // NOTE: Switch case will show compiler error if case is already handled
                switch (item)
                {
                    case string e:
                        break;

                    case int e when e > 4:
                        break;

                    case int e:
                        // Try moving this up
                        break;

                    case IEnumerable<int> e when e.All(i => i > 0):
                        break;

                    case IEnumerable<int> e:
                        break;

                    case Square e when e.Side > 5:
                        break;

                    case Square e when e.Side > 5 && e.Side < 10:
                        // Compiler won't show errors in cases like this
                        break;

                    default:
                        break;
                }
            }
        }

        [Fact]
        public void If_and_casting()
        {
            var items = new List<Shape>
            {
                new Square { Side = 8 },
                new Square { Side = 2 },
                new Circle { Radius = 5},
                new Circle { Radius = 11},
            };

            foreach (var item in items)
            {
                if (item is Square s)
                {
                    // Safe casting with as
                    var a = item as Circle;
                    var h = item as Square;

                    // Unsafe casting
                    //var a2 = (Circle)item;
                }
                else if (item is Circle c && c.Radius > 10)
                { }
                else if (item is Circle ci)
                {
                    // No compiler errors if this is moved up
                }
            }
        }
    }

    public class Shape
    { }

    public class Square : Shape
    {
        public int Side { get; set; }
    }

    public class Circle : Shape
    {
        public int Radius { get; set; }
    }
}