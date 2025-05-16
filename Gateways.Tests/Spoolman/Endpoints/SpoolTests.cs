using FluentAssertions;
using Moq;
using RichardSzalay.MockHttp;
using System.Text.Json;

namespace Gateways.Tests
{
    internal class SpoolTests : EndpointTest<SpoolSpoolmanEndpoint>
    {
        private Vendor vendor = new Vendor() { Id = 1, Name = "Vendor1" };
        private Filament filament = new Filament() { Id = 2, Name = "Vendor1", Material = "PLA", ColorHex = "FFFF00", Vendor = new Vendor() { Id = 1, Name = "Vendor1" } };

        [Test]
        public async Task GivenIdAndWeight_WhenUseSpoolWeight_ShouldReturnTrue()
        {
            // Arrange & Act
            var result = await Endpoint.UseSpoolWeight(2, 100);

            // Assert   
            result.Should().BeTrue();
        }

        [Test]
        public async Task GivenNonExistingSpool_WhenGetOrCreate_CreatedSpoolShouldBeReturned()
        {
            // Arrange & Act
            var result = await Endpoint.GetOrCreateSpool("Vendor1", "PLA", "#FFFF0000", string.Empty, string.Empty);

            // Assert   
            result.Should().NotBeNull();
            result.Filament.Vendor.Name.Should().Be("Vendor1");
            result.Filament.Material.Should().Be("PLA");
            result.Filament.ColorHex.Should().Be("FFFF00");
        }

        [Test]
        public async Task GivenExistingSpoolWithTrayId_WhenGetOrCreate_ExistingSpoolShouldBeReturned()
        {
            // Arrange & Act
            var result = await Endpoint.GetOrCreateSpool("Vendor1", "PLA", "#B7B3A800", "tray_1", string.Empty);

            // Assert   
            result.Should().NotBeNull();
            result.Filament.Vendor.Name.Should().Be("Vendor1");
            result.Filament.Material.Should().Be("PLA");
            result.Filament.ColorHex.Should().Be("B7B3A8");
            result.Extra["active_tray"].Should().Be("\"tray_1\"");
        }

        [Test]
        public async Task GivenExistingSpoolWithTag_WhenGetOrCreate_ExistingSpoolShouldBeReturned()
        {
            // Arrange & Act
            var result = await Endpoint.GetOrCreateSpool("Vendor1", "PLA", "#89898900", string.Empty, "11");

            // Assert   
            result.Should().NotBeNull();
            result.Filament.Vendor.Name.Should().Be("Vendor1");
            result.Filament.Material.Should().Be("PLA");
            result.Filament.ColorHex.Should().Be("898989");
            result.Extra["tag"].Should().Be("\"11\"");
        }

        [Test]
        public async Task GivenExistingSpool_WhenGetOrCreate_ExistingSpoolShouldBeReturned()
        {
            // Arrange & Act
            var result = await Endpoint.GetOrCreateSpool("Vendor1", "PLA", "#FFFFFFFF", string.Empty, string.Empty);

            // Assert   
            result.Should().NotBeNull();
            result.Filament.Vendor.Name.Should().Be("Vendor1");
            result.Filament.Material.Should().Be("PLA");
            result.Filament.ColorHex.Should().Be("FFFFFF");
        }

