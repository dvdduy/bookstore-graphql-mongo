using BookStore.Infrastructure.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;

namespace BookStore.IntegrationTests
{
    public class GraphQLTestFixture : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                    // Override MongoDB configuration for testing
                    // Connecting without authentication (local development)
                    var mongoConfig = new MongoDbConfiguration
                    {
                        Database = "BookStoreTestDB",
                        ConnectionString = "mongodb://localhost:27017"
                    };

                // Replace the MongoDB configuration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(MongoDbConfiguration));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddSingleton(mongoConfig);
            });

            // Use Development environment to enable seeding
            builder.UseEnvironment("Development");
        }

        public async Task<HttpResponseMessage> ExecuteGraphQLQuery(string query, object? variables = null)
        {
            var client = CreateClient();
            
            var request = new
            {
                query = query,
                variables = variables
            };

            var content = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(request),
                System.Text.Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("/graphql", content);
            
            // Log response for debugging
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"GraphQL Response: {responseContent}");
            
            // Re-create response with the same content for the test to consume
            var newResponse = new HttpResponseMessage(response.StatusCode);
            newResponse.Content = new StringContent(responseContent, System.Text.Encoding.UTF8, "application/graphql-response+json");
            foreach (var header in response.Headers)
            {
                newResponse.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
            
            return newResponse;
        }
    }
}

