﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Microsoft.Azure.Amqp;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Azure.Devices.Client.Extensions;
using Microsoft.Azure.Devices.Client.Transport.Amqp;

namespace Microsoft.Azure.Devices.Client.Transport.AmqpIoT
{
    internal class AmqpIoTConnection
    {
        public event EventHandler Closed;
        private readonly AmqpConnection _amqpConnection;
        private readonly AmqpIoTCbsLink _amqpIoTCbsLink;

        internal AmqpIoTConnection(AmqpConnection amqpConnection)
        {
            _amqpConnection = amqpConnection;
            _amqpIoTCbsLink = new AmqpIoTCbsLink(new AmqpCbsLink(amqpConnection));
        }

        internal AmqpIoTCbsLink GetCbsLink()
        {
            return _amqpIoTCbsLink;
        }

        internal void AmqpConnectionClosed(object sender, EventArgs e)
        {
            Closed.Invoke(sender, e);
        }

        internal async Task<AmqpIoTSession> OpenSessionAsync(TimeSpan timeout)
        {
            if (_amqpConnection.IsClosing())
            {
                throw AmqpIoTExceptionAdapter.RESOUCE_DISCONNECTED_EXCEPTION;
            }

            AmqpSessionSettings amqpSessionSettings = new AmqpSessionSettings()
            {
                Properties = new Fields()
            };

            try
            {
                var amqpSession = new AmqpSession(_amqpConnection, amqpSessionSettings, AmqpIoTLinkFactory.GetInstance());
                _amqpConnection.AddSession(amqpSession, new ushort?());
                await amqpSession.OpenAsync(timeout).ConfigureAwait(false);
                return new AmqpIoTSession(amqpSession);
            }
            catch(Exception e) when (!e.IsFatal())
            {
                Exception ex = AmqpIoTExceptionAdapter.ConvertToIoTHubException(e, _amqpConnection);
                if (ReferenceEquals(e, ex))
                {
                    throw;
                }
                else
                {
                    if (ex is AmqpIoTResourceException)
                    {
                        _amqpConnection.SafeClose();
                    }
                    throw ex;
                }
            }
        }

        internal async Task<IAmqpAuthenticationRefresher> CreateRefresherAsync(DeviceIdentity deviceIdentity, TimeSpan timeout)
        {
            if (_amqpConnection.IsClosing())
            {
                throw AmqpIoTExceptionAdapter.RESOUCE_DISCONNECTED_EXCEPTION;
            }
            try
            {
                IAmqpAuthenticationRefresher amqpAuthenticator = new AmqpAuthenticationRefresher(deviceIdentity, _amqpIoTCbsLink);
                await amqpAuthenticator.InitLoopAsync(timeout).ConfigureAwait(false);
                return amqpAuthenticator;
            }
            catch (Exception e) when (!e.IsFatal())
            {
                Exception ex = AmqpIoTExceptionAdapter.ConvertToIoTHubException(e, _amqpConnection);
                if (ReferenceEquals(e, ex))
                {
                    throw;
                }
                else
                {
                    throw ex;
                }
            }

        }

        internal void SafeClose()
        {
            _amqpConnection.SafeClose();
        }

        internal bool IsClosing()
        {
            return _amqpConnection.IsClosing();
        }
    }
}
