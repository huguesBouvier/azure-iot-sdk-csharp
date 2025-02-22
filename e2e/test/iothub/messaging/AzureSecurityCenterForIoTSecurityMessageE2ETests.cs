// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.E2ETests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Microsoft.Azure.Devices.E2ETests.Messaging
{
    [TestClass]
    [TestCategory("E2E")]
    [TestCategory("IoTHub")]
    [TestCategory("LongRunning")]
    public class AzureSecurityCenterForIoTSecurityMessageE2ETests : E2EMsTestBase
    {
        private readonly string _devicePrefix = $"{nameof(AzureSecurityCenterForIoTSecurityMessageE2ETests)}_";
        private readonly string _modulePrefix = $"{nameof(AzureSecurityCenterForIoTSecurityMessageE2ETests)}_";

        private readonly AzureSecurityCenterForIoTLogAnalyticsClient _logAnalyticsClient;

        public AzureSecurityCenterForIoTSecurityMessageE2ETests()
        {
            _logAnalyticsClient = AzureSecurityCenterForIoTLogAnalyticsClient.CreateClient();
        }

        [LoggedTestMethod]
        public Task SecurityMessage_DeviceSendSingleMessage_Amqp()
        {
            return TestSecurityMessageAsync(Client.TransportType.Amqp_Tcp_Only);
        }

        [LoggedTestMethod]
        public Task SecurityMessage_ModuleSendSingleMessage_Amqp()
        {
            return TestSecurityMessageModuleAsync(Client.TransportType.Amqp_Tcp_Only);
        }

        [LoggedTestMethod]
        [TestCategory("Flaky")]
        public Task SecurityMessage_DeviceSendSingleMessage_AmqpWs()
        {
            return TestSecurityMessageAsync(Client.TransportType.Amqp_WebSocket_Only);
        }

        [LoggedTestMethod]
        public Task SecurityMessage_ModuleSendSingleMessage_AmqpWs()
        {
            return TestSecurityMessageModuleAsync(Client.TransportType.Amqp_WebSocket_Only);
        }

        [LoggedTestMethod]
        public Task SecurityMessage_DeviceSendSingleMessage_Mqtt()
        {
            return TestSecurityMessageAsync(Client.TransportType.Mqtt_Tcp_Only);
        }

        [LoggedTestMethod]
        public Task SecurityMessage_ModuleSendSingleMessage_Mqtt()
        {
            return TestSecurityMessageModuleAsync(Client.TransportType.Mqtt_Tcp_Only);
        }

        [LoggedTestMethod]
        public Task SecurityMessage_DeviceSendSingleMessage_MqttWs()
        {
            return TestSecurityMessageAsync(Client.TransportType.Mqtt_WebSocket_Only);
        }

        [LoggedTestMethod]
        public Task SecurityMessage_ModuleSendSingleMessage_MqttWs()
        {
            return TestSecurityMessageModuleAsync(Client.TransportType.Mqtt_WebSocket_Only);
        }

        [LoggedTestMethod]
        public Task SecurityMessage_DeviceSendSingleMessage_Http()
        {
            return TestSecurityMessageAsync(Client.TransportType.Http1);
        }

        private Client.Message ComposeD2CSecurityTestMessage(out string eventId, out string payload, out string p1Value)
        {
            eventId = p1Value = Guid.NewGuid().ToString();
            payload = ComposeAzureSecurityCenterForIoTSecurityMessagePayload(eventId).ToString(Newtonsoft.Json.Formatting.None);

            var message = new Client.Message(Encoding.UTF8.GetBytes(payload))
            {
                Properties = { ["property1"] = p1Value }
            };
            message.SetAsSecurityMessage();

            return message;
        }

        private JObject ComposeAzureSecurityCenterForIoTSecurityMessagePayload(string eventId)
        {
            var now = DateTime.UtcNow;
            return new JObject
            {
                { "AgentVersion", "0.0.1" },
                { "AgentId" , Guid.NewGuid().ToString() },
                { "MessageSchemaVersion", "1.0" },
                { "Events", new JArray
                    {
                        new JObject
                        {
                            { "EventType", "Security" },
                            { "Category", "Periodic" },
                            { "Name", "ListeningPorts" },
                            { "IsEmpty", true },
                            { "PayloadSchemaVersion", "1.0" },
                            { "Id", eventId },
                            { "TimestampLocal", now },
                            { "TimestampUTC", now },
                            { "Payload", new JArray() }
                        }
                    }
                }
            };
        }

        private async Task TestSecurityMessageAsync(Client.TransportType transport)
        {
            using TestDevice testDevice = await TestDevice.GetTestDeviceAsync(Logger, _devicePrefix).ConfigureAwait(false);
            using DeviceClient deviceClient = testDevice.CreateDeviceClient(transport);

            try
            {
                await SendSingleSecurityMessageAsync(deviceClient, testDevice.Id, _logAnalyticsClient).ConfigureAwait(false);
            }
            finally
            {
                await deviceClient.CloseAsync().ConfigureAwait(false);
            }
        }

        private async Task TestSecurityMessageModuleAsync(Client.TransportType transport)
        {
            TestModule testModule = await TestModule.GetTestModuleAsync(_devicePrefix, _modulePrefix, Logger).ConfigureAwait(false);

            using (var moduleClient = ModuleClient.CreateFromConnectionString(testModule.ConnectionString, transport))
            {
                try
                {
                    await SendSingleSecurityMessageModuleAsync(moduleClient, testModule.DeviceId, _logAnalyticsClient).ConfigureAwait(false);
                }
                finally
                {
                    await moduleClient.CloseAsync().ConfigureAwait(false);
                }
            }
        }

        private async Task SendSingleSecurityMessageAsync(
            DeviceClient deviceClient,
            string deviceId,
            AzureSecurityCenterForIoTLogAnalyticsClient logAnalticsTestClient)
        {
            await deviceClient.OpenAsync().ConfigureAwait(false);

            using Client.Message testMessage = ComposeD2CSecurityTestMessage(out string eventId, out string payload, out string p1Value);
            await deviceClient.SendEventAsync(testMessage).ConfigureAwait(false);

            await ValidateEventAsync(deviceId, eventId, logAnalticsTestClient).ConfigureAwait(false);
        }

        private async Task SendSingleSecurityMessageModuleAsync(
            ModuleClient moduleClient,
            string deviceId,
            AzureSecurityCenterForIoTLogAnalyticsClient logAnalticsTestClient)
        {
            await moduleClient.OpenAsync().ConfigureAwait(false);
            using Client.Message testMessage = ComposeD2CSecurityTestMessage(out string eventId, out _, out _);
            await moduleClient.SendEventAsync(testMessage).ConfigureAwait(false);

            await ValidateEventAsync(deviceId, eventId, logAnalticsTestClient).ConfigureAwait(false);
        }

        private async Task ValidateEventAsync(
            string deviceId,
            string eventId,
            AzureSecurityCenterForIoTLogAnalyticsClient logAnalticsTestClient)
        {
            bool isReceivedOms = await logAnalticsTestClient.IsRawEventExist(deviceId, eventId).ConfigureAwait(false);
            Assert.IsTrue(isReceivedOms, "Security message was not received in customer log analytics");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _logAnalyticsClient.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
