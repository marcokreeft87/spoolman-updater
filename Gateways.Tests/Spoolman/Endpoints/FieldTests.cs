using FluentAssertions;
using RichardSzalay.MockHttp;
using System.Reflection;

namespace Gateways.Tests
{
    internal class FieldTests : EndpointTest<FieldSpoolmanEndpoint>
    {
        [Test]
        public async Task GivenExistingField_WhenGetOrCreate_ExistingFieldShouldBeReturned()
        {
            // Arrange
            Endpoint.GetType().GetProperty("FieldType", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(Endpoint, EntityType.Spool);

            // Arrange 
            var result = await Endpoint.CheckFieldExistence();

            // Assert   
            result.Should().BeTrue();
        }

        [Test]
        public async Task GivenNonExistingField_WhenGetOrCreate_CreatedFieldShouldBeReturned()
        {
            // Arrange
            Endpoint.GetType().GetProperty("FieldType", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(Endpoint, EntityType.Filament);

            // Arrange 
            var result = await Endpoint.CheckFieldExistence();

            // Assert   
            result.Should().BeTrue();
        }

        public override void SetupHttpClient(MockHttpMessageHandler mockHandler)
        {
            mockHandler
                .When("/api/v1/field/spool")
                .Respond("application/json", "[{\"name\":\"tag\",\"order\":0,\"field_type\":\"text\",\"key\":\"tag\",\"entity_type\":\"spool\"},{\"name\":\"Active Tray\",\"order\":1,\"field_type\":\"text\",\"default_value\":\"\\\"\\\"\",\"key\":\"active_tray\",\"entity_type\":\"spool\"},{\"name\":\"barcode\",\"order\":0,\"field_type\":\"text\",\"key\":\"barcode\",\"entity_type\":\"spool\"}]");

            mockHandler
                .When("/api/v1/field/filament")
                .Respond("application/json", "[]");

            mockHandler
                .When(HttpMethod.Post, "/api/v1/field/filament/barcode")
                .Respond("application/json", "[{\"name\":\"barcode\",\"order\":0,\"field_type\":\"text\",\"key\":\"barcode\",\"entity_type\":\"filament\"}]");

            mockHandler
                .When(HttpMethod.Post, "/api/v1/field/filament/active_tray")
                .Respond("application/json", "[{\"name\":\"active_tray\",\"order\":0,\"field_type\":\"text\",\"key\":\"active_tray\",\"entity_type\":\"filament\"}]");

            mockHandler
                .When(HttpMethod.Post, "/api/v1/field/filament/tag")
                .Respond("application/json", "[{\"name\":\"tag\",\"order\":0,\"field_type\":\"text\",\"key\":\"tag\",\"entity_type\":\"filament\"}]");
        }
    }
}