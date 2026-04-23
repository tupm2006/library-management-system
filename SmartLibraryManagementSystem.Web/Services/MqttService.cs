// using System;
// using System.Text;
// using System.Threading.Tasks;
// using MQTTnet;
// using MQTTnet.Client;
// using MQTTnet.Client.Options;

// namespace SmartLibraryManagementSystem.Services
// {
//     public class MqttService : IMqttService
//     {
//         private readonly IMqttClient _mqttClient;
//         private readonly IMqttClientOptions _mqttOptions;

//         public MqttService()
//         {
//             var factory = new MqttFactory();
//             _mqttClient = factory.CreateMqttClient();

//             _mqttOptions = new MqttClientOptionsBuilder()
//                 .WithClientId(Guid.NewGuid().ToString())
//                 .WithTcpServer("broker.hivemq.com", 1883)
//                 .WithCleanSession()
//                 .Build();
//         }

//         public async Task PublishMessageAsync(string topic, string payload)
//         {
//             if (!_mqttClient.IsConnected)
//             {
//                 await _mqttClient.ConnectAsync(_mqttOptions);
//             }

//             var message = new MqttApplicationMessageBuilder()
//                 .WithTopic(topic)
//                 .WithPayload(Encoding.UTF8.GetBytes(payload))
//                 .WithExactlyOnceQoS()
//                 .WithRetainFlag()
//                 .Build();

//             await _mqttClient.PublishAsync(message);
//         }
//     }
// }
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;

namespace SmartLibraryManagementSystem.Services
{
    public class MqttService : IMqttService
    {
        private readonly IMqttClient _mqttClient;
        private readonly MqttClientOptions _options;

        public MqttService()
        {
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();

            // Cấu hình kết nối đến HiveMQ cho phiên bản 4.x
            _options = new MqttClientOptionsBuilder()
                .WithTcpServer("broker.hivemq.com")
                .Build();
        }

        public async Task PublishMessageAsync(string topic, string payload)
        {
            if (!_mqttClient.IsConnected)
            {
                await _mqttClient.ConnectAsync(_options);
            }

            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .Build();

            await _mqttClient.PublishAsync(message);
        }
    }
}