using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace csharp_tutorial
{
    public class H_PatternMatching
    {
        [Fact]
        public void Switch()
        {
            var items = new List<object> { 1, 5, "Hello", new List<int> { 1, 2, 3} };

            foreach (var item in items)
            {
                switch (item)
                {
                    case int e when e > 4:
                        break;

                    case int e:
                        break;

                    case string e:
                        break;

                    case IEnumerable<int> e when e.All(i => i > 0):
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
                else if (item is string)
                { }
            }
        }
    }
}