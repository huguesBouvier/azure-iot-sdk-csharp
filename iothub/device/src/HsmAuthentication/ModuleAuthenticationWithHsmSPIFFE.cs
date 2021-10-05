// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Azure.Devices.Client.HsmAuthentication
{
    /// <summary>
    /// Authentication method that uses HSM to get a SAS token.
    /// </summary>
    internal class ModuleAuthenticationWithHsmSPIFFE : ModuleAuthenticationWithTokenRefresh
    {
        private readonly ITokenProvider tokenProvider;
        private readonly string _generationId;

        internal ModuleAuthenticationWithHsmSPIFFE(
            ITokenProvider signatureProvider,
            string deviceId,
            string moduleId,
            string generationId,
            TimeSpan sasTokenTimeToLive,
            int sasTokenRenewalBuffer,
            bool disposeWithClient)
            : base(deviceId, moduleId, (int)sasTokenTimeToLive.TotalSeconds, sasTokenRenewalBuffer, disposeWithClient)
        {
            tokenProvider = signatureProvider ?? throw new ArgumentNullException(nameof(signatureProvider));
            _generationId = generationId;
        }

        ///<inheritdoc/>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        protected override async Task<string> SafeCreateNewToken(string iotHub, int suggestedTimeToLive)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return await tokenProvider.GenerateTokenAsync(ModuleId).ConfigureAwait(false);
        }
    }
}
