using RichardSzalay.MockHttp;
using System.Reflection;

namespace Gateways.Tests
{
    internal class EndpointTest<TSpoolmanEndpoint> where TSpoolmanEndpoint : class
    {
        protected HttpClient HttpClient;
        protected TSpoolmanEndpoint Endpoint;
        protected object[] ConstructorArguments;

        [SetUp]
        public void Setup()
        {
            var mockHandler = new MockHttpMessageHandler();

            SetupHttpClient(mockHandler);

            HttpClient = mockHandler.ToHttpClient();
            HttpClient.BaseAddress = new Uri($"http://localhost:8080/api/v1/");

            SetupConstructorArguments();

            var constructorArguments = new object[] { new SpoolmanConfiguration() { Url = "http://localhost:8080" } };

            if (ConstructorArguments?.Length > 0)
                constructorArguments = constructorArguments.Concat(ConstructorArguments).ToArray();

            var types = constructorArguments.Select(argument => argument.GetType()).ToArray();

            Endpoint = (TSpoolmanEndpoint)typeof(TSpoolmanEndpoint)
                .GetConstructor(types)?
                .Invoke(constructorArguments);

            SetHttpClientProperty(Endpoint);
        }

        protected TSpoolmanEndpoint SetHttpClientProperty<TSpoolmanEndpoint>(TSpoolmanEndpoint endpoint)
            where TSpoolmanEndpoint : class
        {
            var field = endpoint.GetType().GetField("HttpClient", BindingFlags.NonPublic | BindingFlags.Instance);

            typeof(FieldInfo).GetField("m_flags", BindingFlags.NonPublic | BindingFlags.Instance)?
                .SetValue(field, (int)field.Attributes & ~(int)FieldAttributes.InitOnly);

            // Set a new value for the field
            field?.SetValue(endpoint, HttpClient);

            return endpoint;
        }

        public virtual void SetupConstructorArguments()
        {
        }

        public virtual void SetupHttpClient(MockHttpMessageHandler mockHandler)
        {
        }

        [TearDown]
        public void TearDown()
        {
            HttpClient.Dispose();
        }
    }
}