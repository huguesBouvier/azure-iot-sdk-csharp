﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Shared;

namespace Microsoft.Azure.Devices.Client
{
    internal interface IDelegatingHandler : IContinuationProvider<IDelegatingHandler>, IDisposable
    {
        // Transport state.
        Task OpenAsync(TimeoutHelper timeoutHelper);

        Task OpenAsync(CancellationToken cancellationToken);

        Task CloseAsync(CancellationToken cancellationToken);

        Task WaitForTransportClosedAsync();

        bool IsUsable { get; }

        // Telemetry uplink.
        Task SendEventAsync(Message message, CancellationToken cancellationToken);

        Task SendEventAsync(IEnumerable<Message> messages, CancellationToken cancellationToken);

        // Telemetry downlink for devices.
        Task<Message> ReceiveAsync(CancellationToken cancellationToken);

        Task<Message> ReceiveAsync(TimeoutHelper timeoutHelper);

        Task EnableReceiveMessageAsync(CancellationToken cancellationToken);

        // This is to ensure that if device connects over MQTT with CleanSession flag set to false,
        // then any message sent while the device was disconnected is delivered on the callback.
        Task EnsurePendingMessagesAreDeliveredAsync(CancellationToken cancellationToken);

        Task DisableReceiveMessageAsync(CancellationToken cancellationToken);

        Task RejectAsync(string lockToken, CancellationToken cancellationToken);

        Task AbandonAsync(string lockToken, CancellationToken cancellationToken);

        Task CompleteAsync(string lockToken, CancellationToken cancellationToken);

        // Telemetry downlink for modules.
        Task EnableEventReceiveAsync(CancellationToken cancellationToken);

        Task DisableEventReceiveAsync(CancellationToken cancellationToken);

        // Methods.
        Task EnableMethodsAsync(CancellationToken cancellationToken);

        Task DisableMethodsAsync(CancellationToken cancellationToken);

        Task SendMethodResponseAsync(MethodResponseInternal methodResponse, CancellationToken cancellationToken);

        // Twin.
        Task<Twin> SendTwinGetAsync(CancellationToken cancellationToken);

        Task SendTwinPatchAsync(TwinCollection reportedProperties, CancellationToken cancellationToken);

        Task EnableTwinPatchAsync(CancellationToken cancellationToken);

        Task DisableTwinPatchAsync(CancellationToken cancellationToken);

        // Convention driven operations.

        Task<ClientProperties> GetPropertiesAsync(PayloadConvention payloadConvention, CancellationToken cancellationToken);

        Task SendPropertyPatchAsync(ClientPropertyCollection reportedProperties, CancellationToken cancellationToken);
    }
}
