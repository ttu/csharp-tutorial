using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xunit;

namespace csharp_tutorial
{
    public class Generics
    {
        [Fact]
        public void Collections_Generic()
        {
            // Common place to use generics are collections

            var intList = new List<int> { 1, 2, 3 };
            intList.Add(4);

            var intArray = new[] { 1, 2, 3 };

            var stringList = new List<string> { "hello", "test", "ok" };
            stringList.Add("NewItem");

            var dict = new Dictionary<string, User>
            {
                ["test"] = new User { Age = 30 },
                ["unit"] = new User { Age = 25 },
            };

            dict.Add("integration", new User { Age = 50 });

            // And like most languages, C# has also many ways to do a same things
            var dict2 = new Dictionary<string, User>
            {
                { "test", new User { Age = 30 } },
                { "unit", new User { Age = 25 } }
            };

            IReadOnlyList<string> readonlyList = new List<string> { "hello" };

            var list = new List<string> { "hello" };
            var reaondOnlyCollection = new ReadOnlyCollection<string>(list);

            var occupationsMutable = new Dictionary<string, string>
            {
                ["Malcolm"] = "Captain",
                ["Kaylee"] = "Mechanic"
            };
            occupationsMutable["Jayne"] = "Public Relations";
            occupationsMutable.Add("Rick", "Navigation");
        }

        public class GenericFunctionsBag<T> where T : class
        {
            private List<T> _items;

            public int ItemCount => _items.Count;

            public void AddItem(T newItem) => _items.Add(newItem);

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

            // Won't work, because of where T : class
            //var gb = new GenericFunctionsBag<int>();

            genericBag.AddItem(new User());
            var count = genericBag.ItemCount;

            var defUser = genericBag.GetDefault();
            var defInt = genericBag.GetDefault<int>();

            var isUser = genericBag.IsType<User>("{ 'value' : 2 }");
            Assert.False(isUser);

            isUser = genericBag.IsType<User>("{ 'name' : 'jimmy' }");
            Assert.True(isUser);
        }

        [Fact]
        public void Casting()
        {
            var admin = new Admin { Name = "Timmy" };

            var user = new User { Name = "James" };

            // Can't cast to more specific type
            // Safe casting with as
            var adminAsUser = admin as User;
            var willBeNull = user as PowerUser;

            // Unsafe casting
            var willThrow = (PowerUser)user;

            // Common use case
            void handleUser(User us)
            {
                if (us is Admin) { }
                else if (us is PowerUser) { }
            }
        }

        public IEnumerable<User> OrderByAge(IEnumerable<User> users)
        {
            // OrderBy comes from linq, more of that later
            return users.OrderBy(e => e.Age);
        }

        public IEnumerable<T> OrderByAgeGeneric<T>(IEnumerable<T> users) where T : User
        {
            return users.OrderBy(e => e.Age);
        }

        [Fact]
        public void Function_Type_Specific()
        {
            // Common place to use generics are collections

            var admins = new List<Admin> { new Admin { }, new Admin { } };
            var powerUsers = new List<PowerUser> { new PowerUser { }, new PowerUser { } };

            var sortedAdmins = OrderByAge(admins);
            // sortedAdmins are now Users
            //sortedAdmins.First().Type

            var sortedAdminsGenerics = OrderByAgeGeneric(admins);
            // Now sortedAdmins are Admins
            var firstType = sortedAdminsGenerics.First().Type;
        }

        public class User
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        public class PowerUser : User
        {
            public string Password { get; set; }
        }

        public class Admin : User
        {
            public string Type { get; set; }
        }
    }
}