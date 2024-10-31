using MQTTnet;
using MQTTnet.Client;
using System.Security.Cryptography.X509Certificates;

public class MqttService
{
    private IMqttClient mqttClient;

    // Define an event for message received
    public event Action<string> ApplicationMessageReceived;
    private string lastReceivedMessage;


    public async Task ConnectToAwsMqttAsync()
    {
        var factory = new MqttFactory();
        mqttClient = factory.CreateMqttClient();

        // Load your certificates
        var clientCert = new X509Certificate2("certs/device_cert.pfx", "Pa$$w0rd"); // Use the .pfx file here
        var caCert = X509Certificate.CreateFromCertFile("certs/AmazonRootCA1.pem");

        var tlsOptions = new MqttClientOptionsBuilderTlsParameters
        {
            UseTls = true,
            Certificates = new List<X509Certificate>
            {
                clientCert,
                caCert
            },
            AllowUntrustedCertificates = true, // Set this to true if using self-signed certs
            CertificateValidationHandler = context =>
            {
                return true; // Assume certificate is valid
            }
        };

        var options = new MqttClientOptionsBuilder()
            .WithClientId("MyClientId")
            .WithTcpServer("a2lyaa3612tlxh-ats.iot.us-east-2.amazonaws.com", 8883) // AWS IoT MQTT port
            .WithTls(tlsOptions)
            .WithCleanSession()
            .Build();

        try
        {
            await mqttClient.ConnectAsync(options, CancellationToken.None);
            Console.WriteLine("Connected to AWS IoT MQTT successfully.");

            // Subscribe to the topic
            await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("uma/gateway/00aE9F76/electrical/meter").Build());

            // Handle incoming messages
            mqttClient.ApplicationMessageReceivedAsync += async e =>
            {
                lastReceivedMessage = System.Text.Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                // Raise the event for the UI to handle
                ApplicationMessageReceived?.Invoke(lastReceivedMessage);
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error connecting to MQTT: {ex.Message}");
        }
    }
    public string GetLastMessage()
    {
        return lastReceivedMessage; // Return the last received message
    }
}
