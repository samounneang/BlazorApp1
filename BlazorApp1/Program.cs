using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using BlazorApp1.Data;
using MQTTnet.Client;
using MQTTnet;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();


// builder.Services.AddSingleton<IMqttClient>(serviceProvider => {
//     var factory = new MqttFactory();
//     var mqttClient = factory.CreateMqttClient();

//     // Load AWS IoT credentials
//     var options = new MqttClientOptionsBuilder()
//         .WithClientId("esp32-client")
//         .WithTcpServer("a2lyaa3612tlxh-ats.iot.us-east-2.amazonaws.com", 8883)  // Replace with your AWS IoT Endpoint
//         .WithTls(new MqttClientOptionsBuilderTlsParameters
//         {
//             UseTls = true,
//             Certificates = new List<X509Certificate>
//             {
//                 // Load certificates and private key
//                 X509Certificate.CreateFromCertFile("certs/Esp32_cert.crt"),  // Replace with actual cert file
//                 new X509Certificate2("certs/AmazonRootCA1.pem"),  // Replace with your private key
               
//             },
//             AllowUntrustedCertificates = true
//         })
//         .Build();

//     // Connect to MQTT broker
//     mqttClient.ConnectAsync(options).GetAwaiter().GetResult();
//     return mqttClient;
// });

// // Configure MQTT client
// builder.Services.AddSingleton<IMqttClient>(sp =>
// {
//     var mqttFactory = new MqttFactory();

//     var mqttClient = mqttFactory.CreateMqttClient();

//     var options = new MqttClientOptionsBuilder()
//         .WithClientId("MyClientId")
//         .WithTcpServer("a2lyaa3612tlxh-ats.iot.us-east-2.amazonaws.com", 8883) // replace with your AWS IoT endpoint
//         //.WithCredentials("YOUR_USERNAME", "YOUR_PASSWORD") // if needed, otherwise skip
        
//         .WithTls(new MqttClientOptionsBuilderTlsParameters
//         {
//             UseTls = true,
//             // Certificates = new List<X509Certificate>
//             // {
//             //     new X509Certificate2("certs/Esp32_cert.crt"),
//             //     new X509Certificate2("certs/AmazonRootCA1.pem"),
//             //     new X509Certificate2("certs/Private_key.key")
//             // },
//             Certificates = new List<X509Certificate>{
//                 new X509Certificate2("certs/device_cert.pfx", "Pa$$w0rd") // Use the .pfx file here
//             },
//             AllowUntrustedCertificates = false // Set to true if you are testing and using self-signed certificates
//         })
//         .Build();

//     // Connect to MQTT broker
//     mqttClient.ConnectAsync(options).GetAwaiter().GetResult();
//     return mqttClient;
// });

builder.Services.AddSingleton<MqttService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
