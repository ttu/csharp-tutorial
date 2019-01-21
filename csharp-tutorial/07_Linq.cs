using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace csharp_tutorial
{
    public class LinqExamples
    {
        // LINQ was added to standard library before all this was widely used and
        // MS decided to use SQL namespace instead of mathematical one
        // map is select
        // filter is where
        // reduce is aggregate

        // LINQ is a good example of extension methods, delegates and anonymous types

        private class Person
        {
            public string Name { get; set; }
            public int Location { get; set; }
            public int Salary { get; set; }
        }

        [Fact]
        public void Linq()
        {
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
            var mapWithNameAndSalary = team.Select(x => new { x.Name, x.Salary });
            var map2 = team.Select(x => x.Location + ":" + x.Name);

            // Reduce (list to single item), Sum, Average
            var totalWithAfgg = team
                                .Select(x => x.Salary)
                                .Aggregate((acc, curr) => acc + curr);

            var totalExpense = team.Select(x => x.Salary).Sum();
            var totalExpense2 = team.Sum(x => x.Salary);
            var average = team.Average(e => e.Salary);

            var highestSalary = batch1.Where(e => e.Salary == batch1.Max(x => x.Salary));

            // All names to string with aggregate
            var allNames = team.Select(x => x.Name).Aggregate((acc, curr) => acc + "," + curr);

            // First, Single
            var item = batch1.FirstOrDefault(e => e.Location == 3);
            var item2 = batch1.Where(e => e.Location == 2).First();
            var item3 = batch1.Single(e => e.Salary == 3);

            // Group by
            var locationSalary = batch1
                                    .GroupBy(e => e.Location)
                                    .Select(e => new { Location = e.Key, TotalSalary = e.Sum(x => x.Salary) });

            // Zip
            var batch1bonus = new int[] { 5, 2, 3, 4 };
            var bonusAdded = batch1.Zip(batch1bonus, (person, bonus) => new { person.Name, Pay = person.Salary + bonus });
        }

        [Fact]
        public void Linq_ReferenceTypes()
        {
            var employees = new List<Person>(10)
            {
                new Person { Name = "Andy", Location = 1, Salary = 1 },
                new Person { Name = "Thomas", Location = 2, Salary = 1 },
                new Person { Name = "Jefferson", Location = 2, Salary = 2 },
                new Person { Name = "Ben", Location = 1, Salary = 2 }
            };

            // Give raise to all in location 2
            employees
                .Where(x => x.Location == 2)
                .ToList()
                .ForEach(x => x.Salary += 1);

            // Because person is a reference type, original item's salary changes

            // Some think that ForEach should not be part of LINQ as it is mainly used to mutate data
        }

        [Fact]
        public void Linq_Lazy()
        {
            var names = new[] { "Timmy", "Sammy" };

            // Creteate new Person for Timmy and Sammy
            var selected = names.Select(e => new Person { Name = e });

            // Select Timmy
            var timmy = selected.Where(e => e.Name == "Timmy");

            foreach (var s in selected)
                s.Salary = 40;

            var timmysSalary = timmy.SingleOrDefault().Salary;

            Assert.Equal(0, timmysSalary);
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
                SsnList = s.Value.OrderBy(r => r.Ssn).Select(r => r.Ssn).Aggregate((prev, curr) => prev + "," + curr)
            })
            .GroupBy(s => s.SsnList)
            .Select(s => Tuple.Create(s.Select(r => r.RequestId).ToList(), requests[s.First().RequestId]));
        }

        [Fact]
        public void Compare()
        {
            // All thata can also be implemented with using object's Equals method

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

            var results = requests["A"].Contains(requests["B"].First(e => e.Ssn == "2"));
            Assert.False(results);

            results = requests["A"].Contains(requests["A"].First(e => e.Ssn == "2"));
            Assert.False(results);

            results = requests["A"].Contains(new Requester2 { Ssn = "2" });
            Assert.False(results);
        }

        private class Requester2
        {
            public string Ssn { get; set; }

            public string Name { get; set; }

            public override bool Equals(object obj) => obj is Requester2 ? Ssn == ((Requester2)obj).Ssn : false;

            // Dictionaries and Hashsets use hash code to compare elements
            public override int GetHashCode() => Ssn.GetHashCode();
        }
    }
}