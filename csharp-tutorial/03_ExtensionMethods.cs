using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace csharp_tutorial
{
    public class ExensionMethods
    {
        [Fact]
        public void ExtensionMethods_Value()
        {
            var number = 200;
            Assert.True(number.IsPositive());
        }

        [Fact]
        public void ExtensionMethods_Class()
        {
            var person = new Person { FirstName = "Larry", LastName = "Smith" };
            Assert.Equal("Larry Smith", person.FullName());
        }

        [Fact]
        public async Task ExtensionMethods_InsteadOfInheritance()
        {
            var client = new HttpClient();
            var response = await client.PatchAsync("www.google.com", new StringContent("Patch json here"));
        }
    }

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public static class MyExtensions
    {
        public static bool IsPositive(this int value)
        {
            return value > 0;
        }

        public static string FullName(this Person person)
        {
            return person.FirstName + " " + person.LastName;
        }

        public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri, HttpContent content)
        {
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri) { Content = content };
            return await client.SendAsync(request);
        }
    }

    // Without ExtensionMethods we would need to inherit HttpClient
    public class MyHttpClient : HttpClient
    {
        public async Task<HttpResponseMessage> PatchAsync(string requestUri, HttpContent content)
        {
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri) { Content = content };
            return await SendAsync(request);
        }
    }
}