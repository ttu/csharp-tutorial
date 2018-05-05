using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace csharp_tutorial
{
    public class Generics
    {
        public class GenericFunctionsBag<T> where T : class
        {
            public T GetDefault() => default(T);

            public J GetDefault<J>() => default(J);

            public bool IsType<K>(string json) => JsonConvert.DeserializeObject<K>(json) != null;

            public T GetFirstItem(IEnumerable<T> items) => items.First();

            public J GetLast<J>(IEnumerable<J> items) => items.Last();
        }

        [Fact]
        public void GenericsExamples()
        {
            var genericBag = new GenericFunctionsBag<User>();

            var defUser = genericBag.GetDefault();
            var defInt = genericBag.GetDefault<int>();

            var isUser = genericBag.IsType<User>("{ 'value' : 2 }");
            Assert.False(isUser);

            isUser = genericBag.IsType<User>("{ 'name' : 'jimmy' }");
            Assert.True(isUser);
        }

        [Fact]
        public void Collections_Generic()
        {
            // Common place to use generics are collections

            var list = new List<int> { 1, 2, 3 };
            list.Add(4);

            var dict = new Dictionary<string, User>
            {
                ["test"] = new User { Age = 30 },
                ["unit"] = new User { Age = 25 },
            };
        }

        public class User
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }
    }
}