        public override void SetupHttpClient(MockHttpMessageHandler mockHandler)
        {
            mockHandler
               .When(HttpMethod.Get, "/api/v1/spool")
               .Respond("application/json", "[{\"id\":1,\"registered\":\"2025-03-18T18:40:29Z\",\"first_used\":\"2025-03-20T15:27:42Z\",\"last_used\":\"2025-04-08T15:24:07Z\",\"filament\":{\"id\":1,\"registered\":\"2025-03-18T18:40:29Z\",\"name\":\"Gray\",\"vendor\":{\"id\":1,\"registered\":\"2025-03-10T20:38:35Z\",\"name\":\"Vendor1\",\"external_id\":\"Vendor1\",\"extra\":{}},\"material\":\"PLA\",\"price\":0.0,\"density\":1.24,\"diameter\":1.75,\"weight\":1000.0,\"spool_weight\":0.0,\"color_hex\":\"898989\",\"extra\":{}},\"remaining_weight\":950.79,\"initial_weight\":1000.0,\"spool_weight\":250.0,\"used_weight\":49.21,\"remaining_length\":318784.3125052654,\"used_length\":16499.306911498978,\"archived\":false,\"extra\":{\"tag\":\"\\\"11\\\"\"}},{\"id\":2,\"registered\":\"2025-03-18T18:40:31Z\",\"first_used\":\"2025-04-09T14:21:03Z\",\"last_used\":\"2025-04-09T14:21:03Z\",\"filament\":{\"id\":2,\"registered\":\"2025-03-18T18:40:31Z\",\"name\":\"Peru\",\"vendor\":{\"id\":1,\"registered\":\"2025-03-10T20:38:35Z\",\"name\":\"Vendor1\",\"external_id\":\"Vendor1\",\"extra\":{}},\"material\":\"PLA\",\"price\":0.0,\"density\":1.24,\"diameter\":1.75,\"weight\":1000.0,\"spool_weight\":0.0,\"color_hex\":\"B87333\",\"extra\":{}},\"remaining_weight\":996.0,\"initial_weight\":1000.0,\"spool_weight\":250.0,\"used_weight\":4.0,\"remaining_length\":333942.48493909737,\"used_length\":1341.1344776670576,\"archived\":false,\"extra\":{\"active_tray\":\"\\\"tray_2\\\"\"}},{\"id\":3,\"registered\":\"2025-03-18T18:40:32Z\",\"first_used\":\"2025-03-28T10:54:30Z\",\"last_used\":\"2025-03-28T10:54:30Z\",\"filament\":{\"id\":3,\"registered\":\"2025-03-18T18:40:32Z\",\"name\":\"White\",\"vendor\":{\"id\":1,\"registered\":\"2025-03-10T20:38:35Z\",\"name\":\"Vendor1\",\"external_id\":\"Vendor1\",\"extra\":{}},\"material\":\"PLA\",\"price\":0.0,\"density\":1.24,\"diameter\":1.75,\"weight\":1000.0,\"spool_weight\":0.0,\"color_hex\":\"FFFFFF\",\"extra\":{}},\"remaining_weight\":991.0,\"initial_weight\":1000.0,\"spool_weight\":250.0,\"used_weight\":9.0,\"remaining_length\":332266.06684201356,\"used_length\":3017.5525747508796,\"archived\":false,\"extra\":{}},{\"id\":4,\"registered\":\"2025-03-18T18:40:33Z\",\"first_used\":\"2025-03-18T18:41:33Z\",\"last_used\":\"2025-03-18T18:41:33Z\",\"filament\":{\"id\":4,\"registered\":\"2025-03-18T18:40:33Z\",\"name\":\"Black\",\"vendor\":{\"id\":1,\"registered\":\"2025-03-10T20:38:35Z\",\"name\":\"Vendor1\",\"external_id\":\"Vendor1\",\"extra\":{}},\"material\":\"PLA\",\"price\":0.0,\"density\":1.24,\"diameter\":1.75,\"weight\":1000.0,\"spool_weight\":0.0,\"color_hex\":\"161616\",\"extra\":{}},\"remaining_weight\":990.0,\"initial_weight\":1000.0,\"spool_weight\":250.0,\"used_weight\":10.0,\"remaining_length\":331930.78322259674,\"used_length\":3352.836194167644,\"archived\":false,\"extra\":{}},{\"id\":6,\"registered\":\"2025-03-21T11:29:08Z\",\"first_used\":\"2025-03-21T11:29:08Z\",\"last_used\":\"2025-04-12T16:11:26Z\",\"filament\":{\"id\":6,\"registered\":\"2025-03-21T11:29:08Z\",\"name\":\"Black Glossy\",\"vendor\":{\"id\":1,\"registered\":\"2025-03-10T20:38:35Z\",\"name\":\"Vendor1\",\"external_id\":\"Vendor1\",\"extra\":{}},\"material\":\"PETG\",\"price\":0.0,\"density\":1.24,\"diameter\":1.75,\"weight\":1000.0,\"spool_weight\":0.0,\"color_hex\":\"161616\",\"extra\":{}},\"remaining_weight\":835.22,\"initial_weight\":1000.0,\"spool_weight\":250.0,\"used_weight\":164.78,\"remaining_length\":280035.58460927,\"used_length\":55248.034807494434,\"archived\":false,\"extra\":{\"active_tray\":\"\\\"tray_4\\\"\"}},{\"id\":7,\"registered\":\"2025-04-07T17:42:15Z\",\"first_used\":\"2025-04-07T17:42:15Z\",\"last_used\":\"2025-04-12T16:19:41Z\",\"filament\":{\"id\":7,\"registered\":\"2025-04-07T17:42:15Z\",\"name\":\"Silver\",\"vendor\":{\"id\":1,\"registered\":\"2025-03-10T20:38:35Z\",\"name\":\"Vendor1\",\"external_id\":\"Vendor1\",\"extra\":{}},\"material\":\"PLA\",\"price\":0.0,\"density\":1.24,\"diameter\":1.75,\"weight\":1000.0,\"spool_weight\":0.0,\"color_hex\":\"C6C4D2\",\"extra\":{}},\"remaining_weight\":419.0,\"initial_weight\":1000.0,\"spool_weight\":250.0,\"used_weight\":581.0,\"remaining_length\":140483.83653562426,\"used_length\":194799.78288114013,\"archived\":false,\"extra\":{\"active_tray\":\"\\\"tray_3\\\"\"}},{\"id\":8,\"registered\":\"2025-04-09T15:43:13Z\",\"first_used\":\"2025-04-09T15:43:13Z\",\"last_used\":\"2025-04-20T16:27:32Z\",\"filament\":{\"id\":8,\"registered\":\"2025-04-09T15:43:13Z\",\"name\":\"DarkGray\",\"vendor\":{\"id\":1,\"registered\":\"2025-03-10T20:38:35Z\",\"name\":\"Vendor1\",\"external_id\":\"Vendor1\",\"extra\":{}},\"material\":\"PLA\",\"price\":0.0,\"density\":1.24,\"diameter\":1.75,\"weight\":1000.0,\"spool_weight\":0.0,\"color_hex\":\"B7B3A8\",\"extra\":{}},\"remaining_weight\":896.0,\"initial_weight\":1000.0,\"spool_weight\":250.0,\"used_weight\":104.0,\"remaining_length\":300414.12299742096,\"used_length\":34869.4964193435,\"archived\":false,\"extra\":{\"active_tray\":\"\\\"tray_1\\\"\"}}]");

            mockHandler
               .When(HttpMethod.Post, "/api/v1/spool")
               .WithContent("{\"filament_id\":2,\"remaining_weight\":1000,\"initial_weight\":1000,\"spool_weight\":250,\"used_length\":0,\"archived\":false,\"extra\":{}}")
               .Respond("application/json", JsonSerializer.Serialize(new Spool()
               {
                   Id = 6,
                   Filament = filament
               }));

            mockHandler
                .When(HttpMethod.Put, "/api/v1/spool/2/use")
                .WithContent("{\"use_weight\":100}")
                .Respond("application/json", "");
        }

        public override void SetupConstructorArguments()
        {
            var vendorEndpoint = new Mock<IVendorEndpoint>();
            vendorEndpoint.Setup(endpoint => endpoint.GetOrCreate(It.IsAny<string>())).Returns(Task.FromResult(vendor));

            var filamentEndpoint = new Mock<IFilamentEndpoint>();
            filamentEndpoint.Setup(endpoint => endpoint.GetOrCreate(It.IsAny<Vendor>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(filament));

            ConstructorArguments = new object[]
            {
                vendorEndpoint.Object,
                filamentEndpoint.Object,
            };
        }
    }
}