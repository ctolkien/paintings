using Xunit;
using System.Threading.Tasks;
using WorldDomination.Net.Http;
using Painting.Website.Repositories;
using System.IO;
using System.Net.Http;
using Shouldly;
using System.Linq;
using Painting.Website.Models;

namespace Painting.Website.Tests
{
    public class TestObjectNumbers
    {
        [Fact]
        public async Task TestIfFunctionReturnIEnumberableObjectNumbersAsync()
        {
            // Arrange.
            var responseData = File.ReadAllText("Sample Data\\input.json");
            var messageResponse = FakeHttpMessageHandler.GetStringHttpResponseMessage(responseData);
            var options = new HttpMessageOptions
            {
                HttpMethod = HttpMethod.Get,
                RequestUri = "https://www.rijksmuseum.nl/api/nl/collection?key=secret&format=json&type=schilderij&toppieces=True", 
                HttpResponseMessage = messageResponse
            };
            var messageHandler = new FakeHttpMessageHandler(options);
            var service = new ObjectNumberRepository(messageHandler);
            var painting = new Paintings(service); 

            // Act.
            var results = (await painting.GetObjectNumberAsync("secret")).ToArray();

            // Assert.
            options.NumberOfTimesCalled.ShouldBe(1); // We only called that HttpMethod + Uri once.
            results.ShouldNotBeNull();
            results.Length.ShouldBe(10);
            results.First().ShouldBe("SK-A-3148");
            
        }
    }
}
