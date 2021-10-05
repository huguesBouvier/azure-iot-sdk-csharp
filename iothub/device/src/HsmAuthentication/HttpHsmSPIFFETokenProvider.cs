// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client.HsmAuthentication.GeneratedCode;
using Microsoft.Azure.Devices.Client.TransientFaultHandling;

#if !NET451

using Microsoft.Azure.Devices.Client.HsmAuthentication.Transport;

#endif

namespace Microsoft.Azure.Devices.Client.HsmAuthentication
{
    internal class HttpHsmSPIFFETokenProvider : ITokenProvider
    {
        private readonly string _apiVersion;
        private readonly Uri _providerUri;

        private static readonly ITransientErrorDetectionStrategy s_transientErrorDetectionStrategy = new ErrorDetectionStrategy();

        private static readonly RetryStrategy s_transientRetryStrategy = new ExponentialBackoffRetryStrategy(
            retryCount: 3,
            minBackoff: TimeSpan.FromSeconds(2),
            maxBackoff: TimeSpan.FromSeconds(30),
            deltaBackoff: TimeSpan.FromSeconds(3));

        public HttpHsmSPIFFETokenProvider(string providerUri, string apiVersion)
        {
            if (string.IsNullOrEmpty(providerUri))
            {
                throw new ArgumentNullException(nameof(providerUri));
            }
            if (string.IsNullOrEmpty(apiVersion))
            {
                throw new ArgumentNullException(nameof(apiVersion));
            }

            _providerUri = new Uri(providerUri);
            _apiVersion = apiVersion;
        }

        public async Task<string> GenerateTokenAsync(string moduleId)
        {
            if (string.IsNullOrEmpty(moduleId))
            {
                throw new ArgumentNullException(nameof(moduleId));
            }

            using HttpClient httpClient = HttpClientHelper.GetHttpClient(_providerUri);
            try
            {
                var hsmHttpClient = new HttpHsmClient(httpClient)
                {
                    BaseUrl = HttpClientHelper.GetBaseUrl(_providerUri)
                };

                GenerateTokenResponse response = await GenerateTokenAsyncWithRetryAsync(hsmHttpClient, moduleId)
                    .ConfigureAwait(false);

                return response.Token;
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case SwaggerException<ErrorResponse> errorResponseException:
                        throw new HttpHsmComunicationException(
                            $"Error calling GenerateToken: {errorResponseException.Result?.Message ?? string.Empty}",
                            errorResponseException.StatusCode);
                    case SwaggerException swaggerException:
                        throw new HttpHsmComunicationException(
                            $"Error calling GenerateToken: {swaggerException.Response ?? string.Empty}",
                            swaggerException.StatusCode);
                    default:
                        throw;
                }
            }
        }

        private async Task<GenerateTokenResponse> GenerateTokenAsyncWithRetryAsync(
            HttpHsmClient hsmHttpClient,
            string moduleId)
        {
            var transientRetryPolicy = new RetryPolicy(s_transientErrorDetectionStrategy, s_transientRetryStrategy);
            GenerateTokenResponse response = await transientRetryPolicy
                .ExecuteAsync(() => hsmHttpClient.GenerateTokenAsync(_apiVersion, moduleId))
                .ConfigureAwait(false);
            return response;
        }

        private class ErrorDetectionStrategy : ITransientErrorDetectionStrategy
        {
            public bool IsTransient(Exception ex) => ex is SwaggerException se && se.StatusCode >= 500;
        }
    }
}
