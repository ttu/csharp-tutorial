using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace csharp_tutorial
{
    public class A3_Linq
    {
        [Fact]
        public void Lists_Etc()
        {
            // Lists, Arrays, Dictionaries, etc.
            var list = new List<int> { 1, 2, 3 };
            list.Add(4);

            var list2 = new[] { 1, 2, 3 };

            // And like all fucked up languages C# has also many ways to do a same thing
            var dic = new Dictionary<int, string>
            {
                [1] = "test",
                [2] = "unit"
            };

            var dic2 = new Dictionary<int, string>
            {
                { 1, "test" },
                { 2, "test" }
            };
        }

        private class Person
        {
            public string Name { get; set; }
            public int Location { get; set; }
            public int Salary { get; set; }
        }

        [Fact]
        public void Linq()
        {
            // Linq was added to standard library before all this was widely used and
            // MS decided to use SQL namespace instead of mathematical one
            // map is select
            // filter is where
            // reduce is aggregate

            var batch1 = new List<Person>(10)
            {
                new Person { Name = "Andy", Location = 1, Salary = 1 },
                new Person { Name = "Thomas", Location = 2, Salary = 1 },
                new Person { Name = "Jefferson", Location = 2, Salary = 2 },
                new Person { Name = "Ben", Location = 1, Salary = 2 }
            };

            var batch2 = new List<Person>(10)
            {
                new Person { Name = "Jack", Location = 1, Salary = 2 },
                new Person { Name = "Daniel", Location = 2, Salary = 2 },
                new Person { Name = "Sam", Location = 2, Salary = 1 },
                new Person { Name = "Dick", Location = 2, Salary = 2 }
            };

            // Merge (combine 2 lists)
            var team = batch1.Union(batch2);

            // Filter (select only items that match with predicate)
            var location2 = team.Where(x => x.Location == 2);

            // Map (execute selection to every item)
            var map = team.Select(x => new { x.Name, x.Salary });
            var map2 = team.Select(x => x.Location + ":" + x.Name);

            // Reduce (list to single item)
            var totalExpense = team.Select(x => x.Salary).Sum();
            var totalWithAfgg = team.Select(x => x.Salary).Aggregate((prev, curr) => prev + curr);
            var allNames = team.Select(x => x.Name).Aggregate((prev, curr) => prev + "," + curr);

            var batch1bonus = new int[] { 5, 2, 3, 4 };
            var bonusAdded = batch1.Zip(batch1bonus, (person, bonus) => new { person.Name, Pay = person.Salary + bonus });
        }

        [Fact]
        public void WordCount()
        {
            var data = "Deer Bear River\nCar Car River\nDeer Car Bear";

            var split = data.Split('\n');

            var map = split.Select(s => s.Split(' ').Select(i => new { Title = i, Value = 1 }).ToList());
            var map2 = split.Select(s => s.Split(' ').Select(i => new { Title = i, Value = 1 }));
            var map3 = split.Select(s => s.Split(' ').ToList());

            var flat = map3.SelectMany(i => i);
            var groupedByWords = flat.GroupBy(i => i);
            var red = groupedByWords.Select(i => new { Title = i.Key, Count = i.Count() });

            var shuffle = map.SelectMany(i => i).GroupBy(i => i.Title);

            var reduce = shuffle.Select(i => new { Title = i.Key, Count = i.Count() });
        }

        private class Requester
        {
            public string Ssn { get; set; }
            public string Name { get; set; }
        }

        [Fact]
        public void K()
        {
            // Combine requests to sets that have same requesters

            var requests = new Dictionary<string, IList<Requester>>();

            requests.Add("A", new List<Requester> {
                new Requester { Ssn = "1" },
                new Requester { Ssn = "2" },
                new Requester { Ssn = "3" }
            });

            requests.Add("B", new List<Requester> {
                new Requester { Ssn = "2" },
                new Requester { Ssn = "1" },
                new Requester { Ssn = "3" }
            });

            requests.Add("C", new List<Requester> {
                new Requester { Ssn = "3" }
            });

            requests.Add("D", new List<Requester> {
                new Requester { Ssn = "3" }
            });

            requests.Add("E", new List<Requester> {
                new Requester { Ssn = "1" },
                new Requester { Ssn = "2" }
            });

            IEnumerable<Tuple<List<string>, IList<Requester>>> requestSet;

            requestSet = requests.Select(s => new
            {
                RequestId = s.Key,
                Ssn = s.Value.OrderBy(r => r.Ssn).Select(r => r.Ssn).Aggregate((c, n) => c + "," + n)
            })
            .GroupBy(s => s.Ssn)
            .Select(s => Tuple.Create(s.Select(r => r.RequestId).ToList(), requests[s.First().RequestId]));
        }

        [Fact]
        public void Compare()
        {
            // All thata can also be implemented with using objects Equals method

            var requests = new Dictionary<string, IList<Requester2>>();

            requests.Add("A", new List<Requester2> {
                new Requester2 { Ssn = "1" },
                new Requester2 { Ssn = "2" },
                new Requester2 { Ssn = "3" }
            });

            requests.Add("B", new List<Requester2> {
                new Requester2 { Ssn = "2" },
                new Requester2 { Ssn = "1" },
                new Requester2 { Ssn = "3" }
            });

            requests.Add("C", new List<Requester2> {
                new Requester2 { Ssn = "3" }
            });

            requests.Add("D", new List<Requester2> {
                new Requester2 { Ssn = "3" }
            });

            requests.Add("E", new List<Requester2> {
                new Requester2 { Ssn = "1" },
                new Requester2 { Ssn = "2" }
            });

            // B contains all from A (order doesn't matter)
            var hasAll = requests["A"].All(requests["B"].Contains);

            Assert.True(hasAll);
        }

        private class Requester2
        {
            public string Ssn { get; set; }

            public string Name { get; set; }

            public override bool Equals(object obj) => obj is Requester2 ? Ssn == ((Requester2)obj).Ssn : false;

            // Dictionaries and Hashsets use HashCode to compare elements
            public override int GetHashCode() => Ssn.GetHashCode();
        }
    }
}